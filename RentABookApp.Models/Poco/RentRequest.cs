namespace RentABook.Models.Poco
{
    using System;
    using System.Collections.Generic;

    public class RentRequest
    {
        public RentRequest()
        {
            this.BookRents = new HashSet<BookRent>();
        }

        public int Id { get; set; }

        public string OwnerId { get; set; }
        public virtual AppUser Owner { get; set; }

        public string RequesterId { get; set; }
        public virtual AppUser Requester { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public DateTime DateRequested { get; set; }

        public string Message { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public RequestState State { get; set; }

        public virtual ICollection<BookRent> BookRents { get; set; }
    }
}
