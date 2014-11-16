using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentABook.Web.Models
{
    public class SearchViewModel
    {
        public string SearchText { get; set; }

        public int? Town { get; set; }

        public SelectList Towns { get; set; }

        public int? Category { get; set; }

        public SelectList Categories { get; set; }
    }
}