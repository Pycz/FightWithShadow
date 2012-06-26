using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace R2D2
{
    class CStep : IStep
    {
        public CCoord Coord;
        public Steps Step;
        public Steps Type
        {
            set { setTypeOfStep(value); }
            get { return getTypeOfStep(); }
        }
        public void Set(Steps steptype, CCoord coord)
        {
            Coord = coord;
            Type = steptype;
        }
        public void Set(Steps steptype)
        {
            Type = steptype;
        }
        public void Set(CCoord coord)
        {
            Coord = coord;
        }
        public CStep()
        {
            Coord = new CCoord();
            Step = Steps.STEP;
        }

        //IStep Interface functions:
        public void setCoord(ICoordinates coord)
        {
            Coord.Set(coord);
        }
        public void setTypeOfStep(Steps step)
        {
            Step = step;
        }
        public ICoordinates getCoord()
        {
            return Coord;
        }
        public Steps getTypeOfStep()
        {
            return Step;
        }
    }
}
