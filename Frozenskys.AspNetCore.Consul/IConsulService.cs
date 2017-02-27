using Consul;

namespace Frozenskys.AspNetCore.Consul
{
    public interface IConsulService
    {
        void RegisterService(string serviceName);
        CatalogService[] GetServiceEndpoints(string serviceName);
    }
}