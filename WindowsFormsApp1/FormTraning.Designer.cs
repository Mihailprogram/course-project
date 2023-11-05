namespace WindowsFormsApp1
{
    partial class FormTraning
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
            this.comboBoxTrener = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBoxCount = new System.Windows.Forms.ComboBox();
            this.comboBoxTime = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBoxTrener
            // 
            this.comboBoxTrener.FormattingEnabled = true;
            this.comboBoxTrener.Location = new System.Drawing.Point(85, 97);
            this.comboBoxTrener.Name = "comboBoxTrener";
            this.comboBoxTrener.Size = new System.Drawing.Size(145, 24);
            this.comboBoxTrener.TabIndex = 0;
            this.comboBoxTrener.SelectedIndexChanged += new System.EventHandler(this.comboBoxTrener_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Тренер";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Количесвто занятий в неделю";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(82, 215);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Время занятий в минутах";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(85, 302);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 52);
            this.button1.TabIndex = 6;
            this.button1.Text = "Записаться";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBoxCount
            // 
            this.comboBoxCount.FormattingEnabled = true;
            this.comboBoxCount.Location = new System.Drawing.Point(85, 177);
            this.comboBoxCount.Name = "comboBoxCount";
            this.comboBoxCount.Size = new System.Drawing.Size(121, 24);
            this.comboBoxCount.TabIndex = 7;
            this.comboBoxCount.SelectedIndexChanged += new System.EventHandler(this.comboBoxCount_SelectedIndexChanged);
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.FormattingEnabled = true;
            this.comboBoxTime.Location = new System.Drawing.Point(85, 255);
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.Size = new System.Drawing.Size(121, 24);
            this.comboBoxTime.TabIndex = 8;
            this.comboBoxTime.SelectedIndexChanged += new System.EventHandler(this.comboBoxTime_SelectedIndexChanged);
            // 
            // FormTraning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 426);
            this.Controls.Add(this.comboBoxTime);
            this.Controls.Add(this.comboBoxCount);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxTrener);
            this.Name = "FormTraning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Тренировка";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTraning_FormClosing);
            this.Load += new System.EventHandler(this.FormTraning_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTrener;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBoxCount;
        private System.Windows.Forms.ComboBox comboBoxTime;
    }
}