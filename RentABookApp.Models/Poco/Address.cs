namespace RentABookApp.Models.Poco
{
    public class Address
    {
        public int Id { get; set; }

        public virtual Town Town { get; set; }

        public string FullAddress { get; set; }

        public virtual AppUser User { get; set; }
    }
}
