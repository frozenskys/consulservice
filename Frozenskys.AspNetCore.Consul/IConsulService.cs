namespace Frozenskys.AspNetCore.Consul
{
    public interface IConsulService
    {
        void RegisterService(string serviceName);
        void GetServiceEndpoints(string serviceName);
    }
}