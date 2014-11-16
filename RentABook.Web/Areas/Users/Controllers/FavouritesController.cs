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
using RentABook.Web.Code;

namespace RentABook.Web.Areas.Users.Controllers
{
    [Authorize]
    public class FavouritesController : BaseController
    {
        private IRepository<AppUser> users;
        private IRepository<Favourite> favs;

        public FavouritesController(IRepository<AppUser> users, IRepository<Favourite> favs, IRepository<Category> categories, IRepository<Town> towns)
            :base(categories, towns)
        {
            this.users = users;
            this.favs = favs;
        }

        public ActionResult List()
        {
            var favsViewModel = this.favs.All().Where(f => f.CreatorId == this.CurrentUserId).Select(f => new FavouriteItemViewModel
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
            if (string.IsNullOrEmpty(userId) || userId == this.CurrentUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userDb = users.All().Where(u => u.Id == this.CurrentUserId && !u.Favourites.Any(f => f.UserId == userId)).FirstOrDefault();

            if (userDb == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            userDb.Favourites.Add(new Favourite
            {
                CreatorId = this.CurrentUserId,
                UserId = userId,
                DateCreated = DateTime.Now
            });
            users.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Remove(string id)
        {
            string userId = id;

            if (string.IsNullOrEmpty(userId) || userId == this.CurrentUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var favDb = favs.All().Where(f => f.CreatorId == this.CurrentUserId && f.UserId == userId).FirstOrDefault();

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