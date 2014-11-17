namespace RentABook.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using RentABook.Models.Poco;

    public class CommentMap : EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            this.ToTable("Comment");

            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.DateCreated).HasColumnName("DateCreated").HasColumnType("datetime").IsRequired();
            this.Property(x => x.Content).HasColumnName("Content").HasMaxLength(300);

            this.HasRequired(x => x.Author).WithMany(x => x.Comments).HasForeignKey(x => x.AuthorId);
            this.HasRequired(x => x.Book).WithMany(x => x.Comments).HasForeignKey(x => x.BookId).WillCascadeOnDelete(false);
        }
    }
}