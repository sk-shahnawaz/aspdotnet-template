namespace NET.Core.Library.Domain.Infrastructure.Contracts
{
    public interface IEntity
    {
        // Needed in Generic Repository Pattern
        public abstract long Id { get; set; }
    }
}