using NET.Core.Library.Domain.DBModels;

namespace NET.Core.Library.Domain.Infrastructure.EFCore;

public static class TestData
{
    public static List<Author> GetAuthors()
    {
        List<Author> authors = new();
        authors.Add(CreateSingleAuthor(1, "Adam", string.Empty, "Freeman", "9999999999", "CA, US", "adam.freeman@test.com"));
        authors.Add(CreateSingleAuthor(2, "Venkat", string.Empty, "Krishnaswamy", "9999999998", "London, UK", "venkat.krishnaswamy@test.com"));
        authors.Add(CreateSingleAuthor(3, "Christopher", string.Empty, "Williams", "9999999997", "Perth, AU", "christopher.williams@test.com"));
        authors.Add(CreateSingleAuthor(4, "Sam", string.Empty, "Johanson", "9999999996", "WA, US", "sam.johanson@test.com"));
        authors.Add(CreateSingleAuthor(5, "Mithila", string.Empty, "Frost", "9999999995", "Sao Paolo, Brazil", "mithila.frost@test.com"));
        authors.Add(CreateSingleAuthor(6, "Muhammed", string.Empty, "Zubair", "9999999994", "KA, IN", "md.zubair@test.com"));
        authors.Add(CreateSingleAuthor(7, "Xi", "So", "Pang", "9999999993", "Shanghai, CN", "xi.so.pang@test.com"));
        authors.Add(CreateSingleAuthor(8, "Elizabeth", string.Empty, "McKinsley", "9999999992", "Berlin, DE", "elizabeth.mckinsley@test.com"));
        authors.Add(CreateSingleAuthor(9, "Ajay", "Kumar", "Sharma", "9999999991", "Del, IN", "ajay.kumar.sharma@test.com"));
        authors.Add(CreateSingleAuthor(10, "Punit", string.Empty, "Singh", "9999999990", "RJ, IN", "punit.singh@test.com"));
        authors.Add(CreateSingleAuthor(11, "George", string.Empty, "Fernandez", "9999999909", "Lisbon, Portugal", "geroge.fernandez@test.com"));
        authors.Add(CreateSingleAuthor(12, "Ben", "Andrews", "Forouzan", "9999999908", "Tel Aviv, Israel", "ben.andrews.forouzan@test.com"));
        authors.Add(CreateSingleAuthor(13, "Ravish", string.Empty, "Upadhay", "9999999907", "UP, IN", "ravish.upadhay@test.com"));
        authors.Add(CreateSingleAuthor(14, "Amir", string.Empty, "Nizami", "9999999906", "Montreal, Canada", "amir.nizami@test.com"));
        authors.Add(CreateSingleAuthor(15, "Montek", "Singh", "Ahluwalia", "9999999905", "PB, IN", "montel.singh.ahluwalia@test.com"));
        return authors;
    }

    private static Author CreateSingleAuthor(long id, string firstName, string middleName, string lastName, string phoneNumber, string address, string email) =>
        new()
        {
            Id = id,
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Address = address,
            Email = email
        };


}
