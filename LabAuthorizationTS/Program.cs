using LabAuthorizationTS.Identity.Interfaces;
using LabAuthorizationTS.Identity;
using LabAuthorizationTS.Models.Entities;
using LabAuthorizationTS.Services;
using LabAuthorizationTS.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LabAuthorizationTS.Config;
using LabAuthorizationTS.Repositories.Interfaces;
using LabAuthorizationTS.Repositories;
using LabAuthorizationTS.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using LabAuthorizationTS.Authorization;
using LabAuthorizationTS.Authorization.Handlers;
using LabAuthorizationTS.Authorization.Requirements;

var builder = WebApplication.CreateBuilder(args);

var settings = new AppSettings();
builder.Configuration.GetSection("AppSettings").Bind(settings);

// Add services to the container.

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthorizationHandler, ProductResourceOperationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("LabTsDb"));
builder.Services.AddScoped<Seeder>();

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AtLeast18", builder => builder.AddRequirements(new MinimumAgeRequirement(18)));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = settings.JwtIssuer,
        ValidAudience = settings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtKey))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LabAuthorizationTS", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
    });
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();

seeder.SeedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();