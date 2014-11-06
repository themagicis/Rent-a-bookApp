namespace RentABookApp.Models.Poco
{
    public class AppUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public int Age { get; set; }

        public int FeedbackScore { get; set; }

        public bool IsActive { get; set; }
    }
}
