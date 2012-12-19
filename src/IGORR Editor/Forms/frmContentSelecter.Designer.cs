namespace IGORR.Editor.Forms
{
    partial class frmContentSelecter
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
            this.mainDisplay1 = new IGORR.Editor.MainDisplay();
            this.SuspendLayout();
            // 
            // mainDisplay1
            // 
            this.mainDisplay1.Location = new System.Drawing.Point(262, 255);
            this.mainDisplay1.Name = "mainDisplay1";
            this.mainDisplay1.Size = new System.Drawing.Size(10, 10);
            this.mainDisplay1.TabIndex = 0;
            this.mainDisplay1.Text = "mainDisplay1";
            this.mainDisplay1.Visible = false;
            // 
            // frmContentSelecter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.mainDisplay1);
            this.Name = "frmContentSelecter";
            this.Text = "frmContentSelecter";
            this.ResumeLayout(false);

        }

        #endregion

        private MainDisplay mainDisplay1;
    }
}