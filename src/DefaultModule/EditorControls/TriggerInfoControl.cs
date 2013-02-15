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
    public partial class TriggerInfoControl : IGORR.Modules.ObjectControl
    {
        public TriggerInfoControl(BinaryReader reader):base(reader)
        {
            InitializeComponent();
            if (reader != null)
            {
                string triggerName = reader.ReadString();
                bool value = reader.ReadBoolean();
                txtTriggerName.Text = triggerName;
                checkBox1.Checked = value;
            }
        }

        public override byte[] GetObjectBytes()
        {
            MemoryStream stream = new MemoryStream(txtTriggerName.Text.Length + 2);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(txtTriggerName.Text);
            writer.Write(checkBox1.Checked);
            return stream.GetBuffer();
        }
    }
}
