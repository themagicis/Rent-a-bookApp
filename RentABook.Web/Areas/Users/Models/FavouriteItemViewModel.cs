using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Users.Models
{
    public class FavouriteItemViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
    }
}