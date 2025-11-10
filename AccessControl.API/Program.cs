using AccessControl.API;
using AccessControl.API.Filters;
using AccessControl.API.Services.Authentication;
using AccessControl.API.Services.Authentication.JwtFeatures;
using AccessControl.API.Services.Infrastructure.LockUnlock;
using AccessControl.API.Services.Infrastructure.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
    options.Filters.Add<UserIdValidationFilter>();
}).AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AccessControl.API", Version = "v1", });
    c.CustomSchemaIds(type => type.ToString());
    c.DescribeAllParametersInCamelCase();
});

var dbConnectionString = builder.Configuration["ConnectionString"];
//database
builder.Services.AddSingleton(x => MartenFactory.CreateDocumentStore(dbConnectionString))
                .AddScoped(x => MartenFactory.CreateDocumentSession(dbConnectionString));

//JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings.GetSection("SecurityKey").Value))
    };
});

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IValidationService, ValidationService>();

builder.Services.AddScoped<ILockUnlockService, LockUnlockService>();

// HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//MassTransit Domain Event Dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

//TODO: RabbitMQ Configuration
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["Rabbit:Host"] ?? "localhost", builder.Configuration["Rabbit:VHost"] ?? "/", h =>
        {
            h.Username(builder.Configuration["Rabbit:User"] ?? "admin");
            h.Password(builder.Configuration["Rabbit:Pass"] ?? "admin");
        });
        cfg.UseInMemoryOutbox();
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

//TODO: Signal R
//app.MapPost("/doors/{doorId:guid}/unlock", async (Guid doorId, IPublishEndpoint bus) =>
//{
//    await bus.Publish(new UnlockDoor(doorId, Guid.NewGuid(), "api"));
//    return Results.Accepted();
//});

app.Run();
