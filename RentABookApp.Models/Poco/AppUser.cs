namespace RentABook.Models.Poco
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            this.Books = new HashSet<Book>();
            this.RentsAsOwner = new HashSet<BookRent>();
            this.RentsAsReceiver = new HashSet<BookRent>();
            this.RequestsAsOwner = new HashSet<RentRequest>();
            this.RequestsAsRequester = new HashSet<RentRequest>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public int FeedbackScore { get; set; }

        public bool IsDeactivated { get; set; }

        public ICollection<Favourite> Favourites { get; set; }

        public ICollection<Favourite> AsFavourite { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public virtual ICollection<BookRent> RentsAsOwner { get; set; }

        public virtual ICollection<BookRent> RentsAsReceiver { get; set; }

        public virtual ICollection<RentRequest> RequestsAsOwner { get; set; }

        public virtual ICollection<RentRequest> RequestsAsRequester { get; set; }
    }
}
