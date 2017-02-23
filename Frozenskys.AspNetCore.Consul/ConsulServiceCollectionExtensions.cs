﻿using Frozenskys.AspNetCore.Consul;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsulServiceCollectionExtensions
    {
        public static void AddConsul(this IServiceCollection services)
        {
            services.AddSingleton<IConsulService, ConsulService>();
        }
    }
}