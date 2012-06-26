using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces; 
using CCStepsCoords;
//
// Класс для хранения координат всех бонусов, аптечек, возможного положения врага
// и информацию о состоянии здоровья и очков самого робота и протовников
//
//namespace MasterElement {
	namespace Epsilon {
		internal class Mind {

			private  Person i;
			private  Person he;
			private  int m_stepLimit;
			private  List<MapSector> m_moneyXY;
			private  List<MapSector> m_healthXY;
			private  List<MapSector> m_hisMayBeXY;
			/// <summary>	наиболее удаленная точка от нынешнего местоположения
			/// </summary>
			//-----------------------------------------------
			private ICoordinates m_veryFar;
			/// <summary> размер аптечки	
			/// </summary>
			//-----------------------------------------------
			internal static int cHealthM = 20;
			/// <summary> размер бонуса
			/// </summary>
			//-----------------------------------------------
			internal static int cMoneyM = 100;
			/// <summary>неивестные координаты
			/// </summary>
			//-----------------------------------------------
			internal const int unknownCoord = -1;
			/// <summary> динна видимости
			/// </summary>
			//-----------------------------------------------
			internal const int lookLength   = 5;
			/// <summary>
			/// аптечка - лазер
			/// </summary>
			//-----------------------------------------------
			internal static int chealthM_laserF = Mind.cHealthM - Gun.cLaserF;
			/// <summary>
			/// аптечка - ракета
			/// </summary>
			//-----------------------------------------------
			internal static int chealthM_roketF = Mind.cHealthM - Gun.cRoketF;
			/// <summary> личность игрока	
			/// </summary>
			//-----------------------------------------------
			internal  Person I {
				get {
					return i;
				}
			}
			/// <summary>/// личность противника	
			/// </summary>
			//-----------------------------------------------
			internal  Person He {
				get {
					return he;
				}
			}
			/// <summary>/// список всех бонусов на карте
			/// </summary>
			internal  List<MapSector> moneyXY {
				get {
					return m_moneyXY;
				}
				set {
					m_moneyXY = value;
				}
			}
			//-----------------------------------------------
			internal  List<MapSector> healthXY {
				get {
					return m_healthXY;
				}
				set {
					m_healthXY = value;
				}
			}
			/// <summary>/// список всех аптечек на карте
			/// </summary>
			//-----------------------------------------------
			internal List<MapSector> hisMayBeXY {
				get {
					return m_hisMayBeXY;
				}
				set {
					m_hisMayBeXY = value;
				}
			}
			/// <summary>/// возврат имени	
			/// </summary>
			//-----------------------------------------------
			internal string getMyName {
				get {
					return i.getName;
				}
			}
			/// <summary>	наиболее удаленная точка от нынешнего местоположения
			/// </summary>
			//-----------------------------------------------
			internal ICoordinates veryFar {
				get {
					return m_veryFar;
				}
				set {
					m_veryFar = value;
				}
			}	
			/// <summary>/// обновлене информации на основе статистических данных	
			/// </summary>
			//-----------------------------------------------
			internal void statsistic() {
				if (I.DeltaHelth == Mind.chealthM_laserF || I.DeltaHelth == Gun.cLaserF) {
					He.Money += Gun.cLaserM;
				}
				if (I.DeltaHelth == Mind.chealthM_roketF || I.DeltaHelth == Gun.cRoketF) {
					He.Money += Gun.cRoketM;
					He.gun().TurnOnTimer();
				}
				if (I.DeltaHelth == cHealthM) {
					I.WasFound = true;
				}
				if (I.DeltaMoney == Gun.cLaserM) {
					He.Health -= Gun.cLaserF;
				}
				if (I.DeltaMoney == Gun.cRoketM) {
					He.Health -= Gun.cRoketF;
				}
			}
			/// <summary>/// сканирование карты	
			/// </summary>
			//-----------------------------------------------
			internal void Reload(Radar radar) {
				I.reset (true);
				He.reset (false);
				statsistic ();
				radar.clean ();			// подготовка карты к новому сканированию
				radar.scaning ();		// сканировение карты выявление и анализ изменнений
				
			}
			/// <summary>
			/// сохранение информации за текущий ход
			/// </summary>
			internal void DataSave(/*bool wasFound,bool takeHealth*/) {
				i.dataSave (/* wasFound,takeHealth*/ );
				he.dataSave (/* wasFound,takeHealth*/ );
			}
			//---------------------------
			//Build
			internal Mind(int stepLimit,ICoordinates xy) {
				i = new Person ( "Epsilon",xy );
				ICoordinates unknown = StepsAndCoord.Coordinates ( unknownCoord,unknownCoord );
				he = new Person ( "Frag",unknown );
				healthXY = new List<MapSector> ();
				moneyXY = new List<MapSector> ();
				hisMayBeXY = new List<MapSector> ();
				this.m_stepLimit =  stepLimit;
				veryFar = xy;
			}
		}
	}