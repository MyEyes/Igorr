namespace MapEditor
{
    partial class frmContent
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstContent = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstContent
            // 
            this.lstContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstContent.FormattingEnabled = true;
            this.lstContent.Location = new System.Drawing.Point(0, 0);
            this.lstContent.Name = "lstContent";
            this.lstContent.Size = new System.Drawing.Size(284, 262);
            this.lstContent.TabIndex = 0;
            // 
            // frmContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lstContent);
            this.Name = "frmContent";
            this.Text = "frmContent";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstContent;
    }
}