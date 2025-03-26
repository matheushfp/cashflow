using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Security.Cryptography;
public class PasswordEncrypterBuilder
{
    public static IPasswordEncrypter Build()
    {
        var mock = new Mock<IPasswordEncrypter>();

        mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("$2a$10$SJo/AWjMHI5qM2g1rzPXPeyOrOnGnLH7u6Qako.9.FTUsLogeuITG");

        return mock.Object;
    }
}
