namespace RentABook.Web.Areas.Users.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using RentABook.Data.Repositories;
    using RentABook.Models.Poco;
    using RentABook.Web.Areas.Users.Models;
    using RentABook.Web.Code;

    [Authorize]
    [ValidateInput(false)]
    public class AddressController : BaseController
    {
        private IRepository<Town> towns;
        private IRepository<Address> addresses;

        public AddressController(IRepository<Town> towns, IRepository<Address> addresses, IRepository<Category> categories)
            :base(categories, towns)
        {
            this.towns = towns;
            this.addresses = addresses;
        }

        [HttpGet]
        public ActionResult Addresses()
        {
            string userId = User.Identity.GetUserId();
            var addressList = addresses.All().Where(a => a.UserId == userId).Select(a => new AddressDetailsViewModel
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
            var townsList = towns.All().ToList();
            viewModel.Towns = new SelectList(townsList, "Id", "Name");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddress(AddressViewModel address)
        {
            if (ModelState.IsValid && address.TownId.HasValue)
            {
                addresses.Add(new Address
                {
                    UserId = User.Identity.GetUserId(),
                    TownId = address.TownId.Value,
                    FullAddress = address.FullAddress
                });
                addresses.SaveChanges();
                return RedirectToAction("Addresses");
            }

            var townsList = towns.All().ToList();
            address.Towns = new SelectList(townsList, "Id", "Name");
            return View(address);
        }

        [HttpPost]
        public ActionResult DeleteAddress(int id)
        {
            string userId = User.Identity.GetUserId();
            var address = addresses.All().Where(a => a.Id == id && a.UserId == userId).FirstOrDefault();

            if (address == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            addresses.Delete(address);
            addresses.SaveChanges();

            return Content(string.Empty);
        }

        [HttpPost]
        public ActionResult EditAddress(int id)
        {
            string userId = User.Identity.GetUserId();
            var address = addresses.All().Where(a => a.Id == id && a.UserId == userId).FirstOrDefault();

            if (address == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var townsList = towns.All().ToList();
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
                var address = addresses.All().Where(a => a.Id == editAddress.Id && a.UserId == userId).FirstOrDefault();

                if (address == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                address.Id = editAddress.Id;
                address.TownId = editAddress.TownId.Value;
                address.FullAddress = editAddress.FullAddress;

                addresses.SaveChanges();

                AddressDetailsViewModel updatedModel = new AddressDetailsViewModel
                {
                    Id = address.Id,
                    Town = address.Town.Name,
                    FullAddress = address.FullAddress
                };

                return PartialView("_ViewAddress", updatedModel);
            }

            var townsList = towns.All().ToList();
            editAddress.Towns = new SelectList(townsList, "Id", "Name");
            return PartialView("_EditAddress", editAddress);
        }
    }
}