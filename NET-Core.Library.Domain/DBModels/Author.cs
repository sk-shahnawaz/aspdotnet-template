using System.Collections.Generic;

using NET.Core.Library.Domain.Infrastructure.Contracts;

namespace NET.Core.Library.Domain.DBModels
{
    public class Author : IEntity<long>
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<Book> Books { get; set; }
    }
}