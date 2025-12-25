using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public static class SlugHelper
{
    public static string Slugify(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // 1. lower-case
        var text = input.ToLowerInvariant();

        // 2. remove accents (unicode normalization)
        text = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in text)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        text = sb.ToString().Normalize(NormalizationForm.FormC);

        // 3. remove invalid chars
        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

        // 4. replace spaces with hyphen
        text = Regex.Replace(text, @"\s+", "-").Trim('-');

        // 5. collapse multiple hyphens
        text = Regex.Replace(text, @"-+", "-");

        return text;
    }
}
