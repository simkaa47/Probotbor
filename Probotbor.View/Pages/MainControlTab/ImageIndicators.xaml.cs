using CommunityToolkit.Mvvm.Input;
using Probotbor.View.Pages.EquipmentTab;
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

namespace Probotbor.View.Pages.MainControlTab
{
    /// <summary>
    /// Interaction logic for ImageIndicators.xaml
    /// </summary>
    public partial class ImageIndicators : UserControl
    {
        public ImageIndicators()
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


        public RelayCommand OpenProbotborWindow
        {

            get => new RelayCommand(() =>
            {
                OpenEquipWindow(new ProbotbornikWindow());

            });
        }

        public RelayCommand OpenPitatelWindow
        {
            get => new RelayCommand(() =>
            {
                OpenEquipWindow(new PitatelWindow());
            });
        }

        public RelayCommand OpenDrobilkaWindow
        {
            get => new RelayCommand(() =>
            {
                OpenEquipWindow(new DrobilkaWindow());

            });
        }

        public RelayCommand OpenDelitelWindow
        {
            get => new RelayCommand(() =>
            {
                OpenEquipWindow(new DelitelWindow());

            });
        }

        public RelayCommand OpenNakopitelWindow
        {
            get => new RelayCommand(() =>
            {
                OpenEquipWindow(new NakopitelWindow());

            });
        }

        public RelayCommand OpenReturnWindow
        {
            get => new RelayCommand(() =>
            {
                OpenEquipWindow(new ReturnWindow());

            });
        }


        private void OpenEquipWindow(Window window)
        {
            if (App.Current.MainWindow is not null)
            {
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                objBlur.Radius = 10;
                Application.Current.MainWindow.Effect = objBlur;
                window.DataContext = this.DataContext;
                window.ShowDialog();
                Application.Current.MainWindow.Effect = null;
            }
        }





    }
}
