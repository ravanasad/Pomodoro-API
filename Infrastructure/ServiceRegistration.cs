using Application.Configurations;
using Application.Services;
using Application.Services.AuthService;
using Application.Services.PhotoService;
using Application.Services.TokenService;
using Infrastructure.Configurations;
using Infrastructure.Context;
using Infrastructure.Services;
using Infrastructure.Services.AuthService.Google;
using Infrastructure.Services.PhotoService;
using Infrastructure.Services.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddDataProtection();

        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequiredLength = 1;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireDigit = false;
            opt.Password.RequiredUniqueChars = 0;
            opt.User.RequireUniqueEmail = true;
        })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

        services.Configure<TokenSetting>(opt =>
        {
            opt.Issuer = configuration["TokenSettings:Issuer"]!;
            opt.Audience = configuration["TokenSettings:Audience"]!;
            opt.Secret = configuration["TokenSettings:Secret"]!;
            opt.Expiration = int.Parse(configuration["TokenSettings:Expiration"]!);
            opt.CokkieName = configuration["TokenSettings:CookieName"]!;
        });

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
            opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenSettings:Secret"]!)),
                    ValidateLifetime = true,
                    ValidIssuer = configuration["TokenSettings:Issuer"],
                    ValidAudience = configuration["TokenSettings:Audience"],
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = ClaimTypes.Name
                };
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[configuration["TokenSettings:CookieName"]!];
                        return Task.CompletedTask;
                    }
                };
            });


        services.AddHttpContextAccessor();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserTaskService, UserTaskService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILocalPhotoService, LocalPhotoService>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IAuthService, GoogleService>();

        return services;
    }
}
