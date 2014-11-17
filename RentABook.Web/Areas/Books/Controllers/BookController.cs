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
using RentABook.Web.Models;
using RentABook.Web.Hubs;

namespace RentABook.Web.Areas.Books.Controllers
{
    [Authorize]
    public class BookController : BaseController
    {
        private const int pageSize = 3;

        private IRepository<Category> categories;
        private IRepository<Address> addresses;
        private IRepository<Book> books;
        private IRepository<RentRequest> requests;
        private IRepository<AppUser> users;
        private IRepository<BookRent> rents;
        private INotifier notifier;

        public BookController(IRepository<BookRent> rents, IRepository<AppUser> users, IRepository<Category> categories, IRepository<Address> addresses,
            IRepository<Book> books, IRepository<RentRequest> requests, IRepository<Town> towns, INotifier notifier)
            :base(categories, towns)
        {
            this.categories = categories;
            this.addresses = addresses;
            this.books = books;
            this.requests = requests;
            this.users = users;
            this.rents = rents;
            this.notifier = notifier;
        }

        [AllowAnonymous]
        public ActionResult Search(SearchViewModel model)
        {
            int totalResults;
            var books = GetBooks(model, 1, out totalResults);
            int pageCount = totalResults / pageSize + (totalResults % pageSize == 0 ? 0 : 1);

            var resultModel = new SearchResultViewModel
            {
                Books = books,
                CurrentPage = 1,
                PageCount = pageCount,
                SearchText = model.SearchText,
                Category = model.Category,
                Town = model.Town,
                User = model.User
            };

            return View(resultModel);
        }

        public ActionResult SearchPaged(SearchViewModel model, int page)
        {
            int totalResults;
            var books = GetBooks(model, page, out totalResults);

            return PartialView("BookList", books);
        }

        private IEnumerable<ResultBookViewModel> GetBooks(SearchViewModel model, int page, out int totalCount)
        {
            var query = this.books.All().Where(b => b.State != BookState.WaitingForApproval && b.State != BookState.Archived);

            if (!string.IsNullOrEmpty(model.SearchText))
            {
                string searchStr = model.SearchText.ToLower();
                query = query.Where(b => b.Author.ToLower().Contains(searchStr) || b.Title.ToLower().Contains(searchStr));
            }

            if (model.Category.HasValue)
            {
                query = query.Where(b => b.Categories.Any(c => c.Id == model.Category));
            }

            if (model.Town.HasValue)
            {
                query = query.Where(b => b.Address.TownId == model.Town);
            }

            if (!string.IsNullOrEmpty(model.User))
            {
                query = query.Where(b => b.Owner.UserName == model.User);
            }

            totalCount = query.Count();

            var booksFound = query
                .OrderByDescending(b => b.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new ResultBookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    State = b.State,
                    Description = b.ShortDescription,
                    Town = b.Address.Town.Name,
                    Type = b.RentType
                }).ToList();

            return booksFound;
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
                Town = bookDb.Address.Town.Name
            };

            bookModel.BookState = GetBookState(bookDb);

            if ((bookModel.State == BookState.Archived || bookModel.State == BookState.WaitingForApproval) &&
                !(bookModel.BookState.IsUserOwner || bookModel.BookState.IsAdmin))
            {
                return View("NotAvailable");
            }

            return View(bookModel);
        }

        public ActionResult GetBookState(int id)
        {
            var bookDb = this.books.All().Where(b => b.Id == id).FirstOrDefault();

            if (bookDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            BookStateViewModel model = GetBookState(bookDb);
            return PartialView("BookState", model);
        }

        public ActionResult SetBookState(BookStateViewModel model)
        {
            var bookDb = this.books.All().Where(b => b.Id == model.BookId).FirstOrDefault();

            if (bookDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var rent = this.rents.All("Receiver").Where(r => r.OwnerId == this.CurrentUserId && r.Request.BookId == model.BookId && r.State == RentState.Started).First();

            if (model.State == BookState.NotReturned)
            {
                rent.Receiver.FeedbackScore -= 50;
                rent.State = RentState.BookNotReturned;
                bookDb.State = BookState.NotReturned;

                notifier.BookChangedState(bookDb.Id, BookState.NotReturned);
            }
            else
            {
                rent.Receiver.FeedbackScore += 5;
                bookDb.State = BookState.Available;
                rent.State = RentState.Completed;

                notifier.BookChangedState(bookDb.Id, BookState.Available);
            }

            this.books.SaveChanges();
            this.rents.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
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

            bool isPowerUser = this.users.All().First(u => u.Id == this.CurrentUserId).FeedbackScore > 100;

            // set default values for model
            var model = new BookInputModel
            {
                Condition = 2,
                Categories = catList,
                Addresses = addressList,
                RentType = RentType.Free,
                IsPowerUser = isPowerUser
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookInputModel newBook)
        {
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
                    OwnerId = this.CurrentUserId,
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
                    UserId = this.CurrentUserId
                };

                bookDb.History.Add(history);

                this.books.SaveChanges();

                var user = this.users.All().First(u => u.Id == this.CurrentUserId);
                if (bookDb.RentType == RentType.Free)
                {
                    user.FeedbackScore += 10;
                }

                this.users.SaveChanges();

                return RedirectToAction("Details", new { id = bookDb.Id });
            }

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

            newBook.Categories = catList;
            newBook.Addresses = addressList;
            
            return View(newBook);
        }
    }
}