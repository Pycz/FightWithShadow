using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces; 
using CCStepsCoords;


namespace Epsilon {
	/// <summary>Класс построения дерева путей
	/// </summary>
	internal class Logistics {
		
		Mind mind;


		///// <summary>выяснить в каком направлении двигаться из начальной позиции
		///// </summary>
		private IStep comeBack(Ways sector,List<Ways> unUsableSector) {
			int i;
			IStep s = StepsAndCoord.StepsBuilder (EpsilonBot.api.getCoordOfMe (),Steps.STEP );
			while (!StepsAndCoord.isEcualCoord ( sector.back, EpsilonBot.api.getCoordOfMe () )) {
				  i = sector.getIndexIn ( sector.back, unUsableSector );
					sector = unUsableSector[i].Clone ();
			}

			return StepsAndCoord.StepsBuilder(sector.xy,Steps.STEP);
		}
		/// <summary>
		////движение к наиболее удаленной точке от бота
		/// </summary>
		/// <returns></returns>
		private IStep goToFar() {
			int distance = 0;
			bool found = false;

			IStep s = StepsAndCoord.StepsBuilder(EpsilonBot.api.getCoordOfMe(),Steps.STEP);
			Queue<Ways> q1 = new Queue<Ways> (); // основная очередь
			Queue<Ways> q2 = new Queue<Ways> (); // Вспомогательная
			Ways temp = new Ways ( EpsilonBot.api.getCoordOfMe (),0,0,0,mind.He.gun ().getTimerVal,EpsilonBot.api.getCoordOfMe () );		//mind.veryFar
			int maxLen = EpsilonBot.api.endAfterTime ();
			int j = 0;
			List<Ways> unUsableSector = new List<Ways> ();// список использованных вершин
			q1.Enqueue ( temp.Clone() );

			while ((q1.Count > 0 || q2.Count > 0) && !found && maxLen>=distance) {
				q2 = new Queue<Ways> (); // Вспомогательная
				while ((q1.Count > 0)&&(!found)) {
					j++;
					temp = q1.Dequeue ();	// достали вершину из очереди, пора узнать что в ней изменилось
					temp.gun ().decTimer ();
					temp.len = distance;
					//--1------------------
					if (EpsilonBot.api.isNorm ( temp.xy )) {
						 found = StepsAndCoord.isEcualCoord(temp.xy,mind.veryFar);
						//--2---------------------------
						if(!found){
							//unUsableSector.Add ( temp );
							temp.getAllUsableSectors ( unUsableSector,q2 ); //всех соседей данной вершины графа			 в очередь
						}//--2------------------------
					}//--1--------------------------
				}
				q1 = q2;
				distance++;
			}
			if (found || maxLen == distance) {
				s = comeBack ( temp,unUsableSector );//StepsAndCoord.StepsBuilder ( temp.back,Steps.STEP );
			}
			return s;
		}

