namespace DefaultModule.EditorControls
{
    partial class SpawnerInfoControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNumEnemies = new System.Windows.Forms.TextBox();
            this.txtEnemyType = new System.Windows.Forms.TextBox();
            this.txtRespawnTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "EnemyTypeID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(127, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Enemies";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Respawntime (in ms)";
            // 
            // txtNumEnemies
            // 
            this.txtNumEnemies.Location = new System.Drawing.Point(130, 31);
            this.txtNumEnemies.Name = "txtNumEnemies";
            this.txtNumEnemies.Size = new System.Drawing.Size(100, 20);
            this.txtNumEnemies.TabIndex = 3;
            // 
            // txtEnemyType
            // 
            this.txtEnemyType.Location = new System.Drawing.Point(16, 31);
            this.txtEnemyType.Name = "txtEnemyType";
            this.txtEnemyType.Size = new System.Drawing.Size(100, 20);
            this.txtEnemyType.TabIndex = 4;
            // 
            // txtRespawnTime
            // 
            this.txtRespawnTime.Location = new System.Drawing.Point(16, 75);
            this.txtRespawnTime.Name = "txtRespawnTime";
            this.txtRespawnTime.Size = new System.Drawing.Size(100, 20);
            this.txtRespawnTime.TabIndex = 5;
            // 
            // SpawnerInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtRespawnTime);
            this.Controls.Add(this.txtEnemyType);
            this.Controls.Add(this.txtNumEnemies);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SpawnerInfoControl";
            this.Size = new System.Drawing.Size(298, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNumEnemies;
        private System.Windows.Forms.TextBox txtEnemyType;
        private System.Windows.Forms.TextBox txtRespawnTime;
    }
}
