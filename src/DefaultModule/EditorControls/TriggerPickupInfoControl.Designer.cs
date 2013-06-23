namespace DefaultModule.EditorControls
{
    partial class TriggerPickupInfoControl
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
            this.txtTriggerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPlayerInfo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPlayerInfoValue = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTextureName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Trigger Name";
            // 
            // txtTriggerName
            // 
            this.txtTriggerName.Location = new System.Drawing.Point(16, 27);
            this.txtTriggerName.Name = "txtTriggerName";
            this.txtTriggerName.Size = new System.Drawing.Size(100, 20);
            this.txtTriggerName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Global";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(154, 30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "True";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Player Info";
            // 
            // txtPlayerInfo
            // 
            this.txtPlayerInfo.Location = new System.Drawing.Point(16, 66);
            this.txtPlayerInfo.Name = "txtPlayerInfo";
            this.txtPlayerInfo.Size = new System.Drawing.Size(100, 20);
            this.txtPlayerInfo.TabIndex = 5;
            this.txtPlayerInfo.TextChanged += new System.EventHandler(this.txtPlayerInfo_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(151, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Value";
            // 
            // txtPlayerInfoValue
            // 
            this.txtPlayerInfoValue.Location = new System.Drawing.Point(154, 66);
            this.txtPlayerInfoValue.Name = "txtPlayerInfoValue";
            this.txtPlayerInfoValue.Size = new System.Drawing.Size(100, 20);
            this.txtPlayerInfoValue.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Texture";
            // 
            // txtTextureName
            // 
            this.txtTextureName.Location = new System.Drawing.Point(16, 105);
            this.txtTextureName.Name = "txtTextureName";
            this.txtTextureName.Size = new System.Drawing.Size(100, 20);
            this.txtTextureName.TabIndex = 9;
            // 
            // TriggerPickupInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtTextureName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPlayerInfoValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPlayerInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTriggerName);
            this.Controls.Add(this.label1);
            this.Name = "TriggerPickupInfoControl";
            this.Size = new System.Drawing.Size(270, 130);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTriggerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPlayerInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPlayerInfoValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTextureName;
    }
}
