using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
