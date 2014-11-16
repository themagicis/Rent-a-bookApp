using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentABook.Web.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<LatestBooksViewModel> LatestBooks { get; set; }

        public IEnumerable<TopOwnersViewModel> TopOwners { get; set; }
    }
}