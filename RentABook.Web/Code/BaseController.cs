using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Routing;
using RentABook.Web.Models;
using System.Web.Caching;
using RentABook.Data.Repositories;
using RentABook.Models.Poco;

namespace RentABook.Web.Code
{
    public class BaseController : Controller
    {
        private IRepository<Category> categories;
        private IRepository<Town> towns;

        public BaseController(IRepository<Category> categories, IRepository<Town> towns)
        {
            this.towns = towns;
            this.categories = categories;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        public string CurrentUserId 
        { 
            get
            {
                return HttpContext.User.Identity.GetUserId();
            }
        }

        public ActionResult SearchForm()
        {
            SearchViewModel model;

            if (this.HttpContext.Cache["search"] == null)
            {
                model = new SearchViewModel
                {
                    Categories = LoadCategories(),
                    Towns = LoadTowns()
                };

                this.HttpContext.Cache.Insert(
                    "search",
                    model,
                    null,
                    DateTime.Now.AddHours(5),
                    TimeSpan.Zero,
                    CacheItemPriority.Default,
                    null);
            }

            model = ((SearchViewModel)this.HttpContext.Cache["search"]);

            return PartialView(model);
        }

        private SelectList LoadTowns()
        {
            return new SelectList(this.towns.All().ToList(), "Id", "Name", "");
        }

        private SelectList LoadCategories()
        {
            return new SelectList(this.categories.All().ToList(), "Id", "Name", "");
        }
    }
}