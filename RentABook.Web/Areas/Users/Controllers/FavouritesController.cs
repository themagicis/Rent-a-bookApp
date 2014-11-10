using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;
using RentABook.Web.Areas.Users.Models;

namespace RentABook.Web.Areas.Users.Controllers
{
    [Authorize]
    public class FavouritesController : Controller
    {
        private IRepository<AppUser> users;
        private IRepository<Favourite> favs;

        public FavouritesController(IRepository<AppUser> users, IRepository<Favourite> favs)
        {
            this.users = users;
            this.favs = favs;
        }

        public ActionResult List()
        {
            string currentUserId = User.Identity.GetUserId();

            var favsViewModel = this.favs.All().Where(f => f.CreatorId == currentUserId).Select(f => new FavouriteItemViewModel
            {
                UserId = f.UserId,
                FullName = f.User.FirstName + " " + f.User.LastName,
                UserName = f.User.UserName
            }).ToList();

            return View(favsViewModel);
        }

        public ActionResult Add(string id)
        {
            string userId = id;
            string currentUserId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId) || userId == currentUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userDb = users.All().Where(u => u.Id == currentUserId && !u.Favourites.Any(f => f.UserId == userId)).FirstOrDefault();

            if (userDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            userDb.Favourites.Add(new Favourite
            {
                CreatorId = currentUserId,
                UserId = userId,
                DateCreated = DateTime.Now
            });
            users.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Remove(string id)
        {
            string userId = id;
            string currentUserId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId) || userId == currentUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var favDb = favs.All().Where(f => f.CreatorId == currentUserId && f.UserId == userId).FirstOrDefault();

            if (favDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            favs.Delete(favDb);
            favs.SaveChanges();

            return RedirectToAction("List");
        }
    }
}