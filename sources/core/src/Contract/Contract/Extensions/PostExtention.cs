namespace Contract.Extensions;
public static class PostExtention
{
    public static string GetSortPostPropertyName(string? sortColumn)
    {
        return sortColumn?.Trim().ToLower() switch
        {
            "title" => "Title",
            "content" => "Content",
            _ => "Title"
        };
    }
}
