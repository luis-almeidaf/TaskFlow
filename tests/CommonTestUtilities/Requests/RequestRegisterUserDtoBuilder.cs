using Bogus;
using TaskFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUserDtoBuilder
{
    public static RequestRegisterUserDto Build()
    {
        return new Faker<RequestRegisterUserDto>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}