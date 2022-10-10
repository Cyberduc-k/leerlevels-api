using Controller.Security;
using Data;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Repository.Interfaces;
using Service;
using Service.Interfaces;

IHost host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(Worker => Worker.UseNewtonsoftJson().UseMiddleware<JwtMiddleware>())
    .ConfigureOpenApi()
    .ConfigureServices(services => {
        services.AddDbContext<TargetContext>(opts => opts.UseInMemoryDatabase("TestDatabase"));
        services.AddDbContext<ForumContext>(opts => opts.UseInMemoryDatabase("TestDatabase"));

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IForumRepository, ForumRepository>();

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IForumService, ForumService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IMcqRepository, McqRepository>();
        services.AddTransient<IMcqService, McqService>();
    })
    .Build();

host.Run();
