using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Books.Models
{
    public class SearchResultViewModel
    {
        public IEnumerable<ResultBookViewModel> Books { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public string SearchText { get; set; }

        public int? Category { get; set; }

        public int? Town { get; set; }
    }
}