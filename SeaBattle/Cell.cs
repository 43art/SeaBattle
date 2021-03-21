using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SeaBattle
{
    class Cell
    {
        Rectangle rect;
        int state;

        public Cell()
        {
            rect.X = 50;
            rect.Y = 50;
            rect.Width = 47;
            rect.Height = 47;
            state = 0;
        }

        public int getX() { return rect.X; }
        public int getY() { return rect.Y; }
        public int getState() { return state; }
        public void setX(int xx) { rect.X = xx; }
        public void setY(int yy) { rect.Y = yy; }
        public void setState(int sstate) { state = sstate; }

        public void draw(Graphics g)
        {
            Pen pen = new Pen(Brushes.LightGray);
            pen.Width = 3.0F;
            g.DrawRectangle(pen, rect);
            g.FillRectangle(Brushes.White, rect);

            if (state == 0)
            if (state == 1)
            {
                pen = new Pen(Brushes.Gray);
                pen.Width = 3.0F;

                g.DrawLine(pen,     rect.X + 5,                 rect.Y + 5,     rect.X + rect.Width - 5,    rect.Y + rect.Height - 5);
                g.DrawLine(pen,     rect.X + rect.Width - 5,    rect.Y + 5,     rect.X + 5,                 rect.Y + rect.Height - 5);
            }
            if (state == 2)
            {
                pen = new Pen(Brushes.DarkRed);
                pen.Width = 3.0F;

                g.FillRectangle(Brushes.DarkRed, rect.X + 5, rect.Y + 5, rect.Width - 10, rect.Height - 10);
            }
            if (state == 3)
            {
                pen = new Pen(Brushes.DarkRed);
                pen.Width = 3.0F;

                g.FillRectangle(Brushes.DarkRed, rect.X + 5, rect.Y + 5, rect.Width - 10, rect.Height - 10);

                pen = new Pen(Brushes.Gray);
                pen.Width = 3.0F;

                g.DrawLine(pen, rect.X + 5, rect.Y + 5, rect.X + rect.Width - 5, rect.Y + rect.Height - 5);
                g.DrawLine(pen, rect.X + rect.Width - 5, rect.Y + 5, rect.X + 5, rect.Y + rect.Height - 5);
            }
        }
    }
}
