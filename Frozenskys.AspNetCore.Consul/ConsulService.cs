using Consul;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System;
using System.Linq;

namespace Frozenskys.AspNetCore.Consul
{
    public class ConsulService : IConsulService, IDisposable
    {
        private string _svcId;
        private int _port;
        private ConsulClient _client;
        private AgentServiceRegistration _serviceRegistration;
        private IApplicationLifetime _applicationLifetime;

        public ConsulService(IServer server, IApplicationLifetime applicationLifetime, ConsulClient client)
        {
            var AddressFeature = server.Features.Get<IServerAddressesFeature>();
            _port = new Uri(AddressFeature.Addresses.First()).Port;
            _applicationLifetime = applicationLifetime;
            _client = client;
        }

        public void RegisterService(string serviceName)
        {
            _svcId = serviceName;
            _serviceRegistration = new AgentServiceRegistration() { Name = _svcId, ID = _svcId, Port = _port };
            _applicationLifetime.ApplicationStarted.Register(RegisterWithConsul);
            _applicationLifetime.ApplicationStopping.Register(DeRegisterWithConsul);
        }

        public void RegisterService(string serviceName, string[] tags)
        {
            _svcId = serviceName;
            _serviceRegistration = new AgentServiceRegistration() { Name = _svcId, ID = _svcId, Port = _port, Tags = tags };
            _applicationLifetime.ApplicationStarted.Register(RegisterWithConsul);
            _applicationLifetime.ApplicationStopping.Register(DeRegisterWithConsul);
        }

        public void GetServiceEndpoint(string serviceName)
        {
            var serviceList = _client.Catalog.Service(serviceName).GetAwaiter().GetResult();
        }

        private void RegisterWithConsul()
        {
            var result = _client.Agent.ServiceRegister(_serviceRegistration).GetAwaiter().GetResult();
        }

        private void DeRegisterWithConsul()
        {
            var result = _client.Agent.ServiceDeregister(_svcId).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public CatalogService[] GetServiceEndpoints(string serviceName)
        {
            return _client.Catalog.Service(serviceName).GetAwaiter().GetResult().Response;
        }
    }
}