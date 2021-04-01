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

namespace SeaBattle {
    public partial class Connect : Form {
        IPEndPoint ip;

        public Connect() {
            InitializeComponent();
        }

        private void gamestart_button_Click(object sender, EventArgs e) {
            try {
                ip = IPEndPoint.Parse(IPTextBox.Text + ':' + PortTextBox.Text);
                GameStart newForm = new GameStart();
                newForm.Show();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
