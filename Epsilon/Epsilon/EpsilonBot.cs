using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using CCStepsCoords;

	namespace Epsilon {

		public class EpsilonBot:IBot {
			
			//Fields
			/// <summary>	/// БД робота
			/// </summary>
			private Mind  mind;
			/// <summary>	/// Класс сканирования карты
			/// </summary>
			private Radar radar;
			/// <summary>Доступ к API сервера
			/// </summary>
			private static IAPI  m_api;
			/// <summary>Доступ к API сервера
			/// </summary>
			internal static IAPI api {
				get {
					return m_api;
				}
			}
		
			//Methods
			/// <summary>программа боевых действий
			/// </summary>
			private IStep killer() {
				IStep s = Helper.randomVectorStep(mind.I.XY);
				//--2-------------------------
				if (mind.He.WasFound) {		 // точно знаем координаты
					//--3-------------------------
					if (mind.He.Health + Gun.cRoketF <= 0) {		 // сейчас ликвидируем?
						s = Helper.StepsBuilder ( mind.He.XY,mind.I.gun ().roketFire );
					} else {
						//--4-------------------------
						if (mind.I.gun ().canBlasterFire ( mind.He.XY )) {	 // на растоянии <=3
							s = Helper.StepsBuilder ( mind.He.XY,mind.I.gun ().blasterFire );
						} else {
							s = Helper.StepsBuilder ( mind.He.XY,mind.I.gun ().roketFire );				 // на растоянии >3
						}//--4-------------------------
					}//--3-------------------------
				} else {									// точных координат не нимеем
					//--5-------------------------
					if (mind.hisMayBeXY.Count <= 3 && mind.hisMayBeXY.Count >0 ) {		// предполагаем где он
						Random r = new Random ();
						int i = r.Next (1, mind.hisMayBeXY.Count-1 );
						s = Helper.StepsBuilder ( mind.hisMayBeXY.ElementAt ( i ).xy,mind.I.gun ().roketFire );		// тогда ракетой в него
					} else {														 // не знаем где он
						//--6-------------------------
						s = wayOfEvil ( mind.I.XY,api.endAfterTime (),mind.He.gun ().getTimerVal );
					}//--5-------------------------
				}	//--2-------------------------
				return s;
			}
			/// <summary>	если по каким либо причинам стрелять нельзя	прикладываем маршрут к аптечкам, бонусам или простым клеткам
			/// </summary>
			private IStep wayOfEvil(ICoordinates startPoint,int stepCount,int roketTimer) {
                Logistics logistics = new Logistics(mind);
				IStep s = Helper.StepsBuilder ( api.getCoordOfMe (),Steps.STEP );
				//--0----------------------------
				if (api.CanGo ()) {
					s = Helper.randomVectorStep ( mind.I.XY );
					if (mind.I.Health > Gun.cRoketF + Gun.cLaserF) {	// если со здоровьем все в проядке тогда идем за бонусами 
						s = logistics.wayOfEvil ( TypesOfField.BONUS,
																	Logistics.smallBuilder (
																					api.getCoordOfMe (),
																					mind.He.gun ().getTimerVal,
																					api.endAfterTime (),null ),
																	api.endAfterTime (),mind.He.gun ().getTimerVal );//просчитываем дорогу к бонусам
					} else {																																 // иначе 
						s = logistics.wayOfEvil ( TypesOfField.MEDKIT,
																	Logistics.smallBuilder ( api.getCoordOfMe (),
																													 mind.He.gun ().getTimerVal,
																													 api.endAfterTime (),null ),
																	api.endAfterTime (),mind.He.gun ().getTimerVal );//к аптечкам
					}//--6-------------------------
				}//--0----------------------------
				return s;
			}
			/// <summary>	/// Выбор действия
			/// </summary>
			private IStep thinking() {		//возвращает тип хода
				IStep s = new CStep ();
				
				//--1-------------------------																						 
				if (mind.I.gun ().canIFireOfAny) {	 // можем стрелять
					s = killer ();
				} else {														 // иначе
					s = wayOfEvil ( mind.I.XY,api.endAfterTime (),mind.He.gun ().getTimerVal );
				}//--1-------------------------
				return s;
			}
			/// <summary>	/// Сходить
			/// </summary>
			public IStep doSmth() {			
				mind.Reload(radar);					//	сканирование карты, анализ изменнений
                IStep s = Helper.StepsBuilder(api.getCoordOfMe(), Steps.ROCKET);
                  s=  thinking ();			// определение тактики и стратегии
				mind.DataSave ();
				return s;
			}
			/// <summary>Возврат имени бота, не более 14 символов
			/// </summary>
			public string getName() {
				return mind.I.getName;
			}
			/// <summary>	/// Builder
			/// </summary>
			public EpsilonBot() {

			}
			/// <summary>	/// получение ссылки на реализацию методов интерфейсов
			/// </summary>
			public void Initialize(Interfaces.IAPI x_api) {
				m_api = x_api;
				mind = new Mind ( api.endAfterTime (),api.getCoordOfMe () );
				radar = new Radar ( mind,api );
			}
			/// <summary>	/// осуществляет выстрел в самого себя если до конца игры не выжить
			/// чтобы очки не достались противнику
			/// </summary>
			private IStep Hara_Kiri() {
				Steps s = Steps.STEP;
				bool fire = false;
				if (fire) {
					if (mind.I.gun ().canIUseRoket) {
						s = Steps.ROCKET;
					} else {
						s = Steps.BLASTER;
					}
				}
				return Helper.StepsBuilder ( api.getCoordOfMe (),s );
			}
		}
	}