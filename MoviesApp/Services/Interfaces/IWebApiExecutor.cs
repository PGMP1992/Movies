namespace MoviesApp.Services.Interfaces
{
    public interface IWebApiExecutor
    {
        Task<T?> InvokeGet<T>(string relativeUrl);
    }
}