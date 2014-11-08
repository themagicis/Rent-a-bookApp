namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class FeedbackMap : EntityTypeConfiguration<Feedback>
    {
        public FeedbackMap()
        {
            this.ToTable("Feedback");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.FeedbackScore).HasColumnName("FeedbackScore").IsRequired();
            this.Property(x => x.Message).HasColumnName("Message").IsOptional();
        }
    }
}