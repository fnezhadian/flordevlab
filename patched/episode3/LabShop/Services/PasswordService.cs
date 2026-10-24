using Microsoft.AspNetCore.Identity;

namespace LabShop.Services;

public class PasswordService
{
    // PBKDF2 with a random salt for every password. Built into ASP.NET Core.
    // (We only borrow the hasher. We are not adding the whole Identity system.)
    private readonly PasswordHasher<string> _hasher = new();

    // Used when the username does not exist, so the work (and the timing) looks the same
    private readonly string _dummyHash;

    public PasswordService()
    {
        _dummyHash = _hasher.HashPassword("dummy", "not-a-real-password");
    }

    // Returns one string: settings + salt + hash. This is what goes in the PasswordHash column.
    public string Hash(string password) => _hasher.HashPassword("user", password);

    // storedHash is null when the username was not found
    public bool Verify(string? storedHash, string password)
    {
        var result = _hasher.VerifyHashedPassword("user", storedHash ?? _dummyHash, password);
        return storedHash != null && result != PasswordVerificationResult.Failed;
    }
}
