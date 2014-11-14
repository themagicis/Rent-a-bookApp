using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Models
{
    public class HomeStatisticViewModel
    {
        public IEnumerable<LatestBooksViewModel> LatestBooks { get; set; }

        public IEnumerable<TopOwnersViewModel> TopOwners { get; set; }
    }
}