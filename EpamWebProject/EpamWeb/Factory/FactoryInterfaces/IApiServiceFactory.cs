using EpamWeb.Services.ServiceInterfaces;

namespace EpamWeb.Factory.FactoryInterfaces
{
    public interface IApiServiceFactory
    {
        IApiService Create(string baseUrl);
    }
}
