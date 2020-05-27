namespace doomsday_scut
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.exit = new System.Windows.Forms.Button();
            this.new_game = new System.Windows.Forms.Button();
            this.help_about = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // exit
            // 
            this.exit.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.exit.Location = new System.Drawing.Point(294, 416);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(271, 72);
            this.exit.TabIndex = 2;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // new_game
            // 
            this.new_game.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.new_game.Location = new System.Drawing.Point(294, 213);
            this.new_game.Name = "new_game";
            this.new_game.Size = new System.Drawing.Size(271, 72);
            this.new_game.TabIndex = 0;
            this.new_game.Text = "Start New Game";
            this.new_game.UseVisualStyleBackColor = true;
            this.new_game.Click += new System.EventHandler(this.new_game_Click);
            // 
            // help_about
            // 
            this.help_about.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.help_about.Location = new System.Drawing.Point(294, 313);
            this.help_about.Name = "help_about";
            this.help_about.Size = new System.Drawing.Size(271, 72);
            this.help_about.TabIndex = 1;
            this.help_about.Text = "About";
            this.help_about.UseVisualStyleBackColor = true;
            this.help_about.Click += new System.EventHandler(this.help_about_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::doomsday_scut.Properties.Resources.title;
            this.pictureBox1.Location = new System.Drawing.Point(12, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(837, 152);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 550);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.help_about);
            this.Controls.Add(this.new_game);
            this.Controls.Add(this.exit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Doomsday in SCUT v0.9";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button new_game;
        private System.Windows.Forms.Button help_about;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

