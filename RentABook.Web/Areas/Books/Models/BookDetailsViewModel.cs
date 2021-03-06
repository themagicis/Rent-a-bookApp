﻿using RentABook.Models.Poco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Books.Models
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        [UIHint("Slider")]
        [Display(Name = "Condition")]
        public int Condition { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public string OwnerId { get; set; }

        public string OwnerUserName { get; set; }

        [Display(Name = "Owner")]
        public string OwnerFullName { get; set; }

        [Display(Name = "Location of book")]
        public string Address { get; set; }

        public string Town { get; set; }

        public BookState State { get; set; }

        public RentType RentType { get; set; }

        public decimal? Price { get; set; }

        public BookStateViewModel BookState { get; set; }
    }
}