namespace NET.Core.Library.Domain.Infrastructure.Contracts;

public interface IEntity<Tkey>
{
    // Needed in Generic Repository Pattern
    public Tkey Id { get; set; }
}