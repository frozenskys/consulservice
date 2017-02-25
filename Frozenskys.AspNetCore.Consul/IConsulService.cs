using Consul;

namespace Frozenskys.AspNetCore.Consul
{
    public interface IConsulService
    {
        void RegisterService(string serviceName);
        QueryResult GetServiceEndpoints(string serviceName);
    }
}