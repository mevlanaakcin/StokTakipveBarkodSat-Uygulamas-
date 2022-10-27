namespace Barkod
{
    partial class fiyatsor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fiyatsor));
            this.label2 = new System.Windows.Forms.Label();
            this.barcodeTxt = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.price = new System.Windows.Forms.Label();
            this.ordername = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "BARKOD NUMARASI :";
            // 
            // barcodeTxt
            // 
            this.barcodeTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.barcodeTxt.Location = new System.Drawing.Point(219, 101);
            this.barcodeTxt.Name = "barcodeTxt";
            this.barcodeTxt.Size = new System.Drawing.Size(216, 41);
            this.barcodeTxt.TabIndex = 24;
            this.barcodeTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.barcodeTxt_KeyDown);
            this.barcodeTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.barcodeTxt_KeyPress);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(119, 94);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(93, 54);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Location = new System.Drawing.Point(218, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "ÜRÜN FİYATI :";
            // 
            // price
            // 
            this.price.AutoSize = true;
            this.price.BackColor = System.Drawing.Color.White;
            this.price.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.price.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(230)))), ((int)(((byte)(118)))));
            this.price.Location = new System.Drawing.Point(214, 243);
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(182, 63);
            this.price.TabIndex = 27;
            this.price.Text = "0,00 ₺";
            this.price.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ordername
            // 
            this.ordername.AutoSize = true;
            this.ordername.Cursor = System.Windows.Forms.Cursors.Default;
            this.ordername.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ordername.Location = new System.Drawing.Point(114, 170);
            this.ordername.Name = "ordername";
            this.ordername.Size = new System.Drawing.Size(0, 29);
            this.ordername.TabIndex = 29;
            this.ordername.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 26);
            this.label1.TabIndex = 30;
            this.label1.Text = "Ürün Fiyat Sorgulama Ekranı";
            // 
            // fiyatsor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 328);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ordername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.price);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.barcodeTxt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fiyatsor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fiyatsor";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox barcodeTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label price;
        private System.Windows.Forms.Label ordername;
        private System.Windows.Forms.Label label1;
    }
}