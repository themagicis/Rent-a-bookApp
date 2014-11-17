using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Areas.Users.Models
{
    public class RequestListItemModel
    {
        public int RequestId { get; set; }

        public string RequesterName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}