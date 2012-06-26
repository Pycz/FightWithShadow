using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    //*********************************
    //  вспомогательно - чтобы потом не париться с совместимостью
    //  я реализую, а пока можете использовать
    //***********************************

    public interface ICoordinates
    {
        // для "снуля"
        /// <summary>
        /// Координата X, если вы считаете с нуля
        /// </summary>
        int X0
        {
            set;
            get;
        }

        /// <summary>
        /// Координата Y, если вы считаете с нуля
        /// </summary>
        int Y0
        {
            set;
            get;
        }

        // для "сыдиницы"
        /// <summary>
        /// Координата X, если вы считаете с единицы
        /// </summary>
        int X1
        {
            set;
            get;
        }

        /// <summary>
        /// Координата Y, если вы считаете с единицы
        /// </summary>
        int Y1
        {
            set;
            get;
        }

        // для "совместимости" - эквивалентны сединице
        void setX(int x);
        void setY(int y);
        int getX();
        int getY();

        // для "удобства"
        /// <summary>
        /// Копирование в эту координату другой
        /// </summary>
        /// <param name="co">Что копируем</param>
        void Copy(ICoordinates co);

        

    }
}
