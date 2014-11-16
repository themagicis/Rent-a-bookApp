using RentABook.Models.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Books.Models
{
    public class ResultBookViewModel
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public int Id { get; set; }

        public string Description { get; set; }

        public string Town { get; set; }

        public BookState State { get; set; }

        public RentType Type { get; set; }
    }
}