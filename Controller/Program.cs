using Data;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Repository.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureOpenApi()
    .ConfigureServices(services => {
        services.AddDbContext<TargetContext>(opts => opts.UseInMemoryDatabase("TestDatabase"));
        services.AddTransient<IUserRepository, UserRepository>();
    })
    .Build();

host.Run();
