using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentABook.Models.Poco
{
    public class Favourite
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }
        public AppUser Creator { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
