namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class BookHistoryMap : EntityTypeConfiguration<BookHistory>
    {
        public BookHistoryMap()
        {
            this.ToTable("BookHistory");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.DateChanged).HasColumnName("DateChanged").HasColumnType("datetime").IsRequired();
            this.Property(x => x.State).HasColumnName("State").IsRequired();

            this.HasRequired(x => x.Book).WithMany(x => x.History).HasForeignKey(x => x.BookId);
            this.HasRequired(x => x.User).WithMany(x => x.BookHistories).HasForeignKey(x => x.UserId).WillCascadeOnDelete(false);
        }
    }
}