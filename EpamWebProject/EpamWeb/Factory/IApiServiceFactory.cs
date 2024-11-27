using EpamWeb.Services.ServiceInterfaces;

namespace EpamWeb.Factory
{
    public interface IApiServiceFactory
    {
        IApiService Create(string baseUrl);
    }
}
