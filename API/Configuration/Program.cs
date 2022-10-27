using API.Middleware;
using Data;
using FluentValidation;
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
        string connectionString = Environment.GetEnvironmentVariable("LeerLevelsDatabase")!;

        services.AddDbContext<DataContext>(opts => opts.UseSqlServer(connectionString));

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IForumRepository, ForumRepository>();
        services.AddTransient<IForumReplyRepository, ForumReplyRepository>();
        services.AddTransient<IMcqRepository, McqRepository>();
        services.AddTransient<IGroupRepository, GroupRepository>();
        services.AddTransient<ISetRepository, SetRepository>();
        services.AddTransient<ITargetRepository, TargetRepository>();
        services.AddTransient<IBookmarkRepository, BookmarkRepository>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IForumService, ForumService>();
        services.AddScoped<IMcqService, McqService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<ISetService, SetService>();
        services.AddScoped<ITargetService, TargetService>();
        services.AddScoped<IBookmarkService, BookmarkService>();

        services.AddAutoMapper(typeof(Program));
        services.AddValidatorsFromAssemblyContaining<Program>();
    })
    .ConfigureOpenApi()
    .Build();

host.Run();