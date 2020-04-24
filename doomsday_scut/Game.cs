using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.SQLite;
using System.Threading;

namespace doomsday_scut
{
    public partial class Game : Form
    {
        
        Player[] player;        //PCC数量暂定一个
        Map[] map;
        List<Npc> npc;
        List<Bullet> bullet;
        bool start = true;
        static int resolution_value = 64;
        static double resolution_rate = 96.0 / resolution_value;   //只改分母
        struct pic
        {
            public string s;
            public int width;
            public int height;
            public pic(string s,int w,int h)
            {
                this.s = s;
                width = w;
                height = h;
            }
        }

        public Game()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            Point p = Player.key_ctrl(player, map, npc, bullet, e);
            if(p!=new Point(-1,-1))
            {
                x_textbox.Text = p.X.ToString();
                y_textbox.Text = p.Y.ToString();
            }
            if (player[0].trap_2_slow_down_enable && !player[0].trap_2_slow_down_effective)
            {
                traptimer_2_slow_down.Start();
                player[0].speed /= 2;
                player[0].trap_2_slow_down_effective = true;
            }
        }

        private void Game_Load(object sender, EventArgs e)
        {
            map = new Map[10];
            ProcessBar myprocessbar = new ProcessBar("Loading...", "Loading maps...");
            myprocessbar.Show();
            for (int i = 0; i < 10; i++)
            {
                map[i] = new Map(i);
                myprocessbar.setLabelText("Loading maps(" + Convert.ToString(i + 1) + "/10)...");
                myprocessbar.Refresh();
            }
            myprocessbar.Hide();
            initialize_game();
            Draw();
        }
        
        public void initialize_game()
        {
            npc = new List<Npc>();
            bullet = new List<Bullet>();
            map[Map.current_map].initialize_map_item();
            player = new Player[1];

            List<Bitmap> player_ani = new List<Bitmap>() {
                new Bitmap(Properties.Resources._fighter_2_attack_big_knife),
                new Bitmap(Properties.Resources._fighter2_attack),
                new Bitmap(Properties.Resources._fighter2_attack_long)
            };
            player[0] = new Player(576, 240, Comm.DOWN, player_ani) { figure = new Bitmap(Properties.Resources._fighter2) };
            player[0].figure.SetResolution(resolution_value, resolution_value);
            player[0].is_active = 1;
            player[0].weapon_restriction = 2;

            npc.Add(new Npc(240, 240, new Bitmap(Properties.Resources._npc2), Comm.ACTIVE_NPC, Comm.DOWN, new Bitmap(Properties.Resources.npc2_attack))
            {
                map = 0,
                idle_walk_direction = Comm.LEFT,
                idle_walk_time = 20

            });

            //npc.Add(new Npc(600, 240, new Bitmap(Properties.Resources._npc_static_3), Comm.DOWN, Comm.STATIC_NPC, null)
            //{
            //    map = 0
            //});

            npc.Add(new Npc(192, 240, new Bitmap(Properties.Resources._npc2), Comm.ACTIVE_NPC, Comm.DOWN, new Bitmap(Properties.Resources.npc2_attack))
            {
                map = 0,
                idle_walk_direction = Comm.LEFT,
                idle_walk_time = 10
            });

            npc.Add(new Npc(144, 240, new Bitmap(Properties.Resources._npc2), Comm.ACTIVE_NPC, Comm.DOWN, new Bitmap(Properties.Resources.npc2_attack))
            {
                map = 0,
                idle_walk_direction = Comm.LEFT,
                idle_walk_time = 30
            });

        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public void Draw()
        {
            if(start)
                pictureBox1.Refresh();
            //create image g1 on pictureBox1
            Graphics g1 = pictureBox1.CreateGraphics();
            //create image on RAM and let g be the image of picturebox1
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(g1, this.DisplayRectangle);
            //draw image
            
            Map.Draw(map, player, npc, bullet, myBuffer.Graphics, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            //Player.Draw(player,myBuffer.Graphics);
            //release resource
            myBuffer.Render();
            myBuffer.Dispose();
            start = false;
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            Player.key_ctrl_up(player, e);
        }

        public static Color lifebar_color(int totallife,int currentlife)
        {
            double r = Convert.ToDouble(currentlife) / Convert.ToDouble(totallife);
            if (r >= 0.6667) return Color.Lime;
            else if (0.3333 <= r && r < 0.6667) return Color.Yellow;
            else return Color.Red;
        }

        public static Rectangle lifebar_position(int totallife, int currentlife,int x,int y,int x_offset = 5,int y_offset = -5)
        {
            return new Rectangle(x + x_offset, y + y_offset, Convert.ToInt16((32.0 * resolution_rate - 10.0)*(Convert.ToDouble(currentlife) / Convert.ToDouble(totallife))), 5);
        }

        public class Father
        {
            //position
            //public int map = 0;        //map id
            public int x = 0;
            public int y = 0;
            public int coordinate_x = -1;
            public int coordinate_y = -1;

            ////show
            //public Bitmap bitmap;
            //public bool visible = true;

            public int face = Comm.DOWN;
            //public int walk_frame = 0;
            //public long last_walk_time = 0;
            //public long walk_interval = 300;
            //public int speed = Convert.ToInt32(Convert.ToDouble(32 * resolution_rate));
            //public int idle_walk_direction = Comm.DOWN;
            //public int idle_walk_time = 0;
            //public int idle_walk_time_now = 0;

            //public int attack = 10;

            //animation
            public Animation[] anm;
            public int anm_frame = 0;
            public int current_anm = -1;
            public long last_anm_time = 0;
            public int anm_offset_x;
            public int anm_offset_y;

            public Father(int x,int y,int f,int aox = 0,int aoy = 0)
            {
                this.x = x;
                this.y = y;
                coordinate_x = x / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate);
                coordinate_y = y / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate) + 1;
                face = f;
                anm_offset_x = aox;
                anm_offset_y = aoy;
            }

            public Point getCoordinatePoint()
            {
                return new Point(coordinate_x, coordinate_y);
            }

            //npc animation
            public void draw_anm(Graphics g, int map_sx, int map_sy)
            {
                if (anm == null || current_anm >= anm.Length || anm[current_anm] == null || anm[current_anm].bitmap == null)
                {
                    current_anm = -1;
                    anm_frame = 0;
                    last_anm_time = 0;
                    return;
                }
                anm[current_anm].draw(g, anm_frame, face, map_sx + x + anm_offset_x, y + map_sy + anm_offset_x);
                if (Comm.Time() - last_anm_time >= Animation.RATE)
                {
                    anm_frame += 1;
                    last_anm_time = Comm.Time();
                    if (anm_frame / anm[current_anm].anm_rate >= anm[current_anm].max_frame)
                    {
                        current_anm = -1;
                        anm_frame = 0;
                        last_anm_time = 0;
                    }
                }
            }

            public void play_anm(int index)
            {
                current_anm = index;
                anm_frame = 0;
            }
        }

