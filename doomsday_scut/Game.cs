using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doomsday_scut
{
    public partial class Game : Form
    {
        int x = 50, y = 50;
        int face = 1;
        int animation_ctrl = 0;
        int move_step = 15;
        bool start = true;
        Bitmap fighter2 = new Bitmap(Properties.Resources._fighter2);
        public Game()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
        }

        
        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            start = false;
            if (e.KeyCode == Keys.Up) { y -= move_step; face = 4; }
            if (e.KeyCode == Keys.Down) { y += move_step; face = 1; }
            if (e.KeyCode == Keys.Left) { x -= move_step; face = 2; }
            if (e.KeyCode == Keys.Right) { x += move_step; face = 3; }
            Draw();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            Draw();
        }
        
        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageBox.Show("Are you sure to exit?", "Exit", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            //{
            //    e.Cancel = true;
            //}
            //else
            //{
            //    Dispose();
            //    Application.Exit();
            //}
            Application.Exit();
        }


        public void Draw()
        {
            if(start)
                pictureBox1.Refresh();
            fighter2.SetResolution(96, 96);
            //create image g1 on pictureBox1
            Graphics g1 = pictureBox1.CreateGraphics();
            //create image on RAM and let g be the image of picturebox1
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(g1, this.DisplayRectangle);
            //draw image
            animation_ctrl += 1;
            Rectangle crazycoderRgl = new Rectangle(fighter2.Width / 4 * (animation_ctrl % 4), fighter2.Height / 4 * (face - 1), fighter2.Width / 4, fighter2.Height / 4);
            Bitmap b0 = fighter2.Clone(crazycoderRgl, fighter2.PixelFormat);
            Graphics g = myBuffer.Graphics;

            g.DrawImage(b0, x, y);
            myBuffer.Render();
            myBuffer.Dispose();
        }
    }
}
