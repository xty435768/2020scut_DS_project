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
    public partial class BGM_Setting : Form
    {
        public AxWMPLib.AxWindowsMediaPlayer player;
        public BGM_Setting()
        {
            InitializeComponent();
        }

        private void BGM_Setting_Load(object sender, EventArgs e)
        {
            if(player.settings.mute)
            {
                trackBar1.Enabled = false;
                checkBox1.Checked = true;
                label1.Text = "0";
                return;
            }
            trackBar1.Value = player.settings.volume;
            label1.Text = player.settings.volume.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                trackBar1.Enabled = false;
                player.settings.mute = true;
                label1.Text = "0";
            }
            else
            {
                trackBar1.Enabled = true;
                player.settings.mute = false;
                trackBar1.Value = player.settings.volume;
                label1.Text = player.settings.volume.ToString();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            player.settings.volume = trackBar1.Value;
            label1.Text = trackBar1.Value.ToString();
        }
    }
}
