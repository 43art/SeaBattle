using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SeaBattle {
    class StartField {
        private Cell[,] Field;

        public StartField(int StartPosY, int StartPosX) {
            Field = new Cell[10, 10];
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    Cell cell = new Cell();
                    cell.setX(i * 50 + StartPosX);
                    cell.setY(j * 50 + StartPosY);
                    Field[i, j] = cell;
                }
            }
        }

        public void draw(Graphics g) {
            foreach (Cell cell in Field) {
                cell.draw(g);
            }
        }

        //Возвращает клетку по координате 
        public Cell getCellByCoordinate(int i, int j) {
            return Field[(i + 2) / 50, (j + 2) / 50];
        }

        //Возвращает клетку по индексам
        public Cell getCell(int i, int j) {
            if ((i < 10) && (j < 10))
                return Field[i, j];
            else return null;
        }

        //Проверка конца игры после выстрела по своему полю
        public bool checkFinishSelf() {
            foreach (Cell cell in Field) {
                if (cell.getState() == 2)
                    return false;
            }
            return true;
        }

        //Проверка конца игры после выстрела по полю соперника
        public bool checkFinishOpponent() {
            int count = 0;
            foreach (Cell cell in Field)
                if (cell.getState() == 5)
                    count++;
            return (count == 20);
        }
    }
}
