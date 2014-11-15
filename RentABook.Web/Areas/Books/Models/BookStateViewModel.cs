using RentABook.Models.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Books.Models
{
    public class BookStateViewModel
    {
        public int BookId { get; set; }

        public BookState State { get; set; }

        public bool IsUserOwner { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsRequested { get; set; }
    }
}