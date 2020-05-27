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
    public partial class level_jumper : Form
    {
        string code = "123456";
        public int result { get; set; } 
        public level_jumper()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text == code)
            {
                textBox1.Enabled = false;
                ok.Enabled = true;
                comboBox1.Enabled = true;
            }
        }

        private void cancle_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == "")
            {
                return;
            }
            result = Convert.ToInt32(comboBox1.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
