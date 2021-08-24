using Probotbor.ViewModels;
using Probotbor.Views;
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

namespace Probotbor
{
    /// <summary>
    /// Логика взаимодействия для InputPage.xaml
    /// </summary>
    public partial class InputPage : Window
    {
        public TaskWindowVM VM;
        public InputPage()
        {
            InitializeComponent();
            VM = new TaskWindowVM();
            this.DataContext = VM;
        }
        public InputPage(TaskWindowVM taskWindowVM)
        {
            InitializeComponent();
            if (taskWindowVM == null) VM = new TaskWindowVM();
            else VM = taskWindowVM;
            this.DataContext = VM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool founded = false;
            string passwordCheck = "";
            // Поиск в базе данных
            if (this.VM.Users != null) 
            {
                foreach (var item in this.VM.Users)
                {
                    if (item.Login == Login.Text)
                    {
                        founded = true;
                        passwordCheck = item.Password;
                        VM.CurrentUser = item;
                        break;
                    }
                }
            }

            if (founded && this.Password.Password == passwordCheck)
            {
                
                TaskWindow taskWindow = new TaskWindow(VM);                
                taskWindow.Show();
                this.Close();
            }
            else
            {
                Password.Foreground = Brushes.Red;
                Login.Foreground = Brushes.Red;
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.Password.Tag = this.Password.Password;
            Password.Foreground = Brushes.White;
            Login.Foreground = Brushes.White;
        }

        private void Login_MouseEnter(object sender, MouseEventArgs e)
        {
            Password.Foreground = Brushes.White;
            Login.Foreground = Brushes.White;
        }
            
        
    }
    
}
