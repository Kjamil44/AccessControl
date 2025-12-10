using AccessControl.API;
using AccessControl.API.Exceptions;
using AccessControl.API.Filters;
using AccessControl.API.Services.Abstractions.Mediation;
using AccessControl.API.Services.Authentication;
using AccessControl.API.Services.Authentication.JwtFeatures;
using AccessControl.API.Services.Authorization;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using AccessControl.API.Services.Infrastructure.LockUnlock;
using AccessControl.API.Services.Infrastructure.Messaging;
using AccessControl.API.SignalR;
using AccessControl.Contracts;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Controllers
builder.Services.AddControllers(o =>
{
    o.ReturnHttpNotAcceptable = true;
    o.Filters.Add<UserIdValidationFilter>();
}).AddNewtonsoftJson();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AccessControl.API", Version = "v1" });
    c.CustomSchemaIds(t => t.ToString());
    c.DescribeAllParametersInCamelCase();
});

// Marten
var db = builder.Configuration["ConnectionString"];
builder.Services.AddSingleton(_ => MartenFactory.CreateDocumentStore(db))
                .AddScoped(_ => MartenFactory.CreateDocumentSession(db));

// JWT
var jwt = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecurityKey"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = ctx =>
        {
            var accessToken = ctx.Request.Query["access_token"];
            var path = ctx.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/live-events"))
                ctx.Token = accessToken;
            return Task.CompletedTask;
        }
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = ctx =>
        {
            ctx.HandleResponse();

            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ctx.Response.ContentType = "application/json";
            return ctx.Response.WriteAsJsonAsync(CoreError.CreateUnauthorized());
        },
        OnForbidden = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            ctx.Response.ContentType = "application/json";
            return ctx.Response.WriteAsJsonAsync(new CoreError("Forbidden.", 403));
        }
    };
});

// DI
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ILiveEventPublisher, LiveEventPublisher>();
builder.Services.AddScoped<ILockUnlockService, LockUnlockService>();
builder.Services.AddScoped<IAccessValidator, AccessValidator>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MartenSaveChangesBehavior<,>));
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

const string FrontendCors = "FrontendCors";
builder.Services.AddCors(opts =>
{
    opts.AddPolicy(FrontendCors, p => p
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["Rabbit:Host"] ?? "localhost",
                 builder.Configuration["Rabbit:VHost"] ?? "/",
                 h => { h.Username(builder.Configuration["Rabbit:User"] ?? "admin"); h.Password(builder.Configuration["Rabbit:Pass"] ?? "admin"); });

        cfg.UseInMemoryOutbox();
        cfg.ConfigureEndpoints(context);
    });
});

// Command → queue routing
EndpointConvention.Map<TriggerLock>(new Uri("queue:trigger-lock"));
EndpointConvention.Map<TriggerUnlock>(new Uri("queue:trigger-unlock"));

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Dev swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

// CORS must be between UseRouting and auth/authorization, BEFORE MapHub/MapControllers
app.UseCors(FrontendCors);

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<LiveEventsHub>("/hubs/live-events");

app.Run();
