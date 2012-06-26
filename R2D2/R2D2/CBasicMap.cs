using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace R2D2
{
    class CBasicMap
    {
        public TypesOfField[,] map;
        public int Height;
        public int Width;

        public List<CCoord> BonusList;
        public List<CCoord> MedkitList;

        public TypesOfField this[int x, int y]
        {
            get { return map[x, y]; }
            set
            {
                if (map[x, y] != value)
                {
                    if (value == TypesOfField.BONUS)
                        BonusList.Add(new CCoord(x, y));

                    if (value == TypesOfField.MEDKIT)
                        MedkitList.Add(new CCoord(x, y));

                    if ((map[x, y] == TypesOfField.BONUS) && (value != TypesOfField.BONUS))
                        BonusList.Remove(new CCoord(x, y));

                    if ((map[x, y] == TypesOfField.MEDKIT) && (value != TypesOfField.MEDKIT))
                        MedkitList.Remove(new CCoord(x, y));


                    map[x, y] = value;
                }

            }
        }
        public CBasicMap(int width, int height)
        {
            map = new TypesOfField[width, height];
            Width = width;
            Height = height;
            BonusList = new List<CCoord>();
            MedkitList = new List<CCoord>();
        }
    }
}
