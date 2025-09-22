using BCrypt.Net;

namespace DriveOn.Infrastructure.Security;
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
public class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
