using Probotbor.Core.Models.AccessControl;
using System.Windows;

namespace Probotbor.View.Dialogs.AccessControl
{
    /// <summary>
    /// Interaction logic for UserDialogWindow.xaml
    /// </summary>
    public partial class UserDialogWindow : Window
    {
        public UserDialogWindow()
        {
            InitializeComponent();
        }

        public UserDialogWindow(User user)
        {
            InitializeComponent();
            this.DataContext = user;
        }        

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
