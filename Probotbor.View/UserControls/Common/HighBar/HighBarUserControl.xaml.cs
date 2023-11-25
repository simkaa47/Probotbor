using Probotbor.View.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Probotbor.View.UserControls.Common.HighBar
{
    /// <summary>
    /// Interaction logic for HighBarUserControl.xaml
    /// </summary>
    public partial class HighBarUserControl : UserControl
    {
        public HighBarUserControl()
        {
            InitializeComponent();
        }

        public void GoToAuthorisationWindow()
        {
            if (App.Current is App app)
            {
                var authWindow = app.GetWindow<AuthorizationWindow>();
                var currWindow = app.MainWindow;
                app.MainWindow = authWindow;
                authWindow?.Show();
                currWindow?.Close();   
            }
        }

    }
}
