namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            this.ToTable("Category");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.Name).HasColumnName("Name").HasMaxLength(500).IsRequired();
        }
    }
}