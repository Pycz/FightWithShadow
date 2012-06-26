using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace R2D2
{
    public class CBot : IBot
    {
        CState st;
        IAPI API;
        Random Rnd;

        public void Initialize(IAPI Api)
        {
            API = Api;
            st = new CState(API);
        }
        private bool CanKillDarkBotOnThisStep()
        {
            bool res = false;
            if (st.EnHealth <= 50)
                if (st.isMyWeaponReady) res = true;
            return res;
        }
        private  int[,] ListToArr(List<CCoord> list)
        {
            int[,] res = new int[st.MapWidth, st.MapHeight];
            foreach (CCoord it in list)
                res[it.x, it.y] = it.d;
            return res;
        }
        List<CCoord> GetNearUpdates(CCoord coord,int MaxDistance)
        {
            List<CCoord> list = st.Map.GetAliedCells(coord, MaxDistance, true);
            
            List<CCoord> result = new List<CCoord>();

            foreach (CCoord it in list)
                if ((st.Map[it.x, it.y] == TypesOfField.BONUS) || (st.Map[it.x, it.y] == TypesOfField.MEDKIT))
                    result.Add(it);

            result.Sort();

            return result;
        }
        private CCoord FindUpdatesConcentrationArea(int MaxDistanceToArea, int MaxAreaRadius)
        {
            CCoord result = null;//!!!Может вернуть null
            List<CCoord> upd = GetNearUpdates(st.MyCoord, MaxDistanceToArea);

            int max = 0;
            foreach(CCoord it in upd)
            {
                int k = GetNearUpdates(it, MaxAreaRadius).Count;
                if (k > max)
                {
                    max = k;
                    result = it;
                }
            }
            return result;
        }

        private CCoord GetNearestFreeCell(CCoord coord)
        {
            CCoord res = new CCoord();
            if (st.Map[coord.x, coord.y] != TypesOfField.WALL)
                res = coord.Clone();
            else
            {
                List<CCoord> list = st.Map.GetAliedCells(coord, 15, true);
                CCoord it = list[0];
                int i = 1;
                while ((st.Map[it.x, it.y] == TypesOfField.WALL)&&(i<list.Count))
                {
                    it = list[i];
                    i++;
                }
                res = it;
            }
            return res;
        }
        private CCoord getMapCenter()
        {
            int x = st.MapWidth / 2;
            int y = st.MapHeight / 2;
            return GetNearestFreeCell(new CCoord(x, y));
        }

        private void GoToUpdates(CStep step)
        {
            int MaxDistanceToArea = (st.MapHeight+st.MapWidth)/(2+3);
            int MaxAreaRadius = 4;

            CCoord coord;
            if (API.isVisible())
            {
                int k = Useful.Min(API.endAfterTime() - 1, 3);
                coord = FindUpdatesConcentrationArea(k, MaxAreaRadius);
                if (coord == null)
                {
                    List<CCoord> list = GetNearUpdates(st.MyCoord, int.MaxValue);
                    if (list.Count > 0) coord = list[0];
                    else coord = getMapCenter();
                }
                step.Set(Steps.STEP, GetTrackToPoint(coord)[0]);
            } else
            {
                int k = Useful.Min(API.endAfterTime()-4, MaxDistanceToArea);
                coord = FindUpdatesConcentrationArea(k, MaxAreaRadius);
                if (coord == null)
                {
                    List<CCoord> list = GetNearUpdates(st.MyCoord, int.MaxValue);
                    if (list.Count > 0) coord = list[0];
                    else coord = getMapCenter();
                }
                step.Set(Steps.STEP, GetTrackToPoint(coord)[0]);
            }
        }

        private List<CCoord> GetTrackToPoint(CCoord coord)
        {
            if (st.Map[coord.x, coord.y] == TypesOfField.WALL)
                throw new Exception("Попытка посчитать расстояние до стены");

            List<CCoord> result = new List<CCoord>();

            int d = coord.d;
            if (d == 0) d = int.MaxValue;

            List<CCoord> list = st.Map.GetAliedCells(st.MyCoord, d, true);

            int[,] arr = ListToArr(list);

            CCoord c = coord.Clone();

            //добавим самую последнюю точку маршрута
            result.Add(coord.Clone());

            while (!c.EqualsXY(st.MyCoord))
            {
                list = st.Map.GetAliedCells(c, 1, true);
                foreach (CCoord it in list) it.d = arr[it.x, it.y];
                list.Sort();
                c = list[0];
                result.Add(c);
            }
            result.Reverse();

            //удалим точку с которой начался маршрут
            result.RemoveAt(0);

            return result;
        }
        private bool HaveChanceToKill()
        {
            bool res = false;
            int k = API.endAfterTime();

            int t = st.EnHealth / 10;
            if (t * 10 < st.EnHealth) t++;
            res = (t <= k);

            return res;
        }

        public CBot()
        {
            Rnd = new Random(DateTime.Now.Millisecond);
        }
        public IStep doSmth()
        {
            CStep result = new CStep();

            st.Refresh();

            if (st.isEnemyDetected)
            {
                if (CanKillDarkBotOnThisStep())
                {
                    result.Set(Steps.ROCKET, st.GetEnemyCoord() );
                } else
                {
                    if (HaveChanceToKill())
                    {
                        if (API.isVisible())
                        {
                            if(st.isMyWeaponReady)
                                if (API.endAfterTime() <= 4)
                                {
                                    result.Set(Steps.ROCKET, st.GetEnemyCoord());
                                } else
                                {
                                    result.Set(Steps.BLASTER, st.GetEnemyCoord());
                                }
                            else
                                GoToUpdates(result);
                        } else
                        {
                            if(st.isMyWeaponReady)
                                result.Set(Steps.ROCKET, st.GetEnemyCoord());
                            else GoToUpdates(result);
                        }
                    } else
                    {
                        GoToUpdates(result);//1.2.2.1.	Идем к бонусам или аптечкам
                    }
                }
            } else
            {
                if (HaveChanceToKill())
                {
                    if (st.Map.LocCount <= 2)
                    {
                        int k = Rnd.Next(st.Map.LocCount - 1);
                        result.Set(Steps.ROCKET, st.Map.EnemyLocList(k));
                    } else
                    {
                        GoToUpdates(result);//1.2.2.1.	Идем к бонусам или аптечкам1
                    }
                } else
                {
                    GoToUpdates(result);//1.2.2.1.	Идем к бонусам или аптечкам1
                }
            }

            if (!st.CanIStep())
                result.Set(Steps.STEP, st.MyCoord);

            st.PredStep = result;

            return result;
        }
        public string getName()
        {
            return "R2-D2";
        }
    }
}
