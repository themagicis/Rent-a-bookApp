namespace RentABookApp.Models.Poco
{
    using System;

    public class RentRequest
    {
        public int Id { get; set; }

        public virtual AppUser Owner { get; set; }

        public virtual AppUser Requester { get; set; }

        public Book Book { get; set; }

        public DateTime DateRequested { get; set; }

        public string Message { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public RequestState State { get; set; }
    }
}
