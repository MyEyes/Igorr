namespace DefaultModule.EditorControls
{
    partial class TriggeredSpawnerInfoControl
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
            this.label4 = new System.Windows.Forms.Label();
            this.txtActivationTrigger = new System.Windows.Forms.TextBox();
            this.chkActivationGlobal = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtClearedTrigger = new System.Windows.Forms.TextBox();
            this.chkClearedGlobal = new System.Windows.Forms.CheckBox();
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(127, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Activation Trigger";
            // 
            // txtActivationTrigger
            // 
            this.txtActivationTrigger.Location = new System.Drawing.Point(130, 75);
            this.txtActivationTrigger.Name = "txtActivationTrigger";
            this.txtActivationTrigger.Size = new System.Drawing.Size(100, 20);
            this.txtActivationTrigger.TabIndex = 7;
            // 
            // chkActivationGlobal
            // 
            this.chkActivationGlobal.AutoSize = true;
            this.chkActivationGlobal.Location = new System.Drawing.Point(236, 77);
            this.chkActivationGlobal.Name = "chkActivationGlobal";
            this.chkActivationGlobal.Size = new System.Drawing.Size(56, 17);
            this.chkActivationGlobal.TabIndex = 8;
            this.chkActivationGlobal.Text = "Global";
            this.chkActivationGlobal.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Cleared Trigger";
            // 
            // txtClearedTrigger
            // 
            this.txtClearedTrigger.Location = new System.Drawing.Point(16, 118);
            this.txtClearedTrigger.Name = "txtClearedTrigger";
            this.txtClearedTrigger.Size = new System.Drawing.Size(100, 20);
            this.txtClearedTrigger.TabIndex = 10;
            // 
            // chkClearedGlobal
            // 
            this.chkClearedGlobal.AutoSize = true;
            this.chkClearedGlobal.Location = new System.Drawing.Point(130, 120);
            this.chkClearedGlobal.Name = "chkClearedGlobal";
            this.chkClearedGlobal.Size = new System.Drawing.Size(56, 17);
            this.chkClearedGlobal.TabIndex = 11;
            this.chkClearedGlobal.Text = "Global";
            this.chkClearedGlobal.UseVisualStyleBackColor = true;
            // 
            // TriggeredSpawnerInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkClearedGlobal);
            this.Controls.Add(this.txtClearedTrigger);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkActivationGlobal);
            this.Controls.Add(this.txtActivationTrigger);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRespawnTime);
            this.Controls.Add(this.txtEnemyType);
            this.Controls.Add(this.txtNumEnemies);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TriggeredSpawnerInfoControl";
            this.Size = new System.Drawing.Size(298, 147);
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtActivationTrigger;
        private System.Windows.Forms.CheckBox chkActivationGlobal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtClearedTrigger;
        private System.Windows.Forms.CheckBox chkClearedGlobal;
    }
}
