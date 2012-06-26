using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces; 
using CCStepsCoords;

	namespace Epsilon {

		internal class Person {
			// FIELDS
			private string m_name;
			private int m_Health;
			private int m_Money;
			private int m_HealthLast;
			private int m_MoneyLast;
			private Steps typeLastStep;
			private Gun m_gun;
			private ICoordinates m_XY;
			private bool m_wasFound;
		//	private bool m_takeHelth; 
			// CONSTANTS
			private const int cHealthMax = 100;		 // надо инициализировать при запуске игры
			private const int cBaseMoney = 0;			 // надо инициализировать при запуске игры
			// PROPERTIES
			internal ICoordinates XY {
				get {
					return m_XY;
				}
				set {
					m_XY = value;
				}
			}
			internal string getName {
				get {
					return m_name;
				}
			}
			internal int Health {
				set {
					m_Health = value;
					if (m_Health > cHealthMax) {
						m_Health = cHealthMax;
					}
				}
				get {
					return m_Health;
				}
			}
			internal int HealthLast {
				set {
					m_HealthLast = value;
				}
				get {
					return m_HealthLast;
				}
			}
			internal int Money {
				set {
					m_Money = value;
				}
				get {
					return m_Money;
				}
			}
			internal int MoneyLast {
				set {
					m_MoneyLast = value;
				}
				get {
					return m_MoneyLast;
				}
			}
			internal bool WasFound {
				get {
					return m_wasFound;
				}
				set {
					m_wasFound = value;
				}
			}
			//internal bool TakeHealth {
			//  get {
			//    return m_takeHelth;
			//  }
			//}
			internal int DeltaHelth {
				get {
					return HealthLast - Health;
				}
			}
			internal int DeltaMoney {
				get {
					return m_MoneyLast - m_Money;
				}
			}
			// Methods
			internal Gun gun() {
				return m_gun;
			}
			internal void reset(bool need) {
				if (need) {
					Health = EpsilonBot.api.myHealth ();
					Money = EpsilonBot.api.myPoints ();
				}
				WasFound = false;
				gun ().decTimer ();
			}
			/// <summary>/// сохранение текущего капитала и здоровья	
			/// </summary>
			internal void dataSave(/*bool wasFound,bool takeHelth*/) {
				HealthLast = Health;
				MoneyLast = Money;
				//WasFound = wasFound;
				//m_takeHelth = takeHelth;
			}
			// Builder
			internal Person(string name,ICoordinates xy) {
				m_name = name;
				Health = HealthLast = EpsilonBot.api.myHealth ();// cHealthMax;
				Money = MoneyLast = 0;
				m_gun = new Gun ();
				m_XY = xy;
				m_wasFound = false;
			}
		}
	}