        public class Player : Father
        {
            //Class of PCC Role
            public long last_walk_time = 0;         //record last moving time point
            public long walk_interval = 100;        //set time interval between two steps
            public int speed = 48;
            public int life = totallife;
            public static int totallife = 150;
            public int bullet_move_interval = 100;
            public int last_shooting_time = 0;
            public int clip_size = 30;
            public static int maximum_clip_size = 30;
            public int current_weapon = 0;
            public int weapon_restriction;

            public Bitmap figure;                   //figure picture
            public static int current_player = 0;   //current player
            public int is_active = 0;               //current player is active or not

            public bool trap_2_slow_down_enable = false;
            public bool trap_2_slow_down_effective = false;
            public int trap_2_time = 10;
            public int trap_2_count = 0;
            public int normal_knife_attack = 30;

            public Player(int x,int y,int f,List<Bitmap> animation):base(x,y,f)
            {
                anm = new Animation[3];
                for (int i = 0; i < animation.Count; i++) { anm[i] = new Animation(animation[i]); }
                figure = new Bitmap(Properties.Resources._fighter2);
                figure.SetResolution(resolution_value, resolution_value);
            }
            public static Point key_ctrl(Player[] player, Map[] map,List<Npc> npc, List<Bullet> bullet, KeyEventArgs e)
            {
                Point nullpoint = new Point(-1, -1);
                Player p = player[current_player];
                Map m = map[Map.current_map];
                //change role （由于暂时不用该功能，所以先注释掉）
                //if (e.KeyCode == Keys.Tab)  {  key_change_player(player);  }

                //change face or not
                if (e.KeyCode == Keys.Up && p.face != 4) { p.face = 4; }
                if (e.KeyCode == Keys.Down && p.face != 1) { p.face = 1; }
                if (e.KeyCode == Keys.Left && p.face != 2) { p.face = 2; }
                if (e.KeyCode == Keys.Right && p.face != 3) { p.face = 3; }

                //time interval determination
                if(e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                    if (Comm.Time() - p.last_walk_time <= p.walk_interval)
                        return nullpoint;
                else if (e.KeyCode == Keys.Space)
                    if (Comm.Time() - p.last_shooting_time <= p.bullet_move_interval)
                        return nullpoint;
                //move
                if (e.KeyCode == Keys.Up /*&& Map.can_through(map,p.coordinate_x,p.coordinate_y - 1)*/) { if (!p.trap_2_slow_down_enable && p.y % Convert.ToInt32(Convert.ToDouble(32) * resolution_rate) != 0) p.y -= p.speed / 2; else p.y -= p.speed; }
                else if (e.KeyCode == Keys.Down /*&& Map.can_through(map, p.coordinate_x, p.coordinate_y + 1)*/) { if (!p.trap_2_slow_down_enable && p.y % Convert.ToInt32(Convert.ToDouble(32) * resolution_rate) != 0) p.y += p.speed / 2; else p.y += p.speed; }
                else if (e.KeyCode == Keys.Left /*&& Map.can_through(map, p.coordinate_x - 1, p.coordinate_y)*/) { if (!p.trap_2_slow_down_enable && p.x % Convert.ToInt32(Convert.ToDouble(32) * resolution_rate) != 0) p.x -= p.speed / 2; else p.x -= p.speed; }
                else if (e.KeyCode == Keys.Right /*&& Map.can_through(map, p.coordinate_x + 1, p.coordinate_y)*/) { if (!p.trap_2_slow_down_enable && p.x % Convert.ToInt32(Convert.ToDouble(32) * resolution_rate) != 0) p.x += p.speed / 2; else p.x += p.speed; }
                else if (e.KeyCode == Keys.Space && Map.can_through(map, p.coordinate_x + Comm.DIRECTION_MAP[p.face].X, p.coordinate_y + Comm.DIRECTION_MAP[p.face].Y) && p.clip_size > 0)
                {
                    switch (p.current_weapon)
                    {
                        case 0: for (int i = 0; i < npc.Count; i++) if (npc[i].coordinate_x == p.coordinate_x && npc[i].coordinate_y == p.coordinate_y) { npc[i].life -= p.normal_knife_attack; }  break;
                        case 1: bullet.Add(new Bullet(p.x, p.y, 30, p.face)); p.clip_size -= 1; break;
                        default: break;
                    }
                    p.play_anm(p.current_weapon);
                }
                else if (e.KeyCode == Keys.W)
                {
                    p.current_weapon = (++p.current_weapon) % p.weapon_restriction;
                }
                else return nullpoint;

                //update coordinates
                p.coordinate_x = p.x / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate);
                p.coordinate_y = p.y / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate) + 1;
                if(m.mapdata_string[p.coordinate_y * m.map_width + p.coordinate_x] == '2' && !p.trap_2_slow_down_enable)
                {
                    p.trap_2_slow_down_enable = true;   //减速buff激活
                    System.Diagnostics.Debug.Write("trap 2!");
                }
                for (int i = 0; i < m.clip_in_map.Count; i++)
                {
                    if(m.clip_in_map[i].coordinate_x == p.coordinate_x && (m.clip_in_map[i].coordinate_y == p.coordinate_y || m.clip_in_map[i].coordinate_y - p.coordinate_y == -1))
                    {
                        p.clip_size += p.clip_size > maximum_clip_size - 10 ? (maximum_clip_size - p.clip_size) : 10;
                        m.clip_in_map[i].visible = false;
                    }
                }
                for (int i = 0; i < m.blue_liquid_in_map.Count; i++)
                {
                    if (m.blue_liquid_in_map[i].coordinate_x == p.coordinate_x && (m.blue_liquid_in_map[i].coordinate_y == p.coordinate_y || m.blue_liquid_in_map[i].coordinate_y - p.coordinate_y == -1))
                    {
                        p.life += p.life > totallife - 20 ? (totallife - p.life) : 20;
                        m.blue_liquid_in_map[i].visible = false;
                    }
                }

