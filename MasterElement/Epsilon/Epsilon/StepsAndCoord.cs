using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces; 
using CCStepsCoords;

namespace Epsilon {
	internal class StepsAndCoord {



		/// <summary>копирование Списка
		/// </summary>
		/// <param name="m">что копируем</param>
		/// <returns>новый список, старые значения новые ссылки </returns>
		internal static List<Ways> cloneList(List<Ways> m) {
			List<Ways> l = new List<Ways> ();
			for (int i = 0; i < m.Count; i++) {
				l[i].money = m[i].money;
				l[i].health = m[i].health;
				l[i].len = m[i].len;
				l[i].gun ().TurnOnTimer ( m[i].gun ().getTimerVal );
				l[i].xy = StepsAndCoord.Coordinates ( m[i].xy.getX (),m[i].xy.getY () );
				l[i].back = StepsAndCoord.Coordinates ( m[i].back.getX (),m[i].back.getY () );
			}
			return l;
		}

		/// <summary>
		/// проверка на равенство координат
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		internal static bool isEcualCoord(ICoordinates c1,ICoordinates c2){
			return c1.getX () == c2.getX () && c1.getY () == c2.getY ();
		}
		//-----------------------------------------------
		/// <summary>	/// Перегрузка стандартного консруктора	 CCoordinates
		/// </summary>
		//-----------------------------------------------
		internal static ICoordinates Coordinates(int x,int y) {
			ICoordinates temp = new CCoordinates ();	 //дальнейшей обработки если возможно
			otherPositionOfThis ( temp,x,y );
			return temp;
		}
		internal static ICoordinates Coordinates(ICoordinates c,int x,int y) {
			ICoordinates temp = new CCoordinates ();	 //дальнейшей обработки если возможно
			otherPositionOfThis ( temp,c.getX()+x,c.getY()+y );
			return temp;
		}
		/// <summary>/// Смена значений 
		/// </summary>
		//-----------------------------------------------
		internal static void otherPositionOfThis(ICoordinates c,int x,int y) {
			c.setX ( x );
			c.setY ( y );
		}
		/// <summary> Рандомный шаг из данных координат
		/// </summary>
		/// <param name="xy"></param>
		/// <returns></returns>
		//-----------------------------------------------
		internal static IStep randomVectorStep(ICoordinates xy) {
			Random r = new Random ();
			ICoordinates t=xy;
			bool found=false;
			int i =-1;
			while (!found) {
				i = r.Next ( 3 );
				switch (i) {
					case 0: {
						t = StepsAndCoord.Coordinates ( xy.getX () + 1,xy.getY () + 0 );
					} break;
					case 1: {
						t = StepsAndCoord.Coordinates ( xy.getX () - 1,xy.getY () + 0 );
					} break;
					case 2: {
						t = StepsAndCoord.Coordinates ( xy.getX () + 0,xy.getY () + 1 );
					} break;
					case 3: {
						t = StepsAndCoord.Coordinates ( xy.getX () + 0,xy.getY () - 1 );
					} break;
				}
				if (EpsilonBot.api.isNorm ( t )) {
					found = EpsilonBot.api.getTypeOfField ( t ) != TypesOfField.WALL;
				}
			}
		
			return StepsAndCoord.StepsBuilder ( t,Steps.STEP );
		}
		/// <summary>/// Перегрузка конструктора CStep
		/// </summary>
		//-----------------------------------------------
		internal static IStep StepsBuilder(ICoordinates coord,Steps step) {
			IStep s = new CStep ();
			s.setCoord ( coord );
			s.setTypeOfStep (step);
			return s;
		}
	}
}
