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
        
        Player[] player = new Player[1];        //PCC数量暂定一个
        int animation_ctrl = 0;
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
            Player.key_ctrl(player,e);
            Draw();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            
            player[0] = new Player();
            player[0].figure = new Bitmap(Properties.Resources._fighter2);
            player[0].figure.SetResolution(96, 96);
            player[0].is_active = 1;
            Draw();
        }
        
        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            Player.Draw(player,myBuffer.Graphics);
            //release resource
            myBuffer.Render();
            myBuffer.Dispose();
        }

        public class Player
        {
            //Class of PCC Role
            public int x = 50;                      //x coordinate
            public int y = 50;                      //y coordinate
            public int face = 1;
            public static int move_step = 15;
            public int anm_frame = 0;               //current animation frame
            public long last_walk_time = 0;         //record last moving time point
            public long walk_interval = 100;        //set time interval between two steps
            public int speed = 20;

            public Bitmap figure;                   //figure picture
            public static int current_player = 0;   //current player
            public int is_active = 0;               //current player is active or not
            public Player()
            {
                figure = new Bitmap(Properties.Resources._fighter2);
                figure.SetResolution(96, 96);
            }
            public static void key_ctrl(Player[] player, KeyEventArgs e)
            {
                Player p = player[current_player];
                //change role （由于暂时不用该功能，所以先注释掉）
                //if (e.KeyCode == Keys.Tab)  {  key_change_player(player);  }
                
                //change face or not
                if (e.KeyCode == Keys.Up && p.face != 4) { p.face = 4; }
                if (e.KeyCode == Keys.Down && p.face != 1) { p.face = 1; }
                if (e.KeyCode == Keys.Left && p.face != 2) { p.face = 2; }
                if (e.KeyCode == Keys.Right && p.face != 3) { p.face = 3; }

                //time interval determination
                if (Comm.Time() - p.last_walk_time <= p.walk_interval)
                    return;

                //move
                if (e.KeyCode == Keys.Up) { p.y -= p.speed; }
                else if (e.KeyCode == Keys.Down) { p.y += p.speed; }
                else if (e.KeyCode == Keys.Left) { p.x -= p.speed; }
                else if (e.KeyCode == Keys.Right) { p.x += p.speed; }
                else return;

                //animation frame
                p.anm_frame += 1;
                if (p.anm_frame >= int.MaxValue) p.anm_frame = 0;
                //time
                p.last_walk_time = Comm.Time();
            }
            public static void Draw(Player[] player, Graphics g)
            {
                Player p = player[current_player];
                Rectangle crazycoderRgl = new Rectangle(p.figure.Width / 4 * (p.anm_frame % 4), p.figure.Height / 4 * (p.face - 1), p.figure.Width / 4, p.figure.Height / 4);
                Bitmap b0 = p.figure.Clone(crazycoderRgl, p.figure.PixelFormat);
                g.DrawImage(b0, p.x, p.y);
            }

            public static void key_change_player(Player[] player)
            {
                //this function change the role
                for (int i = current_player + 1; i < player.Length; i++)
                {
                    if (player[i].is_active == 1)
                    {
                        set_player(player, current_player, i);
                        return;
                    }
                }
                for (int i = 0; i < current_player; i++)
                {
                    if (player[i].is_active == 1)
                    {
                        set_player(player, current_player, i);
                        return;
                    }
                }
            }

            public static void set_player(Player[] player, int oldindex, int newindex)
            {
                current_player = newindex;
                player[newindex].x = player[oldindex].x;
                player[newindex].y = player[oldindex].y;
                player[newindex].face = player[oldindex].face;
            }

            public static void key_ctrl_up(Player[] player, KeyEventArgs e)
            {
                //this function currect the face of the figure when the key is up
                Player p = player[current_player];
                //animation frame
                p.anm_frame = 0;
                p.last_walk_time = 0;
            }
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            Player.key_ctrl_up(player, e);
            Draw();
        }
    }
}
