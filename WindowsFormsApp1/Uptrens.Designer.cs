namespace WindowsFormsApp1
{
    partial class Uptrens
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxTime = new System.Windows.Forms.ComboBox();
            this.comboBoxCount = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.FormattingEnabled = true;
            this.comboBoxTime.Location = new System.Drawing.Point(40, 146);
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.Size = new System.Drawing.Size(121, 24);
            this.comboBoxTime.TabIndex = 12;
            // 
            // comboBoxCount
            // 
            this.comboBoxCount.FormattingEnabled = true;
            this.comboBoxCount.Location = new System.Drawing.Point(40, 68);
            this.comboBoxCount.Name = "comboBoxCount";
            this.comboBoxCount.Size = new System.Drawing.Size(121, 24);
            this.comboBoxCount.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Время занятий в минутах";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Количесвто занятий в неделю";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(33, 206);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(177, 54);
            this.button1.TabIndex = 13;
            this.button1.Text = "Изменить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Uptrens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 342);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxTime);
            this.Controls.Add(this.comboBoxCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "Uptrens";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Изменить тренировки";
            this.Load += new System.EventHandler(this.Uptrens_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTime;
        private System.Windows.Forms.ComboBox comboBoxCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}