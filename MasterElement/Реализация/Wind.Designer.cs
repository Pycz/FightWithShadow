namespace MasterElement
{
    partial class Wind
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Wind));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Init = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.bot1name = new System.Windows.Forms.Label();
            this.bot2name = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.bot1heal = new System.Windows.Forms.Label();
            this.bot1point = new System.Windows.Forms.Label();
            this.bot2heal = new System.Windows.Forms.Label();
            this.bot2point = new System.Windows.Forms.Label();
            this.timetoend = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeight = 10;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 10;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(640, 640);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.TabStop = false;
            // 
            // Init
            // 
            this.Init.Location = new System.Drawing.Point(680, 457);
            this.Init.Name = "Init";
            this.Init.Size = new System.Drawing.Size(92, 37);
            this.Init.TabIndex = 1;
            this.Init.Text = "Перерисовка карты";
            this.Init.UseVisualStyleBackColor = true;
            this.Init.Click += new System.EventHandler(this.Init_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(680, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 29);
            this.button1.TabIndex = 3;
            this.button1.Text = "След. Ход";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bot1name
            // 
            this.bot1name.AutoSize = true;
            this.bot1name.Location = new System.Drawing.Point(682, 13);
            this.bot1name.Name = "bot1name";
            this.bot1name.Size = new System.Drawing.Size(35, 13);
            this.bot1name.TabIndex = 4;
            this.bot1name.Text = "label1";
            // 
            // bot2name
            // 
            this.bot2name.AutoSize = true;
            this.bot2name.Location = new System.Drawing.Point(737, 13);
            this.bot2name.Name = "bot2name";
            this.bot2name.Size = new System.Drawing.Size(35, 13);
            this.bot2name.TabIndex = 5;
            this.bot2name.Text = "label2";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(680, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(37, 34);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(735, 29);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(37, 34);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // bot1heal
            // 
            this.bot1heal.AutoSize = true;
            this.bot1heal.ForeColor = System.Drawing.Color.Red;
            this.bot1heal.Location = new System.Drawing.Point(682, 71);
            this.bot1heal.Name = "bot1heal";
            this.bot1heal.Size = new System.Drawing.Size(35, 13);
            this.bot1heal.TabIndex = 8;
            this.bot1heal.Text = "label3";
            // 
            // bot1point
            // 
            this.bot1point.AutoSize = true;
            this.bot1point.ForeColor = System.Drawing.Color.LimeGreen;
            this.bot1point.Location = new System.Drawing.Point(682, 90);
            this.bot1point.Name = "bot1point";
            this.bot1point.Size = new System.Drawing.Size(35, 13);
            this.bot1point.TabIndex = 9;
            this.bot1point.Text = "label4";
            // 
            // bot2heal
            // 
            this.bot2heal.AutoSize = true;
            this.bot2heal.ForeColor = System.Drawing.Color.Red;
            this.bot2heal.Location = new System.Drawing.Point(737, 71);
            this.bot2heal.Name = "bot2heal";
            this.bot2heal.Size = new System.Drawing.Size(35, 13);
            this.bot2heal.TabIndex = 10;
            this.bot2heal.Text = "label5";
            // 
            // bot2point
            // 
            this.bot2point.AutoSize = true;
            this.bot2point.ForeColor = System.Drawing.Color.LimeGreen;
            this.bot2point.Location = new System.Drawing.Point(737, 90);
            this.bot2point.Name = "bot2point";
            this.bot2point.Size = new System.Drawing.Size(35, 13);
            this.bot2point.TabIndex = 11;
            this.bot2point.Text = "label6";
            // 
            // timetoend
            // 
            this.timetoend.AutoSize = true;
            this.timetoend.Location = new System.Drawing.Point(710, 200);
            this.timetoend.Name = "timetoend";
            this.timetoend.Size = new System.Drawing.Size(35, 13);
            this.timetoend.TabIndex = 12;
            this.timetoend.Text = "label7";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(400, 320);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(92, 20);
            this.textBox1.TabIndex = 13;
            this.textBox1.Text = "map.dat";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(680, 346);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 58);
            this.button2.TabIndex = 14;
            this.button2.Text = "Загрузить карту и начать заново";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(677, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Времени осталось ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(663, 275);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(676, 247);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Результаты игры";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(680, 150);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(92, 34);
            this.button3.TabIndex = 19;
            this.button3.Text = "Начать заново с новой картой";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(680, 410);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(92, 41);
            this.button4.TabIndex = 20;
            this.button4.Text = "Сохранить карту";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(400, 500);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(91, 37);
            this.button5.TabIndex = 21;
            this.button5.Text = "Сыграть всю игру";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Wind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 662);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.timetoend);
            this.Controls.Add(this.bot2point);
            this.Controls.Add(this.bot2heal);
            this.Controls.Add(this.bot1point);
            this.Controls.Add(this.bot1heal);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.bot2name);
            this.Controls.Add(this.bot1name);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Init);
            this.Controls.Add(this.dataGridView1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 700);
            this.MinimumSize = new System.Drawing.Size(800, 700);
            this.Name = "Wind";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поединок Альфа v.0.2.0";
            this.Load += new System.EventHandler(this.Wind_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button Init;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label bot1name;
        private System.Windows.Forms.Label bot2name;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label bot1heal;
        private System.Windows.Forms.Label bot1point;
        private System.Windows.Forms.Label bot2heal;
        private System.Windows.Forms.Label bot2point;
        private System.Windows.Forms.Label timetoend;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}