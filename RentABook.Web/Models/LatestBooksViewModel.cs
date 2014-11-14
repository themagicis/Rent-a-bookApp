using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Models
{
    public class LatestBooksViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Authors { get; set; }

        public string OwnerName { get; set; }

        public string Town { get; set; }
    }
}