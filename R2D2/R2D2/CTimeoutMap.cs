using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace R2D2
{
    class CTimeoutMap
    {
        static public int Stable = 1000000000; //клетка стабильно чистая
        static public int Wall = -111; //>это стена
        static public int Visible = -70; //клетка в текущем ходу обозревается
        static public int Dirty = -1; //клетка грязная(в ней может находится враг)
        // Значения map:
        // клетка чистая - значит там не может быть врага=)
        // клетка стабильно чистая - значит каждый ход, ее значение не будет уменьшатся на 1
        // 0 < value < 1000000000  => сколько ходов клетка будет чистой        
        // value = 0    =>сейчас клетка чистая

        public int[,] map;
        public int Height;
        public int Width;
        public int this[int x, int y]
        {
            get { return map[x, y]; }
            set { map[x, y] = value; }
        }
        public CTimeoutMap(int width, int height)
        {
            map = new int[width, height];
            Width = width;
            Height = height;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    map[x, y] = Dirty;
        }
    }
}
