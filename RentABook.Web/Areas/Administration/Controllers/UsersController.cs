using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentABook.Web.Areas.Administration.Models;
using System.Web.Security;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentABook.Data;

namespace RentABook.Web.Areas.Administration.Controllers
{
    [Authorize(Roles="SuperAdmin")]
    public class UsersController : BaseController
    {
        private IRepository<AppUser> users;

        public UsersController(IRepository<AppUser> users, IRepository<Category> categories, IRepository<Town> towns)
            :base(categories, towns)
        {
            this.users = users;
        }


        public ActionResult Manage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var users = this.users.All().Select(u => new UserForAdministrationModel
            {
                Id = u.Id,
                UserName = u.UserName,
                FullName = u.FirstName + " " + u.LastName,
                Score = u.FeedbackScore,
                IsAdmin = u.Roles.Any(r => r.RoleId == "ceae6c48-7c97-42ca-83e9-dc611513d51a"),
                IsDeactivated  = u.IsDeactivated
            }).ToDataSourceResult(request);

            return this.Json(users);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, UserForAdministrationModel model)
        {
            var user = this.users.All().Where(u => u.Id == model.Id).FirstOrDefault();

            if (user == null) {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound); 
            }

            user.UserName = model.UserName;
            user.IsDeactivated = model.IsDeactivated;
            users.SaveChanges();

            var userManager = new UserManager<AppUser>(new UserStore<AppUser>(new RentABookDbContext()));
            if (model.IsAdmin && !userManager.IsInRole(user.Id, "Admin"))
            {
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!model.IsAdmin && userManager.IsInRole(user.Id, "Admin"))
            {
                userManager.RemoveFromRole(user.Id, "Admin");
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}