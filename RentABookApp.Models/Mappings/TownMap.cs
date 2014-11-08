namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class TownMap : EntityTypeConfiguration<Town>
    {
        public TownMap()
        {
            this.ToTable("Town");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.Name).HasColumnName("FullAddress").HasMaxLength(100).IsRequired();
        }
    }
}