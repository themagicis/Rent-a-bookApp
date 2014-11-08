namespace RentABook.Models.Poco
{
    using System.Collections.Generic;

    public class Town
    {
        public Town()
        {
            this.Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
