﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MapEditor
{

    public partial class ObjectDialog : Form
    {
        static List<string> _names;
        static List<int> _ids;
        SpawnPoint _spawnPoint;

        public ObjectDialog(int x, int y, SpawnPoint sp)
        {
            InitializeComponent();

            _spawnPoint = sp;
            if (_spawnPoint == null)
            {
                _spawnPoint = new SpawnPoint();
                _spawnPoint.objectID = -1;
            }
            else
            {
                if (_spawnPoint.tepo != null)
                {
                    txtTargetMap.Text = _spawnPoint.tepo.mapID.ToString();
                    txtTargetPosX.Text = _spawnPoint.tepo.X.ToString();
                    txtTargetPosY.Text = _spawnPoint.tepo.Y.ToString();
                }
                if (_spawnPoint.trpa != null)
                {
                    txtTrigName.Text = _spawnPoint.trpa.name;
                    chkGlobal.Checked = _spawnPoint.trpa.global;
                }
            }
            _spawnPoint.X = x;
            _spawnPoint.Y = y;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ObjectDialog_Load(object sender, EventArgs e)
        {
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
                _ids.Add(5004);*/
            }

            lblPos.Text = "(" + _spawnPoint.X.ToString() + "," + SpawnPoint.Y.ToString() + ")";
            comboBox1.Items.Clear();
            for (int x = 0; x < _names.Count; x++)
                comboBox1.Items.Add(_names[x]);
            int index=_ids.IndexOf(_spawnPoint.objectID);
            if (index >= 0)
                comboBox1.SelectedItem = comboBox1.Items[index];
        }

        public SpawnPoint SpawnPoint
        {
            get { return _spawnPoint; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index >= 0)
            {
                if (comboBox1.SelectedIndex == _names.IndexOf("Teleporter"))
                {
                    TeleportPoint tp = new TeleportPoint();
                    try
                    {
                        tp.mapID = int.Parse(txtTargetMap.Text);
                        tp.X = int.Parse(txtTargetPosX.Text);
                        tp.Y = int.Parse(txtTargetPosY.Text);
                        _spawnPoint.tepo = tp;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Please enter valid data for the teleport target!");
                        return;
                    }

                }
                else if (comboBox1.SelectedIndex == _names.IndexOf("TouchTrigger") || comboBox1.SelectedIndex == _names.IndexOf("TriggeredBlocker") || comboBox1.SelectedIndex == _names.IndexOf("AttackTrigger") || comboBox1.SelectedIndex == _names.IndexOf("TriggeredInvBlocker"))
                {
                    TriggerParams trpa = new TriggerParams();
                    try
                    {
                        trpa.name = txtTrigName.Text;
                        trpa.global = chkGlobal.Checked;
                        _spawnPoint.trpa = trpa;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Please enter valid data for the teleport target!");
                        return;
                    }
                }

                _spawnPoint.objectID = _ids[index];
                if (_spawnPoint.objectID == -1)
                    _spawnPoint = null;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == _names.IndexOf("Teleporter"))
            {
                label3.Show();
                label4.Show();
                label5.Show();
                label6.Show();
                txtTargetMap.Show();
                txtTargetPosX.Show();
                txtTargetPosY.Show();
            }
            else
            {
                label3.Hide();
                label4.Hide();
                label5.Hide();
                label6.Hide();
                txtTargetMap.Hide();
                txtTargetPosX.Hide();
                txtTargetPosY.Hide();
            }

            if (comboBox1.SelectedIndex == _names.IndexOf("TouchTrigger") || comboBox1.SelectedIndex == _names.IndexOf("TriggeredBlocker") || comboBox1.SelectedIndex == _names.IndexOf("AttackTrigger") || comboBox1.SelectedIndex == _names.IndexOf("TriggeredInvBlocker"))
            {
                txtTrigName.Show();
                chkGlobal.Show();
            }
            else
            {
                txtTrigName.Hide();
                chkGlobal.Hide();
            }
        }


    }
}
