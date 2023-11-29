using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Probotbor.View.Pages.EquipmentTab
{
    /// <summary>
    /// Interaction logic for EquipmentTabControl.xaml
    /// </summary>
    public partial class EquipmentTabControl : UserControl
    {
        public EquipmentTabControl()
        {
            InitializeComponent();
        }

        private void ScrollToUp(object sender, RoutedEventArgs e)
        {
            ScrollBar.ScrollToTop();
        }

        private void ScrollToDown(object sender, RoutedEventArgs e)
        {
            ScrollBar.ScrollToBottom();
        }
    }
}
