namespace RentABook.Models.Poco
{
    public class Feedback
    {
        public int Id { get; set; }

        public virtual BookRent BookRent { get; set; }

        public int FeedbackScore { get; set; }

        public string Message { get; set; }
    }
}
