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

        public static int UP = 4;
        public static int DOWN = 1;
        public static int LEFT = 2;
        public static int RIGHT = 3;

        public static Point UP_VECTOR = new Point(0, -1);
        public static Point DOWN_VECTOR = new Point(0, 1);
        public static Point LEFT_VECTOR = new Point(-1, 0);
        public static Point RIGHT_VECTOR = new Point(1, 0);
        public static Point[] DIRECTION_MAP = new Point[5] {new Point(0,0), DOWN_VECTOR, LEFT_VECTOR, RIGHT_VECTOR, UP_VECTOR };

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
    }
}
