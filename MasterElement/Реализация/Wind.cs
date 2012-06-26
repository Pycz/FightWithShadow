using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCStepsCoords;
using Interfaces;
using Logic;
using MasterElement.Properties;
using MasterElement;
using System.Reflection;

namespace MasterElement
{

    public partial class Wind : Form
    {
        Bitmap BON;
        Bitmap NOTH;
        Bitmap BOT1;
        Bitmap BOT2;
        Bitmap MED;
        Bitmap WALL;

        Bitmap BONRED;
        Bitmap NOTHRED;
        Bitmap MEDRED;

        Bitmap BONBLUE;
        Bitmap NOTHBLUE;
        Bitmap MEDBLUE;

        Bitmap BONMULT;
        Bitmap NOTHMULT;
        Bitmap MEDMULT;

        Loging LogWindow;

        masterСhief IMGODDAMMITAMAZING;

        public Wind()
        {
            InitializeComponent();
            IMGODDAMMITAMAZING = new masterСhief();
            IMGODDAMMITAMAZING.map.Generate();
            IMGODDAMMITAMAZING.initializ();
        }

        private void Init_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();

            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            DataGridViewImageCell imgCell = new DataGridViewImageCell();
            DataGridViewRow row = new DataGridViewRow();
            int razm = 10;
            if (IMGODDAMMITAMAZING.map.maxX > IMGODDAMMITAMAZING.map.maxY)
            {
                razm = 640 / IMGODDAMMITAMAZING.map.maxX;
            }
            else
            {
                razm = 640 / IMGODDAMMITAMAZING.map.maxY;
            }

            // столбцы
            for (int i = IMGODDAMMITAMAZING.map.maxX; i >= 1; i--)
            {
                imgCol = new DataGridViewImageColumn();
                imgCol.Width = razm;
                dataGridView1.Columns.Add(imgCol);
            }
            // строки с ячейками
            for (int i = IMGODDAMMITAMAZING.map.maxY; i >= 1; i--)
            {
                row = new DataGridViewRow();
                for (int j = 1; j <= IMGODDAMMITAMAZING.map.maxX; j++)
                {
                    imgCell = new DataGridViewImageCell();
                    imgCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    switch (IMGODDAMMITAMAZING.map.map[j, i])
                    {
                        case TypesOfField.BONUS:
                            imgCell.Value = BON;
                            break;
                        case TypesOfField.HI:
                            imgCell.Value = BOT2;
                            break;
                        case TypesOfField.ME:
                            imgCell.Value = BOT1;
                            break;
                        case TypesOfField.MEDKIT:
                            imgCell.Value = MED;
                            break;
                        case TypesOfField.NOTHING:
                            imgCell.Value = NOTH;
                            break;
                        case TypesOfField.WALL:
                            imgCell.Value = WALL;
                            break;
                    }
                    row.Cells.Add(imgCell);
                }
                row.Height = razm;
                dataGridView1.Rows.Add(row);
            }
            // видимость
            foreach (ICoordinates x in IMGODDAMMITAMAZING.map.Visibles[IMGODDAMMITAMAZING.Bot1.Position.X1, IMGODDAMMITAMAZING.Bot1.Position.Y1])
            {
                if (dataGridView1[x.X0,IMGODDAMMITAMAZING.map.maxY - x.Y0-1].Value == MED)
                    dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value = MEDRED;
                if (dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value == BON)
                    dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value = BONRED;
                if (dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value == NOTH)
                    dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value = NOTHRED;
            }
            foreach (ICoordinates x in IMGODDAMMITAMAZING.map.Visibles[IMGODDAMMITAMAZING.Bot2.Position.X1, IMGODDAMMITAMAZING.Bot2.Position.Y1])
            {
                if (dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0-1].Value == MED)
                    dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value = MEDBLUE;
                if (dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value == BON)
                    dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value = BONBLUE;
                if (dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value == NOTH)
                    dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value = NOTHBLUE;

