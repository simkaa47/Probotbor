using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Probotbor.Core;
using Probotbor.Core.Contracts.AccessControl;
using Probotbor.Core.Contracts.Dialog;
using Probotbor.Core.ViewModels;
using Probotbor.View.Dialogs;
using Probotbor.View.Dialogs.AccessControl;
using Probotbor.View.Pages;
using System.Windows;

namespace Probotbor.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public T? GetWindow<T>() where T : Window
        {
            T window = _host.Services.GetRequiredService<T>();
            return window;
        }


        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((conf, services) => 
                {                    
                    services.AddApplicationServices(conf.Configuration);
                    services.AddScoped<IAccessDialogService, UserDialogService>();
                    services.AddScoped<IQuestionDialog, AskDialog>();                    
                    services.AddTransient<AuthorizationWindow>((services) => new AuthorizationWindow()
                    {
                        DataContext = services.GetRequiredService<AccessViewModel>()
                    });
                    services.AddTransient<MainWindow>((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    });
                }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            MainWindow = _host.Services.GetRequiredService<AuthorizationWindow>();            
            MainWindow.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
