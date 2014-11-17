using Kendo.Mvc.UI;
using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Administration.Models;
using RentABook.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Net;

namespace RentABook.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : BaseController
    {
        private IRepository<Book> books;

        public BooksController(IRepository<Book> books, IRepository<Category> categories, IRepository<Town> towns)
            : base(categories, towns)
        {
            this.books = books;
        }

        [HttpGet]
        public ActionResult ApproveList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Approve(int id)
        {
            var book = this.books.All().Where(u => u.Id == id).FirstOrDefault();

            if (book == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            book.State = BookState.Available;
            books.SaveChanges();

            return RedirectToAction("Details", "Book", new { area = "Books", id = id });
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var books = this.books.All().Where(b => b.State == BookState.WaitingForApproval).Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.ShortDescription,
                Owner = b.Owner.UserName
            }).ToDataSourceResult(request);

            return this.Json(books);
        }
    }
}