                if (dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value == NOTHRED)
                    dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value = NOTHMULT;
                if (dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value == BONRED)
                    dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value = BONMULT;
                if (dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value == MEDRED)
                    dataGridView1[x.X0, IMGODDAMMITAMAZING.map.maxY - x.Y0 - 1].Value = MEDMULT;
            }
                            
        }

        private void RefreshLables()
        {
            if (IMGODDAMMITAMAZING.WhoWin == -1)
                label3.Text = "Игра продолжается";
            if (IMGODDAMMITAMAZING.WhoWin == 1)
                label3.Text = "Выйграл бот 1 - "+IMGODDAMMITAMAZING.Bot1.Name;
            if (IMGODDAMMITAMAZING.WhoWin == 2)
                label3.Text = "Выйграл бот 2 - " + IMGODDAMMITAMAZING.Bot2.Name;
            if (IMGODDAMMITAMAZING.WhoWin == 0)
                label3.Text = "Это ничья";
            bot1heal.Text = IMGODDAMMITAMAZING.Bot1.Health.ToString();
            bot2heal.Text = IMGODDAMMITAMAZING.Bot2.Health.ToString();
            bot1point.Text = IMGODDAMMITAMAZING.Bot1.Points.ToString();
            bot2point.Text = IMGODDAMMITAMAZING.Bot2.Points.ToString();
            timetoend.Text = IMGODDAMMITAMAZING.endAfterTime().ToString();
        }

        private void Wind_Load(object sender, EventArgs e)
        {
            //OK
            this.Icon = Properties.Resources.Fight;
            
            BON = Properties.Resources.BONUS;
            NOTH = Properties.Resources.NOTHING;
            BOT1 = Properties.Resources.BOT1;
            BOT2 = Properties.Resources.BOT2;
            MED = Properties.Resources.MED;
            WALL = Properties.Resources.WALL;

            BONRED = Properties.Resources.BONUSRED;
            NOTHRED = Properties.Resources.NOTHINGRED;
            MEDRED = Properties.Resources.MEDRED;

            BONBLUE = Properties.Resources.BONUSBLUE;
            NOTHBLUE = Properties.Resources.NOTHINGBLUE;
            MEDBLUE = Properties.Resources.MEDBLUE;

            BONMULT = Properties.Resources.BONUSMUL;
            NOTHMULT = Properties.Resources.NOTHINGMUL;
            MEDMULT = Properties.Resources.MEDMUL;

            // врубить двойную буферизацию
               typeof(DataGridView).InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               dataGridView1,
               new object[] { true });

            IMGODDAMMITAMAZING.Bot1.GetName();
            bot1name.Text = IMGODDAMMITAMAZING.Bot1.Name;
            IMGODDAMMITAMAZING.Bot2.GetName();
            bot2name.Text = IMGODDAMMITAMAZING.Bot2.Name;
            Init_Click(null, null);
            RefreshLables();

            LogWindow = new Loging();
            LogWindow.Show();

        }

        private void genMap_Click(object sender, EventArgs e)
        {
            IMGODDAMMITAMAZING.map.Generate();
            RefreshLables();
        }

        private void loop_Click(object sender, EventArgs e)
        {
                genMap_Click(null, null);
                Init_Click(null, null);
                RefreshLables();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            IMGODDAMMITAMAZING.NextTurn();
            bot1heal.Text = IMGODDAMMITAMAZING.Bot1.Health.ToString();
            if (IMGODDAMMITAMAZING.Bot1.Health < 0)
                bot1heal.Text = "0";
            bot2heal.Text = IMGODDAMMITAMAZING.Bot2.Health.ToString();
            if (IMGODDAMMITAMAZING.Bot2.Health < 0)
                bot2heal.Text = "0";
            bot1point.Text = IMGODDAMMITAMAZING.Bot1.Points.ToString();
            bot2point.Text = IMGODDAMMITAMAZING.Bot2.Points.ToString();
            timetoend.Text = IMGODDAMMITAMAZING.endAfterTime().ToString();
            IMGODDAMMITAMAZING.ResGame();
            label3.Text = IMGODDAMMITAMAZING.WhoWin.ToString();
            if (IMGODDAMMITAMAZING.WhoWin != -1)
            {
                button1.Text = "Конец игры";
                button1.Enabled = false;
                LogWindow.TheLog.Text += IMGODDAMMITAMAZING.reason;
            }
            Init_Click(null, null);
            RefreshLables();
            string temp="";
            temp+="Бот 1 "+IMGODDAMMITAMAZING.Bot1.Name+" сходил на клетку (" + IMGODDAMMITAMAZING.Bot1.Position.X1.ToString()+","
                + IMGODDAMMITAMAZING.Bot1.Position.Y1.ToString() + ")" + Environment.NewLine;
            LogWindow.TheLog.Text += temp;
            temp = "";
            temp += "Бот 2 " + IMGODDAMMITAMAZING.Bot2.Name + " сходил на клетку (" + IMGODDAMMITAMAZING.Bot2.Position.X1.ToString() + ","
                + IMGODDAMMITAMAZING.Bot2.Position.Y1.ToString() + ")" + Environment.NewLine;
            LogWindow.TheLog.Text += temp;
            
        }

        private void initBots_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IMGODDAMMITAMAZING.ResetGameFromFile(textBox1.Text);
            button1.Enabled = true;
            button1.Text = "След. Ход";
            Init_Click(null, null);
            RefreshLables();
            LogWindow.TheLog.Text = "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            IMGODDAMMITAMAZING.ResetGame();
            button1.Enabled = true;
            button1.Text = "След. Ход";
            Init_Click(null, null);
            RefreshLables();
            LogWindow.TheLog.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IMGODDAMMITAMAZING.map.WriteToFile(textBox1.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            while ((IMGODDAMMITAMAZING.WhoWin == -1))
            {
                button1_Click(null, null);
            }

        }

        

    }
}
