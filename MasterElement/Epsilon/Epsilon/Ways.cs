using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using CCStepsCoords;

namespace Epsilon {
	class Ways {

		private ICoordinates m_back;
		private int m_money;
		private int m_health;
		private int m_len;
		private Gun m_gun;
		private bool m_heCanFire;
		private ICoordinates m_xy;
		public TypesOfField type;


		public override string ToString() {
			return xy.X1.ToString () + " " + xy.Y1.ToString() + " | (back = " + back.X1.ToString() + " "  + back.Y1.ToString()+ ")";
		}


		/// <summary> возврат копии сектора
		/// </summary>
		/// <param name="w"></param>
		/// <returns></returns>
		//internal  void q212copyWays(Ways w) {
		//  back.Copy ( w.back );
		//  money = w.money;
		//  health = w.health;
		//  len = w.len;
		//  gun ().TurnOnTimer ( w.gun().getTimerVal );
		//  xy = new CCoordinates ();
		//  xy.Copy ( w.xy );
		//}

		internal Ways Clone() {
			Ways x = new Ways ();
			x.back.Copy(back);
			x.money = money;
			x.health = health;
			x.len = len;
			x.gun().TurnOnTimer ( gun ().getTimerVal );
			CCoordinates XY = new CCoordinates ();
			XY.Copy( this.xy );
			x.xy = XY;
			return x;
		}

		internal ICoordinates copyBackCoord() {
			ICoordinates d=new CCoordinates();
			d.Copy(this.back);
			return d;	
		}
		//-----------------------------------------------
		/// <summary>
		/// </summary>
		/// <param name="x">отклоннения от родительской вершины</param>
		/// <param name="y">отклоннения от родительской вершины</param>
		/// <param name="result">список выходных значений</param>
		/// <param name="unUseNode">вершины побывавшие в очереди</param>
		/// <param name="parent">родительская вершина</param>
		internal void getUsableSector(int x,int y,List<Ways> result,List<Ways> unUseNode,Ways parent) {
			Ways temp = new Ways ( parent.xy, parent.money, 
														 parent.health, parent.len,
														 parent.gun().getTimerVal, 
														 StepsAndCoord.Coordinates ( x,y ) );
			temp.xy = StepsAndCoord.Coordinates ( x,y );
			if (EpsilonBot.api.isNorm ( temp.xy )) {	// если внутри поля
				// если не стена и еще не посещена
				if (( EpsilonBot.api.getTypeOfField ( temp.xy ) != TypesOfField.WALL) && !isFound(temp.xy,unUseNode)  ) {	
					result.Add ( temp.Clone() );							 // помещаем в очередь для дальнейшей обработки
					unUseNode.Add ( temp.Clone() );
				}
			}
		}
		//-----------------------------------------------
		/// <summary>Возврат всех соседей 
		/// </summary>
		/// <param name="x">отклонение по х</param>
		/// <param name="y">отклонение по у</param>
		/// <param name="result">результирующий лист</param>
		/// <param name="unUseNode"> список использованных вершин</param>
		/// <param name="parent">родительская вершина</param>
		internal List<Ways> getAllUsableSectors(List<Ways> unUsableSector,Queue<Ways> q2) {
			Ways s = this;
			List<Ways> list = new List<Ways> (); //  в которые можно пройти кроме родительской
			this.getUsableSector ( s.xy.getX () + 1,s.xy.getY () + 0,list,unUsableSector,s );
			this.getUsableSector ( s.xy.getX () - 1,s.xy.getY () - 0,list,unUsableSector,s );
			this.getUsableSector ( s.xy.getX () + 0,s.xy.getY () + 1,list,unUsableSector,s );
			this.getUsableSector ( s.xy.getX () - 0,s.xy.getY () - 1,list,unUsableSector,s );
			for (int k = 0; k < list.Count; k++) {
				q2.Enqueue ( list[k] );
			}
			return list;
		}
		internal List<Ways> getAllUsableSectors(List<Ways> unUsableSector,Stack<Ways> s2) {
			Ways s = this;
			List<Ways> list = new List<Ways> (); //  в которые можно пройти кроме родительской
			this.getUsableSector ( s.xy.getX () + 1,s.xy.getY () + 0,list,unUsableSector,s );
			this.getUsableSector ( s.xy.getX () - 1,s.xy.getY () - 0,list,unUsableSector,s );
			this.getUsableSector ( s.xy.getX () + 0,s.xy.getY () + 1,list,unUsableSector,s );
			this.getUsableSector ( s.xy.getX () - 0,s.xy.getY () - 1,list,unUsableSector,s );
			for (int k = 0; k < list.Count; k++) {
				s2.Push ( list[k] );
			}
			return list;
		}
		//-----------------------------------------------
		/// <summary>
		/// получение индекса в списке
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="l"></param>
		/// <returns></returns>
		internal int getIndexIn(ICoordinates c1,List<Ways> l) {
			int i = -1;
			int j;
			bool found = false;
			for ( j = 0; j < l.Count && !found; j++) {
				found = StepsAndCoord.isEcualCoord ( c1,l[j].xy );
			}
			if (found) {
				i = j-1;
			}
			return i;
		}
		//-----------------------------------------------
		/// <summary>есть ли в списке
		/// </summary>
		/// <param name="c"></param>
		/// <param name="l"></param>
		/// <returns></returns>
		internal bool isFound(ICoordinates c,List<Ways> l) {
			return getIndexIn ( c,l ) > -1;
		}
		//-----------------------------------------------
		/// <summary>
		/// ссылка на предка
		/// </summary>
		internal ICoordinates back {
			get {
				return m_back;
			}
			set {
				m_back.Copy( value);
			}
		}
		/// <summary>
		/// количество денег на данном маршруте
		/// </summary>
		internal int money {
			get {
				return m_money;
			}
			set {
				m_money = value;
			}
		}	 
		/// <summary>
		/// количество здоровья
		/// </summary>
		internal int health {
			get {
				return m_health;
			}
			set {
				m_health = value;
			}
		}
		//-----------------------------------------------
		/// <summary>
		/// длиннна маршрута
		/// </summary>
		internal int len {
			get {
				return m_len;
			}
			set {
				m_len = value;
			}
		}
		//-----------------------------------------------
		/// <summary>
		/// таймер оружия противника
		/// </summary>
		/// <summary> координаты
		/// </summary>
		internal ICoordinates xy {
			get {
				return m_xy;
			}
			set {
				m_xy = value;
			}
		}
		//-----------------------------------------------
		/// <summary>контроль за оружием противника
		/// </summary>
		/// <returns></returns>
		internal Gun gun() {
			return m_gun;
		}
		//-----------------------------------------------
		/// <summary>Конструктор
		/// </summary>
		/// <param name="back"></param>
		/// <param name="money">количество денег на данном маршруте</param>
		/// <param name="health">количество аптечек на данном маршруте</param>
		/// <param name="len">длиннна маршрута</param>
		/// <param name="gunTime">активность оружия противника</param>
		/// <param name="xy">координаты исходного (данного) сектора</param>
		internal Ways(ICoordinates back,int money,int health,int len,int gunTime,ICoordinates xy){
			m_back = new CCoordinates ();
		  m_back.Copy(back);
			m_money = money;
		  m_health=health;
		  m_len=len;
			m_gun = new Gun ();
		  m_gun.TurnOnTimer(gunTime);
			m_xy = new CCoordinates ();
		  m_xy.Copy(xy);
		}
		internal Ways() {
			m_back = new CCoordinates ();
			m_gun = new Gun ();
			m_xy = new CCoordinates ();
		}
	}
}
