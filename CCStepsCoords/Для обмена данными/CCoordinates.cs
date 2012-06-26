using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace CCStepsCoords
{
    public class CCoordinates: ICoordinates
    {
        protected int X;
        protected int Y;

        /// <summary>
        /// Координата X, если вы считаете с нуля
        /// </summary>
        public int X0
        {
            set
            {
                X = value+1;
            }
            get
            {
                return X-1;
            }
        }

        /// <summary>
        /// Координата Y, если вы считаете с нуля
        /// </summary>
        public int Y0
        {
            set
            {
                Y = value + 1;
            }
            get
            {
                return Y - 1;
            }
        }

        /// <summary>
        /// Координата X, если вы считаете с единицы
        /// </summary>
        public int X1
        {
            set
            {
                X = value;
            }
            get
            {
                return X;
            }
        }

        /// <summary>
        /// Координата Y, если вы считаете с единицы
        /// </summary>
        public int Y1
        {
            set
            {
                Y = value;
            }
            get
            {
                return Y;
            }
        }

        /// <summary>
        /// Создает координату в начале координат
        /// </summary>
        public CCoordinates()
        {
            X=1;
            Y=1;
        }

        /// <summary>
        /// Копирование в эту координату другой
        /// </summary>
        /// <param name="co">Что копируем</param>
        public void Copy(ICoordinates co)
        {
            this.X1 = co.X1;
            this.Y1 = co.Y1;
        }

        public void setX(int x)
        {
            X = x;
        }

        public void setY(int y)
        {
            Y=y;
        }

        public int getX()
        {
            return this.X;
        }

        public int getY()
        {
            return this.Y;
        }
    }
}
