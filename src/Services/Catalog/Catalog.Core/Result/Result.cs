namespace Catalog.Core.Result;

public class Result
{
    protected internal Result(bool isSuccess, string msg)
    {

        Succeeded = isSuccess;
        Message = msg;
    }

    public bool Succeeded { get; init; }
    public bool Failed => !Succeeded;

    public string Message { get; }

    public static Result Success() => new(true, string.Empty);

    public static Result Failure(string error) => new(false, error);
}

public class Result<T>
{
    private Result(T data, bool isSuccess, string msg)
    {
        Data = data;
        Succeeded = isSuccess;
        Message = msg;
    }

    public bool Succeeded { get; }

    public bool Failed => !Succeeded;

    public string Message { get; }

    public T? Data { get; }


    public static Result<T> Success(T value) => new (value, true, string.Empty);


    public static Result<T> Failure(string error) => new(default, false, error);

    public static Result<T> Create(T? data)
    {
        if (data is not null)
            return Success(data);

        return Failure("");
    }
}