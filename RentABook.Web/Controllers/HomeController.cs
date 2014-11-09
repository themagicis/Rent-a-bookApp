namespace RentABook.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using RentABook.Data.Repositories;
    using RentABook.Models.Poco;

    public class HomeController : Controller
    {
        public HomeController(IRepository<Book> books)
        {
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}