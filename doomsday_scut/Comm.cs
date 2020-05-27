using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace doomsday_scut
{
    class Comm
    {
        //这个类放一些通用方法
        public static long Time()
        {
            DateTime dt1 = new DateTime(1970, 1, 1);
            TimeSpan ts = DateTime.Now - dt1;
            return (long)ts.TotalMilliseconds;
        }
        
        public static TimeSpan Time_span()
        {
            DateTime dt1 = new DateTime(1970, 1, 1);
            return DateTime.Now - dt1;
        }

        public static int NO_DIRECTION = 0;
        public static int UP = 4;
        public static int DOWN = 1;
        public static int LEFT = 2;
        public static int RIGHT = 3;
        public static int RIGHT_UP = 5;
        public static int RIGHT_DOWN = 6;
        public static int LEFT_UP = 7;
        public static int LEFT_DOWN = 8;

        public static Point UP_VECTOR = new Point(0, -1);
        public static Point DOWN_VECTOR = new Point(0, 1);
        public static Point LEFT_VECTOR = new Point(-1, 0);
        public static Point RIGHT_VECTOR = new Point(1, 0);
        public static Point RIGHT_UP_VECTOR = new Point(1, -1);
        public static Point RIGHT_DOWN_VECTOR = new Point(1, 1);
        public static Point LEFT_UP_VECTOR = new Point(-1, -1);
        public static Point LEFT_DOWN_VECTOR = new Point(-1, 1);
        public static Point[] DIRECTION_MAP = new Point[9] 
        {new Point(0,0), DOWN_VECTOR, LEFT_VECTOR, RIGHT_VECTOR, UP_VECTOR,
        RIGHT_UP_VECTOR, RIGHT_DOWN_VECTOR, LEFT_UP_VECTOR, LEFT_DOWN_VECTOR};

        //npc status
        public static int ACTIVE_NPC = 1;
        public static int STATIC_NPC = 0;

        public static int opposite_direction(int direction)
        {
            if (direction == UP) return DOWN;
            else if (direction == DOWN) return UP;
            else if (direction == RIGHT) return LEFT;
            else if (direction == LEFT) return RIGHT;
            return DOWN;
        }

        public static int vector2face(Point p)
        {
            if (p == new Point(0, 0)) return -1;
            if (p == UP_VECTOR) return UP;
            else if (p == DOWN_VECTOR) return DOWN;
            else if (p == LEFT_VECTOR) return LEFT;
            else if (p == RIGHT_VECTOR) return RIGHT;
            else
            {
                Point abs_p = new Point(Math.Abs(p.X - (Math.Abs(p.X) > Math.Abs(p.Y) ? p.Y : p.X)), Math.Abs(p.Y - (Math.Abs(p.X) > Math.Abs(p.Y) ? p.Y : p.X)));
                return vector2face(new Point((p.X - (Math.Abs(p.X) > Math.Abs(p.Y) ? p.Y : p.X)) / (abs_p.X == 0 ? 1 : abs_p.X), (p.Y - (Math.Abs(p.X) > Math.Abs(p.Y) ? p.Y : p.X)) / (abs_p.Y == 0 ? 1 : abs_p.Y)));
            }
        }


        public static DataTable sql_query(string command)
        {
            string path = @System.AppDomain.CurrentDomain.BaseDirectory + @"data.db";
            SQLiteConnection cn = new SQLiteConnection("data source=" + path );
            cn.Open();
            SQLiteCommand cmd = new SQLiteCommand(command, cn) { CommandType = CommandType.Text };
            SQLiteDataReader dReader = cmd.ExecuteReader();
            DataTable dTable = new DataTable();
            dTable.Load(dReader);
            cn.Close();
            return dTable;
        }

        public static void sql_update(string command)
        {
            string path = @System.AppDomain.CurrentDomain.BaseDirectory + @"data.db";
            SQLiteConnection cn = new SQLiteConnection("data source=" + path );
            cn.Open();
            SQLiteCommand cmd = new SQLiteCommand(command, cn) { CommandType = CommandType.Text };
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update error: " + ex.Message + "\n" + command, "SQL Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }
        }

        public static int get_distance_square(Point p1,Point p2)
        {
            return Convert.ToInt32(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
