using Consul;
using Frozenskys.AspNetCore.Consul;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsulServiceCollectionExtensions
    {
        public static void AddConsul(this IServiceCollection services)
        {
            services.AddSingleton<ConsulClient, ConsulClient>();
            services.AddSingleton<IConsulService, ConsulService>();
        }
    }
}