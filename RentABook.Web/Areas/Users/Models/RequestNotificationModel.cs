using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Users.Models
{
    public class RequestNotificationModel
    {
        public int BookId { get; set; }

        public string RequesterName { get; set; }

        public string RequesterUser { get; set; }

        public string BookName { get; set; }
    }
}