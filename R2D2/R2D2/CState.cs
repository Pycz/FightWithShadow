using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace R2D2
{
    class CState
    {
        public CMap Map;
        private IAPI API;

        private int MapWidthVal;
        private int MapHeightVal;
        public int MapWidth
        {
            get { return MapWidthVal; }
        }
        public int MapHeight
        {
            get { return MapHeightVal; }
        }

        public CCoord MyCoord = new CCoord();
        public int MyHealth = 0;
        public int MyPoints = 0;

        private int EnHealthVal = 0;//предположительно!
        public int EnHealth
        {
            get { return EnHealthVal; }
            set
            {
                
                EnHealthVal += value;
                if (EnHealthVal > 100) EnHealthVal = 100;
            }
        }
        public int EnPoints = 0; //предположительно!
        public bool isEnemyDetected
        { 
            get 
            {
                return Map.EnemyDetected();
            } 
        }

        public CState(IAPI Api)
        {
            API = Api;

            MapWidthVal = API.getMaxX() - API.getMinX() + 1;
            MapHeightVal = API.getMaxY() - API.getMinY() + 1;

            Map = new CMap(MapWidthVal, MapHeightVal, API.endAfterTime());

            Map.ReadMap(API);

            MyHealth = API.myHealth();
            MyPoints = API.myPoints();

            MyCoord.Set(API.getCoordOfMe());

            PredStep.Set(Steps.STEP, MyCoord);
        }

        //Структура, хранящая данные, о моем предыдущем шаге
        public CStep PredStep = new CStep();

        //Игроки столкнулись на прошлом ходу
        public bool Faced = false;

        //Счетчик сброса "Возможности стрелять"
        public int MyWeaponTimeout = 0;
        public int DarkWeaponTimeout = 0;

        public bool isMyWeaponReady
        {
            get { return MyWeaponTimeout == 0; }
        }
        public bool isDarkRocketReady
        {
            get { return DarkWeaponTimeout == 0; }
        }

        public int getDistanceToEnemy()
        {
            return Map.GetDistanceToEnemy();
        }

        public CCoord GetEnemyCoord()
        {
                return Map.GetEnemyCoord();
        }
        //Можно ли myBot ходить
        public bool CanIStep()
        {
            return (MyHealth >= 20) || (PredStep.Type != Steps.STEP);
        }

        public  List<CCoord> GetBonusList()
        {
            return Map.GetBonusList();
        }
        public List<CCoord> GetMedkitList()
        {
            return Map.GetMedkitList();
        }
        public List<CCoord> GetUpdatesList()
        {
            return Map.GetUpdatesList();
        }

        private void SetEnemyExactLoc(CCoord coord, int ExpandVal)
        {
            Map.SetExactLoc(coord);

            for (int i = 0; i < ExpandVal; i++)
                Map.ExpandLocArea();
        }

        //Внимание!! до вызова данной функции все поля являются устаревшими!!!!
        public void Refresh()
        {
            int HealthDif =  API.myHealth() - MyHealth;
            int PointsDif =  API.myPoints()-  MyPoints;

            MyHealth = API.myHealth();
            MyPoints = API.myPoints();

            Map.ReadMap(API);

            MyCoord.Set(API.getCoordOfMe());

            if (MyWeaponTimeout   > 0) MyWeaponTimeout--;
            if (DarkWeaponTimeout > 0) DarkWeaponTimeout--;

            Faced = false;

            switch (PredStep.Type) {
                case Steps.STEP:
                    if (!PredStep.Coord.EqualsXY(MyCoord))
                    {
                        Faced = true;
                        EnHealth -= 1;
                    } else{
                        if (PointsDif > 0){} //я взял бонус
                        if (HealthDif > 0){} //я взял аптечку
                    }
                    break;
                case Steps.BLASTER:
                    if (PointsDif > 0)
                    {
                        SetEnemyExactLoc(PredStep.Coord, 1);
                        EnHealth -= 10;
                    }
                    break;
                case Steps.ROCKET:
                    if (PointsDif > 0)
                    {
                        SetEnemyExactLoc(PredStep.Coord, 1);
                        EnHealth -= 50;
                    }
                    break;
            }

            switch (HealthDif)
            {
                case -10:
                    EnPoints += 5;
                    break;
                case -50:
                    EnPoints += 25;
                    DarkWeaponTimeout = 5;
                    break;
                case -1:
                    EnHealth--;
                    break;
            }

            if(API.isVisible())
            {
                if (isEnemyDetected)
                {
                    if(!GetEnemyCoord().EqualsXY(API.getCoordOfEnemy()))
                        throw new Exception("в state.refresh() внутренние данные расходятся с API");
                }else SetEnemyExactLoc(new CCoord(API.getCoordOfEnemy()),0);
            }
        }
        
    }

}//конец namespace A2D2
