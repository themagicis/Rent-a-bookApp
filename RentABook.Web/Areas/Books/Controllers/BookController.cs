using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;

namespace RentABook.Web.Areas.Books.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private IRepository<Category> categories;
        private IRepository<Address> addresses;
        private IRepository<Book> books;

        public BookController(IRepository<Category> categories, IRepository<Address> addresses, IRepository<Book> books)
        {
            this.categories = categories;
            this.addresses = addresses;
            this.books = books;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            bool bookExist = books.All().Any(b => b.Id == id);

            if (!bookExist)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            BookDetailsViewModel bookModel = books.All().Where(b => b.Id == id).Select(b => new BookDetailsViewModel
            {
                Address = b.Address.FullAddress,
                Author = b.Author,
                Categories = b.Categories.Select(c => c.Name).ToList(),
                Condition = b.Condition,
                Id = b.Id,
                OwnerId = b.Owner.Id,
                OwnerUserName = b.Owner.UserName,
                OwnerFullName = b.Owner.FirstName + " " + b.Owner.LastName,
                Price = b.Price,
                RentType = b.RentType,
                ShortDescription = b.ShortDescription,
                State = b.State,
                Title = b.Title,
                Town = b.Address.Town.Name
            }).First();

            string currentUserId = User.Identity.GetUserId();
            bool isUserOwner = bookModel.OwnerId == currentUserId;
            bool isAdmin = User.IsInRole("Admin");

            if (bookModel.State == BookState.Archived && !(isUserOwner || isAdmin))
            {
                return View("Archive");
            }

            return View(bookModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var catList = new SelectList(categories.All().ToList(), "Id", "Name", "");
            string currentUserId = User.Identity.GetUserId();
            var addressList = new SelectList(addresses
                .All()
                .Where(a => a.UserId == currentUserId)
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
            string currentUserId = User.Identity.GetUserId();

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