using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEditor
{
    public partial class NewMapDialog : Form
    {
        public int sizeX;
        public int sizeY;
        public string tileMap;

        public NewMapDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                sizeX = int.Parse(txtSizeX.Text);
                sizeY = int.Parse(txtSizeY.Text);
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Please enter valid data into the fields");
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XNA Content File (*.xnb)|*.xnb";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tileMap = ofd.SafeFileName.Substring(0, ofd.SafeFileName.Length - 4);
                txtTileSet.Text = tileMap;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
