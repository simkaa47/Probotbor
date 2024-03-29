﻿using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

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
