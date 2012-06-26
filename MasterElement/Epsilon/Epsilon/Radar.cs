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
		/// <summary> модель игрового поля
		/// табличное представление графа
		/// </summary>
		internal static Map map; // 
		/// <summary> память робота	
		/// </summary>
		private Mind   mind;					
		/// <summary> информация о только что просмотренных вершинах	
		/// </summary>
		private Queue<MapSector> recycleSectors;

		/// <summary> ссылка для обратного вызова
		/// </summary>
		private IAPI api;
		/// <summary>/// была ли обработана вершина	
		/// </summary>
		//-----------------------------------------------
		/// <summary>	Получение типа для сектора карты из базы
		/// </summary>
		internal static TypesOfField getSectorsType(ICoordinates xy) {	 // тип клетки поля
			return map[xy.getX (),xy.getY ()].type;
		}
		//-----------------------------------------------
		/// <summary>	/// выделение указателей на вершины графа определенного типа в соответсвующую базу
		/// </summary>
		private  void simpleAddBase(MapSector sector,TypesOfField types){
				if (types == TypesOfField.MEDKIT){	 // то просто добавить в базу
					mind.healthXY.Add(sector);
				}
				if (types == TypesOfField.BONUS){
					mind.moneyXY.Add ( sector );
				}
				if (types == TypesOfField.HI){
					mayBeHere ( sector.xy, mind.hisMayBeXY );
				}
		}
		//-----------------------------------------------
		/// <summary>	пердсказание координат противника (вспомогательная ф- ия для whereHe())
		/// </summary>
		private void mayBeHere(ICoordinates xy,List<MapSector> hisMayBeXY){
			if (EpsilonBot.api.isNorm(xy)){													// если внутри поля
				TypesOfField type = api.getTypeOfField ( xy );
				if ( type != TypesOfField.WALL){											 // если не стена
					if (!mind.He.WasFound) {														 // если не найден
						//if (type == TypesOfField.HI){						 // если нашл то в базу заносим 1 значение - точные координаты
							//hisMayBeXY.Clear();
							hisMayBeXY.Add(map[xy.getX(),xy.getY()]);
							map[xy.getX (),xy.getY ()].type = TypesOfField.NOTHING;
					}
				}
			}
		}
		//-----------------------------------------------
		/// <summary> сохранение информации обнаружение противника
		/// </summary>
		/// <param name="xy">его координаты</param>
		private void heDetected(ICoordinates xy) {
			map.map[xy.getX (),xy.getY ()].type = TypesOfField.HI;
			mind.He.WasFound = true;
			mind.He.XY = xy;	
		}
		//-----------------------------------------------
		/// <summary>	/// Вычисление предположительного местоположения Противника относительно предыдущих координат
		/// </summary>
		private  void whereHe(ICoordinates xy,TypesOfField btype,TypesOfField type){
			if (!mind.He.WasFound) {
				ICoordinates c ;
				c = StepsAndCoord.Coordinates ( xy.getX () + 0,xy.getY () + 0 );
				mayBeHere ( c,mind.hisMayBeXY );
				c = StepsAndCoord.Coordinates  (xy.getX () + 1,xy.getY () + 0 );
				mayBeHere ( c, mind.hisMayBeXY );
				c = StepsAndCoord.Coordinates ( xy.getX () - 1,xy.getY () + 0 );
				mayBeHere ( c,mind.hisMayBeXY );
				c = StepsAndCoord.Coordinates ( xy.getX () + 0,xy.getY () - 1 );
				mayBeHere ( c,mind.hisMayBeXY );
				c = StepsAndCoord.Coordinates ( xy.getX () + 0,xy.getY () + 1 );
				mayBeHere ( c,mind.hisMayBeXY );
			}
		}
		//-----------------------------------------------
		/// <summary>	/// Анализ изменений содержимого сектора карты
		/// </summary>
		private void sectorAnalysis(ICoordinates xy,TypesOfField btype,TypesOfField type,int distance) {
			// если до этого там был противник, а теперь исчез, то вычисляем куда он направился
			if (btype == TypesOfField.HI) {
				whereHe ( xy,btype,type );
			}
			if (btype == TypesOfField.MEDKIT || btype == TypesOfField.BONUS) {		// была аптечка или бонус 
				if (type == TypesOfField.NOTHING) {																  // а теперь пусто то он там
					//	mayBeHere ( xy,mind.hisMayBeXY );

						if (btype == TypesOfField.MEDKIT) {
							mind.He.Health += Mind.cHealthM;
							mind.He.WasFound = true;
						}
						if (btype == TypesOfField.BONUS) {
							mind.He.Money += Mind.cMoneyM;
							mind.He.WasFound = true;								
						}
						if (mind.He.WasFound) {
							heDetected ( xy );
						}
						if (btype == TypesOfField.ME) {		// а теперь он знает где я
							mind.I.WasFound = true;
						}
					//}
				}
			}
			if (btype==TypesOfField.NOTHING){
				MapSector m = new MapSector ();
				m.usableForWave = true;
				m.type = type;
				m.xy.Copy ( xy );
			}
		}
		//-----------------------------------------------	
		/// <summary>	/// анализ измений значений вершин
		/// </summary>
		internal void mapSectorReLoad(ICoordinates xy,int distance) {
			TypesOfField type = api.getTypeOfField ( xy );
			TypesOfField btype = Radar.getSectorsType ( xy );
			if (btype != type) {						// если тип  поля изменился
				if (type == TypesOfField.HI){// он
					heDetected(xy);	//  обновить  его координаты				/mayBeHere ( xy,mind.hisMayBeXY );
				}else{																	 // если что нибудь другое
					this.sectorAnalysis ( xy,btype,type,distance );	 // анализируем прошлое значение
				}
				//map.getMapSector(xy).type = api.getTypeOfField ( xy );		// изменяем значения на новые на таблице
			}
		}
		//-----------------------------------------------
		/// <summary>	/// сканирование карты с просчетом минимального пути до каждой вершины из расположения бота
		/// </summary>
		internal void scaning( ) {	 
			int distance = 0;
			Queue<ICoordinates> q1 = new Queue<ICoordinates> (); // основная очередь
			Queue<ICoordinates> q2 = new Queue<ICoordinates> (); // Вспомогательная
			ICoordinates temp = StepsAndCoord.Coordinates ( api.getCoordOfMe ().getX (),api.getCoordOfMe ().getY () );

			if (api.isVisible ()) {
				heDetected ( api.getCoordOfEnemy () );
			}

			q1.Enqueue (temp);
			while (q1.Count != 0 || q2.Count != 0) {
				q2 = new Queue<ICoordinates> ();
				while (q1.Count > 0) {
					temp = q1.Dequeue ();											// достали вершину из очереди, пора узнать что в ней изменилось
					map.getMapSector ( temp ).lenForWave = distance;	// указали  ее удаленность
					map.getMapSector ( temp ).usableForWave = false;
					if (api.isNorm ( temp )) {
						this.mapSectorReLoad ( temp,distance );				// анализ изменений в ячейке
						map.getAllUsableSectors ( map[temp.getX (),temp.getY ()],q2 ); //всех соседей данной вершины графа			 в очередь
					}
				}
				distance++;				 // удаленность волны	
				q1 = q2;
			}
			if (StepsAndCoord.isEcualCoord(api.getCoordOfMe(),mind.veryFar)&&(mind.I.DeltaHelth>0||mind.I.DeltaMoney>0)){
				mind.veryFar = temp;
			}
		}
		private void xFunct() {
			int distance = 0;
			Queue<ICoordinates> q1 = new Queue<ICoordinates> (); // основная очередь
			Queue<ICoordinates> q2 = new Queue<ICoordinates> (); // Вспомогательная
			ICoordinates temp = StepsAndCoord.Coordinates ( api.getCoordOfMe ().getX (),api.getCoordOfMe ().getY () );

			if (api.isVisible ()) {
				heDetected ( api.getCoordOfEnemy () );
			}

			q1.Enqueue ( temp );
			while (q1.Count != 0 || q2.Count != 0) {
				q2 = new Queue<ICoordinates> ();
				while (q1.Count > 0) {
					temp = q1.Dequeue ();											// достали вершину из очереди, пора узнать что в ней изменилось
					map.getMapSector ( temp ).lenForWave = distance;	// указали  ее удаленность
					map.getMapSector ( temp ).usableForWave = false;
					if (api.isNorm ( temp )) {
						this.mapSectorReLoad ( temp,distance );				// анализ изменений в ячейке
						map.getAllUsableSectors ( map[temp.getX (),temp.getY ()],q2 ); //всех соседей данной вершины графа			 в очередь
					}
				}
				distance++;				 // удаленность волны	
				q1 = q2;
			}
			mind.veryFar = temp;
		}
		//-----------------------------------------------
		/// <summary>/// подготовка к новому сканированию 
		/// использованных на данном ходу ячеек	карты для прохода в ширину
		/// </summary>
		internal void clean() {
			map.resetForWave ();
		}
		/// <summary> создается поле, и забиваются расположения стен и всего остального
		/// </summary>
		internal Radar(Mind mind,IAPI api) {

			map = new Map (api.getMinX(),api.getMinY(), api.getMaxX()+1,api.getMaxY()+1);		// создается поле для сканирования

			this.mind = mind;																				// добавляются ссылки для обратного вызова
			this.api = api;

			recycleSectors = new Queue<MapSector>();

			ICoordinates xy = StepsAndCoord.Coordinates ( api.getMinX (),api.getMinY () );

			for (int i = api.getMinX (); i <= api.getMaxX (); i++) {						 // проход по карте построчно

				for (int j = api.getMinY (); j <= api.getMaxY (); j++) {

					StepsAndCoord.otherPositionOfThis ( xy,i,j );
          if (api.isNorm(xy)) {
              map[i, j] = new MapSector();
              if (api.getTypeOfField(xy) == TypesOfField.WALL)  {
                  map[i, j].type = TypesOfField.WALL;
									map[i,j].xy = StepsAndCoord.Coordinates ( i,j );
              }
              else {
								  map[i,j].xy = StepsAndCoord.Coordinates ( i,j );
									map[i,j].type = api.getTypeOfField ( xy );
              }
          }
				}
			}
			xFunct ();
		}
	}
}	 