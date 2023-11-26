using Probotbor.Core.Models.AccessControl;
using Probotbor.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Probotbor.View.Pages
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private async void LoginClick(object sender, MouseButtonEventArgs e)
        {
            var login = GetLoginFromResources();
            if (login != null)
            {
                if (App.Current is App app)
                {
                    var vm = app.GetService<AccessViewModel>();
                    if (vm != null) 
                    { 
                        await vm.LoginAsync(login);
                        if(vm.CurrentUser!=null)
                        {
                            var mainWindow = new MainWindow();
                            app.MainWindow = mainWindow;
                            mainWindow.DataContext = app.GetService<MainViewModel>();
                            mainWindow.Show();
                            this.Close();
                        }
                    }
                }
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var login  = GetLoginFromResources();
            if (login != null) {
                login.Password = pWord.Password;
            }
        }        

        private Login? GetLoginFromResources() 
        {
            if (this.Resources["LoginModel"] == null) return null;
            if (!(this.Resources["LoginModel"] is Login login)) return null;
            return login;
        }

        
    }
}
