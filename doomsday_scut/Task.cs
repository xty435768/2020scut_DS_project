using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doomsday_scut
{
    class Task
    {
        public static void story(int i)
        {
            DialogResult r1;
            if (i == 1)
                r1 = MessageBox.Show("全速前进～YOSORO～！");
            else
                r1 = MessageBox.Show("誰か助けて～っ！");
        }
    }
}
