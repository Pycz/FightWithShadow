using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterElement;
using Interfaces;
using CCStepsCoords;
using System.IO;


namespace Logic
{
    class CCoordinatesLen:CCoordinates,ICoordinates
    {
        internal int Len;
        internal CCoordinatesLen():base()
        {
            Len=0;
        }
    }

    class CMap
    {
        private const double amountWalls=0.30;
        private const double amountMedics=0.10;
        private const double amountBonuses=0.10;
        private const int VisibleRadius=5;
        private const int BlasterRadius=3;
        private const int minRazmMap=10;
        private const int maxRazmMap=50;
        // координаты с 1!
        internal TypesOfField[,] map;
        internal int maxX;  //и количество - и номер последнего!
        internal int maxY;  // но при инициализации массива +1
        private Random rand;
        private int AllSize
        {
            get
            {
                return maxX*maxY;
            }
        }
        private int Walls;
        internal int Spaces;
        internal int Bonuses;
        internal int Medics;
        internal List<ICoordinates> EmptyCoords;
        private int MaxWalls;
        private int MaxBonuses;
        private int MaxMedics;
        internal ICoordinates Bot1;
        internal ICoordinates Bot2;


        internal List<ICoordinates>[,] Visibles;  //видимость на карте из каждой точки


        public CMap()
        {
            maxX = 10;
            maxY = 10;
            rand=new Random();
            Walls=0;
            Spaces=AllSize;
            Bonuses=0;
            Medics=0;
            EmptyCoords=new List<ICoordinates>();
            Bot1 = new CCoordinates();
            Bot2 = new CCoordinates();
        }

        internal void ReadFromFile(string path)
        {

            BinaryReader r = new BinaryReader(File.OpenRead(path));

            maxX = r.ReadInt32();
            maxY = r.ReadInt32();
            Bonuses = 0;
            Medics = 0;
            Spaces = 0;
            Walls = 0;

            map = new TypesOfField[maxX + 1, maxY + 1];
            EmptyCoords = new List<ICoordinates>();
            ICoordinates cur=new CCoordinates();
            int k=0;

            for (int x = 1; x <= maxX; x++)
                for (int y = 1; y <= maxY; y++)
                {
                    k = r.ReadInt32();
                    switch (k)
                    {
                        case 0: 
                            map[x, y] = TypesOfField.NOTHING; 
                            Spaces++;
                            cur = new CCoordinates();
                            cur.X1 = x;cur.Y1=y;
                            EmptyCoords.Add(cur);
                            break;
                        case 1: 
                            map[x, y] = TypesOfField.WALL; 
                            Walls++;  
                            break;
                        case 2:
                            map[x, y] = TypesOfField.BONUS;
                            Bonuses++;
                            break;
                        case 3:
                            map[x, y] = TypesOfField.MEDKIT;
                            Medics++;
                            break;
                        case 8:
                            map[x, y] = TypesOfField.ME;
                            Bot1 = new CCoordinates();
                            Bot1.X1 = x;
                            Bot1.Y1 = y;
                            break;
                        case 9: 
                            map[x, y] = TypesOfField.HI;
                            Bot2 = new CCoordinates();
                            Bot2.X1 = x;
                            Bot2.Y1 = y;
                            break;
                    }
                }
            r.Close();
            MaxWalls = Walls;
            MaxMedics = Medics;
            MaxBonuses = Bonuses;
            SetVisibilitys();
        }
        internal void WriteToFile(string path)
        {
            BinaryWriter wr = new BinaryWriter(File.OpenWrite(path));
            int w = maxX;
            int h = maxY;
            wr.Write(w);
            wr.Write(h);
            for (int i = 1; i <= w; i++)
                for (int j = 1; j <= h; j++)
                {
                    int k = 0;
                    if (map[i,j]==TypesOfField.NOTHING) k = 0;
                    if (map[i,j]==TypesOfField.WALL) k = 1;
                    if (map[i,j]==TypesOfField.BONUS) k = 2;
                    if (map[i,j]==TypesOfField.MEDKIT) k = 3;
                    if (map[i,j]==TypesOfField.ME) k = 8;
                    if (map[i,j]==TypesOfField.HI) k = 9;
                    wr.Write(k);
                }
            wr.Close();
        }

