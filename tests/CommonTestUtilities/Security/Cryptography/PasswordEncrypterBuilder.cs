using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Security.Cryptography;
public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncrypter> _mock;

    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncrypter>();

        _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("$2a$10$SJo/AWjMHI5qM2g1rzPXPeyOrOnGnLH7u6Qako.9.FTUsLogeuITG");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password))
            _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);

        return this;
    }

    public IPasswordEncrypter Build() => _mock.Object;
}
