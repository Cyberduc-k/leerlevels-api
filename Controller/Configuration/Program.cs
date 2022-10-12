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
        services.AddDbContext<DataContext>(opts => opts.UseInMemoryDatabase("TestDatabase"));

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IForumRepository, ForumRepository>();
        services.AddTransient<IForumReplyRepository, ForumReplyRepository>();
        services.AddTransient<IMcqRepository, McqRepository>();
        services.AddTransient<IGroupRepository, GroupRepository>();
        services.AddTransient<ISetRepository, SetRepository>();
        services.AddTransient<ITargetRepository, TargetRepository>();

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IForumService, ForumService>();
        services.AddTransient<IMcqService, McqService>();
        services.AddTransient<IGroupService, GroupService>();
        services.AddTransient<ISetService, SetService>();
        services.AddTransient<ITargetService, TargetService>();

        services.AddAutoMapper(typeof(Program));
    })
    .ConfigureOpenApi()
    .Build();

host.Run();
