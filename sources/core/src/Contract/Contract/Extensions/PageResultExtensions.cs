using Contract.Abstractions.Shared;

public static class PageResultExtensions
{
    public static PageResult<TDest> Cast<TSource, TDest>(this PageResult<TSource> source, Func<TSource, TDest> selector)
    {
        var mappedItems = source.Items
            .Select(selector)
            .ToList();

        return new PageResult<TDest>(
            mappedItems,
            source.PageIndex,
            source.PageSize,
            source.TotalCount
        );
    }
}
