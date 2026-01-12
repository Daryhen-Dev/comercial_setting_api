using setting.Dapper.Ip;
using setting.Dapper.Sucursal;
using setting.Dapper.User;

namespace comercial_setting_api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUsers(this IServiceCollection services)
        {
            services.AddScoped<IUserCad, UserCad>();
            services.AddScoped<IIpLoginCad, IpLoginCad>();
            services.AddScoped<ISucursal, Sucursal>();
            return services;
        }
    }
}
