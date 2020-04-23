using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
    }
}
