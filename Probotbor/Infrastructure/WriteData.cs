using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.Infrastructure
{
    class WriteData
    {
        public int DbNum { get; set; }
        public int StartByte { get; set; }
        public byte[] Buffer;
    }
}
