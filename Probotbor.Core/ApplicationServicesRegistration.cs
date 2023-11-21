using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Probotbor.Core.Contracts.Communication;
using Probotbor.Core.Contracts.DataAccess;
using Probotbor.Core.Infrastructure.DataAccess;
using Probotbor.Core.Infrastructure.DataAccess.Repositories;
using Probotbor.Core.Models.Communication;
using Probotbor.Core.Models.Probotbor;
using Probotbor.Core.Services.Plc;
using Probotbor.Core.ViewModels;
using System.Reflection;

namespace Probotbor.Core
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<CommSettingsOptions>();
            services.ConfigureOptions<ProbotborSettingsOptions>();
            services.AddDbContext<ApplicationContext>(options => { });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<PlcVm>();
            services.AddSingleton<ParametersVm>();
            services.AddSingleton<PlcMainService>();
            services.AddCommunication(configuration);
            services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddLogging();
            services.AddMapper();

            return services; 
        }


        private static IServiceCollection AddCommunication(this IServiceCollection services, IConfiguration configuration)
        {
            var sett = new CommSettings();
            configuration.GetSection("Comm").Bind(sett);
            if(sett.Protocol == CommProtocol.Modbus)
                services.AddSingleton(typeof(ICommunicationService), typeof(ModbusCommunicationService));
            else
                services.AddSingleton(typeof(ICommunicationService), typeof(SiemensCommunicationService));
            return services;
        }


        public static IServiceCollection AddMapper(this IServiceCollection services)
        {

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());


            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