                //animation frame
                p.anm_frame += 1;
                if (p.anm_frame >= int.MaxValue) p.anm_frame = 0;
                //collision
                npc_collision(player, map, npc);
                //time
                p.last_walk_time = Comm.Time();

                return new Point(p.coordinate_x, p.coordinate_y);
            }


            public static void Draw(Player[] player, Graphics g, int map_sx,int map_sy)
            {
                Player p = player[current_player];
                if (p.current_anm < 0)
                {
                    Rectangle crazycoderRgl = new Rectangle(p.figure.Width / 4 * (p.anm_frame % 4), p.figure.Height / 4 * (p.face - 1), p.figure.Width / 4, p.figure.Height / 4);
                    Bitmap b0 = p.figure.Clone(crazycoderRgl, p.figure.PixelFormat);
                    //draw figure
                    g.DrawImage(b0, map_sx + p.x, map_sy + p.y);
                }
                else
                {
                    p.draw_anm(g, map_sx, map_sy);
                }
                //draw lifebar
                g.FillRectangle(new SolidBrush(lifebar_color(totallife, p.life)), lifebar_position(totallife, p.life, map_sx + p.x, map_sy + p.y));
                if(p.trap_2_slow_down_enable)
                    g.FillRectangle(new SolidBrush(Color.Blue), lifebar_position(p.trap_2_time, p.trap_2_time-p.trap_2_count, map_sx + p.x, map_sy + p.y, 5, -15));
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
                //this function correct the face of the figure when the key is up
                Player p = player[current_player];
                //animation frame
                p.anm_frame = 0;
                p.last_walk_time = 0;
            }

            public static void set_position(Player[] player,int x,int y,int face)
            {
                player[current_player].x = x;
                player[current_player].y = y;
                player[current_player].face = face;
            }

            public static Point get_position(Player[] player)
            {
                return new Point(player[current_player].x, player[current_player].y);
            }

            public static int get_face(Player[] player)
            {
                return player[current_player].face;
            }

            public static void npc_collision(Player[] player, Map[] map, List<Npc> npc)
            {
                Player p = player[current_player];
                for (int i = 0; i < npc.Count; i++)
                {
                    if (npc[i] == null)
                        continue;
                    if (npc[i].status == Comm.STATIC_NPC && npc[i].is_collision(p.coordinate_x, p.coordinate_y))
                    {
                        Task.story(i);
                        break;
                    }
                }
            }

