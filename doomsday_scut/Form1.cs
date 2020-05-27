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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void new_game_Click(object sender, EventArgs e)
        {
            Hide();
            level_choose l = new level_choose();
            if (l.ShowDialog() == DialogResult.OK)
            {
                Show();
            }
            
        }

        private void help_about_Click(object sender, EventArgs e)
        {
            using (About h = new About())
            {
                h.ShowDialog();
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
