using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Probotbor.Core.Infrastructure.DataAccess;
using Probotbor.Core.Models.Communication;
using Probotbor.Core.ViewModels;

namespace Probotbor.Core
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<CommSettingsOptions>();
            services.AddDbContext<ApplicationContext>(options => { });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ParametersVm>();
            services.AddCommunication(configuration);


            return services; 
        }


        private static IServiceCollection AddCommunication(this IServiceCollection services, IConfiguration configuration)
        {
            var sett = new CommSettings();
            configuration.GetSection("Comm").Bind(sett);
            return services;
        }
    }
}
