using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using CCStepsCoords;
using Logic;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Security;
using System.Reflection;
using Epsilon;
using R2D2;


namespace MasterElement
{

    /// <summary>
    /// masterСhief - так будет называться основной класс, где статический API лежит к нему обращайтесь за помощью, не забыв подключчить соотв. пространство имен
    /// </summary>
    class masterСhief : IAPI
    {
        private const int MaxPlayTime = 100;  //fix
        private const int MinPlayTime = 90;
        internal CMap map;
        private Random rand;
        private int TimeToEnd;
        internal int CurrentBot;
        internal CGameBot Bot1;
        internal CGameBot Bot2;
        private int BotLose;
        private bool GameEnd;
        internal int WhoWin;
        internal IStep  Step1;
        internal IStep Step2;
        internal string reason;

        internal masterСhief()
        {
            map = new CMap();
            //map.Generate();
            rand = new Random();
            TimeToEnd = rand.Next(MinPlayTime, MaxPlayTime);
            CurrentBot = 1;
            Bot1 = new CGameBot(this, 100, 0, map.Bot1);
            Bot2 = new CGameBot(this, 100, 0, map.Bot2);
            BotLose = -1;
            GameEnd = false;
            WhoWin = -1;
        }




        ///<summary>
        /// Ширина поля по X - количество клеточек
        /// </summary>
        /// <returns>Размерность поля</returns>
        public int getCountX()
        {
            return getMaxX() - getMinX() + 1;
        }

        ///<summary>
        /// Ширина поля по Y - количество клеточек
        /// </summary>
        /// <returns>Размерность поля</returns>
        public int getCountY()
        {
            return getMaxX() - getMinX() + 1;
        }

        ///<summary>
        /// Номер первой ячейки по X
        /// </summary>
        /// <returns>Начальная координата</returns>
        public int getMinX()
        {
            return 1;
        }

        ///<summary>
        /// Номер первой ячейки по Y
        /// </summary>
        /// <returns>Начальная координата</returns>
        public int getMinY()
        {
            return 1;
        }

        /// <summary>
        /// Номер последней ячеки по X
        /// </summary>
        /// <returns>Максимальная координата</returns>
        public int getMaxX()
        {
            return map.maxX;
        }

        /// <summary>
        /// Номер последней ячеки по Y
        /// </summary>
        /// <returns>Максимальная координата</returns>
        public int getMaxY()
        {
            return map.maxY;
        }

        /// <summary>
        /// Возврат типа поля по координате
        /// </summary>
        /// <param name="coord">Координата, для которой нужно узнать тип поля</param>
        /// <returns>Тип поля</returns>
        public TypesOfField getTypeOfField(ICoordinates coord)
        {
            if (CurrentBot == 1)
            {
                if (!isVisible() && map.map[coord.X1, coord.Y1] == TypesOfField.HI)
                    return TypesOfField.NOTHING;
                else
                    return map.map[coord.X1, coord.Y1];
            }
            else
            {
                if (!isVisible() && map.map[coord.X1, coord.Y1] == TypesOfField.ME)
                    return TypesOfField.NOTHING;
                else
                    if (map.map[coord.X1, coord.Y1] == TypesOfField.HI)
                        return TypesOfField.ME;
                    else
                        if (map.map[coord.X1, coord.Y1] == TypesOfField.ME)
                            return TypesOfField.HI;
                        else
                            return map.map[coord.X1, coord.Y1];
            }

        }

        /// <summary>
        /// Информация о том, видят боты друг друга, или нет
        /// </summary>
        public bool isVisible()
        {
            return isVisible(Bot1.Position, Bot2.Position);
        }

        /// <summary>
        /// Возврат координат врага, если его видно. 
        /// </summary>
        /// <returns>Координата врага</returns>
        public ICoordinates getCoordOfEnemy()
        {
            if (!isVisible())
                return null;
            else
                if (CurrentBot == 1)
                    return Bot2.Position;
                else
                    return Bot1.Position;
        }

