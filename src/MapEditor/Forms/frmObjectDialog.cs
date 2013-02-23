using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using IGORR.Modules;

namespace MapEditor
{

    public partial class frmObjectDialog : Form
    {
        
        /*
        static List<string> _names;
        static List<int> _ids;
         */
        static List<ObjectTemplate> _templates;
        SpawnPoint _spawnPoint;
        IGORR.Modules.ObjectControl ctrl = null;

        public frmObjectDialog(int x, int y, SpawnPoint sp)
        {
            InitializeComponent();
            _spawnPoint = sp;
            if (_spawnPoint == null)
            {
                _spawnPoint = new SpawnPoint();
                _spawnPoint.objectID = -1;
                _spawnPoint.X = x;
                _spawnPoint.Y = y;
            }   
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void ObjectDialog_Load(object sender, EventArgs e)
        {
            if (_templates == null)
            {
                _templates = new List<ObjectTemplate>();
                _templates.AddRange(ModuleManager.GetTemplates());
            }
            /*
            if (_names == null)
            {
                _names = new List<string>();
                _ids = new List<int>();

                StreamReader reader = new StreamReader("items.lst");
                try
                {
                    while (!reader.EndOfStream)
                    {
                        _names.Add(reader.ReadLine());
                        _ids.Add(int.Parse(reader.ReadLine()));
                    }
                }
                catch (Exception exc)
                {

                }
                reader.Close();
                /*
                _names.Add("None");
                _ids.Add(-1);

                _names.Add("SpawnPoint");
                _ids.Add('s' - 'a');

                _names.Add("Lava");
                _ids.Add('t' - 'a');

                _names.Add("Legs");
                _ids.Add('b' - 'a');

                _names.Add("Striker");
                _ids.Add('d' - 'a');

                _names.Add("Wings");
                _ids.Add('e' - 'a');

                _names.Add("Teleporter");
                _ids.Add('g' - 'a');

                _names.Add("Slimer");
                _ids.Add(5000);
                 

                _names.Add("Mini Boss Blob");
                _ids.Add(5001);

                _names.Add("King Blob");
                _ids.Add(5002);

                _names.Add("Bossminion");
                _ids.Add(5004);
            
            }
             */ 

            lblPos.Text = "(" + _spawnPoint.X.ToString() + "," + SpawnPoint.Y.ToString() + ")";
            comboBox1.Items.Clear();
            int index=_spawnPoint.objectID;
            for (int x = 0; x < _templates.Count; x++)
            {
                comboBox1.Items.Add(_templates[x]);
                if(_templates[x].TypeID == index)
                    comboBox1.SelectedItem = comboBox1.Items[x];
            }
        }

        public SpawnPoint SpawnPoint
        {
            get { return _spawnPoint; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;

            _spawnPoint.objectID = _templates[index].TypeID;
            if (ctrl != null)
                _spawnPoint.bytes = ctrl.GetObjectBytes();
            if (_spawnPoint.objectID == -1)
                _spawnPoint = null;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjectTemplate tmplt = (ObjectTemplate)comboBox1.SelectedItem;
            if (ctrl != null)
                splitContainer1.Panel2.Controls.Remove(ctrl);
            if (_spawnPoint.objectID == tmplt.TypeID && _spawnPoint.bytes!=null)
                ctrl = tmplt.GetEditorControl(new BinaryReader(new MemoryStream(_spawnPoint.bytes)));
            else
                ctrl = tmplt.GetEditorControl(null);
            if (ctrl != null)
                splitContainer1.Panel2.Controls.Add(ctrl);

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _spawnPoint = null;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


    }
}
