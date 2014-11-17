using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace RentABook.Models.Poco
{
    public class Comment
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public string Content { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public string AuthorId { get; set; }

        public virtual AppUser Author { get; set; }
    }
}
