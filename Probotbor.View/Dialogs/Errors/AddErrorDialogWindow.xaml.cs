using Mapster.Models;
using Probotbor.Core.Models.AccessControl;
using Probotbor.Core.Models.Communication;
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

namespace Probotbor.View.Dialogs.Errors
{
    /// <summary>
    /// Interaction logic for AddErrorDialogWindow.xaml
    /// </summary>
    public partial class AddErrorDialogWindow : Window
    {
        public AddErrorDialogWindow()
        {
            InitializeComponent();
        }

        public AddErrorDialogWindow(Parameter<bool> error)
        {
            this.DataContext = error;
            Error = error;
            InitializeComponent();
        }

        public Parameter<bool>? Error { get; }


        void Accept_Click(object sender, RoutedEventArgs e)
        {
            Error?.Validate();
            if (Error is not null && !Error.HasErrors)
            {
                DialogResult = true;
                Close();
            }
        }

        void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
