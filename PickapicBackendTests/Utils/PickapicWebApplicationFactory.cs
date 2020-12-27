using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PickapicBackend.Data;
using System.Linq;

namespace PickapicBackendTests.Utils
{
    public class PickapicWebApplicationFactory : WebApplicationFactory<PickapicBackend.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(SetInMemoryDatabase);
            base.ConfigureWebHost(builder);
        }

        private void SetInMemoryDatabase(IServiceCollection serviceDescriptors)
        {
            var registeredDataContextOptions = serviceDescriptors
                .Where(descriptor => descriptor.ServiceType == typeof(DbContextOptions<DataContext>))
                .ToArray();

            foreach (var dataContextService in registeredDataContextOptions)
            {
                serviceDescriptors.Remove(dataContextService);
            }

            serviceDescriptors.AddDbContext<DataContext>(opts => opts.UseInMemoryDatabase("PickapicDb"));
        }
    }
}
