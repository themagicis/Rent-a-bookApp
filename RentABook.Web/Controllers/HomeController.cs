namespace RentABook.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using RentABook.Data.Repositories;
    using RentABook.Models.Poco;
    using RentABook.Web.Models;
    using System.Web.Caching;

    public class HomeController : Controller
    {
        private IRepository<AppUser> users;
        private IRepository<Book> books;

        public HomeController(IRepository<AppUser> users, IRepository<Book> books)
        {
            this.users = users;
            this.books = books;
        }

        public ActionResult Index()
        {
            HomeStatisticViewModel statistic;

            if (this.HttpContext.Cache["stat"] == null)
            {
                statistic = new HomeStatisticViewModel
                {
                    TopOwners = GetTopOwners(),
                    LatestBooks = GetLatestBooks()
                };

                this.HttpContext.Cache.Insert(
                    "stat",                           
                    statistic,                        
                    null,                             
                    DateTime.Now.AddMinutes(30),     
                    TimeSpan.Zero,                    
                    CacheItemPriority.Default,        
                    null);                            
            }

            statistic = ((HomeStatisticViewModel)this.HttpContext.Cache["stat"]);

            return View(statistic);
        }

        private IEnumerable<TopOwnersViewModel> GetTopOwners()
        {
            var result = this.users.All().OrderBy(u => u.FeedbackScore).Take(5).Select(u => new TopOwnersViewModel{
                 OwnerName = u.UserName,
                 OwnerFullName = u.FirstName + " " + u.LastName,
                 BooksCount = u.Books.Count,
                 Score = u.FeedbackScore
            });

            return result.ToList();
        }

        private IEnumerable<LatestBooksViewModel> GetLatestBooks()
        {
            var result = this.books.All().OrderBy(b => b.DateCreated).Take(5).Select(b => new LatestBooksViewModel
            {
                Id = b.Id,
                OwnerName = b.Owner.FirstName + " " + b.Owner.LastName,
                Title = b.Title,
                Authors = b.Author,
                Town = b.Address.Town.Name
            });

            return result.ToList();
        }
    }
}