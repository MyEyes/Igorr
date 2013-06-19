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
    public partial class SpawnerInfoControl : IGORR.Modules.ObjectControl
    {
        public SpawnerInfoControl(BinaryReader reader)
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
            }

        }

        public override byte[] GetObjectBytes()
        {
            MemoryStream stream = new MemoryStream(12);
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
            return stream.GetBuffer();
        }
    
    }
}
