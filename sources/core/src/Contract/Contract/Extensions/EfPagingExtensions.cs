using Microsoft.EntityFrameworkCore;
using Contract.Abstractions.Shared;

public static class EfPagingExtensions
{
    public static async Task<PageResult<T>> ToPageResultAsync<T>(
        this IQueryable<T> query,
        int pageIndex,
        int pageSize)
    {
        pageIndex = pageIndex <= 0 ? PageResult<T>.DefaultPageIndex : pageIndex;
        pageSize = pageSize <= 0 ? PageResult<T>.DefaultPageSize :
            Math.Min(pageSize, PageResult<T>.UpperRangePageSize);

        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return new PageResult<T>(items, pageIndex, pageSize, totalCount);
    }
}
