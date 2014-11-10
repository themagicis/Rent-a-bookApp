using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;

namespace RentABook.Web.Areas.Users.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private IRepository<AppUser> users;
        private IRepository<Favourite> favs;

        public UsersController(IRepository<AppUser> users, IRepository<Favourite> favs)
        {
            this.users = users;
            this.favs = favs;
        }

        public ActionResult Index(string username)
        {
            string currentUserId = User.Identity.GetUserId();

            var userViewModel = this.users.All().Where(u => u.UserName == username).Select(u => new UserDetailsViewModel
            {
                Id = u.Id,
                Username = u.UserName,
                FullName = u.FirstName + " " + u.LastName,
                Phone = u.PhoneNumber,
                Age = u.Age,
            }).FirstOrDefault();

            if (userViewModel == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            userViewModel.IsFavourite = favs.All().Count(f => f.CreatorId == currentUserId && f.UserId == userViewModel.Id) > 0;
            userViewModel.IsSelfProfile = userViewModel.Id == currentUserId;

            return View(userViewModel);
        }
    }
}