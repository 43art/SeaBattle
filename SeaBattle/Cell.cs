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
        }

        public int getX() { return rect.X; }
        public int getY() { return rect.Y; }
        public int getState() { return state; }
        public void setX(int xx) { rect.X = xx; }
        public void setY(int yy) { rect.Y = yy; }
        public void setState(int sstate) { state = sstate; }

        public void draw(Graphics g)
        {
            Pen pen = new Pen(Brushes.Gray);
            pen.Width = 3.0F;
            g.DrawRectangle(pen, rect);
            g.FillRectangle(Brushes.White, rect);
        }
    }
}
