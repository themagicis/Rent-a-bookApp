namespace RentABook.Models.Poco
{
    using System.Collections.Generic;

    public class Book
    {
        public Book()
        {
            this.History = new HashSet<BookHistory>();
            this.Requests = new HashSet<RentRequest>();
            this.Categories = new HashSet<Category>();
        }

        public int Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public int Condition { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public string OwnerId { get; set; }
        public virtual AppUser Owner { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public BookState State { get; set; }

        public RentType RentType { get; set; }

        public decimal? Price { get; set; }

        public virtual ICollection<BookHistory> History { get; set; }

        public virtual ICollection<RentRequest> Requests { get; set; }
    }
}
