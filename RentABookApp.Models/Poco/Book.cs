namespace RentABookApp.Models.Poco
{
    public class Book
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public int Condition { get; set; }

        public virtual Category Category { get; set; }

        public virtual AppUser Owner { get; set; }

        public virtual Address Address { get; set; }

        public BookState State { get; set; }

        public RentType RentType { get; set; }

        public decimal? Price { get; set; }
    }
}
