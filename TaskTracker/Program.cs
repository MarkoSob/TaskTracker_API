using Microsoft.EntityFrameworkCore;
using TaskTracker_DAL;
using TaskTracker_BL.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using TaskTracker_BL.Services;
using TaskTracker_BL.Options;
using TaskTracker_BL.Services.SmtpService;
using Serilog;
using TaskTracker.Middlewares;
using TaskTracker_BL.Services.HashService;
using TaskTracker_BL.Services.TokenService;
using TaskTracker_DAL.RolesHelper;
using TaskTracker_DAL.GenericRepository;
using TaskTracker_DAL.BasicGenericRepository;
using TaskTracker_BL.Services.QueryService;
using TaskTracker_BL.Services.GeneratorService;
using TaskTracker_BL.Services.AdminService;
using TaskTracker_BL.Services.TasksService;
using TaskTracker_BL.SignalR.MessageStorage;
using TaskTracker_BL.SignalR.ConnectionStorage;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Hangfire;
using Hangfire.SqlServer;
using TaskTracker_BL.SignalR;
using TaskTracker_BL.Services.QuartzService;
using TaskTracker_BL;
using TaskTracker_BL.Services.CachingService;
using StackExchange.Redis;
using TaskTracker_BL.Services.ImageService;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, _, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();

builder.Services.AddDbContext<EfDbContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var assemblies = new[]
{
    typeof(UserTaskProfile).Assembly,
    typeof(UserProfile).Assembly
};

builder.Services.AddAutoMapper(assemblies);
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<AuthOptions>(
    builder.Configuration.GetSection(nameof(AuthOptions)));
builder.Services.Configure<HashOptions>(
    builder.Configuration.GetSection(nameof(HashOptions)));
builder.Services.Configure<SmtpOptions>(
    builder.Configuration.GetSection(nameof(SmtpOptions)));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISmtpService, GoogleSmtpService>();
builder.Services.AddScoped<IQuartzService, QuartzService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IBasicGenericRepository<>), typeof(BasicGenericRepository<>));

builder.Services.AddSingleton<IHashService, HashService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IRolesHelper, RolesHelper>();
builder.Services.AddSingleton<IQueryService, QueryService>();
builder.Services.AddSingleton<IGeneratorService, GeneratorService>();
builder.Services.AddSingleton<IMessageSenderService, MessageSenderService>();
builder.Services.AddSingleton<IConnectionStorage, ConnectionStorage>();
builder.Services.AddSingleton<IMessageStorage, MessageStorage>();
builder.Services.AddSingleton<IUserIdProvider, AppUser>();

builder.Services.AddCors(x =>
    x.AddDefaultPolicy(x => x
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod()));

builder.Services.AddSignalR();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection(nameof(AuthOptions))["Key"]))

        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/chat")))
                { 
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMvc(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<ICachingService, CachingService>();
builder.Services.AddScoped<IConnectionMultiplexer>(
        x => ConnectionMultiplexer.Connect("localhost:5002"));

builder.Services.AddQuartz(x =>
{
    x.UseMicrosoftDependencyInjectionJobFactory();
    x.UsePersistentStore(q =>
    {
        q.UseSqlServer(sqlServer =>
        {
            sqlServer.ConnectionString = builder.Configuration.GetConnectionString("Default");
            sqlServer.TablePrefix = "QRTZ_";
        });
        q.UseJsonSerializer();
    });
});

builder.Services.AddQuartzServer(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.AddHangfire((configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        })));

builder.Services.AddHangfireServer();
var app = builder.Build();

app.UseCors();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<CustomErrorHandlingMiddleware>();
app.MapHub<SignalRChatHub>("/chat");
app.MapControllers();

app.Run();