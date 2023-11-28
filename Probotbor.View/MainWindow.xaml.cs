using Probotbor.Core.Models.AccessControl;
using Probotbor.Core.Services.Plc;
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

namespace Probotbor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public override void EndInit()
        {
            int num = 1;
            if (App.Current is App app)
            {
                var serv = app.GetService<PlcMainService>();
                if (serv != null) 
                {
                    var settings = serv.ProbotborSettings;
                    SetUnitNumber(app, "PitatelNum", ref num, settings.PitatelExist);
                    SetUnitNumber(app, "DrobilkaNumNum", ref num, settings.DrobilkaExist);
                    SetUnitNumber(app, "DelitelNum", ref num, settings.SecondaryExist);
                    SetUnitNumber(app, "DryNum", ref num, settings.DryUnitExist);
                    SetUnitNumber(app, "IstiratelNum", ref num, settings.IstiratelExist);
                    SetUnitNumber(app, "NakopitelNum", ref num, settings.NakopitelExist);
                    SetUnitNumber(app, "ReturnNum", ref num, settings.ReturnExist);
                }
            }
                base.EndInit();
        }


        void SetUnitNumber(App app, string resourceKey, ref int num, bool needToIncrement)
        {
            if (!needToIncrement) return;
            if (app.Resources[resourceKey] != null)
            {
                if (app.Resources[resourceKey] is string)
                {
                    num++;
                    app.Resources[resourceKey] = num.ToString();
                }

            }
        }
    }
}
