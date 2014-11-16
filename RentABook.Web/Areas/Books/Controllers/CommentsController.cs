using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Books.Models;
using RentABook.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RentABook.Web.Areas.Books.Controllers
{
    public class CommentsController : BaseController
    {
        private IRepository<Book> books;
        private IRepository<AppUser> users;

        public CommentsController(IRepository<Book> books, IRepository<AppUser> users, IRepository<Category> categories, IRepository<Town> towns)
            :base(categories, towns)
        {
            this.books = books;
            this.users = users;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(CommentInputModel model)
        {
            var bookDb = books.All().Where(b => b.Id == model.Id).FirstOrDefault();

            if (bookDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            model.DateCommented = DateTime.Now;
            bookDb.Comments.Add(new Comment
            {
                AuthorId = this.CurrentUserId,
                BookId = model.Id,
                Content = model.Content,
                DateCreated = model.DateCommented
            });

            var user = this.users.All().First(u => u.Id == this.CurrentUserId);
            model.AuthorName = user.FirstName + " " + user.LastName;

            books.SaveChanges();

            return PartialView("Comment", new CommentViewModel[] { model });
        }

        [HttpPost]
        public ActionResult List(int Id)
        {
            var bookDb = books.All().Where(b => b.Id == Id).FirstOrDefault();

            if (bookDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var comments = bookDb.Comments.OrderByDescending(c => c.DateCreated).Select(c => new CommentViewModel
                {
                    Content = c.Content,
                    DateCommented = c.DateCreated,
                    AuthorName = c.Author.FirstName + " " + c.Author.LastName
                }).ToList();


            return PartialView("Comment", comments);
        }
    }
}