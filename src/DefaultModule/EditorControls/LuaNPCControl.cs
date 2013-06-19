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
    public partial class LuaNPCControl : IGORR.Modules.ObjectControl
    {
        public LuaNPCControl(BinaryReader reader):base(reader)
        {
            InitializeComponent();
            if (reader != null)
            {
                string ScriptFile = reader.ReadString();
                string CharFile = reader.ReadString();
                txtScriptFile.Text = ScriptFile;
                txtCharFile.Text = CharFile;
            }
        }

        public override byte[] GetObjectBytes()
        {
            MemoryStream stream = new MemoryStream(txtScriptFile.Text.Length + 2+txtCharFile.Text.Length);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(txtScriptFile.Text);
            writer.Write(txtCharFile.Text);
            return stream.GetBuffer();
        }
    }
}