       //// internal static bool isAllied(ICoordinates pos, ICoordinates sos)
       // {
       //     ICoordinates x = new CCoordinates();
       //     x.Copy(pos);

       //     x.X1++;
       //     if (this.isNorm(x) && ISEQUAL(x, sos))
       //     {
       //         return true;
       //     }
       //     x.Copy(pos);
       //     x.X1--;
       //     if (isNorm(x) && ISEQUAL(x, sos))
       //     {
       //         return true;
       //     }
       //     x.Copy(pos);
       //     x.Y1++;
       //     if (isNorm(x) && ISEQUAL(x, sos))
       //     {
       //         return true;
       //     }
       //     x.Copy(pos);
       //     x.Y1--;
       //     if (isNorm(x) && ISEQUAL(x, sos))
       //     {
       //         return true;
       //     }
       //     return false;
       // }


        internal List<ICoordinates> MAKECOPY(List<ICoordinates> lol)
        {
            List<ICoordinates> ololo=new List<ICoordinates>();
            ICoordinates co=new CCoordinates();
            for(int i=0;i<lol.Count;i++)
            {
                co.Copy(lol[i]);
                ololo.Add(co);
                co=new CCoordinates();
            }
            return ololo;
        }

        internal bool ISEQUAL (ICoordinates one, ICoordinates two)
        {
            return (one.X1==two.X1 && one.Y1==two.Y1);
        }

        internal int FINDEQUALNUM(List<ICoordinates> lol, ICoordinates it)
        {
            for(int i=0;i<lol.Count;i++)
            {
                if(ISEQUAL(it,lol[i]))
                    return i;
            }
            return -1;
        }

        internal int FINDEQUALNUMLEN(List<CCoordinatesLen> lol, ICoordinates it)
        {
            for (int i = 0; i < lol.Count; i++)
            {
                if (ISEQUAL(it, lol[i]))
                    return i;
            }
            return -1;
        }

        private bool isConnected()
        {
            ICoordinates empt=EmptyCoords[0]; // любой пустой элемент

            Stack<CCoordinates> stack=new Stack<CCoordinates>();
            CCoordinates lol=new CCoordinates();
            CCoordinates pushing=new CCoordinates();
            lol.Copy(empt);
            stack.Push(lol);
            int HowMuchEmpty=1; // сначала есть 1 пустая вершина
            bool [,]lables=new bool[maxX+1,maxY+1]; // false по умолчанию
            lables[lol.X1,lol.Y1]=true;
            while(stack.Count!=0)
            {
                lol=stack.Pop();
                foreach(ICoordinates x in this.AlliedNodes(lol))
                {
                    if((this.map[x.X1,x.Y1]!=TypesOfField.WALL)&&(!lables[x.X1,x.Y1]))
                    {
                        pushing = new CCoordinates();
                        pushing.Copy(x);
                        stack.Push(pushing);
                        HowMuchEmpty++;
                        lables[pushing.X1,pushing.Y1]=true;
                    }
                }
            }
            bool ret=false;
            if(HowMuchEmpty==EmptyCoords.Count)
                ret=true;
            
            return ret;
        }

        internal bool isNorm(ICoordinates co)
        {
            return (co.X1 >= 1 && co.X1 <= maxX && co.Y1 >= 1 && co.Y1 <= maxY);
        }

