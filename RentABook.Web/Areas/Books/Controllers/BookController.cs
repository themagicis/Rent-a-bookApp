using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using RentABook.Web.Code;

namespace RentABook.Web.Areas.Books.Controllers
{
    [Authorize]
    public class BookController : BaseController
    {
        private IRepository<Category> categories;
        private IRepository<Address> addresses;
        private IRepository<Book> books;
        private IRepository<RentRequest> requests;

        public BookController(IRepository<Category> categories, IRepository<Address> addresses, IRepository<Book> books, IRepository<RentRequest> requests)
        {
            this.categories = categories;
            this.addresses = addresses;
            this.books = books;
            this.requests = requests;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var bookDb = books.All().Where(b => b.Id == id).FirstOrDefault();

            if (bookDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            BookDetailsViewModel bookModel = new BookDetailsViewModel
            {
                Address = bookDb.Address.FullAddress,
                Author = bookDb.Author,
                Categories = bookDb.Categories.Select(c => c.Name).ToList(),
                Condition = bookDb.Condition,
                Id = bookDb.Id,
                OwnerId = bookDb.Owner.Id,
                OwnerUserName = bookDb.Owner.UserName,
                OwnerFullName = bookDb.Owner.FirstName + " " + bookDb.Owner.LastName,
                Price = bookDb.Price,
                RentType = bookDb.RentType,
                ShortDescription = bookDb.ShortDescription,
                State = bookDb.State,
                Title = bookDb.Title,
                Town = bookDb.Address.Town.Name,
                Comments = bookDb.Comments.Select(c => new CommentViewModel
                {
                    Content = c.Content,
                    DateCommented = c.DateCreated,
                    AuthorName = c.Author.FirstName + " " + c.Author.LastName
                }).ToList()
            };

            bookModel.BookState = GetBookState(bookDb);

            if ((bookModel.State == BookState.Archived || bookModel.State == BookState.WaitingForApproval) &&
                !(bookModel.BookState.IsUserOwner || bookModel.BookState.IsAdmin))
            {
                return View("NotAvailable");
            }

            return View(bookModel);
        }

        [HttpPost]
        public ActionResult RequestBook(RequestBookInputModel model)
        {
            var bookDb = books.All().Where(b => b.Id == model.BookId).FirstOrDefault();

            if (bookDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (bookDb.State != BookState.Available)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bookStateModel = GetBookState(bookDb);

            if (bookStateModel.IsRequested)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string[] dates = model.DateSpan.Split(' ');
            DateTime startTime = DateTime.Parse(dates[0]);
            DateTime endTime = DateTime.Parse(dates[2]);

            this.requests.Add(new RentRequest
            {
                DateRequested = DateTime.Now,
                BookId = bookDb.Id,
                OwnerId = bookDb.OwnerId,
                RequesterId = this.CurrentUserId,
                DateStart = startTime,
                DateEnd = endTime,
                State = RequestState.Requested
            });
            this.requests.SaveChanges();

            bookStateModel.IsRequested = true;

            return PartialView("BookState", bookStateModel);
        }

        private BookStateViewModel GetBookState(Book book)
        {
            var bookStateModel = new BookStateViewModel();
            bookStateModel.BookId = book.Id;
            bookStateModel.State = book.State;

            
            bool isUserOwner = book.OwnerId == this.CurrentUserId;
            bookStateModel.IsUserOwner = isUserOwner;

            bool isAdmin = User.IsInRole("Admin");
            bookStateModel.IsAdmin = isAdmin;

            bookStateModel.IsRequested = requests.All().Any(r => r.BookId == book.Id && r.RequesterId == this.CurrentUserId && r.State == RequestState.Requested);

            return bookStateModel;
        }

        [HttpGet]
        public ActionResult Create()
        {
            var catList = new SelectList(categories.All().ToList(), "Id", "Name", "");
            var addressList = new SelectList(addresses
                .All()
                .Where(a => a.UserId == this.CurrentUserId)
                .Select(a => new
                {
                    Id = a.Id,
                    Address = a.Town.Name + " / " + a.FullAddress
                })
            .ToList(), "Id", "Address", "");

            // set default values for model
            var model = new BookInputModel
            {
                Condition = 2,
                Categories = catList,
                Addresses = addressList,
                RentType = RentType.Free
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookInputModel newBook)
        {
            string currentUserId = this.CurrentUserId;

            if (newBook.RentType == RentType.Deposit || newBook.RentType == RentType.Paid)
            {
                if (!newBook.Price.HasValue)
                {
                    ModelState.AddModelError("Price", "The Price field is required when rent type is Paid or Deposit");
                }
            }

            if (ModelState.IsValid)
            {

                var cats = this.categories.All().Where(c => newBook.CategoryId.Contains(c.Id)).ToList();
                var bookDb = new Book
                {
                    AddressId = newBook.AddressId.Value,
                    Author = newBook.Author,
                    Condition = newBook.Condition,
                    OwnerId = currentUserId,
                    Price = newBook.Price,
                    RentType = newBook.RentType,
                    ShortDescription = newBook.ShortDescription,
                    State = BookState.Available,
                    Title = newBook.Title,
                    Categories = cats,
                    DateCreated = DateTime.Now
                };

                this.books.Add(bookDb);

                var history = new BookHistory()
                {
                    Book = bookDb,
                    State = BookState.Available,
                    DateChanged = DateTime.Now,
                    UserId = currentUserId
                };

                bookDb.History.Add(history);

                this.books.SaveChanges();

                return RedirectToAction("Details", new { id = bookDb.Id });
            }

            var catList = new SelectList(categories.All().ToList(), "Id", "Name", "");
            
            var addressList = new SelectList(addresses
                .All()
                .Where(a => a.UserId == currentUserId)
                .Select(a => new
                {
                    Id = a.Id,
                    Address = a.Town.Name + " / " + a.FullAddress
                })
            .ToList(), "Id", "Address", "");

            newBook.Categories = catList;
            newBook.Addresses = addressList;
            
            return View(newBook);
        }
    }
}