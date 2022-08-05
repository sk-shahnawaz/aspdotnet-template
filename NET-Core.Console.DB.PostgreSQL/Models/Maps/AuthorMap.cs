using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NET.Core.Library.Domain.DBModels;
using NET.Core.Library.Domain.Infrastructure.EFCore;

namespace NET.Core.Console.DB.PostgreSQL.Models.Maps;

public class AuthorMap
{
    public AuthorMap(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("authors");
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Books);

        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
        builder.Property(x => x.MiddleName).HasColumnName("middle_name").HasMaxLength(50);
        builder.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Address).HasColumnName("address").HasMaxLength(255);
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
        builder.Property(x => x.PhoneNumber).HasColumnName("phone_number").HasMaxLength(15).IsRequired();

        foreach (Author author in TestData.GetAuthors())
        {
            builder.HasData(author);
        }
    }
}