		//private ICoordinates getToGo(bool[,] map,ICoordinates xy) {
		//  bool found = false;
		//  if (EpsilonBot.api.isNorm ( StepsAndCoord.Coordinates ( xy,+1,-0 ) ) && !found) {
		//    if (map[xy.getX ()+1,xy.getY ()+0] != false &&!found) {
		//      map[xy.getX (),xy.getY ()] = false;
		//      return StepsAndCoord.Coordinates ( xy.getX ()+1,xy.getY ()+0 );
		//      found = true;
		//    }
		//  }
		//  if (EpsilonBot.api.isNorm ( StepsAndCoord.Coordinates ( xy,+1,-0 ) ) && !found) {
		//    if (map[xy.getX () - 1,xy.getY () + 0] != false && !found) {
		//      map[xy.getX (),xy.getY ()] = false;
		//      return StepsAndCoord.Coordinates ( xy.getX () - 1,xy.getY () + 0 );
		//      found = true;
		//    }
		//  }
		//  if (EpsilonBot.api.isNorm ( StepsAndCoord.Coordinates ( xy,+0,+1 ) ) && !found) {
		//    if (map[xy.getX () + 1,xy.getY () + 0] != false && !found) {
		//      map[xy.getX (),xy.getY ()] = false;
		//      return StepsAndCoord.Coordinates ( xy.getX () + 0,xy.getY () + 1 );
		//      found = true;
		//    }
		//  }
		//  if (EpsilonBot.api.isNorm ( StepsAndCoord.Coordinates ( xy,+0,-1 ) )&&!found) {
		//    if (map[xy.getX () + 1,xy.getY () + 0] != false && !found) {
		//      map[xy.getX (),xy.getY ()] = false;
		//      return StepsAndCoord.Coordinates ( xy.getX () + 0,xy.getY () - 0 );
		//      found = true;
		//    }
		//  }
		//  return null;
		//}
		/// <summary>
		/// находит наиболее выгодный маршрут, такой,
		/// что если все маршруты дают равные результаты, то выбрать кратчайший
		/// если маршрут дает больше аптечек и длиннее чем предыдущий <= 3, то выбрать его
		/// </summary>
		/// <param name="inWays">все ячейки бонусов на данной волне</param>
		/// <param name="lokedWays">ячейки которые были анулированны </param>
		/// <param name="len">оставшаяся длинна дороги</param>
		/// <returns>координаты наиболее выгодного направления</returns>