            public void bullet_move(Map[] map, List<Bullet> bullet)
            {
                for (int i = 0; i < bullet.Count; i++)
                {
                    if (bullet[i] == null)
                        continue;
                    if (bullet[i].disabled)
                        bullet[i] = null;
                    else
                        bullet[i].bullet_time_logic(map);
                }
                //清理已经消失的子弹
                for(int i = bullet.Count - 1; i >= 0; i--)
                {
                    if (bullet[i] == null)
                        bullet.Remove(bullet[i]);
                }
            }

        }

        public class Map
        {
            //这个类负责地图管理与绘制
            public static int map_sx = 0;
            public static int map_sy = 0;
            public static int current_map = 0;          //current map number
            public string bitmap_path;
            public Bitmap bitmap;
            public static Bitmap map_resource = new Bitmap(Properties.Resources.map_test);
            public static Bitmap wall;
            public static Bitmap road;
            public static Bitmap trap_slow;
            public static byte[] wallValue;
            public static byte[] roadValue;
            public static Rectangle wall_r = new Rectangle(64, 0, 32, 32);
            public static Rectangle road_r = new Rectangle(0, 0, 32, 32);
            public static Rectangle trap_slow_r = new Rectangle(128, 0, 32, 32);
            public string mapdata_string;
            public int map_width;
            public int map_height;
            public int this_map_id;
            public static Bitmap blue_liquid = new Bitmap(Properties.Resources.blue_liquid);
            public static Bitmap clip = new Bitmap(Properties.Resources.clip);
            public List<Item> blue_liquid_in_map;
            public List<Item> clip_in_map;

            public Map(Bitmap bmp)
            {
                bitmap = bmp;
            }

            public Map(int id)
            {
                this_map_id = id;
                string path = @System.AppDomain.CurrentDomain.BaseDirectory+@"data.db";
                SQLiteConnection cn = new SQLiteConnection("data source=" + path);
                cn.Open();
                SQLiteCommand cmd = new SQLiteCommand("select * from map_data where map_id = " + Convert.ToString(this_map_id) + ";", cn);
                cmd.CommandType = CommandType.Text;
                SQLiteDataReader dReader = cmd.ExecuteReader();
                DataTable dTable = new DataTable();
                dTable.Load(dReader);
                mapdata_string = dTable.Rows[0]["data"].ToString();
                map_width = Convert.ToInt32(dTable.Rows[0]["width"].ToString());
                map_height = Convert.ToInt32(dTable.Rows[0]["height"].ToString());
                cn.Close();

                //以下三行为测试通过内存修改来初始化地图，但发现效率提升并不大，故暂时废弃
                //wallValue = bitmap2byte(wall, true);
                //roadValue = bitmap2byte(road, true);
                //bitmap = mapBuild(bmp, mapdata_matrix);
                bitmap = LoadImage(id);
                bitmap.SetResolution(resolution_value, resolution_value);
                initialize_map_item();
            }

            public void initialize_map_item()
            {
                blue_liquid_in_map = new List<Item>();
                clip_in_map = new List<Item>();
            }

            public static byte[] bitmap2byte(Bitmap bmp, bool is_unlock)
            {
                //lock BMP in RAM
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData =
                    bmp.LockBits(rect, ImageLockMode.ReadWrite,
                    bmp.PixelFormat);
                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;
                // Declare an array to hold the bytes of the bitmap.
                int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                byte[] rgbValues = new byte[bytes];
                // Copy the RGB values into the array.  
                Marshal.Copy(ptr, rgbValues, 0, bytes);
                // Unlock the bits.解锁
                if (is_unlock)
                    bmp.UnlockBits(bmpData);
                return rgbValues;
            }

            public static Bitmap mapBuild(Bitmap bmp, int [,] mapdata_matrix)
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);  
                IntPtr ptr = bmpData.Scan0;  
                int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                byte[] rgbValues = new byte[bytes];  
                Marshal.Copy(ptr, rgbValues, 0, bytes);

                // copy wall and road to original bmp
                for (int i = 0; i < rgbValues.Length; i += 4)
                {
                    if (mapdata_matrix[((i / 4) / bmp.Width) / 32, ((i / 4) % bmp.Width) / 32] == 0)
                    {
                        for (int j = i; j <= i + 3; j++)
                        {
                            rgbValues[j] = roadValue[(((i / 4) % bmp.Width) % 32) * road.Width + (((i / 4) / bmp.Width) % 32) + j - i];
                        }
                    }
                    else
                    {
                        for (int j = i; j <= i + 3; j++)
                        {
                            rgbValues[j] = wallValue[(((i / 4) / bmp.Width) % 32) * wall.Width + (((i / 4) % bmp.Width) % 32) + j - i];
                        }
                    }
                }
                // Copy the RGB values back to the bitmap 
                Marshal.Copy(rgbValues, 0, ptr, bytes);
                // Unlock the bits.  
                bmp.UnlockBits(bmpData);

                return bmp;
            }


            public static void Draw(Map[] map, Player[] player, List<Npc> npc, List<Bullet> bullet,Graphics g, Rectangle stage)
            {
                Map m = map[current_map];
                Player p = player[Player.current_player];
                //drawing position
                int p_x = Player.get_position(player).X;
                int p_y = Player.get_position(player).Y;
                int map_w = Convert.ToInt32(Convert.ToDouble(m.bitmap.Width) * resolution_rate);
                int map_h = Convert.ToInt32(Convert.ToDouble(m.bitmap.Height) * resolution_rate);

                if(p_x <= stage.Width / 2)
                {
                    map_sx = 0;
                }
                else if (p_x >= map_w  - stage.Width  / 2)
                {
                    map_sx = stage.Width  - map_w ;
                }
                else
                {
                    map_sx = stage.Width / 2 - p_x ;
                }

                if(p_y <= stage.Height / 2)
                {
                    map_sy = 0;
                }
                else if (p_y >= map_h  - stage.Height / 2)
                {
                    map_sy = stage.Height - map_h ;
                }
                else
                {
                    map_sy = stage.Height / 2 - p_y ;
                }
                //draw
                g.DrawImage(m.bitmap, map_sx, map_sy);
                Player.Draw(player, g, map_sx, map_sy);
                for (int i = 0; i < npc.Count; i++)
                {
                    if (npc[i] == null)
                        continue;
                    if (npc[i].map != current_map)
                        continue;
                    npc[i].draw(g, player, map_sx, map_sy);
                }
                
                for(int i = 0; i < bullet.Count; i++)
                {
                    if (bullet[i] == null)
                        continue;
                    bullet[i].draw(g, map_sx, map_sy);
                }

                for (int i = 0; i < m.clip_in_map.Count; i++)
                {
                    if (m.clip_in_map[i] == null) continue;
                    g.DrawImage(clip, map_sx + m.clip_in_map[i].x, map_sy + m.clip_in_map[i].y + Convert.ToInt32(32.0 * resolution_rate));
                }
                for (int i = 0; i < m.blue_liquid_in_map.Count; i++)
                {
                    if (m.blue_liquid_in_map[i] == null) continue;
                    g.DrawImage(blue_liquid, map_sx + m.blue_liquid_in_map[i].x, map_sy + m.blue_liquid_in_map[i].y + Convert.ToInt32(32.0 * resolution_rate));
                }
                g.DrawString("玩家坐标：" + Convert.ToString(p.coordinate_x) + "," + Convert.ToString(p.coordinate_y), new Font("Arial", 12, FontStyle.Bold), Brushes.White, new PointF(10, 10));
                g.DrawString("玩家像素坐标：" + Convert.ToString(p.x) + "," + Convert.ToString(p.y), new Font("Arial", 12, FontStyle.Bold), Brushes.White, new PointF(10, 30));
                g.DrawString("子弹：" + Convert.ToString(p.clip_size) + "/" + Convert.ToString(Player.maximum_clip_size), new Font("Arial", 12, FontStyle.Bold), Brushes.White, new PointF(10, 70));
                g.DrawString("当前武器编号：" + Convert.ToString(p.current_weapon) , new Font("Arial", 12, FontStyle.Bold), Brushes.White, new PointF(10, 90));
            }

            public static void change_map(Map[] map, Player[] player, Npc[] npc, int newindex, int x, int y, int face)
            {
                //unload old map resource
                if (map[current_map].bitmap!=null)
                {
                    map[current_map].bitmap = null;
                }
                //load new map resource
                map[newindex].bitmap = new Bitmap(map[newindex].bitmap_path);
                map[newindex].bitmap.SetResolution(resolution_value, resolution_value);
                //current_map
                current_map = newindex;
                Player.set_position(player, x, y, face);

                //NPC resource
                for (int i = 0; i < npc.Length; i++)
                {
                    if (npc[i] == null)
                        continue;
                    if (npc[i].map == current_map)
                        npc[i].unload();
                    if (npc[i].map == newindex)
                        npc[i].load();
                }
            }
            public static bool can_through(Map[] map,int x,int y)
            {
                Map m = map[current_map];
                if (x < 0) return false;
                else if (x >= m.map_width) return false;
                else if (y < 0) return false;
                else if (y >= m.map_height) return false;

                if (m.mapdata_string[y * m.map_width + x] == '1')
                    return false;
                else
                    return true;
                
            }
            public void WriteMessage(string msg)
            {
                using (FileStream fs = new FileStream(@"e:\test.txt", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.WriteLine("{0}\n", msg, DateTime.Now);
                        sw.Flush();
                    }
                }
            }
            //SQLITE FUNCTION
            public static byte[] ImageToByte(Image image, System.Drawing.Imaging.ImageFormat format)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    image.Save(ms, format);
                    byte[] imageBytes = ms.ToArray();
                    return imageBytes;
                }
            }
            //SQLITE FUNCTION
            public static Image ByteToImage(byte[] imageBytes)
            {
                // Convert byte[] to Image
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = new Bitmap(ms);
                return image;
            }
            //SQLITE FUNCTION
            public static void SaveImage(byte[] imagen,int map_id)
            {
                string conStringDatosUsuarios = @" Data Source = "+ @System.AppDomain.CurrentDomain.BaseDirectory + @"data.db";
                SQLiteConnection con = new SQLiteConnection(conStringDatosUsuarios);
                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = String.Format("UPDATE map_data SET map_image_block_data = (@0) WHERE map_id = " + Convert.ToString(map_id) + ";");
                SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.Binary);
                param.Value = imagen;
                cmd.Parameters.Add(param);
                con.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exc1)
                {
                    MessageBox.Show(exc1.Message);
                }
                con.Close();
            }
            //SQLITE FUNCTION
            Bitmap LoadImage(int map_id)
            {
                Bitmap[] ans = new Bitmap[1];
                string query = "SELECT map_image_data FROM map_data WHERE map_id=" + Convert.ToString(map_id) + ";";
                string conString = @" Data Source = "+ @System.AppDomain.CurrentDomain.BaseDirectory + @"data.db";
                SQLiteConnection con = new SQLiteConnection(conString);
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                con.Open();
                try
                {
                    IDataReader rdr = cmd.ExecuteReader();
                    try
                    {
                        while (rdr.Read())
                        {
                            byte[] a = (System.Byte[])rdr[0];
                            ans[0] = new Bitmap(ByteToImage(a));
                        }
                    }
                    catch (Exception exc) { MessageBox.Show(exc.Message); }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                con.Close();
                return ans[0];
            }

            public static Bitmap string2bitmap(string mapdata_string,int map_width,int map_height)
            {
                //这个函数用于将01string转化为bitmap便于存储
                int [,]mapdata_matrix = new int[map_height, map_width];
                for (int i = 0; i < map_height; i++)
                {
                    for (int j = 0; j < map_width; j++)
                    {
                        mapdata_matrix[i, j] = Convert.ToInt32(mapdata_string[i * map_width + j]) - '0';
                    }
                }
                
                //bmp是要被贴图的对象
                Bitmap bmp = new Bitmap(mapdata_matrix.GetLength(1) * 32, mapdata_matrix.GetLength(0) * 32);
                wall = map_resource.Clone(wall_r, map_resource.PixelFormat);
                road = map_resource.Clone(road_r, map_resource.PixelFormat);
                trap_slow = map_resource.Clone(trap_slow_r, map_resource.PixelFormat);
                for (int i = 0; i < bmp.Height; i++)
                {
                    for (int j = 0; j < bmp.Width; j++)
                    {

                        if (i / 32 >= mapdata_matrix.GetLength(0) || j / 32 >= mapdata_matrix.GetLength(1))
                        {
                            continue;
                        }
                        if (mapdata_matrix[i / 32, j / 32] == 0)
                        {
                            bmp.SetPixel(j, i, road.GetPixel(j % 32, i % 32));
                            //bmp.SetPixel(j, i, Color.White);

                        }
                        else if(mapdata_matrix[i / 32, j / 32] == 2)
                        {
                            bmp.SetPixel(j, i, trap_slow.GetPixel(j % 32, i % 32));
                            //bmp.SetPixel(j, i, Color.Black);
                        }
                        else 
                        {
                            bmp.SetPixel(j, i, wall.GetPixel(j % 32, i % 32));
                        }
                    }
                }
                return bmp;
            }
        }

        public class Npc : Father
        {
            //position
            public int map = 0;        //map id
            public int status;
            public Random r = new Random(Guid.NewGuid().GetHashCode());
            //show
            public string bitmap_path = "";
            public Bitmap bitmap;
            public bool visible = true;

            public int walk_frame = 0;
            public long last_walk_time = 0;
            public long walk_interval = 300;
            public long last_attack_time = 0;
            public long attack_interval = 500;
            public int speed = Convert.ToInt32(Convert.ToDouble(32 * resolution_rate));
            public int idle_walk_direction = Comm.DOWN;
            public int idle_walk_time = 0;
            public int idle_walk_time_now = 0;
            public static int totallife = 150;
            public int life = 150;
            public int attack = 10;
            public bool launch_attack = false;
            public int launch_attack_threshold = 1;

            //load
            public Npc(int x, int y, Bitmap b,int f, int s,Bitmap animation):base(x,y,f)
            {
                bitmap = b;
                status = s;
                anm = new Animation[1];
                anm[0] = new Animation(animation);
                bitmap.SetResolution(resolution_value, resolution_value);
            }

            public void load()
            {
                MessageBox.Show("npcload!");
                if (bitmap_path != "")
                {
                    bitmap = new Bitmap(bitmap_path);
                    
                }
            }

            public void unload()
            {
                if (bitmap != null)
                    bitmap = null;
            }

            public void draw(Graphics g, Player[] player, int map_sx, int map_sy)
            {
                if (!visible)
                    return;
                if (current_anm < 0)
                {
                    if (status == Comm.STATIC_NPC)
                    {
                        Bitmap b = bitmap.Clone(new Rectangle(bitmap.Width / 4 * (walk_frame % 4), bitmap.Height / 4 * (face - 1), bitmap.Width / 4, bitmap.Height / 4), bitmap.PixelFormat);
                        b.SetResolution(resolution_value, resolution_value);
                        g.DrawImage(b, map_sx + x, map_sy + y);
                    }
                    else
                    {
                        draw_character(g, map_sx, map_sy);
                    }
                }
                else
                {
                    draw_anm(g, map_sx, map_sy);
                }
                //draw lifebar
                g.FillRectangle(new SolidBrush(lifebar_color(totallife, life)), lifebar_position(150, life, map_sx + x, map_sy + y));
                g.FillRectangle(new SolidBrush(lifebar_color(Player.totallife, player[0].life)), lifebar_position(150, player[0].life, map_sx + player[0].x, map_sy + player[0].y));
            }

            public void draw_character(Graphics g,int map_sx,int map_sy)
            {
                Rectangle rent = new Rectangle(
                    bitmap.Width / 4 * (walk_frame % 4),
                    bitmap.Height / 4 * (face - 1),
                    bitmap.Width / 4,
                    bitmap.Height / 4);
                Bitmap bitmap0 = bitmap.Clone(rent, bitmap.PixelFormat);
                g.DrawImage(bitmap0, map_sx + x , map_sy + y );
            }

            public void walk(Map[] map, int direction, bool isblock)
            {
                //face
                face = direction;
                //interval determination
                if (Comm.Time() - last_walk_time <= walk_interval)
                    return;
                //walking 
                if(!isblock || Map.can_through(map, coordinate_x + Comm.DIRECTION_MAP[face].X, coordinate_y + Comm.DIRECTION_MAP[face].Y))
                {
                    x += (Comm.DIRECTION_MAP[face].X * speed);
                    y += (Comm.DIRECTION_MAP[face].Y * speed);
                }
                coordinate_x = x / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate);
                coordinate_y = y / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate) + 1;
                //animation frame
                walk_frame += 1;
                if (walk_frame >= int.MaxValue)
                    walk_frame = 0;
                last_walk_time = Comm.Time();
            }
            public void attack_walk(Map[] map, Point direction_vector, bool isblock)
            {
                //interval determination
                if (Comm.Time() - last_walk_time <= walk_interval)
                    return;
                //walking 
                if (!isblock || Map.can_through(map, coordinate_x + direction_vector.X, coordinate_y + direction_vector.Y))
                {
                    x += (direction_vector.X * speed);
                    y += (direction_vector.Y * speed);
                }
                coordinate_x = x / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate);
                coordinate_y = y / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate) + 1;
                //animation frame
                walk_frame += 1;
                if (walk_frame >= int.MaxValue)
                    walk_frame = 0;
                last_walk_time = Comm.Time();
            }

            public void Stop_walk()
            {
                walk_frame = 0;
                last_walk_time = 0;
            }
            public void timer_logic(Map[] map, Player[] player)
            {
                Point abs_vector = new Point(Math.Abs(coordinate_x - player[0].coordinate_x), Math.Abs(coordinate_y - player[0].coordinate_y));
                Point next_attack_moving_vector = new Point((player[0].coordinate_x - coordinate_x) / ((abs_vector.X == 0) ? 1 : abs_vector.X),
                               (player[0].coordinate_y - coordinate_y) / ((abs_vector.Y == 0) ? 1 : abs_vector.Y));
                launch_attack = (abs_vector.X + abs_vector.Y <= launch_attack_threshold) && Map.can_through(map, coordinate_x + next_attack_moving_vector.X, coordinate_y + next_attack_moving_vector.Y);
                if (!launch_attack && idle_walk_time != 0)
                {
                    int direction;
                    //if (0 <= idle_walk_time_now && idle_walk_time_now < idle_walk_time / 2)
                    //    direction = Comm.RIGHT;
                    //else if (idle_walk_time / 2 <= idle_walk_time_now && idle_walk_time_now < idle_walk_time)
                    //    direction = Comm.DOWN;
                    //else if ((-idle_walk_time) / 2 <= idle_walk_time_now && idle_walk_time_now < 0)
                    //    direction = Comm.UP;
                    //else 
                    //    direction = Comm.LEFT;
                    if (0 <= idle_walk_time_now && idle_walk_time_now < idle_walk_time)
                        direction = Comm.DOWN;
                    else if (-idle_walk_time <= idle_walk_time_now && idle_walk_time_now < 0)
                        direction = Comm.UP;
                    else
                        direction = Comm.UP;

                    walk(map, direction, true);
                    if (idle_walk_time_now >= 0)
                    {
                        idle_walk_time_now += 1;
                        if (idle_walk_time_now > idle_walk_time)
                            idle_walk_time_now = -1;
                    }
                    else if (idle_walk_time_now < 0)
                    {
                        idle_walk_time_now -= 1;
                        if (idle_walk_time_now < -idle_walk_time)
                            idle_walk_time_now = 1;
                    }
                    return;
                }
                if (launch_attack)
                {
                    int next_face = Comm.opposite_direction(Comm.vector2face(new Point(coordinate_x - player[0].coordinate_x, coordinate_y - player[0].coordinate_y)));
                    face = next_face == -1 ? face : next_face;
                    attack_walk(map, next_attack_moving_vector, true);
                }

            }

            public bool is_collision(int x, int y)
            {
                //跟剧情人物对线
                if (x == coordinate_x - 1 && y == coordinate_y)
                    face = Comm.LEFT;
                else if (x == coordinate_x + 1 && y == coordinate_y)
                    face = Comm.RIGHT;
                else if (x == coordinate_x && y == coordinate_y - 1)
                    face = Comm.UP;
                else if (x == coordinate_x && y == coordinate_y + 1)
                    face = Comm.DOWN;
                else
                    return false;
                return true;
            }

        }

        public class Bullet : Father
        {
            public int attack;              //单个子弹攻击力值

            public bool disabled = false;   //若子弹撞墙了就把这里设为true，画图遍历时直接销毁这个对象
            public int move_interval = 300;
            public long last_move_time = 0;
            public int speed = Convert.ToInt32(Convert.ToDouble(32 * resolution_rate));
            public Bitmap bitmap = Properties.Resources.bullet;

            public Bullet(int x,int y,int attack,int face):base(x,y,face)
            {
                this.attack = attack;
                if (face == Comm.LEFT)
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
                else if (face == Comm.DOWN)
                    bitmap.RotateFlip(RotateFlipType.Rotate270FlipY);
                else if (face == Comm.UP)
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipXY);

                bitmap.SetResolution(resolution_value, resolution_value);
            }

            public void draw(Graphics g, int map_sx, int map_sy)
            {
                Point p = get_bullet_head_coordinate();
                if(face == Comm.UP||face == Comm.DOWN)
                    g.DrawImage(bitmap, map_sx + x - Convert.ToInt32(16.0 * resolution_rate), map_sy + y);
                else
                    g.DrawImage(bitmap, map_sx + x, map_sy + y - Convert.ToInt32(16.0 * resolution_rate));
                g.DrawString("子弹头坐标：" + Convert.ToString(p.X) + "," + Convert.ToString(p.Y), new Font("Arial", 12, FontStyle.Bold), Brushes.White, new PointF(10, 50));
            }

            public void bullet_move(Map[] map)
            {
                if (Comm.Time() - last_move_time <= move_interval)
                    return;
                if(Map.can_through(map,coordinate_x+Comm.DIRECTION_MAP[face].X,coordinate_y+Comm.DIRECTION_MAP[face].Y))
                {
                    x += (Comm.DIRECTION_MAP[face].X * speed);
                    y += (Comm.DIRECTION_MAP[face].Y * speed);
                }
                else
                {
                    disabled = true;
                }
                coordinate_x = x / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate);
                coordinate_y = y / Convert.ToInt32(Convert.ToDouble(32) * resolution_rate) + 1;
                
                last_move_time = Comm.Time();
            }

            public void bullet_time_logic(Map []map)
            {
                bullet_move(map);
            }

            public Point get_bullet_head_coordinate()
            {
                //返回弹头的坐标，用于判定是否构成伤害
                if (face == Comm.RIGHT)
                    return new Point(coordinate_x + 1, coordinate_y - 1);
                else if (face == Comm.UP)
                    return new Point(coordinate_x, coordinate_y - 1);
                else if (face == Comm.LEFT)
                    return new Point(coordinate_x, coordinate_y - 1);
                else
                    return new Point(coordinate_x, coordinate_y);
            }
        }

        public class Animation
        {
            public static long RATE = 100;
            public Bitmap bitmap;
            public int row;
            public int col;
            public int max_frame;
            public int anm_rate;    //表示放慢anm_rate倍播放动画
            public Animation(Bitmap b)
            {
                bitmap = b;
                if (b != null)
                    bitmap.SetResolution(resolution_value, resolution_value);
                initial_animation();
            }

            public void initial_animation()
            {
                row = 1;
                col = 4;
                anm_rate = 1;
                max_frame = 4;
            }

            public void unload()
            {
                if (bitmap != null)
                    bitmap = null;
            }

            public Bitmap get_bitmap(int frame, int face)
            {
                if (bitmap == null)
                    return null;
                if (frame >= max_frame)
                    return null;
                Rectangle rect = new Rectangle(
                    bitmap.Width / 4 * (frame % 4),
                    bitmap.Height / 4 * (face - 1),
                    bitmap.Width / 4,
                    bitmap.Height / 4);
                return bitmap.Clone(rect, bitmap.PixelFormat);
            }

            public void draw(Graphics g, int frame, int face, int x, int y)
            {
                Bitmap bitmap = get_bitmap(frame / anm_rate, face);
                if (bitmap == null) return;
                g.DrawImage(bitmap, x, y);
            }

        }

        public class Item : Father
        {
            public bool visible;
            
            public Item(Point pixel_p, bool v):base(pixel_p.X,pixel_p.Y,Comm.NO_DIRECTION)
            {
                visible = v;
            }
        }

        private void traptimer_2_slow_down_Tick_1(object sender, EventArgs e)
        {
            if (!player[0].trap_2_slow_down_enable) return;
            player[0].trap_2_count += 1;
            if (player[0].trap_2_count == player[0].trap_2_time)
            {
                traptimer_2_slow_down.Stop();
                player[0].speed *= 2;
                player[0].trap_2_count = 0;
                player[0].trap_2_slow_down_enable = false;
                player[0].trap_2_slow_down_effective = false;
            }
        }

        private void timer_bullet_Tick(object sender, EventArgs e)
        {
            Player p = player[Player.current_player];
            Map m = map[Map.current_map];
            p.bullet_move(map,bullet);
            #region 检测子弹与npc的碰撞
            for (int i = 0; i < bullet.Count; i++)
            {
                for (int j = 0; j < npc.Count; j++)
                {
                    if(bullet[i].get_bullet_head_coordinate() == npc[j].getCoordinatePoint() && npc[j].status == Comm.ACTIVE_NPC)
                    {
                        npc[j].life -= bullet[i].attack;
                        bullet[i].disabled = true;
                        npc_died_logic(npc[j], m, p);
                    }
                }
            }
            #endregion
            for (int i = npc.Count - 1; i >= 0; i--)
            {
                if (!npc[i].visible) npc.Remove(npc[i]);
            }
            for (int i = m.clip_in_map.Count - 1; i >= 0; i--)
            {
                if (!m.clip_in_map[i].visible) m.clip_in_map.Remove(m.clip_in_map[i]);
            }
            for (int i = m.blue_liquid_in_map.Count - 1; i >= 0; i--)
            {
                if (!m.blue_liquid_in_map[i].visible) m.blue_liquid_in_map.Remove(m.blue_liquid_in_map[i]);
            }
            Draw();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            for (int i = 0; i < npc.Count; i++)
            {
                if (npc[i] == null)
                    continue;
                if (npc[i].map != Map.current_map)
                    continue;
                npc[i].timer_logic(map, player);
            }

            #region npc对pcc的攻击
            Player p = player[Player.current_player];
            Map m = map[Map.current_map];
            for (int i = 0; i < npc.Count; i++)
            {
                if (p.getCoordinatePoint() == npc[i].getCoordinatePoint())
                {
                    if (Comm.Time() - npc[i].last_attack_time < npc[i].attack_interval) continue;
                    npc[i].play_anm(0);
                    p.life -= npc[i].attack;
                    npc[i].last_attack_time = Comm.Time();
                    if (p.life <= 0)
                    {
                        timer1.Stop();
                        timer_bullet.Stop();
                        MessageBox.Show("Game Over!");
                        initialize_game();
                        timer1.Start();
                        timer_bullet.Start();
                    }
                }
            }
            #endregion

            #region pcc对npc近身攻击
            for (int i = 0; i < npc.Count; i++)
            {
                npc_died_logic(npc[i],m,p);
            }
            #endregion

        }

        private void npc_died_logic(Npc npc,Map m, Player p)
        {
            Random rd = new Random();
            if (npc.life <= 0)
            {
                npc.visible = false;
                if (p.weapon_restriction < 2)
                {
                    m.blue_liquid_in_map.Add(new Item(new Point(npc.x, npc.y), true));
                }
                else
                {
                    if (rd.Next() % 2 != 0)
                        m.clip_in_map.Add(new Item(new Point(npc.x, npc.y), true));
                    else
                        m.blue_liquid_in_map.Add(new Item(new Point(npc.x, npc.y), true));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string path = @System.AppDomain.CurrentDomain.BaseDirectory + @"data.db";
            SQLiteConnection cn = new SQLiteConnection("data source=" + path);
            cn.Open();
            SQLiteCommand cmd = new SQLiteCommand("select data from map_data where map_id = " + Convert.ToString(Map.current_map) + ";", cn);
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dReader = cmd.ExecuteReader();
            DataTable dTable = new DataTable();
            dTable.Load(dReader);
            string target = dTable.Rows[0]["data"].ToString();

            int[] t = new int[target.Length];
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = target[i] - '0';
            }
            t[Convert.ToInt32(y_textbox.Text) * map[Map.current_map].map_width + Convert.ToInt32(x_textbox.Text)] = textBox1.Text[0] - '0';
            string ans = "";
            for (int i = 0; i < t.Length; i++)
            {
                ans += t[i].ToString();
            }

            SQLiteCommand cmd1 = cn.CreateCommand();
            cmd1.CommandText = String.Format("UPDATE map_data SET data = '"+ ans +"',map_image_data = (@0) WHERE map_id = " + Convert.ToString(Map.current_map) + ";");
            SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.Binary);

            param.Value = Map.ImageToByte(Map.string2bitmap(ans, map[Map.current_map].map_width, map[Map.current_map].map_height), System.Drawing.Imaging.ImageFormat.Png);
            cmd1.Parameters.Add(param);
            cmd1.ExecuteNonQuery();

            cn.Close();
            MessageBox.Show("Done!");
        }

    }
}
