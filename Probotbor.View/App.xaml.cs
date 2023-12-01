using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Probotbor.Core;
using Probotbor.Core.Contracts.AccessControl;
using Probotbor.Core.Contracts.Communication;
using Probotbor.Core.Contracts.Dialog;
using Probotbor.Core.ViewModels;
using Probotbor.View.Dialogs;
using Probotbor.View.Dialogs.AccessControl;
using Probotbor.View.Dialogs.Errors;
using Probotbor.View.Pages;
using System;
using System.Windows;

namespace Probotbor.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public T GetService<T>() where T : notnull
        {
            T servise = _host.Services.GetRequiredService<T>();
            return servise;
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
                    services.AddScoped<IErrorDialog, AddErrorDialog>();

                }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            MainWindow = new AuthorizationWindow();
            MainWindow.DataContext = GetService<AccessViewModel>();
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
