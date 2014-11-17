using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace RentABook.Web.Areas.Users.Models
{
    [DataContract()]
    public class EventViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
             
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Type { get; set; }
    }
}