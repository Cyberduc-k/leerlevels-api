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

        services.AddDbContext<DataContext>(opts => {
            opts.UseSqlServer(connectionString);
            opts.EnableSensitiveDataLogging();
        });

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IForumRepository, ForumRepository>();
        services.AddTransient<IForumReplyRepository, ForumReplyRepository>();
        services.AddTransient<IMcqRepository, McqRepository>();
        services.AddTransient<IAnswerOptionRepository, AnswerOptionRepository>();
        services.AddTransient<IGroupRepository, GroupRepository>();
        services.AddTransient<ISetRepository, SetRepository>();
        services.AddTransient<ITargetRepository, TargetRepository>();
        services.AddTransient<ITargetLinkRepository, TargetLinkRepository>();
        services.AddTransient<IBookmarkRepository, BookmarkRepository>();
        services.AddTransient<ITargetProgressRepository, TargetProgressRepository>();
        services.AddTransient<IMcqProgressRepository, McqProgressRepository>();
        services.AddTransient<IGivenAnswerOptionRepository, GivenAnswerOptionRepository>();

        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IForumService, ForumService>();
        services.AddSingleton<IMcqService, McqService>();
        services.AddSingleton<IGroupService, GroupService>();
        services.AddSingleton<ISetService, SetService>();
        services.AddSingleton<ITargetService, TargetService>();
        services.AddSingleton<ITargetLinkService, TargetLinkService>();
        services.AddSingleton<IBookmarkService, BookmarkService>();
        services.AddSingleton<IProgressService, ProgressService>();

        services.AddSingleton<INotificationService, NotificationService>();

        services.AddAutoMapper(typeof(Program));
        services.AddValidatorsFromAssemblyContaining<Program>();
    })
    .ConfigureOpenApi()
    .Build();

host.Run();