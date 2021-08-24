using Probotbor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.Views.AddClass
{
    class Kanistra: NotifyPropertyChanged
    {
        int num;
        public int Num
        {
            get => num;
            set => Set(ref num, value);
        }
        string id;
        public string Id
        {
            get => id;
            set => Set(ref id, value);
        }


    }
}