		private IStep lookAround(List<Ways> inWays, List<Ways> lokedWays,int len) {
			IStep s = StepsAndCoord.StepsBuilder ( EpsilonBot.api.getCoordOfMe (),Steps.STEP );
			int mon = 0;//начальные 
			int hel = 0;//параметры
			bool[,] map = new bool[EpsilonBot.api.getMaxX () + 1,EpsilonBot.api.getMaxY () + 1];
			foreach( Ways w in lokedWays)
				map[w.xy.getX(),w.xy.getY()] = true;

			//Ways maxSum = new Ways ( null,0,0,0,Gun.cReloadTime,inWays[0].xy );
			//Ways tempSum = new Ways ( null,0,0,0,Gun.cReloadTime,inWays[0].xy );

			List<Ways> path=new List<Ways>(); // хранит все последние вершины
			bool bpop = false;

			int mb = int.MinValue;
			int mm = int.MinValue;

			int i = 0;
			while (inWays.Count > i) { // для каждой из отобранных ячеек посчитать максимальный доход
				Stack<Ways> stack = new Stack<Ways> ();
				
				stack.Push ( inWays[i].Clone() );
				int medcount = 0;
				int maxmedcount = int.MinValue;
 				int boncount = 0;
				int maxboncount = int.MinValue;
				/////////////////////************************************
				Ways w = null;
				while (stack.Count ==  0) {				/////////////////////************************************
					bpop = true;
					w = stack.Peek ();
					/////////////////////************************************	
					if(EpsilonBot.api.isNorm(w.xy))
					{
						if (EpsilonBot.api.getTypeOfField ( w.xy ) == TypesOfField.MEDKIT && !map[w.xy.getX (),w.xy.getY ()]) {
								medcount++;
								map[w.xy.getX (),w.xy.getY ()] = true;
								w.type = TypesOfField.MEDKIT;
							}
						if (EpsilonBot.api.getTypeOfField ( w.xy ) == TypesOfField.BONUS && !map[w.xy.getX (),w.xy.getY ()]) {
								medcount++;
								map[w.xy.getX (),w.xy.getY ()] = true;
								w.type = TypesOfField.BONUS;
							}

							if (stack.Count <= len) {

								if ((stack.Count <= len))
								{

									if( (medcount == maxmedcount)&&(boncount>maxboncount)) {
										maxmedcount = medcount;
										maxboncount = boncount;
										path = stack.ToList<Ways> ();
									}
									if (medcount > maxmedcount) {
										maxmedcount = medcount;
										maxboncount = boncount;
										path = stack.ToList<Ways> ();
									}
								}
								if (stack.Count < len)
								if (w.len < 4) {
									
									Ways t = w.Clone ();
									t.len = 0;
									w.len++;

									if (t.len == 0)
										t.xy.X1++;
									if (t.len == 1)
										t.xy.Y1++;
									if (t.len == 2)
										t.xy.X1--;
									if (t.len == 3)
										t.xy.Y1--;

									stack.Push ( t );
									bpop = false;
								}
							}
					}

					if (bpop) {
						Ways p =  stack.Pop();
						if (p.type == TypesOfField.MEDKIT && map[p.xy.getX (),p.xy.getY ()]) {
							medcount--;
							map[p.xy.getX (),p.xy.getY ()] = false;
						}
						if (p.type == TypesOfField.BONUS && map[p.xy.getX (),p.xy.getY ()]) {
							boncount--;
							map[p.xy.getX (),p.xy.getY ()] = false;
						}
					}
					
				}

				if (path.Count > 0) {
					if( maxmedcount>mm) {
						mm = maxmedcount;
						if (maxboncount>mb) {
							mb = maxboncount;
							path = stack.ToList<Ways> ();
						}
					}
				}

				i++;
			}

			return s;
		}
		/// <summary>
		/// расчет пути к клетке с заданным типом, на удалении не более чем 
		/// </summary>
		/// <param name="stm">тип поиска</param>
		/// <param name="roadlen">длинна допустимого маршрута</param>
		/// <param name="gunTime">заряд оружия противника</param>
		/// <returns>Тип и координаты хода</returns>
		internal IStep wayOfEvil(TypesOfField stm,int roadlen,int gunTime) {
			int distance = 1;
			//int goForFar = 0;
			bool found = false;
			int interationNum = 0;
			IStep s = StepsAndCoord.StepsBuilder ( EpsilonBot.api.getCoordOfMe (),Steps.STEP );
			Queue<Ways> q1 = new Queue<Ways> (); // основная очередь
			Queue<Ways> q2 = new Queue<Ways> (); // Вспомогательная
			Ways temp = new Ways ( EpsilonBot.api.getCoordOfMe (),0,0,0,mind.He.gun ().getTimerVal,EpsilonBot.api.getCoordOfMe () );
			List<Ways> allObjects = new List<Ways> (); // все сектора с необходимого типа 

			List<Ways> unUsableSector = new List<Ways> ();
			List<Ways> unUseHelthSector = new List<Ways> ();

			q1.Enqueue ( temp.Clone () );

			while (interationNum <-1 && !found) {

				while ((q1.Count > 0 || q2.Count > 0) && distance <= roadlen) {
					q2 = new Queue<Ways> (); // Вспомогательная
					while (q1.Count > 0) {
						temp = q1.Dequeue ();											// достали вершину из очереди, пора узнать что в ней изменилось
						temp.gun ().decTimer ();
						temp.len = distance;
						//--1------------------
						if (EpsilonBot.api.isNorm ( temp.xy )) {
							//--4-----------------
							//	if (temp.gun ().getTimerVal > Gun.cminSaflyTime && mind.) {
							//--2------------------
							if (EpsilonBot.api.getTypeOfField ( temp.xy ) == stm) {
								allObjects.Add ( temp );	// все сектора на допустимой длинне
							} else {
								//--3-------------------	 
								if (temp.gun ().getTimerVal > Gun.cminSaflyTime && EpsilonBot.api.getTypeOfField ( temp.xy ) != TypesOfField.NOTHING) {
									temp.getAllUsableSectors ( unUsableSector,q2 ); //всех соседей данной вершины графа			 в очередь
									unUsableSector.Add ( temp );
									if (EpsilonBot.api.getTypeOfField ( temp.xy ) == TypesOfField.MEDKIT && TypesOfField.MEDKIT != stm) {
										unUseHelthSector.Add ( temp );
									}
								}//--3---------------------
							}//--2----------------------
							//}//--4-----------------------
						}//--1-----------------------
						distance++;
						q1 = q2;
					}
				}
				//Ways w = null;
				//w = new Ways ();

				//w.xy.setX ( 1 );
				//w.xy.setY ( 4 );
				//allObjects.Add ( w );
				//w = new Ways ();
				//w.xy.setX ( 4 );
				//w.xy.setY ( 1 );
				//allObjects.Add ( w );
				if (allObjects.Count != 0) {
				  //bool f=false;

					IStep st;
					if (!(allObjects[0].gun ().getTimerVal > Gun.cminSaflyTime)) {
						st = lookAround ( allObjects,unUseHelthSector,4 );/// если маршрут выгоднее то пойти по нему
					}

					// если не выполн то запомнить ближ вершину и продолжить искать
					// просмотреть окрестность <=4  такую, что есть аптечка
					// если нашли то записали в список, и проверили остальные ячейки на данной волне удов пред услов, и выбрали лучш
					// если не нашли то увеличить волну  q1 = q2;		goForFar++ если стало >2 то идем к самой удаленной точке
					// distance++;
				} else {
					interationNum++;
				}
			}
			return goToFar ();// s;
		}

