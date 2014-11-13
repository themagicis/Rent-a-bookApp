namespace RentABook.Models.Poco
{
    using System;

    public class BookHistory
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public string UserId { get; set; }
        public virtual AppUser User { get; set; }

        public DateTime DateChanged { get; set; }

        public BookState State { get; set; }
    }
}
