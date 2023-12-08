using Probotbor.View.UserControls.Buttons;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Probotbor.View.Pages.EquipmentTab
{
    /// <summary>
    /// Interaction logic for EquipmentItemControl.xaml
    /// </summary>
    [ContentProperty("InnerContent")]
    public partial class EquipmentItemControl : UserControl
    {
        public EquipmentItemControl()
        {
            InitializeComponent();
        }

        #region Content
        public static readonly DependencyProperty InnerContentProperty =
        DependencyProperty.Register("InnerContent", typeof(object), typeof(EquipmentItemControl), new PropertyMetadata(null));

        public object InnerContent
        {
            get { return (object)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }
        #endregion

        #region ImagePath
        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(EquipmentItemControl), new PropertyMetadata(string.Empty));
        #endregion

        #region UnitNumber
        public string UnitNumber
        {
            get { return (string)GetValue(UnitNumberProperty); }
            set { SetValue(UnitNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnitNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitNumberProperty =
            DependencyProperty.Register("UnitNumber", typeof(string), typeof(EquipmentItemControl), new PropertyMetadata(string.Empty));
        #endregion

        #region UnitName
        public string UnitName
        {
            get { return (string)GetValue(UnitNameProperty); }
            set { SetValue(UnitNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnitName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitNameProperty =
            DependencyProperty.Register("UnitName", typeof(string), typeof(EquipmentItemControl), new PropertyMetadata(string.Empty));
        #endregion

        #region Status
        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(EquipmentItemControl), new PropertyMetadata(string.Empty));
        #endregion



        public ICommand OnClickCommand
        {
            get { return (ICommand)GetValue(OnClickCommandProperty); }
            set { SetValue(OnClickCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnClickCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnClickCommandProperty =
            DependencyProperty.Register("OnClickCommand", typeof(ICommand), typeof(EquipmentItemControl), new PropertyMetadata(null));



    }
}
