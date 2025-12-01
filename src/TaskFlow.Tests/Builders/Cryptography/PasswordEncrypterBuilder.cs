using Moq;
using TaskFlow.Domain.Security.Cryptography;

namespace TaskFlow.Tests.Builders.Cryptography;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncrypter> _mock;

    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncrypter>();
        _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("!Password1");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password))
            _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);

        return this;
    }

    public IPasswordEncrypter Build()
    {
        return _mock.Object;
    }
}