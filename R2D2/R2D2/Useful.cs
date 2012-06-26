using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Interfaces;

namespace R2D2
{
    static class Useful
    {  
        public static int Max(int a1, int a2)
        {
            int res = a1;
            if (a2 > res) res = a2;
            return res;
        }
        public static int Min(int a1, int a2)
        {
            int res = a1;
            if (a2 < res) res = a2;
            return res;
        }
        public static int Max(CCoord coord, int[,] arr)
        {
            int x = coord.x;
            int y = coord.y;
            int res = int.MaxValue;
            if (x > 0) res = Max(res, arr[x - 1, y]);
            if (y > 0) res = Max(res, arr[x, y - 1]);
            if (x < arr.GetUpperBound(1)) res = Max(res, arr[x + 1, y]);
            if (y < arr.GetUpperBound(2)) res = Max(res, arr[x, y + 1]);
            return res;
        }
        public static int MinButNotLowerThan(CCoord coord, int [,] map)
        {
            //заглушка
            return 1000;
        }

        public static CBasicMap ReadMapFromFile(string path)
        {

            BinaryReader r = new BinaryReader(File.OpenRead(path));
            int w = r.ReadInt32();
            int h = r.ReadInt32();
            CBasicMap map = new CBasicMap(w, h);
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    int k = r.ReadInt32();
                    switch (k)
                    {
                        case 0: map[i, j] = TypesOfField.NOTHING; break;
                        case 1: map[i, j] = TypesOfField.WALL; break;
                        case 2: map[i, j] = TypesOfField.BONUS; break;
                        case 3: map[i, j] = TypesOfField.MEDKIT; break;
                        case 8: map[i, j] = TypesOfField.ME; break;
                        case 9: map[i, j] = TypesOfField.HI; break;
                    }
                }
            r.Close();
            return map;
        }
    }
}
