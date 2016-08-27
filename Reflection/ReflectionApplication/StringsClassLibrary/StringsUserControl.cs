using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StringsClassLibrary
{
    public partial class StringsUserControl : UserControl
    {
        public StringsUserControl()
        {
            InitializeComponent();
        }

        private void concatButton_Click(object sender, EventArgs e)
        {
            textBox3.Text = textBox1.Text + " " + textBox2.Text;
        }
    }
}
