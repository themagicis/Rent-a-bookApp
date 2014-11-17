using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Users.Models
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public int Age { get; set; }

        public string Phone { get; set; }

        public int Score { get; set; }

        public int BooksCount { get; set; }

        public bool IsSelfProfile { get; set; }

        public bool IsFavourite { get; set; }
    }
}