using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhikeStreet.Common.Utility;

namespace ZhikeStreet.winTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mkStr = textBox1.Text;
            string html = Transform(mkStr);
        }

        #region Private

        private string Transform(string mkStr)
        {
            Markdown mk = new Markdown();
            string html = mk.Transform(mkStr);
            return html;
        }

        #endregion
    }
}
