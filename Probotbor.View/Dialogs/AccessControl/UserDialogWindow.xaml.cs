using CommunityToolkit.Mvvm.ComponentModel;
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
            User = user;
        }

        public User User { get; }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            User.Validate();
            if(!User.HasErrors)
            {
                DialogResult = true;
                Close();
            }            
        }

        void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
        }
    }
}
