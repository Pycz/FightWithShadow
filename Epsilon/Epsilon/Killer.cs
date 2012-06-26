//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Interfaces; using CCStepsCoords;

//namespace Epsilon {
//  class Killer {
//    private bool CanAttack() {
//      return mind.He.WasFound || Mind.hisMayBeXY.Count > 0 || mind.I.gun ().canIFireOfAny;
//    }
//    //---------------------------
//    //private IStep GunType() {
//    //}
//    //---------------------------
//    private IStep Attack() {
//      ICoordinates xy;
//      StepsBuilder step = new StepsBuilder ();


//      if (mind.He.WasFound) {
//        xy = mind.He.XY;
//      } else {
//        Random r = new Random ();
//        int i = r.Next ( 3 );
//        xy = Mind.hisMayBeXY.ElementAt ( i ).Ixy;
//      }
//      return Helper.StepsBuilder ( xy,step );
//    }
//    //---------------------------
//    private IStep Hara_Kiri() {						// осуществляет	выстрел в самого себя если до конца игры не выжить
//      StepsBuilder s = StepsBuilder.STEP;								// чтобы очки не достались противнику
//      if (mind.I.gun ().canIUseRoket) {
//        s = StepsBuilder.ROCKET;
//      } else {
//        s = StepsBuilder.BLASTER;
//      }
//      return Helper.StepsBuilder ( api.getCoordOfMe (),s );
//    }
//    //---------------------------
//    //private IStep Ghost() {				// режим невидимки передвижение максимально мешающие противнику обнаружить
//    //                              // стратегическое умение
//    //}															// 
//    //---------------------------
//    /// <summary>
//    /// Выбор действия
//    /// </summary>
//    private IStep Thinking() {		//возвращает тип хода
//      bool found = false;
//      IStep s = new CStep ();
//      if (!found) {

//        if (mind.He.WasFound) {

//        } else {

//        }
//        //if (CanAttack ()) {

//        //  s = Attack ( );
//        //}
//        //if (!CanAttack ()) {


//        //}
//      }






//      s.setTypeOfStep ( StepsBuilder.BLASTER );
//      return s;
//    }
//  }
//}
