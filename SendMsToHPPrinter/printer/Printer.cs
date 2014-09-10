using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnmpSharpNet;

namespace PrinterInfo
{
    interface IPrinter : ILanDevice
    {
        int PageCount { get; set; }
        int DuplexPageCount { get; set; }
        string WorkTime { get; set; }
        string CartridgeModel { get; set; }
        string CartridgeLevel { get; set; }
    }

    class Printer: IPrinter
    {
         // Все переменыые только в закрытом (локальном) доступе
            private string _name, _model, _serial, _ip, _mac; 
            private int _pageCount, _duplexPageCount;
            private string _workTime, _cartridgeModel, _cartridgeLevel;
            // Процедуры для доступа к свойствам объекта
            public string Name
            {
                set { _name = value; }
                get { return _name; }
            }
            public string Model
            {
                set { _model = value; }
                get { return _model; }
            }

            public string Serial
            {
                set { _serial = value; }
                get { return _serial; }
            }
            public string MAC
            {
                set { _mac = value; }
                get { return _mac; }
            }
            public string Ip
            {
                set { _ip = value; }
                get { return _ip; }
            }
            public int PageCount
            {
                set { _pageCount = value; }
                get { return _pageCount; }
            }
            public int DuplexPageCount
            {
                set { _duplexPageCount = value; }
                get { return _duplexPageCount; }
            }
            public string WorkTime
            {
                set { _workTime = value; }
                get { return _workTime; }
            }
            public string CartridgeModel
            {
                set { _cartridgeModel = value; }
                get { return _cartridgeModel; }
            }
            public string CartridgeLevel
            {
                set { _cartridgeLevel = value; }
                get { return _cartridgeLevel; }
            }

            private void setBySNMP()
            {
                Name = SNMPcol("1.3.6.1.2.1.1.5.0"); //Имя принтера
                Model = SNMPcol("1.3.6.1.2.1.25.3.2.1.3.1"); //Модель принтера 
                Serial = SNMPcol("1.3.6.1.2.1.43.5.1.1.17.1"); //Серийный номер 
                PageCount = Convert.ToInt32(SNMPcol("1.3.6.1.2.1.43.10.2.1.4.1.1")); //Количество напечатанных странниц принтером 
                DuplexPageCount = Convert.ToInt32(SNMPcol("1.3.6.1.4.1.11.2.3.9.4.2.1.4.1.2.22.0")); //Дуплекс принтера
                //Получение МАК адреса
                if (SNMPcol("1.3.6.1.2.1.2.2.1.6.1") != string.Empty)// Вариант 1
                {
                    MAC = SNMPcol("1.3.6.1.2.1.2.2.1.6.1"); // Если значение не пустое то это наш MAC адрес принтера
                }
                else if (SNMPcol("1.3.6.1.2.1.2.2.1.6.2") != string.Empty)// Вариант 2
                    {
                        MAC = SNMPcol("1.3.6.1.2.1.2.2.1.6.2"); // Если значение не пустое то это наш MAC адрес принтера
                    }
                    else if (SNMPcol("1.3.6.1.2.1.2.2.1.6.3") != string.Empty)// Вариант 3
                        {
                            MAC = SNMPcol("1.3.6.1.2.1.2.2.1.6.3"); // Если значение не пустое то это наш MAC адрес принтера
                        }
                //
                WorkTime = SNMPcol("1.3.6.1.2.1.1.3.0"); //Время работы принтера 
                CartridgeModel = SNMPcol("1.3.6.1.2.1.43.11.1.1.6.1.1"); //Модель принтера
                CartridgeLevel = SNMPcol("1.3.6.1.2.1.43.11.1.1.9.1.1"); //Текущий запас чернил
            }

            // Запрос данных по протоколу SNMP
            private string SNMPcol(string oIDQuery)
            {
                try
                {

                    OctetString community = new OctetString("public");
                    AgentParameters param = new AgentParameters(community);
                    param.Version = SnmpVersion.Ver1;
                    IpAddress agent = new IpAddress(Ip);//IP address

                    UdpTarget target = new UdpTarget((System.Net.IPAddress)agent, 161, 2000, 1);
                    Pdu pdu = new Pdu(PduType.Get);
                    pdu.VbList.Add(oIDQuery);
                    SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);
                    if (result != null)
                    {
                        if (result.Pdu.ErrorStatus != 0)
                        {
                            return null;
                        }
                        else
                        {
                            return result.Pdu.VbList[0].Value.ToString().Trim();
                        }
                    }
                    target.Close();
                    return null;
                }
                catch (Exception)
                { return null; }
            }
            public Printer(string ip)
            {
                if (ip != string.Empty)
                {
                    Ip = ip;
                    setBySNMP();
                }
            }
    }
}
