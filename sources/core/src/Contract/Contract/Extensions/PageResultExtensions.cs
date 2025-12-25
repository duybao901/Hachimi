using Contract.Abstractions.Shared;

public static class PageResultExtensions
{
    public static PageResult<TDest> Map<TSource, TDest>(
        this PageResult<TSource> source,
        Func<TSource, TDest> mapper
    )
    {
        var mappedItems = source.Items
            .Select(mapper)
            .ToList();

        return new PageResult<TDest>(
            mappedItems,
            source.PageIndex,
            source.PageSize,
            source.TotalCount
        );
    }
}
