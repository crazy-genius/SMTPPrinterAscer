using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

// Класс описывает базовые свойства любого сетевого устройства а так-же нобходимые процедуры обнаружения
namespace PrinterInfo
{
    class LanDevice
    {
        private string _error = string.Empty;
        private string _ip;
        private bool _isOnline;

        public string error
        {
            //set { _error = value; }
            get { return _error; }
        }

        public string Ip
        {
            set { _ip = value; }
            get { return _ip; }
        }

        public bool isOnline
        {
            set { _isOnline = value; }
            get { return _isOnline; }
        }
        private bool lanState()
        {            
            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply rep = p.Send(Ip);
            bool state = (rep.Status == System.Net.NetworkInformation.IPStatus.Success);
            return state;
        }

        private bool IsValidIp(string addr)
        {
            System.Net.IPAddress Ip;
            bool valid = !string.IsNullOrEmpty(addr) && System.Net.IPAddress.TryParse(addr, out Ip);
            return valid;
        }

        public LanDevice(string ip)
        {
            if (IsValidIp(ip))
            {
                Ip = ip;
                if (lanState())
                {
                    isOnline = true;

                }
                else _error = "Устройство не доступно";
            }
            else _error = "Введен не корректный IPv4 адрес";
            
        }

    }
}
