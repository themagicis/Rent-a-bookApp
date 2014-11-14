using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Models
{
    public class TopOwnersViewModel
    {
        public string OwnerName { get; set; }

        public string OwnerFullName { get; set; }

        public int BooksCount { get; set; }

        public int Score { get; set; }
    }
}