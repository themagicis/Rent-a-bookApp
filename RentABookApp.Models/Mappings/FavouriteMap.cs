namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class FavouriteMap : EntityTypeConfiguration<Favourite>
    {
        public FavouriteMap()
        {
            this.ToTable("Favourite");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.DateCreated).HasColumnName("DateCreated").HasColumnType("datetime").IsRequired();

            this.HasRequired(x => x.Creator).WithMany(x => x.Favourites).HasForeignKey(x => x.CreatorId).WillCascadeOnDelete(false);
            this.HasRequired(x => x.User).WithMany(x => x.AsFavourite).HasForeignKey(x => x.UserId).WillCascadeOnDelete(false);
        }
    }
}