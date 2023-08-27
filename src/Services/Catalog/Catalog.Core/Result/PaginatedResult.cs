namespace Catalog.Core.Result;

public class PaginatedResult<T> : Result
{
    public PaginatedResult(List<T> data)
        : base(true, string.Empty)
    {
        Data = data;
    }

    public List<T>? Data { get; set; }

    internal PaginatedResult(bool succeeded, List<T>? data = default, string? message = null, int count = 0, int page = 1, int pageSize = 10)
     : base(succeeded, message ?? string.Empty)
    {
        Data = data;
        CurrentPage = page;
        Succeeded = succeeded;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public static PaginatedResult<T> Failure(string message)
    {
        return new PaginatedResult<T>(false, default, message);
    }

    public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
    {
        return new PaginatedResult<T>(true, data, null, count, page, pageSize);
    }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;
}