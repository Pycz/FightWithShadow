using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace R2D2
{    
    class CMap
    {
        private const int MaxVisibleDistance = 5;
        private int Height;
        private int Width;
        private CBasicMap BMap;
        private CLocationMap LMap;
        private CTimeoutMap TMap;
        private CCoord MyCoord = new CCoord();
        private int EndTimeout = -1;

        private List<CCoord> EnemyVisibleAreaList = new List<CCoord>();
        public List<CCoord> EnemyVisibleArea
        {
            get
            {
                RefreshEnVisArea();
                return EnemyVisibleAreaList;
            }
            set
            {
                EnemyVisibleAreaList = value;
                RefreshEnVisArea();
            }
        }
        /// <summary> 
        /// Обновление данных EnemyVisibleArea. 
        /// Когда к примеру MyCoord было установленно через Set метод, атоматическое обновление не срабатывает)
        ///</summary>
        ///
        private void RefreshEnVisArea()
        {
            if (EnemyVisibleAreaList.Count > 0)
                if (!EnemyVisibleAreaList[0].EqualsXY(MyCoord))
                {
                    EnemyVisibleAreaList = GetEnemyVisibleArea(MyCoord);
                    foreach (CCoord it in EnemyVisibleAreaList) TMap[it.x, it.y] = CTimeoutMap.Visible;
                }
        }
        private int[,] ListToArr(List<CCoord> list, int Width, int Height)
        {
            int[,] res = new int[Width, Height];
            foreach (CCoord it in list)
                res[it.x, it.y] = it.d;
            return res;
        }
        /// <summary> 
        /// Перезагрузка данных TMap, используя текущие данные LMap и EnemyVisibleArea
        ///</summary>
        private void RemapTimeouts()
        {
            //установливаем: либо стена либо необнуляемая клетка:
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (BMap[x, y] == TypesOfField.WALL) TMap[x, y] = CTimeoutMap.Wall; else TMap[x, y] = CTimeoutMap.Stable;

            //теперь отметим видимые:
            foreach (CCoord it in EnemyVisibleArea) TMap[it.x, it.y] = CTimeoutMap.Visible;

            //Так как на месте бонуса не может быть врага, то их таймаут "чистоты" равен нулю:
            foreach (CCoord it in BMap.BonusList) TMap[it.x, it.y] = 0;
            foreach (CCoord it in BMap.MedkitList) TMap[it.x, it.y] = 0;

            //обрабатываем все клетки в которых может быть враг:
            List<CCoord> CellsList = new List<CCoord>();
            foreach (CCoord it in LMap.List) CellsList.Add(new CCoord(it.x, it.y, CTimeoutMap.Dirty));
            ProcessTimeoutCellsList(LMap.List);
        }
        /// <summary>
        /// Вычисляет таймаут "чистоты" клетки в TMap заданной сoord и "волной" изменяет таймаут
        /// во всех граничащих с coord клетках.
        /// Вычисление таймаута в самой клетке происходит путем нахождения минимума среди 
        /// значений ее четырех соседних клеток и знаачением переданным в параметре coord.d
        /// Причем минимум не может быть меньше "-1", разве, что если такое значение содержит параметр coord.d!
        /// В принципе это должно быть очевидно так как клетка со значением  меньше "-1" либо стена, либо она в поле видимости.
        /// !!!Внимание, в данном методе не происходит автоматического удаления клеток  с LMap, так как предпологается, что они там заведомо не содержатся
        /// </summary>
        /// <param name="coord">Указывает координаты клетки(coord.x, coord.y), и некоторое начальное значение(coord.d) </param>
        private void ProcessTimeoutCell(CCoord coord)
        {
            TMap[coord.x, coord.y] = CTimeoutMap.Stable;

            Queue<CCoord> q = new Queue<CCoord>();

            int x = coord.x;
            int y = coord.y;
            int d = coord.d;

            {//если значение какой-то соседней клетки  меньшее d, то присваиваем d новое значение;
                if (x > 0) if (TMap[x - 1, y] >= -1) d = Useful.Min(TMap[x - 1, y], d);
                if (y > 0) if (TMap[x, y - 1] >= -1) d = Useful.Min(TMap[x, y - 1], d);
                if (x < TMap.Width) if (TMap[x + 1, y] >= -1) d = Useful.Min(TMap[x + 1, y], d);
                if (y < TMap.Height) if (TMap[x, y + 1] >= -1) d = Useful.Min(TMap[x, y + 1], d);
            }

            CCoord a = new CCoord(coord.x, coord.y, d);
            q.Enqueue(a);
            while (q.Count != 0)
            {
                a = q.Dequeue();
                if ((a.x >= 0) && (a.x < Width) && (a.y >= 0) && (a.y < Height))//если внутри карты
                    if (TMap[a.x, a.y] >= 0)//если клетка с  неотрицательным временем
                        if (TMap[a.x, a.y] > a.d)
                        {
                            TMap[a.x, a.y] = a.d;
                            //if (a.d >= 0) LMap.DellLocation(new CCoordEx(x, y));
                            q.Enqueue(new CCoord(a.x + 1, a.y, a.d + 1));
                            q.Enqueue(new CCoord(a.x - 1, a.y, a.d + 1));
                            q.Enqueue(new CCoord(a.x, a.y + 1, a.d + 1));
                            q.Enqueue(new CCoord(a.x, a.y - 1, a.d + 1));
                        }
            }
        }
        private void ProcessTimeoutCellsList(List<CCoord> CellsList)
        {
            foreach (CCoord it in CellsList) ProcessTimeoutCell(it);
        }

        //конструктор      
        public CMap(int width, int height)
        {
            Width = width;
            Height = height;
            BMap = new CBasicMap(width, height);
            LMap = new CLocationMap(width, height);
            TMap = new CTimeoutMap(width, height);
        }
        public CMap(int width, int height, int endTimeout)
        {
            Width = width;
            Height = height;
            BMap = new CBasicMap(width, height);
            LMap = new CLocationMap(width, height);
            TMap = new CTimeoutMap(width, height);
            EndTimeout = endTimeout;
        }

        public TypesOfField this[int x, int y]
        {
            get
            {
                return BMap[x, y];
            }
            set
            {
                if (value != BMap[x, y])
                    switch (value)
                    {
                        case TypesOfField.NOTHING:
                            if (BMap[x, y] == TypesOfField.BONUS)
                                if (!MyCoord.EqualsXY(x, y))
                                {//Bonus missed
                                    MissedObj.Set(x, y);
                                    MissedObj.setType(TypesOfField.BONUS);
                                }
                            if (BMap[x, y] == TypesOfField.MEDKIT)
                                if (!MyCoord.EqualsXY(x, y))
                                {//Medkit missed
                                    MissedObj.Set(x, y);
                                    MissedObj.setType(TypesOfField.MEDKIT);
                                }
                            break;
                        case TypesOfField.BONUS:
                            //New bonus on map
                            if (TMap[x, y] < 0) TMap[x, y] = 0;
                            break;
                        case TypesOfField.MEDKIT:
                            //New medkit on map
                            if (TMap[x, y] < 0) TMap[x, y] = 0;
                            break;
                        case TypesOfField.HI:
                            //Enemy Detected:
                            LMap.SetExactLoc(new CCoord(x, y));
                            RemapTimeouts();
                            break;
                        case TypesOfField.ME:
                            //My New Position                        
                            List<CCoord> EnVisList = GetEnemyVisibleArea(new CCoord(x, y));
                            int[,] EnVisArr = ListToArr(EnVisList, Width, Height);
                            List<CCoord> HiddenCells = new List<CCoord>();// клетки которые пропали с поля видимости
                            foreach (CCoord it in EnemyVisibleArea)
                                if (EnVisArr[it.x, it.y] != 0) HiddenCells.Add(new CCoord(x, y, CTimeoutMap.Stable));
                            EnemyVisibleArea = HiddenCells;
                            ProcessTimeoutCellsList(HiddenCells);
                            MyCoord.Set(x, y);
                            break;
                    }
                BMap[x, y] = value;
            }
        }
        /// <summary>
        ///      Выдает список координат клеток(а также расстояние), которые раcположены
        ///      рядом с  клетокой заданной coord, причем  расстояние до которых не более MaxDistance
        /// </summary>
        /// <param name="ConsiderWall">Учитывать ли стены при расчитывании растояния</param>
        /// <returns>"Список List с элементами координат клеток и растояний" </returns>     
        public List<CCoord> GetAliedCells(CCoord coord, int MaxDistance, bool ConsiderWall)
        {
            List<CCoord> List = new List<CCoord>();
            Queue<CCoord> q = new Queue<CCoord>();
            bool[,] isWasHere = new bool[BMap.Width, BMap.Height];

            CCoord a = new CCoord(coord.x, coord.y, 0);
            q.Enqueue(a);
            while (q.Count != 0)
            {
                a = q.Dequeue();
                if ((a.x >= 0) && (a.x < BMap.Width) && (a.y >= 0) && (a.y < BMap.Height))
                    if (!isWasHere[a.x, a.y] && (!ConsiderWall || (BMap[a.x, a.y] != TypesOfField.WALL)))
                    {
                        isWasHere[a.x, a.y] = true;
                        if (a.d <= MaxDistance)
                        {
                            List.Add(new CCoord(a.x, a.y, a.d));

                            q.Enqueue(new CCoord(a.x + 1, a.y, a.d + 1));
                            q.Enqueue(new CCoord(a.x - 1, a.y, a.d + 1));
                            q.Enqueue(new CCoord(a.x, a.y + 1, a.d + 1));
                            q.Enqueue(new CCoord(a.x, a.y - 1, a.d + 1));
                        }
                    }
            }
            //так как нужны только смежные то удаляем саму точку запроса
            List.RemoveAt(0);

            return List;
        }
        public List<CCoord> GetEnemyVisibleArea(CCoord coord)
        {
            List<CCoord> result = new List<CCoord>();
            //добавим саму точку запроса:
            result.Add(coord.Clone());

            List<CCoord> list1 = GetAliedCells(coord, MaxVisibleDistance, false);
            List<CCoord> list2 = GetAliedCells(coord, MaxVisibleDistance, true);

            int[,] m = ListToArr(list1, Width, Height);

            foreach (CCoord t in list2)
                if (m[t.x, t.y] == t.d)
                    result.Add(t);
            return result;
        }
        public List<CCoord> GetBonusList()
        {
            return BMap.BonusList;
        }
        public List<CCoord> GetMedkitList()
        {
            return BMap.MedkitList;
        }
        public List<CCoord> GetUpdatesList()
        {
            List<CCoord> result = new List<CCoord>();
            result.Concat(BMap.BonusList);
            result.Concat(BMap.MedkitList);
            return result;
        }


        public CCoord MissedObj = new CCoord();
        public CCoord EnemyLocList(int i)
        {
            return LMap.List[i];
        }
        public CCoord GetEnemyCoord()
        {
            return LMap.GetEnemyCoord();
        }

        public bool EnemyLoc(int x, int y)
        {
            return LMap[x, y];
        }
        public bool isMissedObj
        {
            get
            {
                return (MissedObj.getType() == TypesOfField.BONUS) || (MissedObj.getType() == TypesOfField.MEDKIT);
            }
        }
        public bool EnemyDetected()
        {
            return LMap.EnemyDetected();
        }

        public void SetExactLoc(CCoord coord)
        {
            if (LMap.EnemyDetected())
                if (LMap.GetEnemyCoord().EqualsXY(coord))
                    return;

            LMap.SetExactLoc(coord);
            RemapTimeouts();
        }
        public void ExpandLocArea()
        {
            int count = LMap.List.Count;
            for (int i = 0; i < count; i++)
            {
                CCoord it = LMap.List[i];
                if (it.x > 0) if (BMap[it.x - 1, it.y] == TypesOfField.NOTHING) LMap[it.x - 1, it.y] = true;
                if (it.y > 0) if (BMap[it.x, it.y - 1] == TypesOfField.NOTHING) LMap[it.x, it.y - 1] = true;
                if (it.x < BMap.Width - 1) if (BMap[it.x + 1, it.y] == TypesOfField.NOTHING) LMap[it.x + 1, it.y] = true;
                if (it.y < BMap.Height - 1) if (BMap[it.x, it.y + 1] == TypesOfField.NOTHING) LMap[it.x, it.y + 1] = true;
            }
        }
        public void CorrectEndTimeout(int endAfterTime)
        {
            int StepPassed = endAfterTime - EndTimeout;
            if ((EndTimeout >= 0) && (StepPassed > 0))
            {
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        if ((TMap[x, y] >= 0) && (TMap[x, y] != CTimeoutMap.Stable))
                        {
                            TMap[x, y] -= StepPassed;

                            if (TMap[x, y] < -1) TMap[x, y] = -1;

                            if ((BMap[x, y] == TypesOfField.BONUS) || (BMap[x, y] == TypesOfField.MEDKIT))
                                if (TMap[x, y] < 0) TMap[x, y] = 0;
                        }
                    }
            }
            for (int i = 1; i <= StepPassed; i++) ExpandLocArea();
            EndTimeout = endAfterTime;
        }
        public void ReadMap(IAPI api)
        {
            CCoord coord = new CCoord();
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    coord.Set(x, y);
                    this[x, y] = api.getTypeOfField(coord);
                }

            if (!MyCoord.EqualsXY(api.getCoordOfMe().X0, api.getCoordOfMe().Y0))
                throw new Exception("Ошибка в API. Несоответствие данных");

            CorrectEndTimeout(api.endAfterTime());
        }

        public int LocCount
        {
            get { return LMap.Count; }
        }
        public int GetDistance(CCoord coord1, CCoord coord2)
        {
            int distance = -1;
            Queue<CCoord> q = new Queue<CCoord>();

            bool[,] isWasHere = new bool[BMap.Width, BMap.Height];

            CCoord a = new CCoord(coord1.x, coord1.y, 0);
            q.Enqueue(a);
            while (q.Count != 0)
            {
                a = q.Dequeue();
                if ((a.x >= 0) && (a.x < BMap.Width) && (a.y >= 0) && (a.y < BMap.Height))
                    if (!isWasHere[a.x, a.y] && (BMap[a.x, a.y] != TypesOfField.WALL))
                    {
                        isWasHere[a.x, a.y] = true;
                        if ((a.x == coord2.x) && (a.y == coord2.y))
                        {
                            distance = a.d;
                        } else
                        {
                            q.Enqueue(new CCoord(a.x + 1, a.y, a.d + 1));
                            q.Enqueue(new CCoord(a.x - 1, a.y, a.d + 1));
                            q.Enqueue(new CCoord(a.x, a.y + 1, a.d + 1));
                            q.Enqueue(new CCoord(a.x, a.y - 1, a.d + 1));
                        }
                    }
            }
            return distance;
        }
        public int GetDistanceToEnemy()
        {
            int distance = 0;
            if (EnemyDetected())
            {
                distance = GetDistance(MyCoord, LMap.GetEnemyCoord());
            }
            return distance;
        }

    }
}
