namespace Subiect2017Tic
{
    partial class GenereazaPoster
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
            this.label1 = new System.Windows.Forms.Label();
            this.ComboLocalitati = new System.Windows.Forms.ComboBox();
            this.ComboImagine = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PicBoxMain = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ImageListBox = new System.Windows.Forms.ListBox();
            this.TitluPosterTb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxMain)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Locatie";
            // 
            // ComboLocalitati
            // 
            this.ComboLocalitati.FormattingEnabled = true;
            this.ComboLocalitati.Location = new System.Drawing.Point(81, 12);
            this.ComboLocalitati.Margin = new System.Windows.Forms.Padding(4);
            this.ComboLocalitati.Name = "ComboLocalitati";
            this.ComboLocalitati.Size = new System.Drawing.Size(121, 28);
            this.ComboLocalitati.TabIndex = 2;
            this.ComboLocalitati.Text = "Localitate";
            // 
            // ComboImagine
            // 
            this.ComboImagine.FormattingEnabled = true;
            this.ComboImagine.Location = new System.Drawing.Point(81, 48);
            this.ComboImagine.Margin = new System.Windows.Forms.Padding(4);
            this.ComboImagine.Name = "ComboImagine";
            this.ComboImagine.Size = new System.Drawing.Size(121, 28);
            this.ComboImagine.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Imagine";
            // 
            // PicBoxMain
            // 
            this.PicBoxMain.Location = new System.Drawing.Point(234, 28);
            this.PicBoxMain.Margin = new System.Windows.Forms.Padding(4);
            this.PicBoxMain.Name = "PicBoxMain";
            this.PicBoxMain.Size = new System.Drawing.Size(621, 524);
            this.PicBoxMain.TabIndex = 6;
            this.PicBoxMain.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 176);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(189, 40);
            this.button1.TabIndex = 7;
            this.button1.Text = "Adauga imaginea";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AdaugaButtonClick);
            // 
            // ImageListBox
            // 
            this.ImageListBox.FormattingEnabled = true;
            this.ImageListBox.ItemHeight = 20;
            this.ImageListBox.Location = new System.Drawing.Point(14, 84);
            this.ImageListBox.Margin = new System.Windows.Forms.Padding(4);
            this.ImageListBox.Name = "ImageListBox";
            this.ImageListBox.Size = new System.Drawing.Size(188, 84);
            this.ImageListBox.TabIndex = 8;
            // 
            // TitluPosterTb
            // 
            this.TitluPosterTb.Location = new System.Drawing.Point(13, 262);
            this.TitluPosterTb.Margin = new System.Windows.Forms.Padding(4);
            this.TitluPosterTb.Name = "TitluPosterTb";
            this.TitluPosterTb.Size = new System.Drawing.Size(188, 26);
            this.TitluPosterTb.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 238);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Titlu Poster";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 312);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(189, 47);
            this.button2.TabIndex = 11;
            this.button2.Text = "Genereaza";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.GenereazaButtonClick);
            // 
            // GenereazaPoster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 676);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TitluPosterTb);
            this.Controls.Add(this.ImageListBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.PicBoxMain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ComboImagine);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ComboLocalitati);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GenereazaPoster";
            this.Text = "GenereazaPoster";
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComboLocalitati;
        private System.Windows.Forms.ComboBox ComboImagine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox PicBoxMain;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox ImageListBox;
        private System.Windows.Forms.TextBox TitluPosterTb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
    }
}