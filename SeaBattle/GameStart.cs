using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SeaBattle
{
    public partial class GameStart : Form
    {
        StartField startfield = new StartField();

        public GameStart()
        {
            InitializeComponent();
        }

        private void GameStart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();

            startfield.draw(g);
        }
    }
}