		internal Logistics(Mind m) {
			mind = m;
		}







		//private ICoordinates m_xy;
		//private int m_gunTimer;
		//private int len;
		//private Logistics m_back;
		//private  Mind mind;



		
		///// <summary>отправной точки - родительская вершина
		///// </summary>
		//internal Logistics back {
		//  get {
		//    return m_back;
		//  }
		//}
		///// <summary>координаты  
		///// </summary>
		//internal ICoordinates xy {
		//  get {
		//    return m_xy;
		//  }
		//  set {
		//    m_xy = value;
		//  }
		//}
		///// <summary>активна ли будет ракета на след ход  
		///// </summary>
		//internal bool isGunActive {
		//  get {
		//    return m_gunTimer == 0;
		//  }
		//}
		///// <summary>получить значение таймера  
		///// </summary>
		//internal int gunTimer {
		//  get {
		//    return m_gunTimer;
		//  }
		//  set {
		//    m_gunTimer = value;
		//  }
		//}
		///// <summary>удаленность от отправной точки  
		///// </summary>
		//internal int roadLen{
		//  get{
		//    return len;
		//  }
		//}
		///// <summary>вести отсчет таймера  
		///// </summary>
		//internal int decGunTimer() {
		//  if (m_gunTimer > 0) {
		//    m_gunTimer--;
		//  }
		//  return m_gunTimer;
		//}
		///// <summary>выяснить в каком направлении двигаться из начальной позиции
		///// </summary>
		//private IStep comeBack(Logistics sector) {
		//  while(sector.back!=mind.I.XY){
		//    sector = sector.back;
		//  }
		//  return StepsAndCoord.StepsBuilder ( sector.xy,Steps.STEP );
		//}
		///// <summary>выяснить есть ли в окрестности аптечи на допустимом растоянии
		///// </summary>
		//private void healthyWay(int epsilonX,int epsilonY,Logistics sector,int stepCount, int len, int healthSum, int moneySum) {
		//  if (len <= stepCount) {
		//    ICoordinates xy = StepsAndCoord.Coordinates ( sector.xy.getX () + epsilonX,sector.xy.getY () + epsilonY );
		//    if (EpsilonBot.api.isNorm ( xy )) {	//если можем идти
		//      if (sector.back.xy != xy) {					// не родительскя
		//        if (EpsilonBot.api.getTypeOfField ( xy ) == TypesOfField.MEDKIT) {
		//          healthSum += Mind.cHealthM;
		//        }
		//        if (EpsilonBot.api.getTypeOfField ( xy ) == TypesOfField.BONUS) {
		//          moneySum += Mind.cMoneyM;
		//        }
		//        healthyWay ( +1,+0,Logistics.smallBuilder ( xy,0,0,sector ),stepCount, len+1, healthSum, moneySum );										// извлекаем всех соседей данной вершины
		//        healthyWay ( -1,-0,Logistics.smallBuilder ( xy,0,0,sector ),stepCount, len+1, healthSum, moneySum );											// и по возможности помещаем в очередь
		//        healthyWay ( +0,+1,Logistics.smallBuilder ( xy,0,0,sector ),stepCount, len+1, healthSum, moneySum );
		//        healthyWay ( -0,-1,Logistics.smallBuilder ( xy,0,0,sector ),stepCount, len+1, healthSum, moneySum );
		//      }
		//    }
		//  }
		//}
		///// <summary>перебивка начального максимума по аптечкам
		///// </summary>
		//private void subEpsilonSectors(int tempMoney,int tempHealth,ref int moneySum,ref int healthSum) {
		//  if (tempHealth > healthSum) {
		//    healthSum = tempHealth;
		//    if (tempMoney > moneySum) {
		//      moneySum = tempMoney;
		//    }
		//  }
		//}
		///// <summary>есть ли путь на котором будут аптечки >=2 в пределах досягаемости
		///// </summary>
		///// <returns></returns>
		//private bool epsilonSectors(TypesOfField needElement,Logistics startPoint,int stepCount,int roketTimer) {
		//  int moneySum = 0;
		//  int healthSum = 0;
		//  int tempMoney = 0;
		//  int tempHealth = 0;
		//  healthyWay ( +1,+0,startPoint,stepCount,Gun.cTimerZeroNone, healthSum,  moneySum  );
		//  subEpsilonSectors ( tempMoney,tempHealth,ref moneySum,ref healthSum );

