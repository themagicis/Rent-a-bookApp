using RentABook.Data.Repositories;
using RentABook.Models.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RentABook.Web.Areas.Users.Models;
using System.Net;

namespace RentABook.Web.Areas.Users.Controllers
{
    public class AddressController : Controller
    {
        private IRepository<Town> townsDb;
        private IRepository<Address> addressDb;

        public AddressController(IRepository<Town> townsDb, IRepository<Address> addressDb)
        {
            this.townsDb = townsDb;
            this.addressDb = addressDb;
        }

        [HttpGet]
        public ActionResult Addresses()
        {
            string userId = User.Identity.GetUserId();
            var addressList = addressDb.All().Where(a => a.UserId == userId).Select(a => new AddressDetailsViewModel
            {
                Id = a.Id,
                Town = a.Town.Name,
                FullAddress = a.FullAddress
            }).ToList();
            return View(addressList);
        }

        [HttpGet]
        public ActionResult CreateAddress()
        {
            var viewModel = new AddressViewModel();
            var townsList = townsDb.All().ToList();
            viewModel.Towns = new SelectList(townsList, "Id", "Name");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddress(AddressViewModel address)
        {
            if (ModelState.IsValid && address.TownId.HasValue)
            {
                addressDb.Add(new Address
                {
                    UserId = User.Identity.GetUserId(),
                    TownId = address.TownId.Value,
                    FullAddress = address.FullAddress
                });
                addressDb.SaveChanges();
                return RedirectToAction("Addresses");
            }

            var townsList = townsDb.All().ToList();
            address.Towns = new SelectList(townsList, "Id", "Name");
            return View(address);
        }

        [HttpPost]
        public ActionResult DeleteAddress(int id)
        {
            string userId = User.Identity.GetUserId();
            var address = addressDb.All().Where(a => a.Id == id && a.UserId == userId).FirstOrDefault();

            if (address == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            addressDb.Delete(address);
            addressDb.SaveChanges();

            return Content(string.Empty);
        }

        [HttpPost]
        public ActionResult EditAddress(int id)
        {
            string userId = User.Identity.GetUserId();
            var address = addressDb.All().Where(a => a.Id == id && a.UserId == userId).FirstOrDefault();

            if (address == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var townsList = townsDb.All().ToList();
            var viewModel = new EditAddressViewModel();
            viewModel.Id = address.Id;
            viewModel.TownId = address.TownId;
            viewModel.FullAddress = address.FullAddress;
            viewModel.Towns = new SelectList(townsList, "Id", "Name");
            return PartialView("_EditAddress", viewModel);
        }

        [HttpPost]
        public ActionResult SaveAddress(EditAddressViewModel editAddress)
        {
            if (ModelState.IsValid && editAddress.TownId.HasValue)
            {
                string userId = User.Identity.GetUserId();
                var address = addressDb.All().Where(a => a.Id == editAddress.Id && a.UserId == userId).FirstOrDefault();

                if (address == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                address.Id = editAddress.Id;
                address.TownId = editAddress.TownId.Value;
                address.FullAddress = editAddress.FullAddress;

                addressDb.SaveChanges();

                AddressDetailsViewModel updatedModel = new AddressDetailsViewModel
                {
                    Id = address.Id,
                    Town = address.Town.Name,
                    FullAddress = address.FullAddress
                };

                return PartialView("_ViewAddress", updatedModel);
            }

            var townsList = townsDb.All().ToList();
            editAddress.Towns = new SelectList(townsList, "Id", "Name");
            return PartialView("_EditAddress", editAddress);
        }
    }
}