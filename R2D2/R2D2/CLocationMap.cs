using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace R2D2
{
    class CLocationMap
    {
        public bool[,] map;
        public CLocationMap(int width, int height)
        {
            map = new bool[width, height];
        }
        public int Count { get { return List.Count; } }
        public bool this[int x, int y]
        {
            get { return map[x, y]; }
            set
            {
                if (value) AddLocation(new CCoord(x, y)); else DellLocation(new CCoord(x, y));
            }
        }

        //данные, о том в каких клетках возможно находиться DarkBot
        public List<CCoord> List = new List<CCoord>();

        private void AddLocation(CCoord cell)
        {
            if (!map[cell.x, cell.y])
            {
                map[cell.x, cell.y] = true;
                List.Add(cell);
            }
        }
        private void DellLocation(CCoord cell)
        {
            if (map[cell.x, cell.y])
            {
                map[cell.x, cell.y] = false;
                List.Remove(cell);
            }
        }
        private void ClearLocation()
        {
            foreach (CCoord it in List) map[it.x, it.y] = false;
            List.Clear();
        }
        //устанавливает конкретное местоположение плохого бота
        public void SetExactLoc(CCoord coord)
        {
            ClearLocation();
            AddLocation(coord.Clone());
        }
        public bool EnemyDetected()
        {
            return List.Count == 1;
        }
        public CCoord GetEnemyCoord()
        {
            if (EnemyDetected())
                return List[0];
            else throw new Exception("Попытка получить координаты врага когда он необнаружен");
        }
    }

}
