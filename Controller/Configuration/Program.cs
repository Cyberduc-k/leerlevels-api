using API.Middleware;
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
    .ConfigureFunctionsWorkerDefaults(worker => {
        worker.UseNewtonsoftJson();
        worker.UseMiddleware<JwtMiddleware>();
        worker.UseMiddleware<ExceptionMiddleware>();
    })
    .ConfigureServices(services => {
        services.AddDbContext<UserContext>(opts => opts.UseInMemoryDatabase("TestDatabase"), ServiceLifetime.Singleton);
        services.AddDbContext<TargetContext>(opts => opts.UseInMemoryDatabase("TestDatabase"), ServiceLifetime.Singleton);
        services.AddDbContext<ForumContext>(opts => opts.UseInMemoryDatabase("TestDatabase"), ServiceLifetime.Singleton);

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IForumRepository, ForumRepository>();
        services.AddTransient<IForumReplyRepository, ForumReplyRepository>();
        services.AddTransient<IMcqRepository, McqRepository>();

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IForumService, ForumService>();
        services.AddTransient<IMcqService, McqService>();

        services.AddAutoMapper(typeof(Program));
    })
    .ConfigureOpenApi()
    .Build();

host.Run();
