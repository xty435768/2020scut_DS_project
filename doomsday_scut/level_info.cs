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
    public partial class level_info : Form
    {
        public int disable_time = 0;
        public string message = "";
        public string next_level_message = "";
        public TimeSpan time;
        public level_info(int d, TimeSpan t, string m, string next_level_info = "")
        {
            InitializeComponent();
            message = m;
            disable_time = d;
            time = t;
            next_level_message = next_level_info;
        }

        private void level_info_Load(object sender, EventArgs e)
        {
            Text = message;
            label1.Text = message;
            label3.Text = next_level_message;
            button1.Text = "OK (" + disable_time + ")";
            label2.Text = "Time:  " + time.Hours + ":" + time.Minutes + ":" + time.Seconds + "." + time.Milliseconds;
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(disable_time > 0)
            {
                disable_time--;
                if(disable_time == 0)
                {
                    button1.Enabled = true;
                    button1.Text = "OK";
                    timer1.Stop();
                }
                button1.Text = "OK (" + disable_time + ")";
            }
        }
    }
}
