using Bogus;
using TaskFlow.Domain.Entities;
using TaskFlow.Tests.CommonTestUtilities.Cryptography;

namespace TaskFlow.Tests.CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Build()
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();

        var user = new Faker<User>()
            .RuleFor(user => user.Id, _ => Guid.NewGuid())
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, (_, user) => passwordEncrypter.Encrypt(user.Password));
        
        return user;
    }
}