        /// <summary>
        /// Все соседние вершины в пределах поля
        /// </summary>
        /// <param name="lol">Для координаты</param>
        /// <returns></returns>
        internal List<ICoordinates> AlliedNodes(ICoordinates lol)
        {
            List<ICoordinates> ret=new List<ICoordinates>();
            ICoordinates x=new CCoordinates();
            x.Copy(lol);

            x.X1++;
            if(isNorm(x))
            {
                ret.Add(x);
                x = new CCoordinates();
            }
            x.Copy(lol);
            x.X1--;
            if(isNorm(x))
            {
                ret.Add(x);
                x = new CCoordinates();
            }
            x.Copy(lol);
            x.Y1++;
            if(isNorm(x))
            {
                ret.Add(x);
                x = new CCoordinates();
            }
            x.Copy(lol);
            x.Y1--;
            if(isNorm(x))
            {
                ret.Add(x);
            }
            return ret;
        }

        internal void SetRandBonus()
        {
            int del=rand.Next(0,EmptyCoords.Count-1);
            map[EmptyCoords[del].X1,EmptyCoords[del].Y1]=TypesOfField.BONUS;
            Bonuses++;
            Spaces--;
            EmptyCoords.RemoveAt(del);
        }

        internal void SetRandMedic()
        {
            int del=rand.Next(0,EmptyCoords.Count-1);
            map[EmptyCoords[del].X1,EmptyCoords[del].Y1]=TypesOfField.MEDKIT;
            Medics++;
            Spaces--;
            EmptyCoords.RemoveAt(del);
        }

        internal void Generate()
        {
            maxX = rand.Next(minRazmMap, maxRazmMap);
            maxY = rand.Next(minRazmMap, maxRazmMap);
            map = new TypesOfField[maxX + 1, maxY + 1];
            Walls = 0;
            Spaces = AllSize;
            Bonuses = 0;
            Medics = 0;
            ICoordinates cur = new CCoordinates();
            EmptyCoords = new List<ICoordinates>(); //а тут все с 0 :(
            for (int i = 1; i <= maxX; i++)
                for (int j = 1; j <= maxY; j++)
                {
                    cur.X1 = i;
                    cur.Y1 = j;
                    EmptyCoords.Add(cur);
                    map[i, j] = TypesOfField.NOTHING;
                    cur = new CCoordinates();
                }

            //забили список всех пустых вершин. Начинаем генерацию стен.
            MaxWalls = rand.Next(0, (int)((double)AllSize * (amountWalls))); // стен не более amountWalls
            MaxMedics = rand.Next(0, (int)((double)AllSize * (amountMedics)));
            MaxBonuses = rand.Next(0, (int)((double)AllSize * (amountBonuses)));
            int CurrentNumInList;
            List<ICoordinates> ForRand = MAKECOPY(EmptyCoords);
            int thisIsEqual = 0;
            bool foundNorm = false;
            for (int i = 1; i <= MaxWalls; i++)
            {
                Walls++;
                Spaces--;
                while (!(foundNorm || (ForRand.Count == 0)))
                {
                    CurrentNumInList = rand.Next(0, ForRand.Count - 1);
                    cur = ForRand[CurrentNumInList];
                    map[cur.X1, cur.Y1] = TypesOfField.WALL;
                    ForRand.RemoveAt(CurrentNumInList);
                    thisIsEqual= FINDEQUALNUM(EmptyCoords, cur);
                    EmptyCoords.RemoveAt(thisIsEqual);  // тут может быть фигня
                    if (isConnected())
                    {
                        foundNorm = true;
                    }
                    else
                    {
                        map[cur.X1, cur.Y1] = TypesOfField.NOTHING;
                        EmptyCoords.Add(cur);
                    }
                }

                if (!foundNorm)
                {
                    i = MaxWalls + 1;
                }
                else
                {
                    ForRand = MAKECOPY(EmptyCoords);
                    foundNorm = false;
                }
            }

            //генерация бонусов   // пока все сразу, во время - потом
            for (int i = 1; i <= MaxBonuses; i++)
            {
                SetRandBonus();
            }
            // генерация аптечек - //аналогично
            for (int i = 1; i <= MaxMedics; i++)
            {
                SetRandMedic();
            }

            // генерация игроков
            // 1
            int del = rand.Next(0, EmptyCoords.Count - 1);
            map[EmptyCoords[del].X1, EmptyCoords[del].Y1] = TypesOfField.ME;
            Spaces--;
            Bot1.Copy(EmptyCoords[del]);
            EmptyCoords.RemoveAt(del);

            // генерация игроков
            // 2
            del = rand.Next(0, EmptyCoords.Count - 1);
            map[EmptyCoords[del].X1, EmptyCoords[del].Y1] = TypesOfField.HI;
            Spaces--;
            Bot2.Copy(EmptyCoords[del]);
            EmptyCoords.RemoveAt(del);

            // генерация видимости между клетками
            SetVisibilitys();
            // вроде все
        }

