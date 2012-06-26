using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces; 
using CCStepsCoords;

namespace Epsilon {
	// 
	//Classes
	//
	internal class MapSector {
		//
		//Fields
		//
		private  TypesOfField m_type;
		private  ICoordinates m_xy;
		private  ICoordinates[] mom;
		private  int m_roadLength;
		private  int i;
		

		//Propeties
		ICoordinates this[int i] {
			set {
				if (i >= 0 && i < 2) {
					mom[i] = value;
					momCountInc();
				}
			}
			get {
				if (i >= 0 && i < 2) {
					return mom[i];
				} else {
					return Helper.Coordinates ( -1,-1 );
				}
			}
		}
		internal bool usable {
			get {
				return i < 2;
			}	
		}
		internal TypesOfField type {
			get {
				return m_type;
			}
			set {
				m_type = value;
			}
		}
		internal int roadLength {
			get {
				return m_roadLength;
			}
			set {
				m_roadLength = value;
			}
		}
		internal ICoordinates xy {
			get {
				return m_xy;
			}
			set {
				m_xy = value;
			}
		}
		//Methods
		/// <summary>
		/// 
		/// </summary>
		private void momCountInc() {
			i++;
		}
		/// <summary> затирка координат материнских вершин
		/// </summary>
		internal void cleanToStep() {
			i = 0;//mom = new ICoordinates[3];
		}
		/// <summary>добавляет клетку поля для дальнейшей обработки при проходе карты в глубь - в ширь  
		/// </summary>
		private void getUsableSector(int x,int y,Queue<MapSector> result,int len,ref int nextLevelCount) { 
			ICoordinates temp = Helper.Coordinates ( x,y );
			if (EpsilonBot.api.isNorm ( temp )) {																																						// если внутри поля
				if ((EpsilonBot.api.getTypeOfField ( temp ) != TypesOfField.WALL) && Radar.usableSector ( temp )) {	// если можно ходить
					MapSector f = new MapSector ( this.xy,temp,len );  // сама вершина станиовится мамой				// и вершина не была посещена
					result.Enqueue ( f );															 // помещаем в очередь для дальнейшей обработки
					nextLevelCount ++;
				} 
			}
		}
		/// <summary>/// получает все соседние вершины для данной вершины неоренитированного графа 
		/// </summary>
		internal Queue<MapSector> getAllUsableSectors(int len,ref int nextLevelCount) {		
			Queue<MapSector> queue = new Queue<MapSector> ();															  //  в которые можно пройти кроме родительской
			this.getUsableSector ( this.xy.getX () + 1,this.xy.getY (),queue,len,ref  nextLevelCount );
			this.getUsableSector ( this.xy.getX () - 1,this.xy.getY (),queue,len,ref  nextLevelCount );
			this.getUsableSector ( this.xy.getX (),this.xy.getY () + 1,queue,len,ref  nextLevelCount );
			this.getUsableSector ( this.xy.getX (),this.xy.getY () - 1,queue,len,ref  nextLevelCount );
			return queue;
		}

		internal void setValue(ICoordinates back,int roadLength) {
			this.mom[1] = back;
			this.roadLength = roadLength;
		}
		//Builder
		internal MapSector( ) {
			this.mom = new ICoordinates[3];
			this.mom = null;
			this.xy = null;
			roadLength = 0;
			i = 0;
			type = TypesOfField.NOTHING;
		}
		internal MapSector(ICoordinates mom,ICoordinates I,int roadlength) {
			i = 0;
			this.mom = new ICoordinates[3];
			this[i]= mom;
			this.xy = I; 
			roadLength = roadlength;
			type = TypesOfField.WALL;
		}
	}
}
