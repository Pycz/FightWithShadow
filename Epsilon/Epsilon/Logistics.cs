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
		private ICoordinates m_xy;
		private int m_gunTimer;
		private int len;
		private Logistics m_back;
		private  Mind mind;

		/// <summary>отправной точки - родительская вершина
		/// </summary>
		internal Logistics back {
			get {
				return m_back;
			}
		}
		/// <summary>координаты  
		/// </summary>
		internal ICoordinates xy {
			get {
				return m_xy;
			}
			set {
				m_xy = value;
			}
		}
		/// <summary>активна ли будет ракета на след ход  
		/// </summary>
		internal bool isGunActive {
			get {
				return m_gunTimer == 0;
			}
		}
		/// <summary>получить значение таймера  
		/// </summary>
		internal int gunTimer {
			get {
				return m_gunTimer;
			}
			set {
				m_gunTimer = value;
			}
		}
		/// <summary>удаленность от отправной точки  
		/// </summary>
		internal int roadLen{
			get{
				return len;
			}
		}
		/// <summary>вести отсчет таймера  
		/// </summary>
		internal int decGunTimer() {
			if (m_gunTimer > 0) {
				m_gunTimer--;
			}
		  return m_gunTimer;
		}
		/// <summary>выяснить в каком направлении двигаться из начальной позиции
		/// </summary>
		private IStep comeBack(Logistics sector) {
			while(sector.back!=mind.I.XY){
				sector = sector.back;
			}
			return Helper.StepsBuilder ( sector.xy,Steps.STEP );
		}
		/// <summary>выяснить есть ли в окрестности аптечи на допустимом растоянии
		/// </summary>
		private void healthyWay(int epsilonX,int epsilonY,Logistics sector,int stepCount, int len, int healthSum, int moneySum) {
			if (len <= stepCount) {
				ICoordinates xy = Helper.Coordinates ( sector.xy.getX () + epsilonX,sector.xy.getY () + epsilonY );
				if (EpsilonBot.api.isNorm ( xy )) {	//если можем идти
					if (sector.back.xy != xy) {					// не родительскя
						if (EpsilonBot.api.getTypeOfField ( xy ) == TypesOfField.MEDKIT) {
							healthSum += Mind.cHealthM;
						}
						if (EpsilonBot.api.getTypeOfField ( xy ) == TypesOfField.BONUS) {
							moneySum += Mind.cMoneyM;
						}
						healthyWay ( +1,+0,Logistics.smallBuilder ( xy,0,0,sector ),stepCount, len+1, healthSum, moneySum );										// извлекаем всех соседей данной вершины
						healthyWay ( -1,-0,Logistics.smallBuilder ( xy,0,0,sector ),stepCount, len+1, healthSum, moneySum );											// и по возможности помещаем в очередь
						healthyWay ( +0,+1,Logistics.smallBuilder ( xy,0,0,sector ),stepCount, len+1, healthSum, moneySum );
						healthyWay ( -0,-1,Logistics.smallBuilder ( xy,0,0,sector ),stepCount, len+1, healthSum, moneySum );
					}
				}
			}
		}
		/// <summary>перебивка начального максимума по аптечкам
		/// </summary>
		private void subEpsilonSectors(int tempMoney,int tempHealth,ref int moneySum,ref int healthSum) {
			if (tempHealth > healthSum) {
				healthSum = tempHealth;
				if (tempMoney > moneySum) {
					moneySum = tempMoney;
				}
			}
		}
		/// <summary>есть ли путь на котором будут аптечки >=2 в пределах досягаемости
		/// </summary>
		/// <returns></returns>
		private bool epsilonSectors(TypesOfField needElement,Logistics startPoint,int stepCount,int roketTimer) {
			int moneySum = 0;
			int healthSum = 0;
			int tempMoney = 0;
			int tempHealth = 0;
			healthyWay ( +1,+0,startPoint,stepCount,Gun.cTimerZeroNone, healthSum,  moneySum  );
			subEpsilonSectors ( tempMoney,tempHealth,ref moneySum,ref healthSum );

			healthyWay ( -1,-0,startPoint,stepCount,Gun.cTimerZeroNone, tempHealth, tempMoney );
			subEpsilonSectors ( tempMoney,tempHealth,ref moneySum,ref healthSum );

			healthyWay ( +0,+1,startPoint,stepCount,Gun.cTimerZeroNone, tempHealth, tempMoney );
			subEpsilonSectors ( tempMoney,tempHealth,ref moneySum,ref healthSum );

			healthyWay ( +0,-1,startPoint,stepCount,Gun.cTimerZeroNone, tempHealth, tempMoney );
			subEpsilonSectors ( tempMoney,tempHealth,ref moneySum,ref healthSum );
			return (healthSum >= 2 * Mind.cHealthM) /*спорная часть*//*|| (moneySum >= 2*Mind.cMoneyM && healthSum >= Mind.cHealthM)*/;
		}
		/// <summary>получить соседей данной вершины 
		///соседи корректны если это не родительские вершины, 
		///и можно без боязненно проходить через них,
		///т.е не обращать внимания на обнаружение противником
		/// </summary>
		/// <param name="epsilonX">отклоненние по х</param>
		/// <param name="epsilonY">отклонение по у</param>
		/// <param name="sector"> текущая вершина</param>
		/// <returns></returns>
		private void possibleWays(int epsilonX,int epsilonY,Logistics sector,ref Queue<Logistics> x) {
				ICoordinates xy = Helper.Coordinates ( sector.xy.getX () + epsilonX,sector.xy.getY () + epsilonY );
				if (EpsilonBot.api.isNorm ( xy )) {
					if (sector.back != xy) {					 // если это не отправная точка
						if (!sector.isGunActive) {			 // если на следующий ход противник не сможет стрелять ракетой
																						 // можем взять то что лежит здесь
							x.Enqueue(Logistics.smallBuilder(xy,sector.decGunTimer(),sector.roadLen+1,sector));	 //добавляем
						}
					}
				}
			}
		/// <summary>	/// расчет пути к сектору карты запрашиваемого типа
		/// </summary>
		/// <param name="needElement">тип ячейки к которой необходимо проложить маршрут</param>
		/// <param name="startPoint">отправная точка</param>
		/// <param name="stepCount">максимальная удаленность объекта</param>
		/// <returns></returns>
		internal IStep wayOfEvil(TypesOfField needElement,Logistics startPoint,int stepCount,int roketTimer) {
			int roadLen = 0;
			bool found = false;
			bool exit = false;
			IStep s = Helper.StepsBuilder ( startPoint.xy,Steps.STEP );
			Queue<Logistics> wayBase = new Queue<Logistics> ();	//база путей возможных за данную длинну
			Queue<Logistics> forWalk = new Queue<Logistics> ();	//для прохода по графу
			Logistics x = new Logistics ( mind.I.XY,mind.He.gun ().getTimerVal,roadLen,startPoint );
			forWalk.Enqueue ( x );

			while (!found && !exit && forWalk.Count > 0 && stepCount > 0 ) {
				//--1-------------------------
				if (forWalk.Peek ().roadLen < stepCount) {
					roadLen = forWalk.Peek ().roadLen;

					while (!exit && roadLen == forWalk.Peek ().roadLen){ // обрабатываем все вершины на данной волне
						x = forWalk.Dequeue ();
                        
						//--2-------------------------
						if (EpsilonBot.api.getTypeOfField ( x.xy ) == needElement) {		// если нашли необходимую ячейку то стоп
							//--3-------------------------
							x.gunTimer=Gun.cTimerZeroNone;
							if (epsilonSectors ( TypesOfField.MEDKIT,x,Math.Min(x.gunTimer,stepCount-roadLen),x.gunTimer)){	// и если есть в окрестности аптечки
								//wayBase.Enqueue ( x );	
								found = true;	
							}//--3------------------------
						} else {																												// иначе
							possibleWays ( +1,+0,x,ref forWalk );											// извлекаем всех соседей данной вершины
							possibleWays ( -1,-0,x,ref forWalk );											// и по возможности помещаем в очередь
							possibleWays ( +0,+1,x,ref forWalk );
							possibleWays ( -0,-1,x,ref forWalk );
						}//--2------------------------
                        exit = forWalk.Count == 0;
					}//--1-------------------------
				} else {		// если радиус волны больше чем длиннна возможного пути
					exit = true;
				}
			}
			if (found) {
				//x = findBestWay ( wayBase );// спорный  момент по выбору путей
				s = comeBack ( x );
			}
			return s;
		}
		private Logistics findBestWay(Queue<Logistics> q) {
			Logistics[] arr = q.ToArray ();
			Logistics temp=Logistics.smallBuilder(arr[0].xy,arr[0].gunTimer,9999999,arr[0].back);
				for (int i = 0; i < arr.Length-1;i++ ) {
				if (arr[i].roadLen< temp.roadLen){
					temp = arr[i];
				}
			}
				return temp;
		}
		/// <summary>конструктор  
		/// </summary>
		/// <param name="needElement">тип ячейки к которой необходимо проложить маршрут</param>
		/// <param name="startPoint">отправная точка</param>
		/// <param name="stepCount">максимальная удаленность объекта</param>
		/// <returns></returns>
		internal Logistics(ICoordinates xy,int gunTimer,int roadLen,Logistics back){
			this.len = roadLen;
			this.m_gunTimer = gunTimer;
			this.m_xy = xy;
			this.m_back = back;
            this.mind = mind;
			
		}
        internal Logistics(Mind mind)
        {
          this.mind = mind;

        }
		/// <summary>удобное представление консруктора anyWalker  
		/// </summary>
		/// <param name="needElement">тип ячейки к которой необходимо проложить маршрут</param>
		/// <param name="startPoint">отправная точка</param>
		/// <param name="stepCount">максимальная удаленность объекта</param>
		/// <returns></returns>
		internal static Logistics smallBuilder(ICoordinates xy,int gunTimer,int roadLen,Logistics back) {
			return new Logistics ( xy,gunTimer,roadLen,back);
		}
		///<summary> обратный вызов
		///</summary>
		internal  void callBack(Mind _mind) {
			this.mind = _mind;
		}
	}
}