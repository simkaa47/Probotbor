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

        

        public  void CloseAuthorisationWindow()
        {
            if(App.Current is App app)
            {
                var mainWindow = app.GetWindow<MainWindow>();
                app.MainWindow = mainWindow;
                mainWindow?.Show();
                this.Close();                
            }
        }
    }
}
