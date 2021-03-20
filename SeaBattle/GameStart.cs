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
        private class Cell
        {
            Rectangle rect;
            int state;

            public Cell()
            {
                rect.X = 50;
                rect.Y = 50;
                rect.Width = 47;
                rect.Height = 47;
            }

            public int getX() { return rect.X; }
            public int getY() { return rect.Y; }
            public int getState() { return state; }
            public void setX(int xx) { rect.X = xx; }
            public void setY(int yy) { rect.Y = yy; }
            public void setState(int sstate) { state = sstate; }

            public void draw(Graphics g)
            {
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 3.0F;
                g.DrawRectangle(pen, rect);
                g.FillRectangle (Brushes.White, rect);
            }
        }

        public GameStart()
        {
            InitializeComponent();
        }

        private void GameStart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();

            Cell cell = new Cell();
            cell.draw(g);

            Cell cell1 = new Cell();
            cell1.setX(100);
            cell1.setY(100);
            cell1.draw(g);
        }
    }
}
