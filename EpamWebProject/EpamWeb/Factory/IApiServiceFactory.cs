using EpamWeb.Services;

namespace EpamWeb.Factory
{
    public interface IApiServiceFactory
    {
        IApiService Create(string baseUrl);
    }
}
