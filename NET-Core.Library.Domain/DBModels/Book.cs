using System;

using NET.Core.Library.Domain.Infrastructure.Contracts;

namespace NET.Core.Library.Domain.DBModels
{
    public class Book : IEntity<long>
    {
        public long Id { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime PublishedOn { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}