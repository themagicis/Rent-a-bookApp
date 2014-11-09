using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Users.Models
{
    public class AddressDetailsViewModel
    {
        public int? Id { get; set; }

        public string Town { get; set; }

        public string FullAddress { get; set; }
    }
}