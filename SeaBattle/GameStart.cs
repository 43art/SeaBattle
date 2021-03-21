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
        int i = 0;
        int sds = 4;    //single-deck   ship
        int dds = 3;    //double-deck   ship
        int tds = 2;    //three-deck    ship
        int fds = 1;    //four-deck     ship

        public GameStart()
        {
            InitializeComponent();
            label1.Text = "Однопалубных кораблей осталось " + sds.ToString();
            label2.Text = "Двухпалубных кораблей осталось " + dds.ToString();
            label3.Text = "Трехпалубных кораблей осталось " + tds.ToString();
            label4.Text = "Четырехпалубных кораблей осталось " + fds.ToString();
        }

        private void GameStart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();
            startfield.draw(g);
        }

        private void GameStart_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.X < 498) && (e.Y < 498))
            {
                if (e.Button.ToString() == "Left")
                {
                    Cell cell = startfield.getCellByCoordinate(e.X, e.Y);
                    cell.setState(2);
                    Graphics g = CreateGraphics();
                    cell.draw(g);
                }

                if (e.Button.ToString() == "Right")
                {
                    Cell cell = startfield.getCellByCoordinate(e.X, e.Y);
                    cell.setState(0);
                    Graphics g = CreateGraphics();
                    cell.draw(g);
                }
            }
        }
    }
}
