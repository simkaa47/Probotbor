using Probotbor.Core.ViewModels;
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

namespace Probotbor.View.UserControls.Parameters
{
    /// <summary>
    /// Interaction logic for NumericParameter.xaml
    /// </summary>
    public partial class NumericParameter : ParameterCommon
    {
        public NumericParameter()
        {
            InitializeComponent();
        }

        public override void EndInit()
        {
            base.EndInit();
            if(App.Current is App app)
            {
                var plcVm = app.GetService<PlcVm>();
                if(plcVm != null) 
                {
                    Input.InputBindings[0].Command = plcVm.WriteParameterCommand;
                }
            }
            
        }

    }
}
