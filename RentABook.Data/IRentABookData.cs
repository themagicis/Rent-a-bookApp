namespace RentABook.Data
{
    using RentABook.Data.Repositories;
    using RentABook.Models.Poco;

    public interface IRentABookData
    {
        IRepository<AppUser> Users { get; }
        
        int SaveChanges();
    }
}
