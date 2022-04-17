namespace Shop.Api.HttpIn.Responses;

public class SuccessResponse<T> where T : class
{
    public SuccessResponse(T data) => Data = data;

    public T Data { get; }
}