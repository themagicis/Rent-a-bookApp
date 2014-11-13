namespace RentABook.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using RentABook.Data.Repositories;
    using RentABook.Models.Poco;


    //public class RentABookData : IRentABookData
    //{
    //    private DbContext context;
    //    private IDictionary<Type, object> repositories;

    //    public RentABookData(DbContext context)
    //    {
    //        this.context = context;
    //        this.repositories = new Dictionary<Type, object>();
    //    }

    //    public IQueryable<T> GetByEntity<T>() where T : class
    //    {
    //        return this.context.Set<T>().AsQueryable();
    //    }

    //    private IRepository<T> GetRepository<T>() where T : class
    //    {
    //        var typeOfRepository = typeof(T);
    //        if (!this.repositories.ContainsKey(typeOfRepository))
    //        {
    //            var newRepository = Activator.CreateInstance(typeof(EFRepository<T>), context);
    //            this.repositories.Add(typeOfRepository, newRepository);
    //        }

    //        return (IRepository<T>)this.repositories[typeOfRepository];
    //    }

    //    public int SaveChanges()
    //    {
    //        return this.context.SaveChanges();
    //    }

    //}
}
