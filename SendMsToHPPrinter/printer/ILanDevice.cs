using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinterInfo
{
    interface ILanDevice
    {
        //bool isOnline { get; set; }
        string Name { get; set; }
        string Model { get; set; }
        string Serial { get; set; }
        string Ip { get; set; }
        string MAC { get; set; }

    }
}
