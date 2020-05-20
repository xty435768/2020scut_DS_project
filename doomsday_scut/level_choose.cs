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
    public partial class level_choose : Form
    {
        public level_choose()
        {
            InitializeComponent();
        }
        
        private void level_button_Click(object sender, EventArgs e)
        {
            Hide();
            string l = ((Button)sender).Name;
            Game.Map.current_map = Convert.ToInt16(l.Substring(l.Length - 1));
            Game g = new Game();
            if (g.ShowDialog() == DialogResult.OK)
            {
                g = null;
                set_button();
                Show();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void level_choose_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void level_choose_Load(object sender, EventArgs e)
        {
            set_button();
        }

        private void set_button()
        {
            DataTable dt = Comm.sql_query("select * from profile;");
            foreach (Button item in gb1.Controls)
            {
                item.Enabled = !(dt.Rows[Convert.ToInt16(item.Name.Substring(item.Name.Length - 1))]["time"] is DBNull);
            }
        }
    }
}
