using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using CCStepsCoords;
using MasterElement;

namespace Logic
{
    class CGameBot
    {
        masterСhief Eyes;
        internal IBot Mind;
        private string name;
        internal int Health;
        internal int Points;
        internal int TurnsToShoot;
        internal ICoordinates Position;
        internal bool isDead
        {
            get { return Health <= 0; }
        }
        internal bool isSlow
        {
            get { return Health < 20; }
        }
        internal bool canShoot
        {
            get { return TurnsToShoot == 0; }
        }
        internal int TurnsToStep;
        internal bool canGo
        {
            get{return TurnsToStep==0;}
        }
        internal string Name
        {
            set
            {
                if (value.Length > 14)
                {
                    name = value.Substring(0, 14);
                }
                else
                {
                    name = value;
                }
            }
            get
            {
                return name;
            }
        }

        internal CGameBot(masterСhief eye, int health, int points, ICoordinates pos)
        {
            Eyes = eye;
            Health = health;
            Points = points;
            TurnsToShoot = 0;
            Position = pos;
            TurnsToStep = 0;
        }

        internal void SetMind(IBot mind)
        {
            Mind=mind;
        }

        internal void GetName()
        {
            Name = Mind.getName();
        }

    }
}
