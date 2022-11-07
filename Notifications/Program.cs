using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Repository.Interfaces;
using Service;
using Service.Interfaces;

IHost host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => {
        string connectionString = Environment.GetEnvironmentVariable("LeerLevelsDatabase")!;

        services.AddDbContext<DataContext>(opts => opts.UseSqlServer(connectionString));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
    })
    .Build();

host.Run();
