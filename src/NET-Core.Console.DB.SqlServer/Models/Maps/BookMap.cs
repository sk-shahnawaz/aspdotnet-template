using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NET.Core.Library.Domain.DBModels;

namespace NET.Core.Console.DB.SqlServer.Models.Maps;

public class BookMap
{
    public BookMap(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Isbn).IsUnique();
        builder.HasOne(x => x.Author);

        builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
        builder.Property(x => x.Isbn).HasColumnName("Isbn").HasMaxLength(13).IsRequired();
        builder.Property(x => x.Title).HasColumnName("Title").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Summary).HasColumnName("Summary").HasMaxLength(255);
        builder.Property(x => x.PublishedOn).HasColumnName("PublishedOn").HasMaxLength(50);
        builder.Property(x => x.AuthorId).HasColumnName("AuthorId").IsRequired(true);

        //builder.HasData();
    }
}
