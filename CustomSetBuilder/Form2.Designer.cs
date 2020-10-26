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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeViewFolders = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnSelectDirectory = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.flowLayoutPanelStage = new System.Windows.Forms.FlowLayoutPanel();
            this.panelCopyContainer = new System.Windows.Forms.Panel();
            this.panelAdd3 = new System.Windows.Forms.Panel();
            this.labelAdd3 = new System.Windows.Forms.Label();
            this.panelAdd5 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panelAdd10 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panelAdd1 = new System.Windows.Forms.Panel();
            this.labelAdd1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.toolStripButtonCreatePDF = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCardCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelCopyContainer.SuspendLayout();
            this.panelAdd3.SuspendLayout();
            this.panelAdd5.SuspendLayout();
            this.panelAdd10.SuspendLayout();
            this.panelAdd1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1292, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonCreatePDF});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1292, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeViewFolders);
            this.panel1.Controls.Add(this.pictureBoxPreview);
            this.panel1.Controls.Add(this.btnSelectDirectory);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(289, 687);
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
            this.treeViewFolders.Location = new System.Drawing.Point(0, 23);
            this.treeViewFolders.Name = "treeViewFolders";
            this.treeViewFolders.SelectedImageIndex = 1;
            this.treeViewFolders.Size = new System.Drawing.Size(289, 664);
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
            this.btnSelectDirectory.Name = "btnSelectDirectory";
            this.btnSelectDirectory.Size = new System.Drawing.Size(289, 23);
            this.btnSelectDirectory.TabIndex = 0;
            this.btnSelectDirectory.Text = "Select Folder";
            this.btnSelectDirectory.UseVisualStyleBackColor = true;
            this.btnSelectDirectory.Click += new System.EventHandler(this.btnSelectDirectory_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(289, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 687);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(292, 49);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanelStage);
            this.splitContainer1.Panel2.Controls.Add(this.panelCopyContainer);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 687);
            this.splitContainer1.SplitterDistance = 437;
            this.splitContainer1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.splitter2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(437, 687);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(3, 3);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 0);
            this.splitter2.TabIndex = 0;
            this.splitter2.TabStop = false;
            // 
            // flowLayoutPanelStage
            // 
            this.flowLayoutPanelStage.AllowDrop = true;
            this.flowLayoutPanelStage.AutoScroll = true;
            this.flowLayoutPanelStage.AutoSize = true;
            this.flowLayoutPanelStage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelStage.Location = new System.Drawing.Point(143, 0);
            this.flowLayoutPanelStage.Name = "flowLayoutPanelStage";
            this.flowLayoutPanelStage.Size = new System.Drawing.Size(416, 687);
            this.flowLayoutPanelStage.TabIndex = 1;
            // 
            // panelCopyContainer
            // 
            this.panelCopyContainer.AllowDrop = true;
            this.panelCopyContainer.BackColor = System.Drawing.Color.LightGray;
            this.panelCopyContainer.Controls.Add(this.label2);
            this.panelCopyContainer.Controls.Add(this.labelCardCount);
            this.panelCopyContainer.Controls.Add(this.label1);
            this.panelCopyContainer.Controls.Add(this.panelAdd3);
            this.panelCopyContainer.Controls.Add(this.panelAdd5);
            this.panelCopyContainer.Controls.Add(this.panelAdd10);
            this.panelCopyContainer.Controls.Add(this.panelAdd1);
            this.panelCopyContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelCopyContainer.Location = new System.Drawing.Point(0, 0);
            this.panelCopyContainer.MinimumSize = new System.Drawing.Size(102, 687);
            this.panelCopyContainer.Name = "panelCopyContainer";
            this.panelCopyContainer.Size = new System.Drawing.Size(143, 687);
            this.panelCopyContainer.TabIndex = 2;
            // 
            // panelAdd3
            // 
            this.panelAdd3.AllowDrop = true;
            this.panelAdd3.BackColor = System.Drawing.Color.SkyBlue;
            this.panelAdd3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAdd3.Controls.Add(this.labelAdd3);
            this.panelAdd3.Location = new System.Drawing.Point(5, 221);
            this.panelAdd3.Name = "panelAdd3";
            this.panelAdd3.Size = new System.Drawing.Size(134, 67);
            this.panelAdd3.TabIndex = 1;
            this.panelAdd3.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelAdd3_DragDrop);
            this.panelAdd3.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelAdd3_DragEnter);
            // 
            // labelAdd3
            // 
            this.labelAdd3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAdd3.AutoSize = true;
            this.labelAdd3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdd3.Location = new System.Drawing.Point(35, 17);
            this.labelAdd3.Name = "labelAdd3";
            this.labelAdd3.Size = new System.Drawing.Size(37, 26);
            this.labelAdd3.TabIndex = 0;
            this.labelAdd3.Text = "x3";
            // 
            // panelAdd5
            // 
            this.panelAdd5.AllowDrop = true;
            this.panelAdd5.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelAdd5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAdd5.Controls.Add(this.label3);
            this.panelAdd5.Location = new System.Drawing.Point(6, 332);
            this.panelAdd5.Name = "panelAdd5";
            this.panelAdd5.Size = new System.Drawing.Size(134, 67);
            this.panelAdd5.TabIndex = 2;
            this.panelAdd5.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelAdd5_DragDrop);
            this.panelAdd5.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelAdd5_DragEnter);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(35, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 26);
            this.label3.TabIndex = 0;
            this.label3.Text = "x5";
            // 
            // panelAdd10
            // 
            this.panelAdd10.AllowDrop = true;
            this.panelAdd10.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panelAdd10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAdd10.Controls.Add(this.label4);
            this.panelAdd10.Location = new System.Drawing.Point(6, 443);
            this.panelAdd10.Name = "panelAdd10";
            this.panelAdd10.Size = new System.Drawing.Size(134, 67);
            this.panelAdd10.TabIndex = 3;
            this.panelAdd10.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelAdd10_DragDrop);
            this.panelAdd10.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelAdd10_DragEnter);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(28, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 26);
            this.label4.TabIndex = 0;
            this.label4.Text = "x10";
            // 
            // panelAdd1
            // 
            this.panelAdd1.AllowDrop = true;
            this.panelAdd1.BackColor = System.Drawing.Color.SteelBlue;
            this.panelAdd1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAdd1.Controls.Add(this.labelAdd1);
            this.panelAdd1.Location = new System.Drawing.Point(6, 110);
            this.panelAdd1.Name = "panelAdd1";
            this.panelAdd1.Size = new System.Drawing.Size(134, 67);
            this.panelAdd1.TabIndex = 0;
            this.panelAdd1.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelAdd1_DragDrop);
            this.panelAdd1.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelAdd1_DragEnter);
            // 
            // labelAdd1
            // 
            this.labelAdd1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAdd1.AutoSize = true;
            this.labelAdd1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdd1.Location = new System.Drawing.Point(35, 19);
            this.labelAdd1.Name = "labelAdd1";
            this.labelAdd1.Size = new System.Drawing.Size(37, 26);
            this.labelAdd1.TabIndex = 0;
            this.labelAdd1.Text = "x1";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Location = new System.Drawing.Point(0, 425);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(252, 262);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPreview.TabIndex = 2;
            this.pictureBoxPreview.TabStop = false;
            // 
            // toolStripButtonCreatePDF
            // 
            this.toolStripButtonCreatePDF.Image = global::CustomSetBuilder.Properties.Resources.page_white_acrobat;
            this.toolStripButtonCreatePDF.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCreatePDF.Name = "toolStripButtonCreatePDF";
            this.toolStripButtonCreatePDF.Size = new System.Drawing.Size(85, 22);
            this.toolStripButtonCreatePDF.Text = "Create PDF";
            this.toolStripButtonCreatePDF.Click += new System.EventHandler(this.toolStripButtonCreatePDF_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Copy Cards";
            // 
            // labelCardCount
            // 
            this.labelCardCount.AutoSize = true;
            this.labelCardCount.Location = new System.Drawing.Point(30, 23);
            this.labelCardCount.Name = "labelCardCount";
            this.labelCardCount.Size = new System.Drawing.Size(54, 16);
            this.labelCardCount.TabIndex = 5;
            this.labelCardCount.Text = "0 Cards";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Drop Cards Below";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 736);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form2";
            this.Text = "Form2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form2_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panelCopyContainer.ResumeLayout(false);
            this.panelCopyContainer.PerformLayout();
            this.panelAdd3.ResumeLayout(false);
            this.panelAdd3.PerformLayout();
            this.panelAdd5.ResumeLayout(false);
            this.panelAdd5.PerformLayout();
            this.panelAdd10.ResumeLayout(false);
            this.panelAdd10.PerformLayout();
            this.panelAdd1.ResumeLayout(false);
            this.panelAdd1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeViewFolders;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.Button btnSelectDirectory;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelStage;
        private System.Windows.Forms.Panel panelCopyContainer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panelAdd5;
        private System.Windows.Forms.Panel panelAdd3;
        private System.Windows.Forms.Panel panelAdd1;
        private System.Windows.Forms.Panel panelAdd10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelAdd3;
        private System.Windows.Forms.Label labelAdd1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton toolStripButtonCreatePDF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCardCount;
        private System.Windows.Forms.Label label2;
    }
}