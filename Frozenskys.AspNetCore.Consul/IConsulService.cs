using Consul;

namespace Frozenskys.AspNetCore.Consul
{
    public interface IConsulService
    {
        void RegisterService(string serviceName);
        void RegisterService(string serviceName, string[] tags);
        CatalogService[] GetServiceEndpoints(string serviceName);
    }
}