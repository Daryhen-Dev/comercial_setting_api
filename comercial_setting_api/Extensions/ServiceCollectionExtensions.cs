using setting.Dapper.User;

namespace comercial_setting_api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUsers(this IServiceCollection services)
        {
            services.AddScoped<IUserCad, UserCad>();
            return services;
        }

    }
}
