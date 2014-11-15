using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Books.Models
{
    public class CommentViewModel
    {
        public string AuthorName { get; set; }

        public DateTime DateCommented { get; set; }

        public string Content { get; set; }
    }
}