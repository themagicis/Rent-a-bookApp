namespace RentABook.Models.Poco
{
    using System.Collections.Generic;

    public class Address
    {
        public Address()
        {
            this.Books = new HashSet<Book>();
        }

        public int Id { get; set; }

        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public string FullAddress { get; set; }

        public string UserId { get; set; }
        public virtual AppUser User { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
