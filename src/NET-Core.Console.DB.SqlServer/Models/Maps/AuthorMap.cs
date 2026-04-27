using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NET.Core.Library.Domain.DBModels;
using NET.Core.Library.Domain.Infrastructure.EFCore;

namespace NET.Core.Console.DB.SqlServer.Models.Maps;

public class AuthorMap
{
    public AuthorMap(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Books);

        builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
        builder.Property(x => x.FirstName).HasColumnName("FirstName").HasMaxLength(50).IsRequired();
        builder.Property(x => x.MiddleName).HasColumnName("MiddleName").HasMaxLength(50);
        builder.Property(x => x.LastName).HasColumnName("LastName").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Address).HasColumnName("Address").HasMaxLength(255);
        builder.Property(x => x.Email).HasColumnName("Email").HasMaxLength(100).IsRequired();
        builder.Property(x => x.PhoneNumber).HasColumnName("PhoneNumber").HasMaxLength(15).IsRequired();

        foreach (Author author in TestData.GetAuthors())
        {
            builder.HasData(author);
        }
    }
}