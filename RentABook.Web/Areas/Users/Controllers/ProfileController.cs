using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace RentABook.Web.Areas.Users.Controllers
{
    [Authorize]
    [ValidateInput(false)]
    public class ProfileController : Controller
    {
        

        public ProfileController()
        {
        }

        // GET: Users/Profile/Settings
        [HttpGet]
        public ActionResult Settings()
        {
            return View();
        }
    }
}