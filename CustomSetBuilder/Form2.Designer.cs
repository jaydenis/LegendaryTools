namespace CustomSetBuilder
{
    partial class Form2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeViewFolders = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnSelectDirectory = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.flowLayoutPanelStage = new System.Windows.Forms.FlowLayoutPanel();
            this.labelCardCount = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStaged = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addXCardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuPreview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuAdd1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuIAdd3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuIAdd5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuAdd10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnStartOver = new System.Windows.Forms.Button();
            this.btnCreatePDF = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.contextMenuStaged.SuspendLayout();
            this.contextMenuPreview.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeViewFolders);
            this.panel1.Controls.Add(this.btnSelectDirectory);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(217, 734);
            this.panel1.TabIndex = 2;
            // 
            // treeViewFolders
            // 
            this.treeViewFolders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFolders.FullRowSelect = true;
            this.treeViewFolders.HideSelection = false;
            this.treeViewFolders.HotTracking = true;
            this.treeViewFolders.ImageIndex = 0;
            this.treeViewFolders.ImageList = this.imageList1;
            this.treeViewFolders.Location = new System.Drawing.Point(0, 32);
            this.treeViewFolders.Margin = new System.Windows.Forms.Padding(2);
            this.treeViewFolders.Name = "treeViewFolders";
            this.treeViewFolders.SelectedImageIndex = 1;
            this.treeViewFolders.Size = new System.Drawing.Size(217, 702);
            this.treeViewFolders.TabIndex = 1;
            this.treeViewFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFolders_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Iconfactory Folder.ico");
            this.imageList1.Images.SetKeyName(1, "Pictures Folder.ico");
            this.imageList1.Images.SetKeyName(2, "Raster App.ico");
            // 
            // btnSelectDirectory
            // 
            this.btnSelectDirectory.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSelectDirectory.Location = new System.Drawing.Point(0, 0);
            this.btnSelectDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectDirectory.Name = "btnSelectDirectory";
            this.btnSelectDirectory.Size = new System.Drawing.Size(217, 32);
            this.btnSelectDirectory.TabIndex = 0;
            this.btnSelectDirectory.Text = "Select Folder";
            this.btnSelectDirectory.UseVisualStyleBackColor = true;
            this.btnSelectDirectory.Click += new System.EventHandler(this.btnSelectDirectory_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(217, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(2, 734);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(219, 54);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanelStage);
            this.splitContainer1.Size = new System.Drawing.Size(856, 680);
            this.splitContainer1.SplitterDistance = 419;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.splitter2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(419, 680);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(2, 2);
            this.splitter2.Margin = new System.Windows.Forms.Padding(2);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(2, 0);
            this.splitter2.TabIndex = 0;
            this.splitter2.TabStop = false;
            // 
            // flowLayoutPanelStage
            // 
            this.flowLayoutPanelStage.AllowDrop = true;
            this.flowLayoutPanelStage.AutoScroll = true;
            this.flowLayoutPanelStage.AutoSize = true;
            this.flowLayoutPanelStage.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.flowLayoutPanelStage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelStage.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelStage.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanelStage.Name = "flowLayoutPanelStage";
            this.flowLayoutPanelStage.Size = new System.Drawing.Size(434, 680);
            this.flowLayoutPanelStage.TabIndex = 1;
            this.flowLayoutPanelStage.DragDrop += new System.Windows.Forms.DragEventHandler(this.flowLayoutPanelStage_DragDrop);
            this.flowLayoutPanelStage.DragEnter += new System.Windows.Forms.DragEventHandler(this.flowLayoutPanelStage_DragEnter);
            this.flowLayoutPanelStage.DragOver += new System.Windows.Forms.DragEventHandler(this.flowLayoutPanelStage_DragOver);
            // 
            // labelCardCount
            // 
            this.labelCardCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCardCount.AutoSize = true;
            this.labelCardCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCardCount.Location = new System.Drawing.Point(417, 14);
            this.labelCardCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCardCount.Name = "labelCardCount";
            this.labelCardCount.Size = new System.Drawing.Size(71, 20);
            this.labelCardCount.TabIndex = 5;
            this.labelCardCount.Text = "0 Cards";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "PDF files (*.pdf)|*.pdf";
            this.saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // contextMenuStaged
            // 
            this.contextMenuStaged.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDelete,
            this.toolStripSeparator1,
            this.addXCardsToolStripMenuItem});
            this.contextMenuStaged.Name = "contextMenuStaged";
            this.contextMenuStaged.Size = new System.Drawing.Size(140, 54);
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Image = global::CustomSetBuilder.Properties.Resources.cancel;
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(139, 22);
            this.toolStripMenuItemDelete.Text = "Delete";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // addXCardsToolStripMenuItem
            // 
            this.addXCardsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x1ToolStripMenuItem,
            this.x3ToolStripMenuItem,
            this.x5ToolStripMenuItem,
            this.x10ToolStripMenuItem});
            this.addXCardsToolStripMenuItem.Name = "addXCardsToolStripMenuItem";
            this.addXCardsToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.addXCardsToolStripMenuItem.Text = "Add X Cards";
            // 
            // x1ToolStripMenuItem
            // 
            this.x1ToolStripMenuItem.Name = "x1ToolStripMenuItem";
            this.x1ToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.x1ToolStripMenuItem.Text = "x1";
            this.x1ToolStripMenuItem.Click += new System.EventHandler(this.x1ToolStripMenuItem_Click);
            // 
            // x3ToolStripMenuItem
            // 
            this.x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
            this.x3ToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.x3ToolStripMenuItem.Text = "x3";
            this.x3ToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuIAdd3_Click);
            // 
            // x5ToolStripMenuItem
            // 
            this.x5ToolStripMenuItem.Name = "x5ToolStripMenuItem";
            this.x5ToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.x5ToolStripMenuItem.Text = "x5";
            this.x5ToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuIAdd5_Click);
            // 
            // x10ToolStripMenuItem
            // 
            this.x10ToolStripMenuItem.Name = "x10ToolStripMenuItem";
            this.x10ToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.x10ToolStripMenuItem.Text = "x10";
            this.x10ToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuAdd10_Click);
            // 
            // contextMenuPreview
            // 
            this.contextMenuPreview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAllToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolStripMenuAdd1,
            this.toolStripMenuIAdd3,
            this.toolStripMenuIAdd5,
            this.toolStripMenuAdd10,
            this.toolStripSeparator3});
            this.contextMenuPreview.Name = "contextMenuPreview";
            this.contextMenuPreview.Size = new System.Drawing.Size(118, 126);
            // 
            // addAllToolStripMenuItem
            // 
            this.addAllToolStripMenuItem.Image = global::CustomSetBuilder.Properties.Resources.arrow_out;
            this.addAllToolStripMenuItem.Name = "addAllToolStripMenuItem";
            this.addAllToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addAllToolStripMenuItem.Text = "Add All";
            this.addAllToolStripMenuItem.Click += new System.EventHandler(this.addAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(114, 6);
            // 
            // toolStripMenuAdd1
            // 
            this.toolStripMenuAdd1.Name = "toolStripMenuAdd1";
            this.toolStripMenuAdd1.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuAdd1.Text = "Add x1";
            this.toolStripMenuAdd1.Click += new System.EventHandler(this.toolStripMenuAdd1_Click);
            // 
            // toolStripMenuIAdd3
            // 
            this.toolStripMenuIAdd3.Name = "toolStripMenuIAdd3";
            this.toolStripMenuIAdd3.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuIAdd3.Text = "Add x3";
            this.toolStripMenuIAdd3.Click += new System.EventHandler(this.toolStripMenuIAdd3_Click);
            // 
            // toolStripMenuIAdd5
            // 
            this.toolStripMenuIAdd5.Name = "toolStripMenuIAdd5";
            this.toolStripMenuIAdd5.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuIAdd5.Text = "Add x5";
            this.toolStripMenuIAdd5.Click += new System.EventHandler(this.toolStripMenuIAdd5_Click);
            // 
            // toolStripMenuAdd10
            // 
            this.toolStripMenuAdd10.Name = "toolStripMenuAdd10";
            this.toolStripMenuAdd10.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuAdd10.Text = "Add x10";
            this.toolStripMenuAdd10.Click += new System.EventHandler(this.toolStripMenuAdd10_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(114, 6);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnStartOver);
            this.panel2.Controls.Add(this.btnCreatePDF);
            this.panel2.Controls.Add(this.labelCardCount);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(219, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(856, 54);
            this.panel2.TabIndex = 6;
            // 
            // btnStartOver
            // 
            this.btnStartOver.BackColor = System.Drawing.Color.DarkOrange;
            this.btnStartOver.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStartOver.Image = global::CustomSetBuilder.Properties.Resources.arrow_undo;
            this.btnStartOver.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartOver.Location = new System.Drawing.Point(3, 7);
            this.btnStartOver.Name = "btnStartOver";
            this.btnStartOver.Size = new System.Drawing.Size(115, 37);
            this.btnStartOver.TabIndex = 1;
            this.btnStartOver.Text = "Start Over";
            this.btnStartOver.UseVisualStyleBackColor = false;
            this.btnStartOver.Click += new System.EventHandler(this.btnStartOver_Click);
            // 
            // btnCreatePDF
            // 
            this.btnCreatePDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreatePDF.BackColor = System.Drawing.Color.LimeGreen;
            this.btnCreatePDF.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatePDF.Image = global::CustomSetBuilder.Properties.Resources.page_white_acrobat;
            this.btnCreatePDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreatePDF.Location = new System.Drawing.Point(724, 3);
            this.btnCreatePDF.Name = "btnCreatePDF";
            this.btnCreatePDF.Size = new System.Drawing.Size(127, 45);
            this.btnCreatePDF.TabIndex = 0;
            this.btnCreatePDF.Text = "Create PDF";
            this.btnCreatePDF.UseVisualStyleBackColor = false;
            this.btnCreatePDF.Click += new System.EventHandler(this.btnCreatePDF_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 734);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form2";
            this.Text = "Custom Set Builder";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.contextMenuStaged.ResumeLayout(false);
            this.contextMenuPreview.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeViewFolders;
        private System.Windows.Forms.Button btnSelectDirectory;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelStage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label labelCardCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStaged;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip contextMenuPreview;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuAdd1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuIAdd3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuIAdd5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuAdd10;
        private System.Windows.Forms.ToolStripMenuItem addXCardsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x10ToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnStartOver;
        private System.Windows.Forms.Button btnCreatePDF;
        private System.Windows.Forms.ToolStripMenuItem addAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}