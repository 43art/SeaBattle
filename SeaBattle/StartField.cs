using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SeaBattle
{
    class StartField
    {
        private Cell [,] Field;

        public StartField()
        {
            Field = new Cell[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Cell cell = new Cell();
                    cell.setX(i * 50);
                    cell.setY(j * 50);
                    Field[i,j] = cell;
                }
            }
        }

        public void draw(Graphics g)
        {
            foreach (Cell cell in Field)
            {
                cell.draw(g);
            }

        }

    }
}
