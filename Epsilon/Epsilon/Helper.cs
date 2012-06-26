using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces; 
using CCStepsCoords;

namespace Epsilon {
	internal class Helper {

		/// <summary>	/// Перегрузка стандартного консруктора	 CCoordinates
		/// </summary>
		internal static ICoordinates Coordinates(int x,int y) {
			ICoordinates temp = new CCoordinates ();																														 //дальнейшей обработки если возможно
			temp.setX ( x );
			temp.setY ( y );
			return temp;
		}
		/// <summary>/// Смена значений 
		/// </summary>
		internal static void otherPositionOfThis(ICoordinates c,int x,int y) {
			c.setX ( x );
			c.setY ( y );
		}
		internal static IStep randomVectorStep(ICoordinates xy) {
			Random r = new Random ();
			ICoordinates t=xy;
			bool found=false;
			int i =-1;
			while (!found) {
				i = r.Next ( 3 );
				switch (i) {
					case 0: {
						t = Helper.Coordinates ( xy.getX () + 1,xy.getY () + 0 );
						found = EpsilonBot.api.isNorm ( t );
					} break;
					case 1: {
						t = Helper.Coordinates ( xy.getX () - 1,xy.getY () + 0 );
						found = EpsilonBot.api.isNorm ( t );
					} break;
					case 2: {
						t = Helper.Coordinates ( xy.getX () + 0,xy.getY () + 1 );
						found = EpsilonBot.api.isNorm ( t );
					} break;
					case 3: {
						t = Helper.Coordinates ( xy.getX () + 0,xy.getY () - 1 );
						found = EpsilonBot.api.isNorm ( t );
					} break;
				}
			}
			return Helper.StepsBuilder ( t,Steps.STEP );
		}
		/// <summary>/// Перегрузка конструктора CStep
		/// </summary>
		internal static IStep StepsBuilder(ICoordinates coord,Steps step) {
			IStep s = new CStep ();
			s.setCoord ( coord );
			s.setTypeOfStep (step);
			return s;
		}
	}
}
