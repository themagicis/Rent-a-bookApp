namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class BookRentMap : EntityTypeConfiguration<BookRent>
    {
        public BookRentMap()
        {
            this.ToTable("BookRent");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.DateStart).HasColumnName("DateStart").HasColumnType("datetime").IsRequired();
            this.Property(x => x.DateEnd).HasColumnName("DateEnd").HasColumnType("datetime").IsRequired();
            this.Property(x => x.State).HasColumnName("State").IsRequired();

            this.HasRequired(x => x.Owner).WithMany(x => x.RentsAsOwner).HasForeignKey(x => x.OwnerId);
            this.HasRequired(x => x.Receiver).WithMany(x => x.RentsAsReceiver).HasForeignKey(x => x.ReceiverId).WillCascadeOnDelete(false);
            this.HasRequired(x => x.Request).WithMany(x => x.BookRents).HasForeignKey(x => x.RequestId).WillCascadeOnDelete(false);
            this.HasOptional(x => x.FeedBackOwner).WithMany(x => x.OwnerRents).HasForeignKey(x => x.FeedbackOwnerId);
            this.HasOptional(x => x.FeedBackReceiver).WithMany(x => x.ReceiverRents).HasForeignKey(x => x.FeedbackReceiverId);
        }
    }
}