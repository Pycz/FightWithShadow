using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace R2D2
{
    class CCoord : ICoordinates, IComparable
    {
        public int x = 0;
        public int y = 0;
        public int d = 0;
        public CCoord Set(ICoordinates coord)
        {
            x = coord.X0;
            y = coord.Y0;
            return this;
        }
        public CCoord Set(int X, int Y)
        {
            x = X;
            y = Y;
            return this;
        }
        public CCoord Set(int X, int Y, int D)
        {
            x = X;
            y = Y;
            d = D;
            return this;
        }
        public CCoord() { }
        public CCoord(int X, int Y)
        {
            Set(X, Y);
        }
        public CCoord(int X, int Y, int D)
        {
            Set(X, Y, D);
        }
        public CCoord(ICoordinates coord)
        {
            Set(coord);
        }
        public void Copy(CCoord coord)
        {
            x = coord.x;
            y = coord.y;
            d = coord.d;
        }
        public CCoord Clone()
        {
            return new CCoord(x, y, d);
        }
        public bool EqualsXY(CCoord eq)
        {
            return (eq != null) && (x == eq.x) && (y == eq.y); ;
        }
        public bool EqualsXY(int X, int Y)
        {
            return (x == X) && (y == Y); ;
        }
        public bool EqualsXY(ICoordinates eq)
        {
            return (eq != null) && (x == eq.X0) && (y == eq.Y0); ;
        }
        public bool EqualsD(CCoord eq)
        {
            return (eq != null) && (d == eq.d);
        }
        public bool EqualsD(int D)
        {
            return (d == D);
        }
        public TypesOfField getType()
        {
            if ((d >= 0) && (d <= 5)) return (TypesOfField)d;
            else throw new Exception("Клетка не является клеткой типа TypesOfField");
        }
        public void setType(TypesOfField ty)
        {
            if (((int)ty >= 0) && ((int)ty <= 5)) d = (int)ty;
            else throw new Exception("Клетка не является клеткой типа TypesOfField");
        }
        //IComparable Interface functions:
        public int CompareTo(object obj)
        {
            int result = 0;
            if (((CCoord)obj).d > d) result = 1;
            else
                if (((CCoord)obj).d < d) result = -1;
            return result;
        }
        //ICoordinates Interface functions:
        public int X0
        {
            set { x = value; }
            get { return x; }
        }
        public int Y0
        {
            set { y = value; }
            get { return y; }
        }
        public int X1
        {
            set { x = value - 1; }
            get { return x + 1; }
        }
        public int Y1
        {
            set { y = value - 1; }
            get { return y + 1; }
        }
        public void Copy(ICoordinates coord)
        {
            x = coord.X0;
            y = coord.Y0;
        }
        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public void setX(int X)
        {
            x = X;
        }
        public void setY(int Y)
        {
            y = Y;
        }
    }
}
