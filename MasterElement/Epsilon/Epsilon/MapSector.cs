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
	internal class MapSector:StepsAndCoord {
		//
		//Fields
		//
		private TypesOfField m_type;
		private ICoordinates m_xy;
		private int m_lenForWave;
		/// <summary>Для первоночального сканирования карты
		/// </summary>
		private bool m_Usale;


		/// <summary>
		/// Удаленность при движении в ширь
		/// </summary>
		internal int lenForWave {
			get {
				return m_lenForWave;
			}
			set {
				m_lenForWave = value;
			}
		}
		/// <summary>
		/// Вывод параметров 
		/// </summary>
		/// <returns></returns>
		public override string  ToString(){
			return "x = " + xy.getX ().ToString () + " y = " + xy.getY ().ToString () + " type = " + m_type.ToString () + " usable " + m_Usale.ToString ();
		}
		/// <summary>Была ли посещена клетка при проходе в ширину
		/// </summary>
		internal bool usableForWave {
			get {
				return m_Usale;
			}
			set {
				m_Usale = value;
			}
		}
		/// <summary> Тип ячейки
		/// </summary>
		internal TypesOfField type {
			get {
				return m_type;
			}
			set {
				m_type = value;
			}
		}
		/// <summary> Координаты
		/// </summary>
		internal ICoordinates xy {
			get {
				return m_xy;
			}
			set {
				m_xy = value;
			}
		}

		/// <summary>Сброс значений для прохода в ширину
		/// </summary>
		

		/// <summary>Создание клеток для первоначального прохода
		/// все клетки помечаются как разрешенные для сканирования в ширь
		/// </summary>
		internal MapSector() {
			usableForWave = true;
            type = TypesOfField.NOTHING;
			lenForWave = 0;
			xy = new CCoordinates ();
		}
	}
}