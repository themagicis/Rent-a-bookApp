using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentABook.Web.Areas.Users.Models
{
    public class AddressViewModel
    {
        [Required]
        [Display(Name="Town")]
        public int? TownId { get; set; }

        [Required]
        [Display(Name = "Address")]
        [MaxLength(100)]
        public string FullAddress { get; set; }

        public SelectList Towns { get; set; }
    }
}