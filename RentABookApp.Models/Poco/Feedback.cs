namespace RentABook.Models.Poco
{
    using System.Collections.Generic;

    public class Feedback
    {
        public Feedback()
        {
            this.OwnerRents = new HashSet<BookRent>();
            this.ReceiverRents = new HashSet<BookRent>();
        }

        public int Id { get; set; }

        public int FeedbackScore { get; set; }

        public string Message { get; set; }

        public virtual ICollection<BookRent> OwnerRents { get; set; }

        public virtual ICollection<BookRent> ReceiverRents { get; set; }
    }
}
