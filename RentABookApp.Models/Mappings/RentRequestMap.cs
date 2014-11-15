namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class RentRequestMap : EntityTypeConfiguration<RentRequest>
    {
        public RentRequestMap()
        {
            this.ToTable("RentRequest");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.DateRequested).HasColumnName("DateRequested").HasColumnType("datetime").IsRequired();
            this.Property(x => x.Message).HasColumnName("Message").HasMaxLength(500);
            this.Property(x => x.DateStart).HasColumnName("DateStart").HasColumnType("datetime").IsRequired();
            this.Property(x => x.DateEnd).HasColumnName("DateEnd").HasColumnType("datetime").IsRequired();
            this.Property(x => x.State).HasColumnName("State").IsRequired();

            this.HasRequired(x => x.Owner).WithMany(x => x.RequestsAsOwner).HasForeignKey(x => x.OwnerId).WillCascadeOnDelete(false);
            this.HasRequired(x => x.Requester).WithMany(x => x.RequestsAsRequester).HasForeignKey(x => x.RequesterId).WillCascadeOnDelete(false);
            this.HasRequired(x => x.Book).WithMany(x => x.Requests).HasForeignKey(x => x.BookId);
        }
    }
}