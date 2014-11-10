namespace RentABook.Web.Areas.Users.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using RentABook.Data.Repositories;
    using RentABook.Models.Poco;
    using RentABook.Web.Areas.Users.Models;
    using System.Net;

    [Authorize]
    [ValidateInput(false)]
    public class ProfileController : Controller
    {
        private IRepository<AppUser> users;

        public ProfileController(IRepository<AppUser> users)
        {
            this.users = users;
        }

        // GET: Users/Profile/Settings
        [HttpGet]
        public ActionResult Settings()
        {
            string currentUserId = User.Identity.GetUserId();
            var userModel = users
                .All()
                .Where(u => u.Id == currentUserId)
                .Select(u => new UpdateProfileViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Phone = u.PhoneNumber,
                    Age = u.Age
                })
                .FirstOrDefault();

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(UpdateProfileViewModel userProfileUpdate)
        {
            if (ModelState.IsValid) 
            {
                string currentUserId = User.Identity.GetUserId();
                var user = users
                    .All()
                    .Where(u => u.Id == currentUserId)
                    .FirstOrDefault();

                if (user != null) {
                    user.Age = userProfileUpdate.Age;
                    user.FirstName = userProfileUpdate.FirstName;
                    user.LastName = userProfileUpdate.LastName;
                    user.PhoneNumber = userProfileUpdate.Phone;

                    users.SaveChanges();

                    userProfileUpdate.IsUpdated = true;
                }
            }

            return View("Settings", userProfileUpdate);
        }
    }
}