using Probotbor.Infrastructure;
using Probotbor.ViewModels;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Probotbor.Views
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        internal TaskWindowVM taskVM;
        public TaskWindow(TaskWindowVM vm)
        {
            taskVM = vm;
            this.DataContext = taskVM;
            InitializeComponent();
            
        }

        private void changeUserBtn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            InputPage inputPage = new InputPage(this.taskVM);           
            inputPage.Show();
            this.Close();
        }

        
    }
}