		//  healthyWay ( -1,-0,startPoint,stepCount,Gun.cTimerZeroNone, tempHealth, tempMoney );
		//  subEpsilonSectors ( tempMoney,tempHealth,ref moneySum,ref healthSum );

		//  healthyWay ( +0,+1,startPoint,stepCount,Gun.cTimerZeroNone, tempHealth, tempMoney );
		//  subEpsilonSectors ( tempMoney,tempHealth,ref moneySum,ref healthSum );

		//  healthyWay ( +0,-1,startPoint,stepCount,Gun.cTimerZeroNone, tempHealth, tempMoney );
		//  subEpsilonSectors ( tempMoney,tempHealth,ref moneySum,ref healthSum );
		//  return (healthSum >= 2 * Mind.cHealthM) /*спорная часть*//*|| (moneySum >= 2*Mind.cMoneyM && healthSum >= Mind.cHealthM)*/;
		//}
		///// <summary>получить соседей данной вершины 
		/////соседи корректны если это не родительские вершины, 
		/////и можно без боязненно проходить через них,
		/////т.е не обращать внимания на обнаружение противником
		///// </summary>
		///// <param name="epsilonX">отклоненние по х</param>
		///// <param name="epsilonY">отклонение по у</param>
		///// <param name="sector"> текущая вершина</param>
		///// <returns></returns>
		//private void possibleWays(int epsilonX,int epsilonY,Logistics sector,ref Queue<Logistics> x) {
		//    ICoordinates xy = StepsAndCoord.Coordinates ( sector.xy.getX () + epsilonX,sector.xy.getY () + epsilonY );
		//    if (EpsilonBot.api.isNorm ( xy )) {
		//      if (sector.back != xy) {					 // если это не отправная точка
		//        if (!sector.isGunActive) {			 // если на следующий ход противник не сможет стрелять ракетой
		//                                         // можем взять то что лежит здесь
		//          x.Enqueue(Logistics.smallBuilder(xy,sector.decGunTimer(),sector.roadLen+1,sector));	 //добавляем
		//        }
		//      }
		//    }
		//  }
		///// <summary>	/// расчет пути к сектору карты запрашиваемого типа
		///// </summary>
		///// <param name="needElement">тип ячейки к которой необходимо проложить маршрут</param>
		///// <param name="startPoint">отправная точка</param>
		///// <param name="stepCount">максимальная удаленность объекта</param>
		///// <returns></returns>
		//internal IStep wayOfEvil(TypesOfField needElement,Logistics startPoint,int stepCount,int roketTimer) {
		//  int roadLen = 0;
		//  bool found = false;
		//  bool exit = false;
		//  IStep s = StepsAndCoord.StepsBuilder ( startPoint.xy,Steps.STEP );
		//  Queue<Logistics> wayBase = new Queue<Logistics> ();	//база путей возможных за данную длинну
		//  Queue<Logistics> forWalk = new Queue<Logistics> ();	//для прохода по графу
		//  Logistics x = new Logistics ( mind.I.XY,mind.He.gun ().getTimerVal,roadLen,startPoint );
		//  forWalk.Enqueue ( x );

