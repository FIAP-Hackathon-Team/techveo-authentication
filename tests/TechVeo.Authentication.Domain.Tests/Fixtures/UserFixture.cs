using Bogus;
using TechVeo.Authentication.Domain.Entities;
using TechVeo.Authentication.Domain.ValueObjects;

namespace TechVeo.Authentication.Domain.Tests.Fixtures
{
    public class UserFixture
    {
        private readonly Faker _faker;

        public UserFixture()
        {
            _faker = new Faker("pt_BR");
        }

        public User CreateValidUser(string? username = null, string? role = null)
        {
            var firstName = _faker.Name.FirstName();
            var lastName = _faker.Name.LastName();
            var fullName = $"{firstName} {lastName}";
            var email = _faker.Internet.Email(firstName, lastName);

            return new User(
                new Name(fullName),
                username ?? _faker.Internet.UserName(firstName, lastName),
                role ?? "User",
                new Email(email));
        }
    }
}
