using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Books.Models
{
    public class RequestBookInputModel
    {
        public int BookId { get; set; }

        [Required]
        [UIHint("Calendar")]
        public string DateSpan { get; set; }
    }
}