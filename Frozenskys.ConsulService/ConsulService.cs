using Consul;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System;
using System.Linq;

namespace ConsulService
{
    public class ConsulService : IConsulService, IDisposable
    {
        private ConsulClient _client;
        private IApplicationLifetime _applicationLifetime;
        private string _svcId;
        private int _port;
        private AgentServiceRegistration _serviceRegistration;

        public ConsulService(IServer server, IApplicationLifetime applicationLifetime)
        {
            var AddressFeature = server.Features.Get<IServerAddressesFeature>();
            _applicationLifetime = applicationLifetime;
            _port = new Uri(AddressFeature.Addresses.First()).Port;
            _client = new ConsulClient();
        }

        public void RegisterService(string serviceName = "")
        {
            _svcId = serviceName;
            _serviceRegistration = new AgentServiceRegistration() { Name = _svcId, ID = _svcId, Port = _port };
            _applicationLifetime.ApplicationStarted.Register(RegisterWithConsul);
            _applicationLifetime.ApplicationStopping.Register(DeRegisterWithConsul);
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
    }
}