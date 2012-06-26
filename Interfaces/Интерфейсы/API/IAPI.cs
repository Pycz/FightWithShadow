using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{

    /// <summary>
    /// Типы поля
    /// </summary>
    public enum TypesOfField { NOTHING, WALL, ME, HI, BONUS, MEDKIT };  

    /// <summary>
    /// Интерфейс, определяющий возможности API
    /// </summary>
    public interface IAPI
    {
        /// <summary>
        /// Вхоимость координаты в поле
        /// </summary>
        /// <param name="co">Входит ли эта координата?</param>
        /// <returns>Если вне поля - то false</returns>
        bool isNorm(ICoordinates co);    // только если входят в область поля, не за границами.

        ///<summary>
        /// Ширина поля по X - количество клеточек
        /// </summary>
        /// <returns>Размерность поля</returns>
        int getCountX();

        ///<summary>
        /// Ширина поля по Y - количество клеточек
        /// </summary>
        /// <returns>Размерность поля</returns>
        int getCountY();

        ///<summary>
        /// Номер первой ячейки по X
        /// </summary>
        /// <returns>Начальная координата</returns>
        int getMinX();

        ///<summary>
        /// Номер первой ячейки по Y
        /// </summary>
        /// <returns>Начальная координата</returns>
        int getMinY();

        /// <summary>
        /// Номер последней ячеки по X
        /// </summary>
        /// <returns>Максимальная координата</returns>
        int getMaxX();

        /// <summary>
        /// Номер последней ячеки по Y
        /// </summary>
        /// <returns>Максимальная координата</returns>
        int getMaxY();

        /// <summary>
        /// Возврат типа поля по координате
        /// </summary>
        /// <param name="coord">Координата, для которой нужно узнать тип поля</param>
        /// <returns>Тип поля</returns>
        TypesOfField getTypeOfField(ICoordinates coord);

        /// <summary>
        /// Информация о том, видят боты друг друга, или нет
        /// </summary>
        bool isVisible();

        /// <summary>
        /// Возврат координат врага, если его видно. 
        /// </summary>
        /// <returns>Координата врага</returns>
        ICoordinates getCoordOfEnemy();

        /// <summary>
        /// Возврат координат текущего бота.
        /// </summary>
        /// <returns>Координаты бота</returns>
        ICoordinates getCoordOfMe();

        /// <summary>
        /// Сколько у бота осталось жизни
        /// </summary>
        /// <returns>Скока жизни</returns>
        int myHealth();

        /// <summary>
        /// Сколько ходов осталось до конца игры
        /// </summary>
        /// <returns>Скока ходов осталось</returns>
        int endAfterTime();

        /// <summary>
        /// Возврат количества очков у бота
        /// </summary>
        /// <returns>ОЧКИ НННАДА?!</returns>
        int myPoints();

        /// <summary>
        /// Возврат видимости между двумя любыми клетками
        /// </summary>
        /// <param name="coord1">Первая клетка</param>
        /// <param name="coord2">Вторая клетка</param>
        /// <returns>Да, если можно, нет, если нельзя</returns>
        bool isVisible(ICoordinates coord1, ICoordinates coord2);  //перегрузка

        bool CanGo();
    }
}
