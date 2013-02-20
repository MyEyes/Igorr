namespace DefaultModule.EditorControls
{
    partial class LuaNPCControl
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
            this.txtScriptFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCharFile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Script File";
            // 
            // txtScriptFile
            // 
            this.txtScriptFile.Location = new System.Drawing.Point(16, 37);
            this.txtScriptFile.Name = "txtScriptFile";
            this.txtScriptFile.Size = new System.Drawing.Size(100, 20);
            this.txtScriptFile.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Char File";
            // 
            // txtCharFile
            // 
            this.txtCharFile.Location = new System.Drawing.Point(154, 37);
            this.txtCharFile.Name = "txtCharFile";
            this.txtCharFile.Size = new System.Drawing.Size(100, 20);
            this.txtCharFile.TabIndex = 3;
            // 
            // TriggerInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtCharFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtScriptFile);
            this.Controls.Add(this.label1);
            this.Name = "TriggerInfoControl";
            this.Size = new System.Drawing.Size(270, 80);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtScriptFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCharFile;
    }
}
