namespace Route
{
    partial class RouteForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.encryptTriple = new System.Windows.Forms.CheckBox();
            this.encrypt = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.encryptDest = new System.Windows.Forms.TextBox();
            this.encryptSource = new System.Windows.Forms.TextBox();
            this.encryptKey = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.decryptTriple = new System.Windows.Forms.CheckBox();
            this.decrypt = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.decryptDest = new System.Windows.Forms.TextBox();
            this.decryptSource = new System.Windows.Forms.TextBox();
            this.decryptKey = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(982, 651);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.encryptTriple);
            this.tabPage1.Controls.Add(this.encrypt);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.encryptDest);
            this.tabPage1.Controls.Add(this.encryptSource);
            this.tabPage1.Controls.Add(this.encryptKey);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(974, 618);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Режим шифрования";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // encryptTriple
            // 
            this.encryptTriple.AutoSize = true;
            this.encryptTriple.Location = new System.Drawing.Point(621, 562);
            this.encryptTriple.Name = "encryptTriple";
            this.encryptTriple.Size = new System.Drawing.Size(145, 24);
            this.encryptTriple.TabIndex = 8;
            this.encryptTriple.Text = "Модификация";
            this.encryptTriple.UseVisualStyleBackColor = true;
            // 
            // encrypt
            // 
            this.encrypt.Location = new System.Drawing.Point(358, 546);
            this.encrypt.Name = "encrypt";
            this.encrypt.Size = new System.Drawing.Size(178, 54);
            this.encrypt.TabIndex = 6;
            this.encrypt.Text = "шифруем";
            this.encrypt.UseVisualStyleBackColor = true;
            this.encrypt.Click += new System.EventHandler(this.encrypt_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(90, 321);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Шифрованный текст";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Исходный текст";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(90, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ключ";
            // 
            // encryptDest
            // 
            this.encryptDest.Location = new System.Drawing.Point(90, 344);
            this.encryptDest.Multiline = true;
            this.encryptDest.Name = "encryptDest";
            this.encryptDest.ReadOnly = true;
            this.encryptDest.Size = new System.Drawing.Size(773, 183);
            this.encryptDest.TabIndex = 2;
            // 
            // encryptSource
            // 
            this.encryptSource.Location = new System.Drawing.Point(90, 130);
            this.encryptSource.Multiline = true;
            this.encryptSource.Name = "encryptSource";
            this.encryptSource.Size = new System.Drawing.Size(773, 171);
            this.encryptSource.TabIndex = 1;
            // 
            // encryptKey
            // 
            this.encryptKey.Location = new System.Drawing.Point(90, 57);
            this.encryptKey.Name = "encryptKey";
            this.encryptKey.Size = new System.Drawing.Size(773, 26);
            this.encryptKey.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.decryptTriple);
            this.tabPage2.Controls.Add(this.decrypt);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.decryptDest);
            this.tabPage2.Controls.Add(this.decryptSource);
            this.tabPage2.Controls.Add(this.decryptKey);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(974, 618);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Режим расшифрования";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // decryptTriple
            // 
            this.decryptTriple.AutoSize = true;
            this.decryptTriple.Location = new System.Drawing.Point(618, 558);
            this.decryptTriple.Name = "decryptTriple";
            this.decryptTriple.Size = new System.Drawing.Size(145, 24);
            this.decryptTriple.TabIndex = 14;
            this.decryptTriple.Text = "Модификация";
            this.decryptTriple.UseVisualStyleBackColor = true;
            // 
            // decrypt
            // 
            this.decrypt.Location = new System.Drawing.Point(359, 542);
            this.decrypt.Name = "decrypt";
            this.decrypt.Size = new System.Drawing.Size(178, 54);
            this.decrypt.TabIndex = 13;
            this.decrypt.Text = "расшифровываем";
            this.decrypt.UseVisualStyleBackColor = true;
            this.decrypt.Click += new System.EventHandler(this.decrypt_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(91, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Шифрованный текст";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 322);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Исходный текст";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(91, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Ключ";
            // 
            // decryptDest
            // 
            this.decryptDest.Location = new System.Drawing.Point(91, 345);
            this.decryptDest.Multiline = true;
            this.decryptDest.Name = "decryptDest";
            this.decryptDest.ReadOnly = true;
            this.decryptDest.Size = new System.Drawing.Size(773, 178);
            this.decryptDest.TabIndex = 9;
            // 
            // decryptSource
            // 
            this.decryptSource.Location = new System.Drawing.Point(91, 126);
            this.decryptSource.Multiline = true;
            this.decryptSource.Name = "decryptSource";
            this.decryptSource.Size = new System.Drawing.Size(773, 176);
            this.decryptSource.TabIndex = 8;
            // 
            // decryptKey
            // 
            this.decryptKey.Location = new System.Drawing.Point(91, 53);
            this.decryptKey.Name = "decryptKey";
            this.decryptKey.Size = new System.Drawing.Size(773, 26);
            this.decryptKey.TabIndex = 7;
            // 
            // RouteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 651);
            this.Controls.Add(this.tabControl1);
            this.Name = "RouteForm";
            this.Text = "Шифр маршрутной перестановки";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button encrypt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox encryptDest;
        private System.Windows.Forms.TextBox encryptSource;
        private System.Windows.Forms.TextBox encryptKey;
        private System.Windows.Forms.Button decrypt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox decryptDest;
        private System.Windows.Forms.TextBox decryptSource;
        private System.Windows.Forms.TextBox decryptKey;
        private System.Windows.Forms.CheckBox encryptTriple;
        private System.Windows.Forms.CheckBox decryptTriple;
    }
}

