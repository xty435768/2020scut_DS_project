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
        public string next_level_button_text = "Next Level";
        public int current_level;
        public TimeSpan time;
        public level_info(int d, TimeSpan t, string m, string next_level_info = "")
        {
            InitializeComponent();
            message = m;
            disable_time = d;
            time = t;
            next_level_message = next_level_info;
            if (message == "Game Over!")
            {
                next_level_button_text = "OK";
            }
            else
            {
                next_level_button_text =  "Next Level";
            }
        }

        private void level_info_Load(object sender, EventArgs e)
        {
            Text = message;
            label1.Text = message;
            label3.Text = next_level_message;
            button1.Text = next_level_button_text + " (" + disable_time + ")";
            button2.Text = "Level Select (" + disable_time + ")";
            label2.Text = "Time:  " + time.Hours + ":" + time.Minutes + ":" + time.Seconds + "." + time.Milliseconds;
            long t = Convert.ToInt64(Comm.sql_query("select * from profile where level_id="+current_level.ToString()).Rows[0]["time"]);
            label2.Text += "  Highest record: " + t / 3600000 + ":" + t / 60000 + ":" + t / 1000 + "." + t % 1000;
            if (next_level_message == " ")
            {
                button1.Hide();
            }
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
                    button1.Text = next_level_button_text;
                    button2.Enabled = true;
                    button2.Text = "Level Select";
                    timer1.Stop();
                    return;
                }
                button1.Text = next_level_button_text + " (" + disable_time + ")";
                button2.Text = "Level Select (" + disable_time + ")";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
