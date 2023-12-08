using System.Windows;

namespace Probotbor.View.Dialogs
{
    /// <summary>
    /// Interaction logic for AskDialogWindow.xaml
    /// </summary>
    public partial class AskDialogWindow : Window
    {
        public AskDialogWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
       
    }
}
