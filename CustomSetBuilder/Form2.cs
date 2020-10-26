
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        List<Image> SelectedPictires = new List<Image>();
        PictureBox activePictureBox;
        PictureBox activePictureBoxStaged;
        List<PictureBox> cardList = new List<PictureBox>();
        PdfDocument document;
        public Form2()
        {
            InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionDetails = $"v{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            Text = Text + " " + versionDetails; //change form title
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ChooseFolder(folderBrowserDialog1.SelectedPath);
            }

            btnCreatePDF.Enabled = false;
            //DirectoryInfo info = new DirectoryInfo(@"../..");
            //if (info.Exists)
            //{
            //    ChooseFolder(info.FullName);
            //}
        }

        public void DrawPage(PdfPage page, List<Image> currentImageList)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);

            DrawImageScaled(gfx, currentImageList);
        }

        void DrawImageScaled(XGraphics gfx, List<Image> currentImageList)
        {
            int leftMargin = 30;
            int topMargin = 30;
            int x = 30;
            int y = 20;
            int h = 250;
            int w = 180;
            int rowNumber = 0;
            int colNumber = 0;
            int x_offset = 2;
            int y_offset = 2;

            foreach (var item in currentImageList)
            {
                if (item != null)
                {
                    if (rowNumber == 0)
                        y = topMargin + y_offset;

                    if (rowNumber == 1)
                        y = h + topMargin + (y_offset * 2);

                    if (rowNumber == 2)
                        y = (h * 2) + topMargin + (y_offset * 3);

                    if (colNumber == 0)
                        x = leftMargin + x_offset;

                    if (colNumber == 1)
                        x = w + leftMargin + (x_offset * 2);

                    if (colNumber == 2)
                    {
                        x = (w * 2) + leftMargin + (x_offset * 3);
                        colNumber = 0;
                        rowNumber++;
                    }
                    else
                    {
                        colNumber++;
                    }

                    MemoryStream strm = new MemoryStream();
                    item.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
                    XImage image = XImage.FromStream(strm);
                    gfx.DrawImage(image, x, y, w, h);
                }

            }
        }

        private void GeneratePDF()
        {
            try
            {
                document = new PdfDocument();
                PdfPage page;

                var tempList = new List<Image>();
                int totalCards = SelectedPictires.Count;
                int remainingCards = totalCards;
                foreach (Image img in SelectedPictires)
                {
                    tempList.Add(img);
                    if(tempList.Count == 9)
                    {
                        page = document.AddPage();
                        DrawPage(page, tempList);
                        tempList = new List<Image>();
                        remainingCards = remainingCards - 9;
                    }

                    if (remainingCards < 9 && remainingCards == tempList.Count)
                    {
                        page = document.AddPage();
                        DrawPage(page, tempList);
                    }


                }   
                

                        //if (chkIncludeCardBacks.Checked)
                        //{
                        //    page = document.AddPage();
                        //    DrawPage(page, imageListBacks);
                        //}


                

                XForm xform = new XForm(document,
                        XUnit.FromMillimeter(70),
                        XUnit.FromMillimeter(55));

                //XGraphics formGfx = XGraphics.FromForm(xform);

                saveFileDialog1.ShowDialog();
                //string fileName = $"test{numX.Value}{numY.Value}.pdf";
                //document.Save(@"C:\Test\" + fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Doh!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);

            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            }

            int imageCount = 0;
            foreach (var file in directoryInfo.GetFiles())
            {
                if (file.Extension == ".jpg" || file.Extension == ".png")
                {
                    var imgNode = new TreeNode(file.Name);
                    imgNode.Tag = file.FullName;
                    imgNode.ImageIndex = 2;

                    directoryNode.Nodes.Add(imgNode);

                    

                    imageCount++;
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
                flowLayoutPanel1.Controls.Clear();
                if (cardList == null)
                    cardList = new List<PictureBox>();
                else
                    cardList.Clear();

                foreach (TreeNode node in e.Node.Nodes)
                {
                    if (node.Tag != null)
                    {
                        //CardControl card = new CardControl(Convert.ToString(node.Tag));
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.AllowDrop = true;
                        pictureBox.Image = new Bitmap(Convert.ToString(node.Tag));
                        pictureBox.ImageLocation = Convert.ToString(node.Tag);
                        pictureBox.Size = new Size(180, 250);
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
                        //flowLayoutPanel1.Controls.Add(card);
                    }                 
                }

                if (cardList.Count > 0)
                    flowLayoutPanel1.Controls.AddRange(cardList.ToArray());
                
            }
            else
            {
                pictureBoxPreview.Image = new Bitmap(Convert.ToString(e.Node.Tag));
                pictureBoxPreview.ImageLocation = Convert.ToString(e.Node.Tag);
                
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
            var pb = (PictureBox)sender;
            pb.BackColor = Color.White;
            if (activePictureBox == pb)
            {
                ControlPaint.DrawBorder(e.Graphics, pb.ClientRectangle,
                   Color.Blue, 2, ButtonBorderStyle.Solid,  // Left
                   Color.Blue, 2, ButtonBorderStyle.Solid,  // Top
                   Color.Blue, 2, ButtonBorderStyle.Solid,  // Right
                   Color.Blue, 2, ButtonBorderStyle.Solid); // Bottom
            }
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
                PictureBox pb = new PictureBox();
                pb.Image = picture.Image;
                pb.ImageLocation = picture.ImageLocation;
                pb.Size = new Size(180, 250);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.ContextMenuStrip = this.contextMenuStaged;
                pb.MouseDown += Pb_MouseDown;
                pb.Click += Pb_Click;

                int i = flowLayoutPanelStage.Controls.Count;              

                pb.Name = $"picture_{i++}";

                SelectedPictires.Add(pb.Image);
                labelCardCount.Text = $"{SelectedPictires.Count} Cards";
                btnCreatePDF.Enabled = true;
                return pb;
            }catch(Exception ex)
            {
                return null;
            }
        }

        private void Pb_Click(object sender, EventArgs e)
        {
            SelectBoxStaged((PictureBox)sender);
        }

        private void Pb_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    SelectBoxStaged((PictureBox)sender);
                    break;

                case MouseButtons.Right:
                    SelectBoxStaged((PictureBox)sender);
                    contextMenuStaged.Show(this, new Point(e.X, e.Y));
                    break;
            }
        }

        private void panelAdd1_DragDrop(object sender, DragEventArgs e)
        {

            AddXCards(1, activePictureBox);
        }

        private void panelAdd3_DragDrop(object sender, DragEventArgs e)
        {
            AddXCards(3, activePictureBox);

        }

        private void panelAdd5_DragDrop(object sender, DragEventArgs e)
        {
            AddXCards(5, activePictureBox);
        }

        private void panelAdd10_DragDrop(object sender, DragEventArgs e)
        {
            AddXCards(10, activePictureBox);
        }

        private void panelAdd1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            //panelAdd1.BackgroundImage = activePictureBox.Image;
            //flowLayoutPanelStage.Controls.Add(activePictureBox);
        }

        private void panelAdd3_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void panelAdd5_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        
        private void panelAdd10_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void toolStripButtonCreatePDF_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GeneratePDF();
            this.Cursor = Cursors.Default;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (document != null)
            {
                string name = saveFileDialog1.FileName;
                document.Save(name);
            }
        }

        private void AddXCards(int addCount, PictureBox pictureBox)
        {
            if (pictureBox != null)
            {
                for (int i = 0; i < addCount; i++)
                {
                    flowLayoutPanelStage.Controls.Add(CopyImage(pictureBox));
                }
                flowLayoutPanelStage.ScrollControlIntoView(flowLayoutPanelStage.Controls[flowLayoutPanelStage.Controls.Count - 1]);
            }
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

                SelectedPictires.Clear();

                foreach (PictureBox p in flowLayoutPanelStage.Controls)
                {
                    SelectedPictires.Add(p.Image);
                }

                labelCardCount.Text = $"{SelectedPictires.Count} Cards";
                if(SelectedPictires.Count == 0)
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

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddXCards(3, activePictureBoxStaged);
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddXCards(5, activePictureBoxStaged);
        }

        private void x10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddXCards(10, activePictureBoxStaged);
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
            try
            {
                flowLayoutPanelStage.Controls.Clear();
                SelectedPictires.Clear();
                activePictureBoxStaged = null;
                btnCreatePDF.Enabled = false;
                labelCardCount.Text = $"0 Cards";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
    }
}