        internal void ShowOnConsole()
        {
            for (int i = 1; i <= maxY; i++)
            {
                for (int j = 1; j <= maxX; j++)
                {
                    switch (map[j, i])
                    {
                        case TypesOfField.NOTHING:
                            Console.Write(" ");
                            break;
                        case TypesOfField.WALL:
                            Console.Write("X");
                            break;
                        case TypesOfField.MEDKIT:
                            Console.Write("M");
                            break;
                        case TypesOfField.BONUS:
                            Console.Write("!");
                            break;
                        case TypesOfField.HI:
                            Console.Write("2");
                            break;
                        case TypesOfField.ME:
                            Console.Write("1");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }


        private void SetVisibilitys()
        {
            Visibles = new List<ICoordinates>[maxX + 1, maxY + 1];
            for (int i = 1; i <= maxX; i++)
            {
                for (int j = 1; j <= maxY; j++)
                {
                    if (map[i, j] == TypesOfField.WALL)
                        Visibles[i, j] = null;
                    else
                        Visibles[i, j] = ListOfVisible(i, j);
                }
            }
        }

        private List<ICoordinates> ListOfRadius(ICoordinates co, int Radius)
        {
            ICoordinates cur=new CCoordinates();
            cur.Copy(co);
            List<ICoordinates> ret=new List<ICoordinates>();
            List<ICoordinates> allied=new List<ICoordinates>();
            Stack<ICoordinates> stack1 = new Stack<ICoordinates>();
            Stack<ICoordinates> stack2 = new Stack<ICoordinates>();
            Stack<ICoordinates> swap=null;
            ret.Add(co);
            bool [,]gone=new bool[maxX+1,maxY+1]; 

            stack1.Push(cur);
            gone[cur.X1, cur.Y1] = true;
            for(int y=0;y<Radius;y++)
            {
                while(stack1.Count!=0)
                {
                    cur=new CCoordinates();
                    cur.Copy(stack1.Pop());
                    allied=AlliedNodes(cur);
                    foreach (ICoordinates x in allied)
                    {
                        if(!gone[x.X1,x.Y1])
                        {
                            stack2.Push(x);
                            gone[x.X1,x.Y1]=true;
                        }
                    }
                }
                foreach( ICoordinates x in stack2)
                {
                    cur=new CCoordinates();
                    cur.Copy(x);
                    ret.Add(cur);
                }
                swap=stack2;
                stack2=stack1;
                stack1=swap;
            }
            return ret;
        }

        private List<CCoordinatesLen> ListOfRadiusLen(ICoordinates co, int Radius)
        {
            CCoordinatesLen cur=new CCoordinatesLen();
            CCoordinatesLen co1=new CCoordinatesLen();
            cur.Copy(co);
            co1.Copy(co);
            co1.Len=0;
            List<CCoordinatesLen> ret=new List<CCoordinatesLen>();
            List<ICoordinates> allied=new List<ICoordinates>();
            Stack<CCoordinatesLen> stack1 = new Stack<CCoordinatesLen>();
            Stack<CCoordinatesLen> stack2 = new Stack<CCoordinatesLen>();
            Stack<CCoordinatesLen> swap=null;
            ICoordinates temp = new CCoordinatesLen();
            CCoordinatesLen forPush=new CCoordinatesLen();
            ret.Add(co1);
            bool [,]gone=new bool[maxX+1,maxY+1]; 

            stack1.Push(cur);
            gone[cur.X1, cur.Y1] = true;
            for(int y=0;y<Radius;y++)
            {
                while(stack1.Count!=0)
                {
                    cur=new CCoordinatesLen();
                    cur.Copy(stack1.Pop());
                    temp.Copy(cur);
                    allied=AlliedNodes(temp);
                    foreach (ICoordinates x in allied)
                    {
                        if(!gone[x.X1,x.Y1])
                        {
                            forPush=new CCoordinatesLen();
                            forPush.Copy(x);
                            forPush.Len=y+1;
                            stack2.Push(forPush);
                            gone[x.X1,x.Y1]=true;
                        }
                    }
                }
                foreach( CCoordinatesLen x in stack2)
                {
                    cur=new CCoordinatesLen();
                    cur.Copy(x);
                    cur.Len=x.Len;
                    ret.Add(cur);
                }
                swap=stack2;
                stack2=stack1;
                stack1=swap;
            }
            return ret;
        }

        internal bool canBlasterShootBeetween(ICoordinates one, ICoordinates two)
        {
            bool ret=true;
            List<ICoordinates> whereCan=ListOfRadius(one,BlasterRadius);
            int pos=FINDEQUALNUM(whereCan,two);
            if(pos==-1)
            {
                ret=false;
            }
            return ret;
        }


        private List<ICoordinates> ListOfVisible(int i, int j)
        {
            CCoordinatesLen cur = new CCoordinatesLen();
            CCoordinatesLen co = new CCoordinatesLen();
            co.X1=i;
            co.Y1=j;
            cur.Copy(co);
            cur.Len = 0;
            CCoordinatesLen co1=new CCoordinatesLen();
            co1.Copy(co);
            co1.Len=0;
            List<ICoordinates> ret = new List<ICoordinates>();
            List<CCoordinatesLen> forWithWall = new List<CCoordinatesLen>();
            List<ICoordinates> allied=new List<ICoordinates>();
            Stack<CCoordinatesLen> stack1 = new Stack<CCoordinatesLen>();
            Stack<CCoordinatesLen> stack2 = new Stack<CCoordinatesLen>();
            Stack<CCoordinatesLen> swap=null;
            ICoordinates temp = new CCoordinatesLen();
            CCoordinatesLen forPush=new CCoordinatesLen();
            ret.Add(co1);
            bool [,]gone=new bool[maxX+1,maxY+1]; 

            stack1.Push(cur);
            gone[cur.X1, cur.Y1] = true;

            for(int y=0;y<VisibleRadius;y++)
            {
                while(stack1.Count!=0)
                {
                    cur=new CCoordinatesLen();
                    cur.Copy(stack1.Pop());
                    temp.Copy(cur);
                    allied=AlliedNodes(temp);
                    foreach (ICoordinates x in allied)
                    {
                        if(!gone[x.X1,x.Y1] && !(map[x.X1,x.Y1]==TypesOfField.WALL))
                        {
                            forPush=new CCoordinatesLen();
                            forPush.Copy(x);
                            forPush.Len=y+1;
                            stack2.Push(forPush);
                            gone[x.X1,x.Y1]=true;
                        }
                    }
                }
                foreach( CCoordinatesLen x in stack2)
                {
                    cur=new CCoordinatesLen();
                    cur.Copy(x);
                    cur.Len=x.Len;
                    forWithWall.Add(cur);
                }
                swap=stack2;
                stack2=stack1;
                stack1=swap;
            }
            //bool found=false;
            
            List<CCoordinatesLen> forWithOutWall = ListOfRadiusLen(co,VisibleRadius);
            // так. теперь сравниваем. и добавляем.
            foreach (CCoordinatesLen x in forWithOutWall)
            {
                if ((FINDEQUALNUMLEN(forWithWall, x) != -1) && (forWithWall[FINDEQUALNUMLEN(forWithWall, x)].Len == /* >=*/ x.Len))
                {
                    ret.Add(x);
                }
            }
            return ret;
        }
    }
}
