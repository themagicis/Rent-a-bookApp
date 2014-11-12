using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace RentABook.Web.Areas.Books.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private IRepository<Category> categories;
        private IRepository<Address> addresses;

        public BookController(IRepository<Category> categories, IRepository<Address> addresses)
        {
            this.categories = categories;
            this.addresses = addresses;
        }

        [HttpGet]
        public ActionResult Create()
        {
            var catList = new SelectList(categories.All().ToList(), "Id", "Name", "");
            string currentUserId = User.Identity.GetUserId();
            var addressList = new SelectList( addresses.All().Where(a => a.UserId == currentUserId).ToList(), "Id", "FullAddress", "");

            var model = new BookInputModel
            {
                Condition = 2,
                Categories = catList,
                Addresses = addressList,
                RentType = RentType.Free
            };
            return View(model);
        }
    }
}