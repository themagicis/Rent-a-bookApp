using Kendo.Mvc.UI;
using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using RentABook.Web.Areas.Administration.Models;
using RentABook.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Net;

namespace RentABook.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConfigController : BaseController
    {
        private IRepository<Category> categories;
        private IRepository<Town> towns;

        public ConfigController(IRepository<Category> categories, IRepository<Town> towns)
            :base(categories, towns)
        {
            this.categories = categories;
            this.towns = towns;
        }

        [HttpGet]
        public ActionResult Categories()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Towns()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadCategories([DataSourceRequest]DataSourceRequest request)
        {
            var categories = this.categories.All().Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Books = c.Books.Count
            }).ToDataSourceResult(request);

            return this.Json(categories);
        }

        [HttpPost]
        public ActionResult ReadTowns([DataSourceRequest]DataSourceRequest request)
        {
            var towns = this.towns.All().Select(t => new TownViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Addresses = t.Addresses.Count
            }).ToDataSourceResult(request);

            return this.Json(towns);
        }

        [HttpPost]
        public ActionResult UpdateCategory([DataSourceRequest]DataSourceRequest request, CategoryViewModel model)
        {
            var category = this.categories.All().Where(u => u.Id == model.Id).FirstOrDefault();

            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            category.Name = model.Name;
            categories.SaveChanges();

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult UpdateTown([DataSourceRequest]DataSourceRequest request, TownViewModel model)
        {
            var town = this.towns.All().Where(u => u.Id == model.Id).FirstOrDefault();

            if (town == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            town.Name = model.Name;
            categories.SaveChanges();

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult CreateCategory([DataSourceRequest]DataSourceRequest request, CategoryViewModel model)
        {
            this.categories.Add(new Category
            {
                Name = model.Name
            });
            this.categories.SaveChanges();

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult CreateTown([DataSourceRequest]DataSourceRequest request, TownViewModel model)
        {
            this.towns.Add(new Town
            {
                Name = model.Name
            });
            this.towns.SaveChanges();

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}