using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationAPI.Extensions;
public static class UsernameGenerator
{
    public static string GenerateUsername(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return "user_" + RandomHex(8);

        // 1. Remove accents (Bảo → Bao)
        var normalized = fullName.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        var noDiacritics = sb.ToString().Normalize(NormalizationForm.FormC);

        // 2. Remove all characters except letters/digits/spaces
        var cleaned = new string(noDiacritics
            .Where(ch => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch))
            .ToArray());

        // 3. Convert to lowercase, replace spaces with _
        var baseName = cleaned.Trim().ToLower().Replace(" ", "_");

        // Nếu empty → fallback
        if (string.IsNullOrWhiteSpace(baseName))
            baseName = "user";

        // 4. Add random hex
        return $"{baseName}_{RandomHex(8)}";
    }

    private static string RandomHex(int bytes)
    {
        var buffer = RandomNumberGenerator.GetBytes(bytes);
        return Convert.ToHexString(buffer).ToLower();
    }
}
