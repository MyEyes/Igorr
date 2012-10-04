using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;

namespace MapEditor
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

		private ToolStripButton[] layerButtons;

        private void frmMain_Load(object sender, EventArgs e)
        {
            mapDisplay1.SetUpContent();
            mapDisplay1.SetTileSelecter(tileSelecter1);
            tileSelecter1.SetScrollbar(vScrollBar1);

            mapDisplay1.SetMap(new Map(100, 100, "tileset"));
            mapDisplay1.Focus();

			layerButtons = new [] { btnBackground, btnObstacle, btnObject, btnForeground };
			btnObstacle.Checked = true;
        }

		private void layerChanged (object sender, EventArgs e)
		{
			if (!((ToolStripButton)sender).Checked)
				return;

			foreach (ToolStripButton button in layerButtons)
			{
				if (button == sender)
				{
					mapDisplay1.SetLayer ((int)button.Tag);
					continue;
				}

				button.Checked = false;
			}
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			frmNewMapDialog nmd = new frmNewMapDialog();
            try
            {
                if (nmd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    mapDisplay1.SetMap(new Map(nmd.sizeX, nmd.sizeY, nmd.tileMap));
            }
            catch (ContentLoadException cle)
            {
                MessageBox.Show("Could not create new Map");
            }
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			mapDisplay1.Save();
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Mapfile (*.map) |*.map";
            ofd.ShowDialog();

            if (!string.IsNullOrWhiteSpace(ofd.SafeFileName))
            {
                mapDisplay1.SetMap(new Map(ofd.SafeFileName));

                int layer = -1;
                if (btnBackground.Checked)
                    layer = 0;
                else if (btnObstacle.Checked)
                    layer = 1;
                else if (btnForeground.Checked)
                    layer = 2;
                else if (btnObject.Checked)
                    layer = 3;

                mapDisplay1.SetLayer(layer);
            }
		}

		private void tileSelecter1_MouseEnter(object sender, EventArgs e)
		{
			tileSelecter1.Focus();
		}

		private void mapDisplay1_MouseEnter(object sender, EventArgs e)
		{
			mapDisplay1.Focus();
		}

		private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
		{
			tileSelecter1.Focus();
		}

        private void btnContent_Click(object sender, EventArgs e)
        {
        }
    }
}
