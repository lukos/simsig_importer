using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimsigImporter
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void AddMessage(string message, Color color)
        {
            if (!progressListView.InvokeRequired)
                progressListView.Items.Add(new ListViewItem(new[] { message }, null, color, Color.White, null));
            else
            {
                Action write = delegate { AddMessage(message, color); };
                progressListView.Invoke(write);
            }

            
        }

        public void EnableButton()
        {
            if(!btnClose.InvokeRequired)
                btnClose.Enabled = true;
            else
            {
                Action write = delegate { EnableButton(); };
                btnClose.Invoke(write);
            }
        }
    }
}
