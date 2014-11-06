namespace RentABookApp.Models.Poco
{
    using System;

    public class BookRent
    {
        public int Id { get; set; }

        public virtual AppUser Owner { get; set; }

        public virtual AppUser Receiver { get; set; }

        public virtual RentRequest Request { get; set; }

        public virtual Book Book { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public RentState State { get; set; }

        public virtual Feedback FeedBackOwner { get; set; }

        public virtual Feedback FeedBackReceiver { get; set; }
    }
}
