using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NET.Core.Library.Domain.DBModels;

namespace NET.Core.Console.DB.PostgreSQL.Models.Maps;

public class BookMap
{
    public BookMap(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Isbn).IsUnique();
        builder.HasOne(x => x.Author);

        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.Isbn).HasColumnName("isbn").HasMaxLength(13).IsRequired();
        builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Summary).HasColumnName("summary").HasMaxLength(255);
        builder.Property(x => x.PublishedOn).HasColumnName("published_on").HasMaxLength(50);
        builder.Property(x => x.AuthorId).HasColumnName("author_id").IsRequired(true);

        //builder.HasData();
    }
}
