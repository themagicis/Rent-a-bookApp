using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Users.Models;
using RentABook.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentABook.Web.Areas.Users.Controllers
{
    [Authorize]
    public class RequestsController : BaseController
    {
        private IRepository<RentRequest> requests;

        public RequestsController(IRepository<RentRequest> requests, IRepository<Category> categories, IRepository<Town> towns)
            :base(categories, towns)
        {
            this.requests = requests;
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
    }
}