
using Kaliko.ImageLibrary;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomSetBuilder
{
    public partial class Form2 : Form
    {
        Dictionary<string,KalikoImage> SelectedPictures = new Dictionary<string, KalikoImage>();
        List<string> SelectedPicturesPath = new List<string>();
        PictureBox activePictureBox;
        PictureBox activePictureBoxStaged;
        List<PictureBox> cardList = new List<PictureBox>();
        PdfDocument document;
        bool printCutlines = false;
        public Form2()
        {
            InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionDetails = $"v{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            Text = Text + " " + versionDetails; //change form title

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            //{
            folderBrowserDialog1.SelectedPath = @"C:\Users\jayte\OneDrive\TableTopGaming\Legendery-Marvel\CustomSets\test";
               ChooseFolder(folderBrowserDialog1.SelectedPath);
            //}

            btnCreatePDF.Enabled = false;
           
        }       

        public void DrawPage(PdfPage page, List<KalikoImage> currentImageList)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);

            DrawImageScaled(gfx, currentImageList);
        }

        void DrawImageScaled(XGraphics gfx, List<KalikoImage> currentImageList)
        {
            int leftMargin = 30;
            int topMargin = 20;
            int x = 30;
            int y = 20;
            int h = 250;
            int w = 180;
            int rowNumber = 0;
            int colNumber = 0;

            int leftStart = (w * 3);

            if (chkCutlines.Checked == true)
            {
                XPen pen = new XPen(XColors.Black, 1);

                foreach (var item in currentImageList)
                {
                    if (item != null)
                    {
                        y = (h * rowNumber) + topMargin;
                        x = (w * colNumber) + leftMargin;

                        gfx.DrawLine(pen, x, 1, x, topMargin + 10);
                        gfx.DrawLine(pen, x, 760, x, 780);
                        gfx.DrawLine(pen, 2, y, leftMargin + 10, y);
                        gfx.DrawLine(pen, leftStart + 10, y, leftStart + leftMargin + 30, y);

                        if (colNumber == 2)
                        {
                            colNumber = 0;
                            rowNumber++;
                        }
                        else
                        {
                            colNumber++;
                        }
                    }
                }

                gfx.DrawLine(pen, leftStart + 10, y, leftStart + leftMargin + 30, y);
                gfx.DrawLine(pen, 180 + x, 1, 180 + x, topMargin + 10);
                gfx.DrawLine(pen, 180 + x, 760, 180 + x, 780);
                gfx.DrawLine(pen, 2, 250 + y, leftMargin + 10, 250 + y);
                gfx.DrawLine(pen, leftStart + 10, 250 + y, leftStart + leftMargin + 30, 250 + y);
            }
            x = 30;
            y = 20;
            h = 250;
            w = 180;
            rowNumber = 0;
            colNumber = 0;

            foreach (var item in currentImageList)
            {
                if (item != null)
                {
                    y = (h * rowNumber) + topMargin;
                    x = (w * colNumber) + leftMargin;

                    if (colNumber == 2)
                    {
                        colNumber = 0;
                        rowNumber++;
                    }
                    else
                    {
                        colNumber++;
                    }

                    MemoryStream strm = new MemoryStream();
                    item.SavePng(strm);
                    XImage image = XImage.FromStream(strm);
                    gfx.DrawImage(image, x, y, w, h);
                }
            }
        }

        private void GeneratePDF()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                document = new PdfDocument();
                PdfPage page;
                

                var tempList = new List<KalikoImage>();
                int totalCards = SelectedPictures.Count;
                int remainingCards = totalCards;
                foreach (KeyValuePair<string,KalikoImage> item in SelectedPictures)
                {

                    tempList.Add(item.Value);
                    if(tempList.Count == 9)
                    {
                        page = document.AddPage();
                       
                        DrawPage(page, tempList);
                        tempList = new List<KalikoImage>();
                        remainingCards = remainingCards - 9;
                    }

                    if (remainingCards < 9 && remainingCards == tempList.Count && remainingCards !=0 && tempList.Count != 0)
                    {
                        page = document.AddPage();
                        DrawPage(page, tempList);
                    }
                }                   

                XForm xform = new XForm(document,
                        XUnit.FromMillimeter(70),
                        XUnit.FromMillimeter(55));

                saveFileDialog1.ShowDialog();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Doh!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            this.Cursor = Cursors.WaitCursor;

            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));

            this.Cursor = Cursors.Default;
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
           
            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                if (file.Extension == ".jpg" || file.Extension == ".png")
                {
                    var imgNode = new TreeNode(file.Name);
                    imgNode.Tag = file.FullName;
                    imgNode.ImageIndex = 2;
                   
                    directoryNode.Nodes.Add(imgNode);
                }
            }


            return directoryNode;
        }

        public void ChooseFolder(string selectedPath)
        {
            ListDirectory(treeViewFolders, selectedPath);
        }

        private void btnSelectDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ChooseFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        private void treeViewFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            if (e.Node.Nodes.Count > 0 || e.Node.ImageIndex == 0 || e.Node.ImageIndex == -1)
            {
                this.Cursor = Cursors.WaitCursor;
                if (flowLayoutPanel1.Controls.Count > 0)
                    foreach (Control pb in flowLayoutPanel1.Controls.OfType<PictureBox>())
                        pb.Dispose();

                flowLayoutPanel1.Controls.Clear();                
                cardList = new List<PictureBox>();

                foreach (TreeNode node in e.Node.Nodes)
                {
                   
                    if (node.Tag != null)
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.AllowDrop = true;
                        pictureBox.Image = new Bitmap(Convert.ToString(node.Tag));
                        pictureBox.ImageLocation = Convert.ToString(node.Tag);
                        pictureBox.Size = new Size(250, 325);
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox.MouseClick += PictureBox_MouseClick;
                        pictureBox.MouseDown += PictureBox_MouseDown;
                        pictureBox.MouseMove += PictureBox_MouseMove;
                        pictureBox.DragEnter += PictureBox_DragEnter;
                        pictureBox.DragDrop += PictureBox_DragDrop;
                        pictureBox.Paint += PictureBox_Paint;
                        pictureBox.ContextMenuStrip = contextMenuPreview;
                        pictureBox.Name = $"picture_{cardList.Count}";
                        
                        cardList.Add(pictureBox);
                    }                 
                }

                if (cardList.Count > 0)
                    flowLayoutPanel1.Controls.AddRange(cardList.ToArray());

                this.Cursor = Cursors.Default;
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            
            switch (e.Button)
            {

                case MouseButtons.Left:
                    SelectBox((PictureBox)sender);
                    break;

                case MouseButtons.Right:
                    SelectBox((PictureBox)sender);
                    contextMenuPreview.Show(this, new Point(e.X, e.Y));
                    break;
            }

        }

        private void PictureBox_DragDrop(object sender, DragEventArgs e)
        {
            var target = (PictureBox)sender;
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                var source = (PictureBox)e.Data.GetData(typeof(PictureBox));
                if (source != target)
                {
                    Console.WriteLine("Do DragDrop from " + source.Name + " to " + target.Name);
                    // You can swap the images out, replace the target image, etc.
                    //SwapImages(source, target);

                    activePictureBox = null;
                    SelectBox(target);
                    return;
                }
            }
            Console.WriteLine("Don't do DragDrop");
        }

        private void PictureBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            SelectBox((PictureBox)sender);
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var pb = (PictureBox)sender;
                if (pb.Image != null)
                {
                    pb.DoDragDrop(pb, DragDropEffects.Copy);
                }
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var pb = (PictureBox)sender;
            pb.BackColor = Color.White;
            if (activePictureBox == pb)
            {
                ControlPaint.DrawBorder(e.Graphics, pb.ClientRectangle,
                   Color.Red, 1, ButtonBorderStyle.Solid,  // Left
                   Color.Red, 1, ButtonBorderStyle.Solid,  // Top
                   Color.Red, 1, ButtonBorderStyle.Solid,  // Right
                   Color.Red, 1, ButtonBorderStyle.Solid); // Bottom
            }
            this.Cursor = Cursors.Default;
        }

        private void SelectBox(PictureBox pb)
        {
            if (activePictureBox != pb)
            {
                activePictureBox = pb;
                
            }
           
            // Cause each box to repaint
            foreach (var box in cardList) box.Invalidate();
        }

        private void SelectBoxStaged(PictureBox pb)
        {
            if (activePictureBoxStaged != pb)
            {
                activePictureBoxStaged = pb;
                activePictureBoxStaged.Name = pb.Name;
                
            }
           

            // Cause each box to repaint
            foreach (PictureBox box in flowLayoutPanelStage.Controls) box.Invalidate();
        }

        private void SwapImages(PictureBox source, PictureBox target)
        {
            //if (source.BackgroundImage == null && target.BackgroundImage == null)
            //{
            //    return;
            //}

            //var temp = target.BackgroundImage;
            //target.BackgroundImage = source.BackgroundImage;
            //source.BackgroundImage = temp;
        }

        private PictureBox CopyImage(PictureBox picture)
        {
            try
            {
                KalikoImage kalikoImage = new KalikoImage(picture.ImageLocation);
                PictureBox pb = new PictureBox();
                pb.Image = kalikoImage.GetAsBitmap();
                pb.ImageLocation = picture.ImageLocation;
                pb.Size = new Size(250, 325);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.ContextMenuStrip = this.contextMenuStaged;
                pb.AllowDrop = true;
                pb.DragDrop += StagingPictureBox_DragDrop;
                pb.DragEnter += StagingPictureBox_DragEnter;
                pb.MouseClick += StagingPictureBox_MouseClick;
                pb.MouseMove += StagingPictureBox_MouseMove;
                pb.Paint += StagingPictureBox_Paint;
                pb.DoubleClick += Pb_DoubleClick;
               //pb.MouseDown += Pb_MouseDown;
                //pb.Click += Pb_Click;

                int i = flowLayoutPanelStage.Controls.Count;              

                pb.Name = $"picture_{i++}";
                SelectedPicturesPath.Add(pb.ImageLocation);
                SelectedPictures.Add(pb.Name,kalikoImage);
                labelCardCount.Text = $"{SelectedPictures.Count} Cards";
                btnCreatePDF.Enabled = true;
                return pb;
            }catch(Exception ex)
            {
                return null;
            }
        }

        private void Pb_DoubleClick(object sender, EventArgs e)
        {
            StagingSelectBox((PictureBox)sender);
            KalikoImage kalikoImage = SelectedPictures.Where(x => x.Key == activePictureBoxStaged.Name).FirstOrDefault().Value;
            kalikoImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            activePictureBoxStaged.Image = kalikoImage.GetAsBitmap();
        }

        private void Pb_Click(object sender, EventArgs e)
        {
            StagingSelectBox((PictureBox)sender);
        }



        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                if (document != null)
                {
                    string name = saveFileDialog1.FileName;
                    document.Save(name);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AddXCards(int addCount, PictureBox pictureBox)
        {
            this.Cursor = Cursors.WaitCursor;
            if (pictureBox != null)
            {
                for (int i = 0; i < addCount; i++)
                {
                    flowLayoutPanelStage.Controls.Add(CopyImage(pictureBox));
                }
                flowLayoutPanelStage.ScrollControlIntoView(flowLayoutPanelStage.Controls[flowLayoutPanelStage.Controls.Count - 1]);
            }
            this.Cursor = Cursors.Default;
        }

        private void toolStripMenuAdd1_Click(object sender, EventArgs e)
        {
            AddXCards(1, activePictureBox);
        }

        private void toolStripMenuIAdd3_Click(object sender, EventArgs e)
        {
            AddXCards(3, activePictureBox);
        }

        private void toolStripMenuIAdd5_Click(object sender, EventArgs e)
        {
            AddXCards(5, activePictureBox);
        }

        private void toolStripMenuAdd10_Click(object sender, EventArgs e)
        {
            AddXCards(10, activePictureBox);
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (PictureBox p in flowLayoutPanelStage.Controls)
                {
                    if (p.Name == activePictureBoxStaged.Name)
                        flowLayoutPanelStage.Controls.Remove(p);
                }

                SelectedPictures.Clear();
                SelectedPicturesPath.Clear();

                foreach (PictureBox p in flowLayoutPanelStage.Controls)
                {
                    KalikoImage kalikoImage = new KalikoImage(p.ImageLocation);
                    int i = flowLayoutPanelStage.Controls.Count;
                    string name = $"picture_{i++}";
                    SelectedPictures.Add(name,kalikoImage);
                    SelectedPicturesPath.Add(p.ImageLocation);
                }

                labelCardCount.Text = $"{SelectedPictures.Count} Cards";
                if(SelectedPictures.Count == 0)
                    btnCreatePDF.Enabled = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void x1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddXCards(1, activePictureBoxStaged);
        }

        private void flowLayoutPanelStage_DragDrop(object sender, DragEventArgs e)
        {
            AddXCards(1, activePictureBox);
        }

        private void flowLayoutPanelStage_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void flowLayoutPanelStage_DragOver(object sender, DragEventArgs e)
        {

        }

        private void btnStartOver_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                
                if (flowLayoutPanelStage.Controls.Count > 0)
                    foreach (PictureBox pb in flowLayoutPanelStage.Controls.OfType<PictureBox>())
                        pb.Dispose();

                
                flowLayoutPanelStage.Controls.Clear();
                SelectedPictures.Clear();
                SelectedPicturesPath.Clear();
                activePictureBoxStaged = null;
                btnCreatePDF.Enabled = false;
                labelCardCount.Text = $"0 Cards";
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Cursor = Cursors.Default;
        }

        private void btnCreatePDF_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GeneratePDF();
            this.Cursor = Cursors.Default;
        }

        private void addAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (PictureBox box in flowLayoutPanel1.Controls)
            {
                AddXCards(1, box);
            }
        }

        #region staging
        /// <summary>
        /// Fires after dragging has completed on the target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StagingPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            var target = (PictureBox)sender;
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                var source = (PictureBox)e.Data.GetData(typeof(PictureBox));
                if (source != target)
                {
                    Console.WriteLine("Do DragDrop from " + source.Name + " to " + target.Name);
                    // You can swap the images out, replace the target image, etc.
                    StagingSwapImages(source, target);

                   // activePictureBoxStaged = null;
                 

                    StagingSelectBox(target);
                    return;
                }
            }
            Console.WriteLine("Don't do DragDrop");
        }

        /// <summary>
        /// Set the target's accepted DragDropEffect. Should match the source.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StagingPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Handle mouse click on picture box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StagingPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            StagingSelectBox((PictureBox)sender);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    //StagingSelectBox((PictureBox)sender);
                    break;

                case MouseButtons.Right:
                    //StagingSelectBox((PictureBox)sender);
                    contextMenuStaged.Show(this, new Point(e.X, e.Y));
                    break;
            }
        }

        /// <summary>
        /// Only start DragDrop if the mouse moves. Allows MouseClick to trigger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StagingPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var pb = (PictureBox)sender;
                if (pb.Image != null)
                {
                    pb.DoDragDrop(pb, DragDropEffects.Move);
                }
            }
        }

        /// <summary>
        /// Override paint so we can draw a border on a selected image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StagingPictureBox_Paint(object sender, PaintEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var pb = (PictureBox)sender;
            pb.BackColor = Color.White;
            if (activePictureBoxStaged == pb)
            {
                ControlPaint.DrawBorder(e.Graphics, pb.ClientRectangle,
                   Color.Blue, 3, ButtonBorderStyle.Solid,  // Left
                   Color.Blue, 3, ButtonBorderStyle.Solid,  // Top
                   Color.Blue, 3, ButtonBorderStyle.Solid,  // Right
                   Color.Blue, 3, ButtonBorderStyle.Solid); // Bottom
            }
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Set the selected image, and trigger repaint on all boxes.
        /// </summary>
        /// <param name="pb"></param>
        private void StagingSelectBox(PictureBox pb)
        {
            if (activePictureBoxStaged != pb)
            {
                activePictureBoxStaged = pb;
                activePictureBoxStaged.Name = pb.Name;
            }
            else
            {
               // activePictureBoxStaged = null;
            }


            // Cause each box to repaint
            foreach (PictureBox box in flowLayoutPanelStage.Controls) box.Invalidate();


        }

        /// <summary>
        /// Swap images between two PictureBoxes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private void StagingSwapImages(PictureBox source, PictureBox target)
        {
            if (source.Image == null && target.Image == null)
            {
                return;
            }

            var temp = target.Image;
            target.Image = source.Image;
            //target.ImageLocation = source.ImageLocation;
            //target.Name = source.Name;

            source.Image = temp;
            //source.ImageLocation = temp.ImageLocation;
            //source.Name = temp.Name;

        }
        #endregion

        private void btnSaveProject_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string path in SelectedPicturesPath)
            {
                stringBuilder.Append($"{path}{System.Environment.NewLine}");
            }

            MessageBox.Show(stringBuilder.ToString());
        }

        private void toolStripMenuItemRotateImage_Click(object sender, EventArgs e)
        {
            
        }

        private void chkCutlines_CheckedChanged(object sender, EventArgs e)
        {
            printCutlines = chkCutlines.Checked;
        }

        private void toolStripRotateRight_Click(object sender, EventArgs e)
        {
            KalikoImage kalikoImage = SelectedPictures.Where(x => x.Key == activePictureBoxStaged.Name).FirstOrDefault().Value;
            kalikoImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            activePictureBoxStaged.Image = kalikoImage.GetAsBitmap();
        }

        private void toolStripRotateLeft_Click(object sender, EventArgs e)
        {
            KalikoImage kalikoImage = SelectedPictures.Where(x => x.Key == activePictureBoxStaged.Name).FirstOrDefault().Value;
            kalikoImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            activePictureBoxStaged.Image = kalikoImage.GetAsBitmap();
        }

       
    }
}
