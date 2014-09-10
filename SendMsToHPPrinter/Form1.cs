using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace PrinterInfo
{
    public partial class Form1 : Form
    {
        public XmlDocument printDoc; // Переменная для хранения XML
        public Form1()
        {
            InitializeComponent();
            //
            printDoc = new XmlDocument(); // Инициализируем экземпляр класса
            printDoc.LoadXml(Properties.Resources.printerList); //Загружаем XML сохраненный в ресурсах проекта
            comboBox1.Items.Clear();  // Очищаем список
            //
            getFromXml(); // Запускаем процедуру наполнения списка из XML
            // Важно данная строка может выдать ошибку !!!!!!! добавить проверку на наличие 8 элемента
            comboBox1.SelectedIndex = 8; // Выбираем значение (192.168.176.58) 
        }
        
        // Процедура загрузки данных для списка
        public void getFromXml()
        {
            cleanBox(); // Очищаем поля ввода
            foreach (XmlNode node in printDoc.DocumentElement) // Проходимся по всем узлам XML
            {
                string str = null; // Переменная будет принимать текст
                foreach (XmlNode child in node.ChildNodes) // Пройдемся по детям каждого узла
                {   
                    if (child.LocalName.ToLower() == "ipv4") // Если имя совподает с заданным то
                    {
                        str =  child.InnerText; // Заполняем переменную
                    }
                    if (child.LocalName.ToLower() == "inventary") // Если имя совподает с заданным то
                    {
                        if (str != null) str = string.Format("{0} - {1}", str, child.InnerText); // Дополняем переменную
                    }
                }
                comboBox1.Items.Add(str); // Заносим результат в список
            }
        }
        // Процедура получения данных ранее сохраненых в XML
        public void selectFromXml()
        {
            int _id = comboBox1.SelectedIndex; // Получим индекс выброного значения ниспадающего списка
            XmlNodeList xmlList = printDoc.SelectNodes(string.Format("/DATA/PRINTER[@id='{0}']",_id)); // Создаем экземпляр класса и указываем путь до нод
            // Производим очистку полей ввода
            cleanBox(); 
            textBox1.Clear(); // Поле Инв. Номер 

            //Теперь пройдемся по всем нодам
            foreach (XmlNode node in xmlList)
            {                
                // Для каждого узла проверяем его детей на соответствие необходимым параметрам
                foreach (XmlNode child in node.ChildNodes)
                {
                    switch (child.LocalName.ToLower())
                    {
                        case "model":
                            //
                            textBox4.Text = child.InnerText;
                            break;
                        case "ipv4":
                            ipBox.Text = child.InnerText;
                            break;
                        case "mac":
                            textBox7.Text = child.InnerText;
                            break;
                        case "serial":
                            textBox5.Text = child.InnerText;
                            break;
                        case "inventary":
                            textBox1.Text = child.InnerText;
                            break;
                        default:
                            break;
                    }  
                }
            }
        }

        private void cleanBox()
        {
            // Просто очищащаем поля ввода
            //textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();   
        }
   
        private void button3_Click(object sender, EventArgs e)
        {
            cleanBox(); // Очистим поля перед вводом
            LanDevice device = new LanDevice(ipBox.Text);
            if (device.isOnline)
            {
                Printer printer = new Printer(device.Ip); // Создадим экземпляр класса
                textBox3.Text = Convert.ToString((printer.PageCount));
                textBox4.Text = printer.Model;
                textBox5.Text = printer.Serial;
                textBox6.Text = printer.WorkTime;
                textBox7.Text = printer.MAC;
                textBox8.Text = Convert.ToString(printer.DuplexPageCount);
            }
            else MessageBox.Show(device.error);
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            // Данная функция выполняется если пользователь сменил значение в списке.
            selectFromXml(); //Загружаем данные на форму из XML
        }
    }
}
