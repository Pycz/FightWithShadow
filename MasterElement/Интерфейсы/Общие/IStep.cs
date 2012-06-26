using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterElement
{
    //*********************************
    //  вспомогательно - чтобы потом не париться с совместимостью
    //  я реализую, а пока можете использовать
    //***********************************

    enum Steps {STEP, BLASTER, ROCKET};   //виды ходов: шаг, выстрел из бластера, выстрел ракетой

    interface IStep
    {
        /// <summary>
        /// Задать координату шага
        /// </summary>
        /// <param name="coord">Координата хода</param>
        void setCoord(ICoordinates coord);

        /// <summary>
        /// Задать тип шага
        /// </summary>
        /// <param name="step">Тип шага</param>
        void setTypeOfStep(Steps step);

        // это для меня :). Вам все равно вроде не должно понадобиться
        ICoordinates getCoord();
        Steps getTypeOfStep();
    }
}
