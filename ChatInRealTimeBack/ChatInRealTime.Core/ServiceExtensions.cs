using ChatInRealTime.Core.Interfaces;
using ChatInRealTime.Core.Mapper;
using ChatInRealTime.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChatInRealTime.Core
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddMemoryCache();
            return services;
        }
       
    }
}
