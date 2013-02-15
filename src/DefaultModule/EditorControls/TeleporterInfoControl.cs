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
    public partial class TeleporterInfoControl : IGORR.Modules.ObjectControl
    {
        public TeleporterInfoControl(BinaryReader reader)
        {
            InitializeComponent();
            if (reader != null)
            {
                txtMapID.Text = reader.ReadInt32().ToString();
                txtPosX.Text = reader.ReadInt32().ToString();
                txtPosY.Text = reader.ReadInt32().ToString();
            }
        }

        public override byte[] GetObjectBytes()
        {
            MemoryStream stream = new MemoryStream(12);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(int.Parse(txtMapID.Text));
            writer.Write(int.Parse(txtPosX.Text));
            writer.Write(int.Parse(txtPosY.Text));
            return stream.GetBuffer();
        }
    }
}
