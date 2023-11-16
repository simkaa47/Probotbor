using Microsoft.Extensions.DependencyInjection;
using Probotbor.Core.ViewModels;

namespace Probotbor.Core
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            return services;
        }
    }
}