        /// <summary>
        /// Возврат координат текущего бота.
        /// </summary>
        /// <returns>Координаты бота</returns>
        public ICoordinates getCoordOfMe()
        {
            if (CurrentBot == 1)
                return Bot1.Position;
            else
                return Bot2.Position;
        }

        /// <summary>
        /// Сколько у бота осталось жизни
        /// </summary>
        /// <returns>Скока жизни</returns>
        public int myHealth()
        {
            if (CurrentBot == 1)
                return Bot1.Health;
            else
                return Bot2.Health;
        }

        /// <summary>
        /// Сколько ходов осталось до конца игры
        /// </summary>
        /// <returns>Скока ходов осталось</returns>
        public int endAfterTime()
        {
            return TimeToEnd;
        }

        /// <summary>
        /// Возврат количества очков у бота
        /// </summary>
        /// <returns>ОЧКИ НННАДА?!</returns>
        public int myPoints()
        {
            if (CurrentBot == 1)
                return Bot1.Points;
            else
                return Bot2.Points;
        }

        /// <summary>
        /// Возврат видимости между двумя любыми клетками
        /// </summary>
        /// <param name="coord1">Первая клетка</param>
        /// <param name="coord2">Вторая клетка</param>
        /// <returns>Да, если можно, нет, если нельзя</returns>
        public bool isVisible(ICoordinates coord1, ICoordinates coord2)  //перегрузка
        {
            bool ret;
            if (!(map.map[coord1.X1, coord1.Y1] == TypesOfField.WALL && map.map[coord2.X1, coord2.Y1] == TypesOfField.WALL) &&
                map.FINDEQUALNUM(map.Visibles[coord1.X1, coord1.Y1], coord2) != -1)  // оба не WALL и одна из координат видима из другой
            {
                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        public bool isNorm(ICoordinates co)
        {
            return map.isNorm(co);
        }

        public bool CanGo()
        {
            if (CurrentBot == 1)
                return Bot1.canGo;
            else
                return Bot2.canGo;
        }

        internal bool isAllied(ICoordinates on, ICoordinates tw)
        {
            return ((on.X1 == tw.X1 && on.Y1 == tw.Y1 + 1) || (on.X1 == tw.X1 && on.Y1 == tw.Y1 - 1) || (on.X1 + 1 == tw.X1 && on.Y1 == tw.Y1) || (on.X1 - 1 == tw.X1 && on.Y1 == tw.Y1) || (on.X1 == tw.X1 && on.Y1 == tw.Y1));
        }

        internal void NextTurn()
        {

            CurrentBot = 1;
            
            Step1 = Bot1.Mind.doSmth();
            CurrentBot = 2;
            Step2 = Bot2.Mind.doSmth();

            //обработка стрельбы
            if (Step1.getTypeOfStep() == Steps.BLASTER)
            {
                if (Bot1.canShoot && map.canBlasterShootBeetween(Bot1.Position, Step1.getCoord()))
                {
                    if (Step1.getCoord().X1 == Bot1.Position.X1 && Step1.getCoord().Y1 == Bot1.Position.Y1)
                    {
                        Bot1.Health -= 10;
                    }
                    else
                    {
                        if (Step1.getCoord().X1 == Bot2.Position.X1 && Step1.getCoord().Y1 == Bot2.Position.Y1)
                        {
                            Bot2.Health -= 10;
                            Bot1.Points += 5;
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    BotLose = 1;
                    reason = "Бот 1 не может стрелять сюда бластером";
                }
            }

            if (Step1.getTypeOfStep() == Steps.ROCKET)
            {
                if (Bot1.canShoot)
                {
                    if (Step1.getCoord().X1 == Bot1.Position.X1 && Step1.getCoord().Y1 == Bot1.Position.Y1)
                    {
                        Bot1.Health -= 50;
                        Bot1.TurnsToShoot = 5;
                    }
                    else
                    {
                        if (Step1.getCoord().X1 == Bot2.Position.X1 && Step1.getCoord().Y1 == Bot2.Position.Y1)
                        {
                            Bot2.Health -= 50;
                            Bot1.Points += 25;
                            Bot1.TurnsToShoot = 5;
                        }
                        else
                        {
                            Bot1.TurnsToShoot = 5;
                        }
                    }
                }
                else
                {
                    BotLose = 1;
                    reason = "Бот 1 не может стрелять сюда ракетой";
                }
            }

            if (Step2.getTypeOfStep() == Steps.BLASTER)
            {
                if (Bot2.canShoot && map.canBlasterShootBeetween(Bot2.Position, Step2.getCoord()))
                {
                    if (Step2.getCoord().X1 == Bot2.Position.X1 && Step2.getCoord().Y1 == Bot2.Position.Y1)
                    {
                        Bot2.Health -= 10;
                    }
                    else
                    {
                        if (Step2.getCoord().X1 == Bot1.Position.X1 && Step2.getCoord().Y1 == Bot1.Position.Y1)
                        {
                            Bot1.Health -= 10;
                            Bot2.Points += 5;
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    BotLose = 2;
                    reason = "Бот 2 не может стрелять сюда бластером";
                }
            }

            if (Step2.getTypeOfStep() == Steps.ROCKET)
            {
                if (Bot2.canShoot)
                {
                    if (Step2.getCoord().X1 == Bot2.Position.X1 && Step2.getCoord().Y1 == Bot2.Position.Y1)
                    {
                        Bot2.Health -= 50;
                        Bot2.TurnsToShoot = 5;
                    }
                    else
                    {
                        if (Step2.getCoord().X1 == Bot1.Position.X1 && Step2.getCoord().Y1 == Bot1.Position.Y1)
                        {
                            Bot1.Health -= 50;
                            Bot2.Points += 25;
                            Bot2.TurnsToShoot = 5;
                        }
                        else
                        {
                            Bot2.TurnsToShoot = 5;
                        }
                    }
                }
                else
                {
                    BotLose = 2;
                    reason = "Бот 2 не может стрелять сюда ракетой";
                }
            }
            // обработка ходов
            //if(CMap.isAllied(Bot1.Position,Step1.getCoord()))
            if ((Step1.getTypeOfStep() == Steps.STEP && Bot1.canGo && Step2.getTypeOfStep() == Steps.STEP && Bot2.canGo &&  //лбы
                Step1.getCoord().X1 == Step2.getCoord().X1 && Step1.getCoord().Y1 == Step2.getCoord().Y1) ||
                (Step1.getTypeOfStep() == Steps.STEP && Step2.getTypeOfStep() == Steps.STEP && Bot2.canGo &&
                Step1.getCoord().X1 == Step2.getCoord().X1 && Step1.getCoord().Y1 == Step2.getCoord().Y1) ||
                (Step1.getTypeOfStep() == Steps.STEP && Bot1.canGo && Step2.getTypeOfStep() == Steps.STEP &&
                Step1.getCoord().X1 == Step2.getCoord().X1 && Step1.getCoord().Y1 == Step2.getCoord().Y1))
                {
                    Bot1.Health--;
                    Bot2.Health--;
                }
            else
            {
                // первый
                if (Step1.getTypeOfStep() == Steps.STEP)
                {
                    if (Bot1.canGo)
                    {
                        if (isNorm(Step1.getCoord()) && isAllied(Step1.getCoord(), Bot1.Position) && map.map[Step1.getCoord().X1, Step1.getCoord().Y1] != TypesOfField.WALL)
                        {
                            map.map[Bot1.Position.X1, Bot1.Position.Y1] = TypesOfField.NOTHING;
                            Bot1.Position = Step1.getCoord();
                            if (!Bot1.isDead && map.map[Step1.getCoord().X1, Step1.getCoord().Y1] == TypesOfField.MEDKIT)
                            {
                                map.Medics--;
                                map.Spaces++;
                                Bot1.Health += 20;
                                if (Bot1.Health > 100)
                                    Bot1.Health = 100;
                            }
                            if (map.map[Step1.getCoord().X1, Step1.getCoord().Y1] == TypesOfField.BONUS)
                            {
                                map.Bonuses--;
                                map.Spaces++;
                                Bot1.Points += 100;
                            }
                            map.map[Bot1.Position.X1, Bot1.Position.Y1] = TypesOfField.ME;

                        }
                        else
                        {
                            BotLose = 1;
                            reason = "Бот 1 не может сюда ходить";
                        }
                    }
                    else
                    {
                        if (!(Step1.getCoord().X1 == Bot1.Position.X1 && Step1.getCoord().Y1 == Bot1.Position.Y1))
                        {
                            BotLose = 1;
                            reason = "Бот 1 ранен и не может сюда ходить";
                        }

                    }
                }
                // второй
                if (Step2.getTypeOfStep() == Steps.STEP)
                {
                    if (Bot2.canGo)
                    {
                        if (isNorm(Step2.getCoord()) && isAllied(Step2.getCoord(), Bot2.Position) && map.map[Step2.getCoord().X1, Step2.getCoord().Y1] != TypesOfField.WALL)
                        {
                            map.map[Bot2.Position.X1, Bot2.Position.Y1] = TypesOfField.NOTHING;
                            Bot2.Position = Step2.getCoord();
                            if (!Bot2.isDead && map.map[Step2.getCoord().X1, Step2.getCoord().Y1] == TypesOfField.MEDKIT)
                            {
                                map.Medics--;
                                map.Spaces++;
                                Bot2.Health += 20;
                                if (Bot2.Health > 100)
                                    Bot2.Health = 100;
                            }
                            if (map.map[Step2.getCoord().X1, Step2.getCoord().Y1] == TypesOfField.BONUS)
                            {
                                map.Bonuses--;
                                map.Spaces++;
                                Bot2.Points += 100;
                            }
                            map.map[Bot2.Position.X1, Bot2.Position.Y1] = TypesOfField.HI;
                        }
                        else
                        {
                            BotLose = 2;
                            reason = "Бот 2 не может сюда ходить";
                        }
                    }
                    else
                    {
                        if (!(Step2.getCoord().X1 == Bot2.Position.X1 && Step2.getCoord().Y1 == Bot2.Position.Y1))
                        {
                            BotLose = 2;
                            reason = "Бот 2 ранен и не может сюда ходить";
                        }
                    }
                }

                //финализация

                TimeToEnd--;
                GameEnd = Bot1.isDead || Bot2.isDead || TimeToEnd == 0 || BotLose != -1;
                if (!GameEnd)
                {
                    if (Bot1.isSlow)
                        if (Bot1.TurnsToStep == 0)
                            Bot1.TurnsToStep = 1;
                        else
                            Bot1.TurnsToStep = 0;

                    if (Bot2.isSlow)
                        if (Bot2.TurnsToStep == 0)
                            Bot2.TurnsToStep = 1;
                        else
                            Bot2.TurnsToStep = 0;

                    if (Bot1.TurnsToShoot > 0)
                        Bot1.TurnsToShoot--;
                    if (Bot2.TurnsToShoot > 0)
                        Bot2.TurnsToShoot--;
                }

            }
        }
        // добить
        internal void initializ()
        {


            IBot p1 = null;
            IBot p2 = null;
            try
            {
                p1 = new Epsilon.EpsilonBot();
                //заменить на соответствующее
            }
            catch (SecurityException)
            {
                BotLose = 1;
            }

            try
            {
                p2 = new CBot();

            }
            catch (SecurityException)
            {
                BotLose = 1;
            }


            Bot1.Mind = p1;
            Bot2.Mind = p2;

            try
            {
                Bot1.Mind.Initialize(this);
            }
            catch (SecurityException)
            {
                BotLose = 1;
            }
            catch (NullReferenceException)
            {
                BotLose = 1;
            }

            try
            {
                Bot2.Mind.Initialize(this);
            }
            catch (SecurityException)
            {
                BotLose = 2;
            }
            catch (NullReferenceException)
            {
                BotLose = 2;
            }

            try
            {
                Bot1.Name = Bot1.Mind.getName();
            }
            catch (SecurityException)
            {
                BotLose = 1;
            }
            catch (NullReferenceException)
            {
                BotLose = 1;
            }

            try
            {
                Bot2.Name = Bot2.Mind.getName();
            }
            catch (SecurityException)
            {
                BotLose = 2;
            }
            catch (NullReferenceException)
            {
                BotLose = 2;
            }


        }
        internal void ResetGameFromFile(string path)
        {
            map = new CMap();
            map.ReadFromFile(path);
            TimeToEnd = rand.Next(MinPlayTime, MaxPlayTime);
            CurrentBot = 1;
            Bot1 = new CGameBot(this, 100, 0, map.Bot1);
            Bot2 = new CGameBot(this, 100, 0, map.Bot2);
            BotLose = -1;
            GameEnd = false;
            WhoWin = -1;
            initializ();
        }  


        internal void ResetGame()
        {
            map = new CMap();
            map.Generate();
            TimeToEnd = rand.Next(MinPlayTime, MaxPlayTime);
            CurrentBot = 1;
            Bot1 = new CGameBot(this, 100, 0, map.Bot1);
            Bot2 = new CGameBot(this, 100, 0, map.Bot2);
            BotLose = -1;
            GameEnd = false;
            WhoWin = -1;
            initializ();
        }
            
        //internal void StartGame()
        //{
        //    initializ();
        //    while (TimeToEnd != 0 && BotLose == -1 && !GameEnd)
        //    {

        //        NextTurn();
        //    }
        //    switch (BotLose)
        //    {
        //        case 1:
        //            WhoWin = 2;
        //            break;
        //        case 2:
        //            WhoWin = 1;
        //            break;
        //        default:
        //            {
        //                if ((Bot1.isDead && Bot2.isDead) || (!Bot1.isDead && !Bot2.isDead && Bot1.Points == Bot2.Points))
        //                {
        //                    WhoWin = 0;
        //                }
        //                if ((!Bot1.isDead && Bot2.isDead) || (!Bot1.isDead && !Bot2.isDead && Bot1.Points > Bot2.Points))
        //                {
        //                    WhoWin = 1;
        //                }
        //                if ((!Bot2.isDead && Bot1.isDead) || (!Bot1.isDead && !Bot2.isDead && Bot1.Points < Bot2.Points))
        //                {
        //                    WhoWin = 2;
        //                }
        //                break;
        //            }
        //    }
        //}
        internal void ResGame()
        {
            if(TimeToEnd == 0 || BotLose != -1 || GameEnd)
                switch (BotLose)
                {
                    case 1:
                        WhoWin = 2;
                        
                        break;
                    case 2:
                        WhoWin = 1;
                        
                        break;
                    default:
                        {
                            if ((Bot1.isDead && Bot2.isDead) || (!Bot1.isDead && !Bot2.isDead && Bot1.Points == Bot2.Points))
                            {
                                WhoWin = 0;
                            }
                            if ((!Bot1.isDead && Bot2.isDead) || (!Bot1.isDead && !Bot2.isDead && Bot1.Points > Bot2.Points))
                            {
                                WhoWin = 1;
                            }
                            if ((!Bot2.isDead && Bot1.isDead) || (!Bot1.isDead && !Bot2.isDead && Bot1.Points < Bot2.Points))
                            {
                                WhoWin = 2;
                            }
                            break;
                        }
                }
        }
    }
}
