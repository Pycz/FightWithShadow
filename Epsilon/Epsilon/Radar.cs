using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces; 
using CCStepsCoords;

// класс содержит набор методов для сканирования карты с целью поиска противника и составлеию баз аптечек и бонусов
namespace Epsilon {


	internal class Radar {

		// Fields
		/// <summary>/// модель игрового поля	
		/// </summary>
		private static MapSector[,] map; // табличное представление графа
		/// <summary>/// память робота	
		/// </summary>
		private Mind   mind;						// доступ к памяти робота	 осуществление обратного вызова
		/// <summary>/// информация о только что просмотренных вершинах	
		/// </summary>
		private Queue<MapSector> recycleSectors;
		/// <summary>/// информация о только что просмотренных вершинах	
		/// </summary>
		private IAPI api;
		///Propeties

		///Methods
			
		/// <summary>/// была ли обработана вершина	
		/// </summary>
		internal static bool usableSector(ICoordinates xy) {		 
			return map[xy.getX (),xy.getY ()].usable;
		}
		//-----------------------------------------------
		/// <summary>	/// Получение типа для сектора карты из базы
		/// </summary>
		internal static TypesOfField getSectorsType(ICoordinates xy) {	 // тип клетки поля
			return map[xy.getX (),xy.getY ()].type;
		}
		//-----------------------------------------------
		/// <summary>	/// выделение указателей на вершины графа определенного типа в соответсвующую базу
		/// </summary>
		private  void simpleAddBase(MapSector sector,TypesOfField btypes,TypesOfField types){
				if (types == TypesOfField.MEDKIT){	 // то просто добавить в базу
					mind.healthXY.AddFirst(sector);
				}
				if (types == TypesOfField.BONUS){
					mind.moneyXY.AddFirst ( sector );
				}
				if (types == TypesOfField.HI){
					if (!mind.hisMayBeXY.Contains ( sector )) {
						mind.hisMayBeXY.AddFirst ( sector );
					}
				}
		}
		//-----------------------------------------------
		/// <summary>	/// пердсказание координат противника (вспомогательная ф- ия для whereHe())
		/// </summary>
		private void mayBeHere(int x,int y,LinkedList<MapSector> hisMayBeXY,MapSector sector){
			ICoordinates xy=Helper.Coordinates(x,y);
			if (EpsilonBot.api.isNorm(xy)){																				// если внутри поля
				TypesOfField type = api.getTypeOfField ( xy );
				if ( type != TypesOfField.WALL){											 // если не стена
					if (!mind.He.WasFound) {															 // если не найден
						if (type == TypesOfField.HI){						 // если нашл то в базу заносим 1 значение - точные координаты
							hisMayBeXY.Clear();
							hisMayBeXY.AddFirst(sector);
							mind.He.WasFound = true;
						}
						if (!mind.He.WasFound && type == TypesOfField.NOTHING && !api.isVisible ()) {
							hisMayBeXY.AddFirst ( sector );// если пустая клетка, и его не видно то он может быть там										 
						}
					}
				}
			}
		}
		//-----------------------------------------------
		/// <summary>	/// Вычисление предположительного местоположения Противника относительно предыдущих координат
		/// </summary>
		private  void whereHe(MapSector sector,TypesOfField btype,TypesOfField type){
			if (!mind.He.WasFound) {
				//int a = 1;
				//int b = 0;
				//for (int i = 1; i <= 4; i++) {
				//  mayBeHere ( sector.xy.getX () + a,sector.xy.getY () + b,mind.hisMayBeXY,sector );
				//  if (i == 1 || i==3) {
				//    a = -a;
				//    b = -b;
				//  }
				//  if (i == 2) {
				//    b = -a;
				//    a = 0;
				//  }
				//}
				mayBeHere ( sector.xy.getX () + 1,sector.xy.getY (),mind.hisMayBeXY,sector );
				mayBeHere ( sector.xy.getX () - 1,sector.xy.getY (),mind.hisMayBeXY,sector );
				mayBeHere ( sector.xy.getX (),sector.xy.getY () - 1,mind.hisMayBeXY,sector );
				mayBeHere ( sector.xy.getX (),sector.xy.getY () + 1,mind.hisMayBeXY,sector );
			}
		}
		//-----------------------------------------------
		/// <summary>	/// Анализ изменений содержимого сектора карты
		/// </summary>
		private void sectorAnalysis(MapSector sector,TypesOfField btype,TypesOfField type) {
			if (btype == TypesOfField.HI) {	// если до этого там был противник то вычисляем куда он направился
				whereHe ( sector,btype,type );
				simpleAddBase ( sector,btype,type );	// и перебиваем базу
			}
			if (btype == TypesOfField.NOTHING) {	 // если до этого небыло ничего
				simpleAddBase ( sector,btype,type ); // просто перебиваем базу
			}
			if (btype == TypesOfField.MEDKIT || btype == TypesOfField.BONUS) {		// была аптечка или бонус 
				if (type == TypesOfField.NOTHING) {																  // а теперь пусто то он там
					//if (api.isVisible ( api.getCoordOfMe (),sector.xy/*api.getCoordOfEnemy ()*/ )) {
						mayBeHere ( sector.xy.getX (),sector.xy.getY (),mind.hisMayBeXY,sector );
						if (btype == TypesOfField.MEDKIT) {
							mind.He.Health += Mind.cHealthM;
							mind.He.WasFound = true;
						}
						if (btype == TypesOfField.BONUS) {
							mind.He.Money += Mind.cMoneyM;
							mind.He.WasFound = true;								
						}
						if (mind.He.WasFound) {
							mind.hisMayBeXY.Clear ();
							mind.hisMayBeXY.AddFirst ( sector );
							mind.He.XY = sector.xy;
						}
						if (btype == TypesOfField.ME) {		// а теперь он знает где я
							mind.I.WasFound = true;
						}
					//}
				}
			}
		}
		//-----------------------------------------------	
		/// <summary>	/// анализ измений значений вершин
		/// </summary>
		internal void mapSectorReLoad(MapSector sector) {
			TypesOfField type = api.getTypeOfField ( sector.xy );
			TypesOfField btype = Radar.getSectorsType ( sector.xy );
			if (btype != type) {						// если тип  поля изменился
				if (type == TypesOfField.HI){// он
					mayBeHere ( sector.xy.getX (),sector.xy.getY (),mind.hisMayBeXY,sector );	//  обновить  его координаты
				}else{																	 // если что нибудь другое
					this.sectorAnalysis ( sector,btype,type );	 // анализируем прошлое значение
				}
				sector.type = api.getTypeOfField ( sector.xy );		// изменяем значения на новые на таблице
			}
		}
		//-----------------------------------------------
		/// <summary>	/// сканирование карты с просчетом минимального пути до каждой вершины из расположения бота
		/// </summary>
		internal void scaning( ) {	 
			int distance = 1;
			Queue<MapSector> queue = new Queue<MapSector> ();
			MapSector temp = new MapSector ( api.getCoordOfMe (),api.getCoordOfMe (),distance );
			queue.Enqueue (temp);

			int levelCount = 1;			 // кол-во вершин на данной волне
			int nextLevelCount = 0;	 // на след волне

			while ( queue.Count>0 ) {
				while ( levelCount>0 ) {
					temp = queue.Dequeue ();											// достали вершину из очереди, пора узнать что в ней изменилось
                    if (api.isNorm(temp.xy))
                    {
                        this.mapSectorReLoad(temp);									// анализ изменений в ячейке
                        queue.Concat ( temp.getAllUsableSectors ( distance,ref nextLevelCount ) );  	//всех соседей данной вершины графа 																										
                        levelCount--;																																	// добавили в имеющуюся очередь
                        // обновили информацию о вершине
                        recycleSectors.Enqueue(temp);
                    }
				}																																								
				levelCount = nextLevelCount;
				nextLevelCount = 0;
				distance++;				 // удаленность волны
			}
		}
		//-----------------------------------------------
		/// <summary>/// подготовка к новому сканированию использованных на данном ходу ячеек
		/// </summary>
		internal void clean() {
			while (recycleSectors.Count > 0) {
				recycleSectors.Dequeue ().cleanToStep ();
			}
		}
		/// <summary>/// Конструктор
		/// </summary>
		internal Radar(Mind mind,IAPI api) {
			map = new MapSector[api.getMaxX()+1,api.getMaxY()+1];		// создается поле для сканирования
			map.Initialize ();
			this.mind = mind;
			this.api = api;
			recycleSectors = new Queue<MapSector>();
			ICoordinates xy = Helper.Coordinates ( api.getMinX (),api.getMinY () );
			for (int i = api.getMinX (); i <= api.getMaxX (); i++) {
				for (int j = api.getMinY (); j <= api.getMaxY (); j++) {

					Helper.otherPositionOfThis ( xy,i,j );
                    if (api.isNorm(xy))
                    {
                        map[i, j] = new MapSector(xy, xy, 0);
                        if (api.getTypeOfField(xy) == TypesOfField.WALL)
                        {
                            map[i, j].type = TypesOfField.WALL;
                        }
                        else
                        {
                            map[i, j].type = TypesOfField.NOTHING;
                        }
                    }
				}
			}
		}
	}
}	 