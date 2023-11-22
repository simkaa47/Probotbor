using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Probotbor.Core;
using Probotbor.Core.Contracts.AccessControl;
using Probotbor.Core.Contracts.Dialog;
using Probotbor.Core.ViewModels;
using Probotbor.View.Dialogs;
using Probotbor.View.Dialogs.AccessControl;
using System.Windows;

namespace Probotbor.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((conf, services) => 
                {                    
                    services.AddApplicationServices(conf.Configuration);
                    services.AddScoped<IAccessDialogService, UserDialogService>();
                    services.AddScoped<IQuestionDialog, AskDialog>();                    
                    services.AddSingleton<MainWindow>((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    });
                }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            MainWindow = _host.Services.GetRequiredService<MainWindow>();            
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
