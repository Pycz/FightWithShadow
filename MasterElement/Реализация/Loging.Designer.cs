namespace MasterElement
{
    partial class Loging
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
            this.TheLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TheLog
            // 
            this.TheLog.Location = new System.Drawing.Point(12, 12);
            this.TheLog.Multiline = true;
            this.TheLog.Name = "TheLog";
            this.TheLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TheLog.Size = new System.Drawing.Size(749, 238);
            this.TheLog.TabIndex = 0;
            // 
            // Loging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 262);
            this.Controls.Add(this.TheLog);
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 38);
            this.Name = "Loging";
            this.Text = "Loging";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox TheLog;

    }
}