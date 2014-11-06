namespace RentABookApp.Models.Poco
{
    using System;

    public class BookHistory
    {
        public int Id { get; set; }

        public virtual Book Book { get; set; }

        public DateTime DateChanged { get; set; }

        public BookState State { get; set; }
    }
}
