namespace MapEditor
{
    partial class frmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.tileSelecter1 = new MapEditor.TileSelecter();
            this.mapDisplay1 = new MapEditor.MapDisplay();
            this.btnNewMap = new System.Windows.Forms.ToolStripButton();
            this.btnLoad = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnBackground = new System.Windows.Forms.ToolStripButton();
            this.btnObstacle = new System.Windows.Forms.ToolStripButton();
            this.btnForeground = new System.Windows.Forms.ToolStripButton();
            this.btnObject = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnContent = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 28);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.vScrollBar1);
            this.splitContainer2.Panel1.Controls.Add(this.tileSelecter1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.mapDisplay1);
            this.splitContainer2.Size = new System.Drawing.Size(877, 568);
            this.splitContainer2.SplitterDistance = 299;
            this.splitContainer2.TabIndex = 11;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.LargeChange = 1;
            this.vScrollBar1.Location = new System.Drawing.Point(276, 3);
            this.vScrollBar1.Maximum = 0;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(21, 562);
            this.vScrollBar1.TabIndex = 9;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // tileSelecter1
            // 
            this.tileSelecter1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tileSelecter1.Location = new System.Drawing.Point(3, 3);
            this.tileSelecter1.Name = "tileSelecter1";
            this.tileSelecter1.Size = new System.Drawing.Size(272, 562);
            this.tileSelecter1.TabIndex = 1;
            this.tileSelecter1.Text = "tileSelecter1";
            this.tileSelecter1.MouseEnter += new System.EventHandler(this.tileSelecter1_MouseEnter);
            // 
            // mapDisplay1
            // 
            this.mapDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapDisplay1.Location = new System.Drawing.Point(0, 0);
            this.mapDisplay1.Name = "mapDisplay1";
            this.mapDisplay1.Size = new System.Drawing.Size(574, 568);
            this.mapDisplay1.TabIndex = 0;
            this.mapDisplay1.Text = "mapDisplay1";
            this.mapDisplay1.MouseEnter += new System.EventHandler(this.mapDisplay1_MouseEnter);
            // 
            // btnNewMap
            // 
            this.btnNewMap.Image = ((System.Drawing.Image)(resources.GetObject("btnNewMap.Image")));
            this.btnNewMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewMap.Name = "btnNewMap";
            this.btnNewMap.Size = new System.Drawing.Size(51, 22);
            this.btnNewMap.Text = "New";
            this.btnNewMap.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
            this.btnLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(53, 22);
            this.btnLoad.Text = "Load";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(51, 22);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnBackground
            // 
            this.btnBackground.CheckOnClick = true;
            this.btnBackground.Image = ((System.Drawing.Image)(resources.GetObject("btnBackground.Image")));
            this.btnBackground.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBackground.Name = "btnBackground";
            this.btnBackground.Size = new System.Drawing.Size(91, 22);
            this.btnBackground.Tag = 0;
            this.btnBackground.Text = "Background";
            this.btnBackground.CheckedChanged += new System.EventHandler(this.layerChanged);
            // 
            // btnObstacle
            // 
            this.btnObstacle.CheckOnClick = true;
            this.btnObstacle.Image = ((System.Drawing.Image)(resources.GetObject("btnObstacle.Image")));
            this.btnObstacle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnObstacle.Name = "btnObstacle";
            this.btnObstacle.Size = new System.Drawing.Size(73, 22);
            this.btnObstacle.Tag = 1;
            this.btnObstacle.Text = "Obstacle";
            this.btnObstacle.CheckedChanged += new System.EventHandler(this.layerChanged);
            // 
            // btnForeground
            // 
            this.btnForeground.CheckOnClick = true;
            this.btnForeground.Image = ((System.Drawing.Image)(resources.GetObject("btnForeground.Image")));
            this.btnForeground.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnForeground.Name = "btnForeground";
            this.btnForeground.Size = new System.Drawing.Size(89, 22);
            this.btnForeground.Tag = 2;
            this.btnForeground.Text = "Foreground";
            this.btnForeground.CheckedChanged += new System.EventHandler(this.layerChanged);
            // 
            // btnObject
            // 
            this.btnObject.CheckOnClick = true;
            this.btnObject.Image = ((System.Drawing.Image)(resources.GetObject("btnObject.Image")));
            this.btnObject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnObject.Name = "btnObject";
            this.btnObject.Size = new System.Drawing.Size(62, 22);
            this.btnObject.Tag = 3;
            this.btnObject.Text = "Object";
            this.btnObject.CheckedChanged += new System.EventHandler(this.layerChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewMap,
            this.btnLoad,
            this.btnSave,
            this.toolStripSeparator1,
            this.btnBackground,
            this.btnObstacle,
            this.btnForeground,
            this.btnObject,
            this.toolStripSeparator2,
            this.btnContent});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(877, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 25);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 571);
            this.splitter1.TabIndex = 13;
            this.splitter1.TabStop = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnContent
            // 
            this.btnContent.Image = ((System.Drawing.Image)(resources.GetObject("btnContent.Image")));
            this.btnContent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnContent.Name = "btnContent";
            this.btnContent.Size = new System.Drawing.Size(70, 22);
            this.btnContent.Text = "Content";
            this.btnContent.Click += new System.EventHandler(this.btnContent_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 596);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer2);
            this.Name = "frmMain";
            this.Text = "IGORR Editor";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapDisplay mapDisplay1;
		private TileSelecter tileSelecter1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.ToolStripButton btnNewMap;
		private System.Windows.Forms.ToolStripButton btnLoad;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnBackground;
		private System.Windows.Forms.ToolStripButton btnObstacle;
		private System.Windows.Forms.ToolStripButton btnForeground;
		private System.Windows.Forms.ToolStripButton btnObject;
		private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnContent;
        private System.Windows.Forms.Splitter splitter1;
    }
}

