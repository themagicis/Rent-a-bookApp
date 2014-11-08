namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class BookMap : EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            this.ToTable("Book");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.Author).HasColumnName("Author").HasMaxLength(100).IsRequired();
            this.Property(x => x.Title).HasColumnName("Title").HasMaxLength(50).IsRequired();
            this.Property(x => x.ShortDescription).HasColumnName("ShortDescription").HasMaxLength(200).IsOptional();
            this.Property(x => x.Condition).HasColumnName("Condition").IsRequired();

            this.HasRequired(x => x.Category).WithMany(x => x.Books).HasForeignKey(x => x.CategoryId);
            this.HasRequired(x => x.Owner).WithMany(x => x.Books).HasForeignKey(x => x.OwnerId);
            this.HasRequired(x => x.Address).WithMany(x => x.Books).HasForeignKey(x => x.AddressId);
        }
    }
}
