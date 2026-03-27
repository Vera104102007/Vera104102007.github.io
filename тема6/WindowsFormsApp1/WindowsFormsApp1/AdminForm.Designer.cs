namespace WindowsFormsApp1
{
    partial class AdminForm
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
            comboBox1 = new ComboBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            numericUpDown1 = new NumericUpDown();
            groupBox3 = new GroupBox();
            textBox1 = new TextBox();
            groupBox4 = new GroupBox();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            label1 = new Label();
            textBox2 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            openFileDialog1 = new OpenFileDialog();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.Plum;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(7, 31);
            comboBox1.Margin = new Padding(4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(180, 24);
            comboBox1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Location = new Point(28, 21);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(397, 66);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Выберете тему:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(numericUpDown1);
            groupBox2.Location = new Point(28, 102);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(397, 71);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Выберете уровень:";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(7, 33);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(180, 22);
            numericUpDown1.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(textBox1);
            groupBox3.Location = new Point(28, 188);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(397, 100);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Введите вопрос";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(7, 38);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(180, 22);
            textBox1.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textBox5);
            groupBox4.Controls.Add(textBox4);
            groupBox4.Controls.Add(textBox3);
            groupBox4.Controls.Add(radioButton3);
            groupBox4.Controls.Add(radioButton2);
            groupBox4.Controls.Add(radioButton1);
            groupBox4.Location = new Point(28, 304);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(397, 162);
            groupBox4.TabIndex = 5;
            groupBox4.TabStop = false;
            groupBox4.Text = "Введите варианты ответа:";
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(18, 117);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(14, 13);
            radioButton3.TabIndex = 2;
            radioButton3.TabStop = true;
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(18, 77);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(14, 13);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(18, 38);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(14, 13);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 494);
            label1.Name = "label1";
            label1.Size = new Size(81, 16);
            label1.TabIndex = 6;
            label1.Text = "Картинка:";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(115, 491);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(310, 22);
            textBox2.TabIndex = 7;
            // 
            // button1
            // 
            button1.BackColor = Color.Pink;
            button1.Location = new Point(28, 535);
            button1.Name = "button1";
            button1.Size = new Size(166, 52);
            button1.TabIndex = 8;
            button1.Text = "Выбрать фото";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Pink;
            button2.Location = new Point(242, 535);
            button2.Name = "button2";
            button2.Size = new Size(183, 52);
            button2.TabIndex = 9;
            button2.Text = "Добавить вопрос в XML";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(49, 32);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(321, 22);
            textBox3.TabIndex = 3;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(49, 73);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(321, 22);
            textBox4.TabIndex = 4;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(49, 115);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(321, 22);
            textBox5.TabIndex = 5;
            // 
            // AdminForm
            // 
            AutoScaleDimensions = new SizeF(9F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Plum;
            ClientSize = new Size(461, 614);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            Margin = new Padding(4);
            Name = "AdminForm";
            Text = "AdminForm";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private TextBox textBox5;
        private TextBox textBox4;
        private TextBox textBox3;
    }
}