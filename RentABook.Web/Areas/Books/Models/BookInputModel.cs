using RentABook.Models.Poco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentABook.Web.Areas.Books.Models
{
    public class BookInputModel
    {
        [Required]
        [Display(Name = "Authors (comma separated)")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Short description")]
        public string ShortDescription { get; set; }

        [Required]
        [Display(Name = "Condition")]
        [UIHint("Slider")]
        public int Condition { get; set; }

        [Required]
        [Display(Name = "Genre(s)")]
        public int[] CategoryId { get; set; }

        public SelectList Categories { get; set; }

        [Required]
        [Display(Name = "Location of book")]
        public int? AddressId { get; set; }

        public SelectList Addresses { get; set; }

        [Required]
        public RentType RentType { get; set; }

        [Display(Name="Price")]
        public decimal? Price { get; set; }

        public bool IsPowerUser { get; set; }
    }
}