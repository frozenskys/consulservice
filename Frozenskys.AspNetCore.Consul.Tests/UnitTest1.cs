using Consul;
using Microsoft.AspNetCore.Hosting.Internal;
using System;
using System.Diagnostics;
using Xunit;

namespace Frozenskys.AspNetCore.Consul.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var _client = new ConsulClient();
            var serviceList = _client.Catalog.Service("consul").GetAwaiter().GetResult();
            Debugger.Break();
        }
    }
}