		//  while (!found && !exit && forWalk.Count > 0 && stepCount > 0 ) {
		//    //--1-------------------------
		//    if (forWalk.Peek ().roadLen < stepCount) {
		//      roadLen = forWalk.Peek ().roadLen;

		//      while (!exit && roadLen == forWalk.Peek ().roadLen){ // обрабатываем все вершины на данной волне
		//        x = forWalk.Dequeue ();
                        
		//        //--2-------------------------
		//        if (EpsilonBot.api.getTypeOfField ( x.xy ) == needElement) {		// если нашли необходимую ячейку то стоп
		//          //--3-------------------------
		//          x.gunTimer=Gun.cTimerZeroNone;
		//          if (epsilonSectors ( TypesOfField.MEDKIT,x,Math.Min(x.gunTimer,stepCount-roadLen),x.gunTimer)){	// и если есть в окрестности аптечки
		//            //wayBase.Enqueue ( x );	
		//            found = true;	
		//          }//--3------------------------
		//        } else {																												// иначе
		//          possibleWays ( +1,+0,x,ref forWalk );											// извлекаем всех соседей данной вершины
		//          possibleWays ( -1,-0,x,ref forWalk );											// и по возможности помещаем в очередь
		//          possibleWays ( +0,+1,x,ref forWalk );
		//          possibleWays ( -0,-1,x,ref forWalk );
		//        }//--2------------------------
		//                    exit = forWalk.Count == 0;
		//      }//--1-------------------------
		//    } else {		// если радиус волны больше чем длиннна возможного пути
		//      exit = true;
		//    }
		//  }
		//  if (found) {
		//    //x = findBestWay ( wayBase );// спорный  момент по выбору путей
		//    s = comeBack ( x );
		//  }
		//  return s;
		//}
		//private Logistics findBestWay(Queue<Logistics> q) {
		//  Logistics[] arr = q.ToArray ();
		//  Logistics temp=Logistics.smallBuilder(arr[0].xy,arr[0].gunTimer,9999999,arr[0].back);
		//    for (int i = 0; i < arr.Length-1;i++ ) {
		//    if (arr[i].roadLen< temp.roadLen){
		//      temp = arr[i];
		//    }
		//  }
		//    return temp;
		//}
		///// <summary>конструктор  
		///// </summary>
		///// <param name="needElement">тип ячейки к которой необходимо проложить маршрут</param>
		///// <param name="startPoint">отправная точка</param>
		///// <param name="stepCount">максимальная удаленность объекта</param>
		///// <returns></returns>
		//internal Logistics(ICoordinates xy,int gunTimer,int roadLen,Logistics back){
		//  this.len = roadLen;
		//  this.m_gunTimer = gunTimer;
		//  this.m_xy = xy;
		//  this.m_back = back;
		//  this.mind = mind;
			
		//}
		//internal Logistics(Mind mind)
		//{
		//  this.mind = mind;
		//  m_back = this;
		//}
		///// <summary>удобное представление консруктора anyWalker  
		///// </summary>
		///// <param name="needElement">тип ячейки к которой необходимо проложить маршрут</param>
		///// <param name="startPoint">отправная точка</param>
		///// <param name="stepCount">максимальная удаленность объекта</param>
		///// <returns></returns>
		//internal static Logistics smallBuilder(ICoordinates xy,int gunTimer,int roadLen,Logistics back) {
		//  return new Logistics ( xy,gunTimer,roadLen,back);
		//}
		/////<summary> обратный вызов
		/////</summary>
		//internal  void callBack(Mind _mind) {
		//  this.mind = _mind;
		//}
	}
}