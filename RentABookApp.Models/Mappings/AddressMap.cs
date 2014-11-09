namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            this.ToTable("Address");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.FullAddress).HasColumnName("FullAddress").HasMaxLength(100).IsRequired();

            this.HasRequired(x => x.Town).WithMany(x => x.Addresses).HasForeignKey(x => x.TownId);
            this.HasRequired(x => x.User).WithMany(x => x.Addresses).HasForeignKey(x => x.UserId).WillCascadeOnDelete(false);
        }
    }
}