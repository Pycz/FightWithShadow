using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterElement
{
    class CStep :IStep
    {
        private ICoordinates myCoord;
        private Steps myStep;

        public CStep()
        {
            myCoord=new CCoordinates();
            myStep=Steps.STEP;
        }

        /// <summary>
        /// Задать координату шага
        /// </summary>
        /// <param name="coord">Координата хода</param>
        public void setCoord(ICoordinates coord)
        {
            myCoord = coord;
        }


        /// <summary>
        /// Задать тип шага
        /// </summary>
        /// <param name="step">Тип шага</param>
        public void setTypeOfStep(Steps step)
        {
            myStep = step;
        }

        public ICoordinates getCoord()
        {
            return myCoord;
        }

        public Steps getTypeOfStep()
        {
            return myStep;
        }
    }
}
