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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mapDisplay1.SetTileSelecter(tileSelecter1);
            tileSelecter1.SetScrollbar(hScrollBar1);
            mapDisplay1.SetMap(new Map(mapDisplay1.Content, 100, 100, "tileset"));
            mapDisplay1.Click += mapDisplay1_Click;
            tileSelecter1.Click += tileSelecter1_Click;
            mapDisplay1.Focus();
            mapDisplay1.SetLayer(0);
        }

        private void mapDisplay1_Click(object sender, EventArgs e)
        {
            mapDisplay1.Focus();
        }

        private void tileSelecter1_Click(object sender, EventArgs e)
        {
            tileSelecter1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                mapDisplay1.SetLayer(0);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                mapDisplay1.SetLayer(1);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                mapDisplay1.SetLayer(2);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            mapDisplay1.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Mapfile (*.map) |*.map";
            ofd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(ofd.SafeFileName))
            {
                mapDisplay1.SetMap(new Map(mapDisplay1.Content, ofd.SafeFileName));
                int layer = -1;
                if (radioButton1.Checked)
                    layer = 0;
                else if (radioButton2.Checked)
                    layer = 1;
                else if (radioButton3.Checked)
                    layer = 2;
                else if (radioButton4.Checked)
                    layer = 3;
                mapDisplay1.SetLayer(layer);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                mapDisplay1.SetLayer(3);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewMapDialog nmd = new NewMapDialog();
            try
            {
                if (nmd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    mapDisplay1.SetMap(new Map(mapDisplay1.Content, nmd.sizeX, nmd.sizeY, nmd.tileMap));
            }
            catch (ContentLoadException cle)
            {
                MessageBox.Show("Could not create new Map");
            }
        }

        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        
    }
}
