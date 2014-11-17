using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Users.Models;
using RentABook.Web.Code;
using RentABook.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RentABook.Web.Areas.Users.Controllers
{
    [Authorize]
    public class RequestsController : BaseController
    {
        private IRepository<RentRequest> requests;
        private IRepository<BookRent> rents;
        private IRepository<Book> books;
        private INotifier notifier;

        public RequestsController(IRepository<Book> books, IRepository<RentRequest> requests, IRepository<BookRent> rents, IRepository<Category> categories, IRepository<Town> towns, INotifier notifier)
            :base(categories, towns)
        {
            this.requests = requests;
            this.rents = rents;
            this.books = books;
            this.notifier = notifier;
        }

        [HttpPost]
        public ActionResult Notifications()
        {
            var model = this.requests
                .All()
                .Where(r => r.State == RequestState.Requested && r.OwnerId == this.CurrentUserId)
                .Select(r => new RequestNotificationModel
                {
                    BookId = r.BookId,
                    BookName = r.Book.Title,
                    RequesterName = r.Requester.FirstName + " " + r.Requester.LastName,
                    RequesterUser = r.Requester.UserName
                }).ToList();

            return PartialView("RequestNotifications", model);
        }

        [HttpPost]
        public ActionResult List(int id)
        {
            var book = this.books.All().Where(b => b.Id == id).FirstOrDefault();

            if (book == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (book.State == BookState.Available) {
                 var model = this.requests
                    .All()
                    .Where(r => r.State == RequestState.Requested &&  r.OwnerId == this.CurrentUserId && r.BookId == id)
                    .Select(r => new RequestListItemModel
                    {
                        RequestId = r.Id,
                        RequesterName = r.Requester.FirstName + " " + r.Requester.LastName,
                        StartDate = r.DateStart,
                        EndDate = r.DateEnd
                    }).ToList();

                return PartialView("RequestList", model);
            }
            else if (book.State == BookState.InRent)
            {
                var rent = this.rents.All().FirstOrDefault(r => r.State == RentState.Started && r.Request.BookId == id);

                if (rent.DateEnd < DateTime.Now) 
                {
                     return PartialView("ManageRent", id);
                }
                else
                {
                    return PartialView("RentInProgress");
                }
            }
            else 
            {
                return PartialView("SetAvailable", id);
            }
        }

        [HttpPost]
        public ActionResult Accept(int id)
        {
            var requestDb = this.requests.All("Book").FirstOrDefault(r => r.Id == id);

            if (requestDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var bookId = requestDb.BookId;
            var otherRequests = this.requests.All().Where(r => r.Id != id && r.BookId == bookId && r.State == RequestState.Requested).ToList();

            requestDb.State = RequestState.Approved;

            foreach (var request in otherRequests)
            {
                request.State = RequestState.Rejected;
            }

            requestDb.Book.State = BookState.InRent;

            requests.SaveChanges();

            this.rents.Add(new BookRent
            {
                DateStart = requestDb.DateStart,
                DateEnd = requestDb.DateEnd,
                OwnerId = requestDb.OwnerId,
                ReceiverId = requestDb.RequesterId,
                RequestId = requestDb.Id,
                State = RentState.Started
            });

            this.rents.SaveChanges();

            notifier.BookChangedState(requestDb.BookId, BookState.InRent);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}