using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DefaultModule.EditorControls
{
    public partial class LightColorControl : IGORR.Modules.ObjectControl
    {
        Color color;
        public LightColorControl(BinaryReader reader):base(reader)
        {
            InitializeComponent();
            color = Color.White;
            if (reader != null)
            {
                color = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                txtRadius.Text = reader.ReadInt32().ToString();
            }
            pictureBox1.BackColor = color;
        }

        public override byte[] GetObjectBytes()
        {
            MemoryStream stream = new MemoryStream(8);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(color.A);
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
            int radius = int.Parse(txtRadius.Text);
            writer.Write(radius);
            return stream.GetBuffer();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            color = colorDialog1.Color;
            pictureBox1.BackColor = color;
        }
    }
}
