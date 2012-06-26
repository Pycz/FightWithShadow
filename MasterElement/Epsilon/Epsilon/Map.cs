using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using CCStepsCoords;

namespace Epsilon {
	public class Map {

		private static int m_minX;
		private static int m_minY;
		private static int m_weigth;
		private static int m_heigth;
		internal MapSector[,] map;


	 /// <summary>
		/// доступ к карте
	 /// </summary>
	 /// <param name="i">x</param>
	 /// <param name="j">y</param>
	 /// <returns></returns>
		internal MapSector this[int i,int j] {
			get {
				return map[i,j];
			}
			set {
				map[i,j] = value;
			}
		}

		/// <summary>
		////ширина
		/// </summary>
		internal int weight {
			get {
				return m_weigth;
			}
		}
		/// <summary>
		/// высота
		/// </summary>
		internal int heigth {
			get {
				return m_heigth;
			}
		}
		/// <summary>
		/// сброс значений для прохода вширину
		/// </summary>
		internal void resetForWave() {
			for (int i = m_minX; i < weight; i++) {						 // проход по карте построчно
				for (int j = m_minY; j < heigth; j++) {
					map[i,j].usableForWave = true;
				}
			}
		}

		/// <summary>
		////Получение значения карты по координате
		/// </summary>
		/// <param name="xy">координаты запрашиваемой ячейки</param>
		/// <returns>клетка карты</returns>
		internal MapSector getMapSector(ICoordinates xy) {
			return map[xy.getX (),xy.getY ()];
		}
		/// <summary>
		/// добавляет клетку поля для дальнейшей обработки при проходе карты в глубь - в ширь
		/// </summary>
		/// <param name="x">смещение по х</param>
		/// <param name="y">смещение по у</param>
		/// <param name="result"></param>
		private void getUsableSector(int x,int y,List<ICoordinates> result) {
			ICoordinates temp = StepsAndCoord.Coordinates ( x,y );
			if (EpsilonBot.api.isNorm ( temp )) {	// если внутри поля

				if ((EpsilonBot.api.getTypeOfField ( temp ) != TypesOfField.WALL) && 
							map[x,y].usableForWave) {	// если не стена и еще не посещена
					//MapSector f = new MapSector ();  	// и вершина не была посещена
					result.Add ( temp );							 // помещаем в очередь для дальнейшей обработки
					map[x,y].usableForWave = false;
				}
			}
		}
		/// <summary>
		////получить всех соседей данной вершины
		/// </summary>
		/// <returns></returns>
		internal List<ICoordinates> getAllUsableSectors(MapSector s,Queue<ICoordinates> q) {
			List<ICoordinates> list = new List<ICoordinates> ();															  //  в которые можно пройти кроме родительской
			this.getUsableSector ( s.xy.getX () + 1,s.xy.getY () + 0,list );
			this.getUsableSector ( s.xy.getX () - 1,s.xy.getY () - 0,list );
			this.getUsableSector ( s.xy.getX () + 0,s.xy.getY () + 1,list );
			this.getUsableSector ( s.xy.getX () - 0,s.xy.getY () - 1,list );
			for (int i = 0; i < list.Count; i++) {
				q.Enqueue ( list[i] );
			}
			return list;
		}
		/// <summary>
		/// Размер карты
		/// </summary>
		public  Map(int minX,int minY,int w,int h) {
			m_minX = minX;
			m_minY = minY;
			m_weigth = w;
			m_heigth = h;
			map = new MapSector[w,h];
		}
	}
}
