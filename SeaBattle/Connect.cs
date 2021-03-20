using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace SeaBattle
{
    public partial class Connect : Form
    {
        public Connect()
        {
            InitializeComponent();
        }

        //IPAddress ip = IPAddress.Parse("127.0.0.1");
        IPEndPoint ip;
        
        private void button1_Click(object sender, EventArgs e)
        {
            int port = 0;
            try
            {
                port = Convert.ToInt32(textBox1.Text);
                //ip = IPAddress.Parse("127.0.0.1");
                if ((port >= 0) && (port < 65536))
                {
                    ip = IPEndPoint.Parse("127.0.0.1:" + port.ToString());
                    //Раскомментить для отображения порта
                    //MessageBox.Show(ip.ToString());

                    GameStart newForm = new GameStart();
                    newForm.Show();
                    
                    //Продумать как закрыть форму с коннектом, не закрывая все остальное
                }
                else
                    MessageBox.Show("Некорректное значение порта подключения");
            }
            catch//Обработка исключений – произошла ошибка
            {
                MessageBox.Show("Проверьте правильность ввода чисел!");
            }
        }
    }
}
