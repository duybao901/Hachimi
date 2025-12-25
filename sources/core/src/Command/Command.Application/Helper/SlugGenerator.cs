public static class SlugGenerator
{
    public static string Generate(string title)
    {
        var baseSlug = SlugHelper.Slugify(title);
        var suffix = RandomHelper.RandomBase36(4);

        return $"{baseSlug}-{suffix}";
    }
}
