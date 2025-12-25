using System.Security.Cryptography;

public static class RandomHelper
{
    private const string Base36 = "abcdefghijklmnopqrstuvwxyz0123456789";

    public static string RandomBase36(int length)
    {
        var bytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = Base36[bytes[i] % Base36.Length];
        }

        return new string(chars);
    }
}
