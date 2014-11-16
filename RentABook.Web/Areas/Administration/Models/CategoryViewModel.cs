using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentABook.Web.Areas.Administration.Models
{
    public class CategoryViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [UIHint("NonEditable")]
        public int Books { get; set; }
    }
}