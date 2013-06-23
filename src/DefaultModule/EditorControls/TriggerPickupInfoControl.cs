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
    public partial class TriggerPickupInfoControl : IGORR.Modules.ObjectControl
    {
        public TriggerPickupInfoControl(BinaryReader reader):base(reader)
        {
            InitializeComponent();
            if (reader != null)
            {
                string triggerName = reader.ReadString();
                bool value = reader.ReadBoolean();
                txtTriggerName.Text = triggerName;
                checkBox1.Checked = value;
                txtPlayerInfo.Text = reader.ReadString();
                txtPlayerInfoValue.Text = reader.ReadInt32().ToString();
                txtTextureName.Text = reader.ReadString();
            }
        }

        public override byte[] GetObjectBytes()
        {
            MemoryStream stream = new MemoryStream(txtTriggerName.Text.Length + 2 + txtPlayerInfo.Text.Length + 1 + 4 + txtTextureName.Text.Length + 1);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(txtTriggerName.Text);
            writer.Write(checkBox1.Checked);
            writer.Write(txtPlayerInfo.Text);

            int value = 0;
            int.TryParse(txtPlayerInfoValue.Text, out value);
            writer.Write(value);

            writer.Write(txtTextureName.Text);

            return stream.GetBuffer();
        }

        private void txtPlayerInfo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
