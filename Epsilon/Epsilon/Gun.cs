using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Interfaces;

namespace Epsilon {
	internal class Gun {

		// Field

		private int m_roketTimer;

		// CONSTANTS

		private const int cReloadTime = 5;						// надо инициализировать при запуске игры
		internal const bool cRoketUsable = true;
		internal const bool cUnRoketUsable = false;
		internal const int cTimerZeroNone = 0;
		internal const int cLaserM = 5;							// надо инициализировать при запуске игры
		internal const int cRoketM = 25;						// надо инициализировать при запуске игры
		internal const int cLaserF = -10;						// надо инициализировать при запуске игры
		internal const int cRoketF = -50;					 // надо инициализировать при запуске игры
		//internal const int chealthM_laserF = Mind.cHealthM + cLaserF;
		//internal const int chealthM_roketF = Mind.cHealthM + cRoketF;
		// PROPERTIES

		/// <summary>/// получение значения таймера 
		/// </summary>
		internal int getTimerVal {
			get {
				return m_roketTimer;
			}
		}
		/// <summary>/// возможность использовать ракету 
		/// </summary>
		internal bool canRocetFire {
			get {
				return (m_roketTimer == cTimerZeroNone);
			}
		}
		/// <summary>/// возможность использовать ракету
		/// </summary>
		internal bool canIUseRoket {
			get {
				return (m_roketTimer == cTimerZeroNone);
			}
		}
		/// <summary>/// можно ли использовать оружие 
		/// </summary>
		internal bool canIFireOfAny {
			get {
				return this.canRocetFire;
			}
		}
		/// <summary>/// возможность стрельбы из бластера 
		/// </summary>
		internal bool canBlasterFire(ICoordinates xy) {
			return System.Math.Abs ( xy.getX () - EpsilonBot.api.getCoordOfMe ().getX () ) <= 3 &&
						 System.Math.Abs ( xy.getY () - EpsilonBot.api.getCoordOfMe ().getY () ) <= 3 &&
						 this.canIFireOfAny;
		}
		/// <summary>///  выстрел из бластера 
		/// </summary>
		internal Steps blasterFire {
			get {
				return Steps.BLASTER;
			}
		}
		/// <summary>///  выстрел ракетой 
		/// </summary>
		internal Steps roketFire {
			get {
				this.TurnOnTimer ();
				return Steps.ROCKET;
			}
		}
		// Methods
		/// <summary>/// Включение таймера 
		/// </summary>
		internal void TurnOnTimer() {
			m_roketTimer = cReloadTime;
		}
		/// <summary>/// обратный отсчет
		/// </summary>
		internal void decTimer() {
			if (m_roketTimer > 0) {
				m_roketTimer--;
			}
		}
		/// <summary>/// конструктор 
		/// </summary>
		internal Gun() {
			m_roketTimer = cTimerZeroNone;
		}
	}
}