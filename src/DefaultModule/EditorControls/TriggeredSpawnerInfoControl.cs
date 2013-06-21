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
    public partial class TriggeredSpawnerInfoControl : IGORR.Modules.ObjectControl
    {
        public TriggeredSpawnerInfoControl(BinaryReader reader)
            : base(reader)
        {
            InitializeComponent();
            if (reader != null)
            {
                string EnemyID = reader.ReadInt32().ToString();
                string NumEnemies = reader.ReadInt32().ToString();
                string respawnTime = reader.ReadInt32().ToString();
                txtEnemyType.Text = EnemyID;
                txtNumEnemies.Text = NumEnemies;
                txtRespawnTime.Text = respawnTime;
                chkActivationGlobal.CheckState = reader.ReadBoolean() ? CheckState.Checked : CheckState.Unchecked;
                txtActivationTrigger.Text = reader.ReadString();

                chkClearedGlobal.CheckState = reader.ReadBoolean() ? CheckState.Checked : CheckState.Unchecked;
                txtClearedTrigger.Text = reader.ReadString();
            }

        }

        public override byte[] GetObjectBytes()
        {
            MemoryStream stream = new MemoryStream(12 + txtClearedTrigger.Text.Length + txtActivationTrigger.Text.Length + 2 + 2);
            BinaryWriter writer = new BinaryWriter(stream);
            int enemyid=0;
            int numenemies=0;
            int respawnTime=5000;
            int.TryParse(txtEnemyType.Text, out enemyid);
            int.TryParse(txtNumEnemies.Text, out numenemies);
            int.TryParse(txtRespawnTime.Text, out respawnTime);
            writer.Write(enemyid);
            writer.Write(numenemies);
            writer.Write(respawnTime);
            writer.Write(chkActivationGlobal.CheckState == CheckState.Checked);
            writer.Write(txtActivationTrigger.Text);
            writer.Write(chkClearedGlobal.CheckState == CheckState.Checked);
            writer.Write(txtClearedTrigger.Text);
            return stream.GetBuffer();
        }
    
    }
}
