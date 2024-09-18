namespace hw1
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_loadimage = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_intensity_level = new System.Windows.Forms.Button();
            this.btn_shrink = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.shrink_num = new System.Windows.Forms.TextBox();
            this.btn_zoom = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.zoom_num = new System.Windows.Forms.TextBox();
            this.method = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.level_num = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_loadimage
            // 
            this.btn_loadimage.Location = new System.Drawing.Point(1352, 24);
            this.btn_loadimage.Name = "btn_loadimage";
            this.btn_loadimage.Size = new System.Drawing.Size(169, 80);
            this.btn_loadimage.TabIndex = 0;
            this.btn_loadimage.Text = "Load image";
            this.btn_loadimage.UseVisualStyleBackColor = true;
            this.btn_loadimage.Click += new System.EventHandler(this.btn_loadimage_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(40, 49);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(324, 292);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btn_intensity_level
            // 
            this.btn_intensity_level.Location = new System.Drawing.Point(1352, 162);
            this.btn_intensity_level.Name = "btn_intensity_level";
            this.btn_intensity_level.Size = new System.Drawing.Size(169, 73);
            this.btn_intensity_level.TabIndex = 2;
            this.btn_intensity_level.Text = "Intensity level";
            this.btn_intensity_level.UseVisualStyleBackColor = true;
            this.btn_intensity_level.Click += new System.EventHandler(this.btn_intensity_level_Click);
            // 
            // btn_shrink
            // 
            this.btn_shrink.Location = new System.Drawing.Point(1134, 336);
            this.btn_shrink.Name = "btn_shrink";
            this.btn_shrink.Size = new System.Drawing.Size(169, 78);
            this.btn_shrink.TabIndex = 4;
            this.btn_shrink.Text = "shrink";
            this.btn_shrink.UseVisualStyleBackColor = true;
            this.btn_shrink.Click += new System.EventHandler(this.btn_shrink_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(413, 49);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(352, 292);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // shrink_num
            // 
            this.shrink_num.Location = new System.Drawing.Point(1181, 445);
            this.shrink_num.Name = "shrink_num";
            this.shrink_num.Size = new System.Drawing.Size(100, 25);
            this.shrink_num.TabIndex = 6;
            // 
            // btn_zoom
            // 
            this.btn_zoom.Location = new System.Drawing.Point(1352, 336);
            this.btn_zoom.Name = "btn_zoom";
            this.btn_zoom.Size = new System.Drawing.Size(176, 78);
            this.btn_zoom.TabIndex = 7;
            this.btn_zoom.Text = "zoom";
            this.btn_zoom.UseVisualStyleBackColor = true;
            this.btn_zoom.Click += new System.EventHandler(this.btn_zoom_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(1054, 445);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "shrink rate:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(1312, 435);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "zoom rate:";
            // 
            // zoom_num
            // 
            this.zoom_num.Location = new System.Drawing.Point(1428, 435);
            this.zoom_num.Name = "zoom_num";
            this.zoom_num.Size = new System.Drawing.Size(100, 25);
            this.zoom_num.TabIndex = 10;
            // 
            // method
            // 
            this.method.FormattingEnabled = true;
            this.method.Items.AddRange(new object[] {
            "Nearest",
            "Bilinear",
            "Bicubic"});
            this.method.Location = new System.Drawing.Point(1316, 268);
            this.method.Name = "method";
            this.method.Size = new System.Drawing.Size(121, 23);
            this.method.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(1198, 268);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "method:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(1149, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 28);
            this.label5.TabIndex = 14;
            this.label5.Text = "level:";
            // 
            // level_num
            // 
            this.level_num.Location = new System.Drawing.Point(1246, 207);
            this.level_num.Name = "level_num";
            this.level_num.Size = new System.Drawing.Size(100, 25);
            this.level_num.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1587, 603);
            this.Controls.Add(this.level_num);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.method);
            this.Controls.Add(this.zoom_num);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_zoom);
            this.Controls.Add(this.shrink_num);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btn_shrink);
            this.Controls.Add(this.btn_intensity_level);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_loadimage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_loadimage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_intensity_level;
        private System.Windows.Forms.Button btn_shrink;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox shrink_num;
        private System.Windows.Forms.Button btn_zoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox zoom_num;
        private System.Windows.Forms.ComboBox method;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox level_num;
    }
}

