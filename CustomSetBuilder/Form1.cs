using CustomSetBuilder.UserControls;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomSetBuilder
{
    public partial class Form1 : Form
    {
        List<Image> imageListBacks = new List<Image>();
        List<string> imageList = new List<string>();
        List<string> imageListChecked = new List<string>();
        protected bool validData;
        string path;
        protected Image image;
        protected Thread getImageThread;
        PictureBox activePictureBox;

        PdfDocument document;
        bool linkSpacing = false;

        int x_offset = 1;
        int y_offset = 1;

        private enum States { Idle, Move, Copy };
        private States CurrentState = States.Idle;
        private DragDropEffects CurrentEffect = DragDropEffects.Move;
       

        List<PictureBox> boxes = new List<PictureBox>();
       

        List<UCTabPage> ucTabPages = new List<UCTabPage>();
        UCTabPage selectedPage;
        string selectedImageNode = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            picPreview.AllowDrop = true;

            for (int i = 0; i < 9; i++)
            {
                imageListBacks.Add(picCardBack.BackgroundImage);                
            }
            
            UCTabPage ucTab = new UCTabPage(imageListBacks);

            ucTabPages.Add(ucTab);

            tabPage1.Controls.Add(ucTabPages[0]);
            selectedPage = (UCTabPage)tabPage1.Controls[0];

            
            //ListDirectory(treeViewFolders,@"C:\\");
            
            
        }

        public void DrawPage(PdfPage page, List<Image> currentImageList)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);

            DrawImageScaled(gfx, currentImageList);
        }

        void DrawImageScaled(XGraphics gfx, List<Image> currentImageList)
        {
            int leftMargin = Convert.ToInt32(numLeftMargin.Value);
            int topMargin = Convert.ToInt32(numTopMargin.Value);
            int x = 30;
            int y = 20;
            int h = 250;
            int w = 180;
            int rowNumber = 0;
            int colNumber = 0;

            foreach (var item in currentImageList)
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

                //var fileName = item;
                MemoryStream strm = new MemoryStream();
                item.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

                //XImage xfoto = XImage.FromStream(strm);

                XImage image = XImage.FromStream(strm);
                gfx.DrawImage(image, x, y, w, h);

            }
        }

        private void GeneratePDF()
        {
            try
            {
                document = new PdfDocument();
                PdfPage page;

                foreach (TabPage tab in tabControl1.TabPages)
                {
                    foreach (UCTabPage tabPage in tab.Controls)
                    {
                        page = document.AddPage();
                        DrawPage(page, tabPage.imageListTemp);
                    }
                }



                if (chkIncludeCardBacks.Checked)
                {
                    imageList.Clear();
                    page = document.AddPage();

                    DrawPage(page, imageListBacks);
                }

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
                MessageBox.Show(ex.ToString());
            }
        }

        private bool GetFilename(out string filename, DragEventArgs e)
        {
            bool ret = false;
           
                filename = string.Empty;

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        string ext = System.IO.Path.GetExtension(filename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

        protected void LoadImage()
        {
            image = new Bitmap(path);
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
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
            if (imageCount > 0)
                directoryNode.ExpandAll();

            return directoryNode;
        }

        public void ChooseFolder(string selectedPath)
        {
            txtFolderPath.Text = selectedPath;
            ListDirectory(treeViewFolders, selectedPath);
        }

        private void SelectBox(PictureBox pb)
        {
            if (selectedPage.selectedPic != pb)
            {
                selectedPage.selectedPic = pb;
            }
            else
            {
                selectedPage.selectedPic = null;
            }

            // Cause each box to repaint
            foreach (var box in boxes) box.Invalidate();
        }

        private void SwapImages(PictureBox source, PictureBox target)
        {
            if (source.Image == null && target.Image == null)
            {
                return;
            }

            if (imageList.Contains(Convert.ToString(source.Tag)))
            {
                imageList.Remove(Convert.ToString(source.Tag));
            }

            var temp = target.Image;
            target.Tag = source.Tag;
            target.Image = source.Image;
            source.Image = temp;
        }

        private void PictureBox_DoubleClick(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.jpg; *.png)|*.jpg; *.png";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // display image in picture box  
                    PictureBox pictureBox = (PictureBox)sender;
                    imageList.Add(openFileDialog1.FileName);
                    pictureBox.Image = new Bitmap(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }
      
        private void PictureBox_DragOver(object sender, DragEventArgs e)
        {
            this.activePictureBox = (PictureBox)sender;
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
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

        private void PictureBox_DragEnter(object sender, DragEventArgs e)
        {
            string filename;
            
            validData = GetFilename(out filename, e);
            if (validData)
            {
                path = filename;
                getImageThread = new Thread(new ThreadStart(LoadImage));
                getImageThread.Start();
                e.Effect = DragDropEffects.Copy;

            }
            else
            {
                e.Effect = DragDropEffects.Move;

            }
        }

        private void PictureBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (validData)
                {
                    while (getImageThread.IsAlive)
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }

                    this.activePictureBox.Image = image;
                    this.activePictureBox.Tag = path;

                }
                else
                {

                    var target = (PictureBox)sender;
                    //picPreview.Image = new Bitmap(Convert.ToString(e.Node.Tag));
                    //this.selectedImageNode = Convert.ToString(e.Node.Tag);
                    if (e.Data.GetDataPresent(typeof(PictureBox)))
                    {
                        var source = (PictureBox)e.Data.GetData(typeof(PictureBox));
                        if (source != target)
                        {
                            Console.WriteLine("Do DragDrop from " + source.Name + " to " + target.Name);
                            // You can swap the images out, replace the target image, etc.
                            SwapImages(source, target);

                            selectedPage.selectedPic = null;
                            SelectBox(target);
                            return;
                        }
                    }
                    else
                    {
                        if (this.selectedImageNode != string.Empty)
                        {
                            var source = picPreview;
                            if (source != target)
                            {
                                Console.WriteLine("Do DragDrop from " + source.Name + " to " + target.Name);
                                // You can swap the images out, replace the target image, etc.
                                SwapImages(source, target);

                                selectedPage.selectedPic = null;
                                SelectBox(target);
                                return;
                            }
                        }
                    }

                    Console.WriteLine("Don't do DragDrop");
                }
            }catch(Exception ex)
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

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            var selectedPath = string.Empty;                
            if (document != null)
            {

                string name = saveFileDialog1.FileName;
                document.Save(name);
            }
        }

        private void ImageLayout_Changed(object sender, EventArgs e)
        {
            int leftMargin = Convert.ToInt32(numLeftMargin.Value);
            int topMargin = Convert.ToInt32(numTopMargin.Value);
            int x = 30; 
            int y = 20; 
            int h = 100;
            int w = 70;
            int rowNumber = 0;
            int colNumber = 0;           

            foreach (var item in panelPreview.Controls)
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


                PictureBox pictureBox = (PictureBox)item;
                pictureBox.Location = new Point(x, y);

            }
        }

        private void chkLinkSpacing_CheckStateChanged(object sender, EventArgs e)
        {
            linkSpacing = chkLinkSpacing.Checked;
        }

        private void numX_ValueChanged(object sender, EventArgs e)
        {
            if (linkSpacing)
                y_offset = Convert.ToInt32(numX.Value);

            ImageLayout_Changed(sender, e);
        }

        private void numY_ValueChanged(object sender, EventArgs e)
        {
            if (linkSpacing)
                x_offset = Convert.ToInt32(numY.Value);

            ImageLayout_Changed(sender, e);
        }       

        private void btnDirectorySelector_Click(object sender, EventArgs e)
        {            
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ChooseFolder(folderBrowserDialog1.SelectedPath);
            }

            
        }

        private void treeViewFolders_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        
        protected void treeViewFolders_MouseUp(object sender, MouseEventArgs e)
        {
            CurrentState = States.Idle;
        }

        protected void treeViewFolders_KeyUp(object sender, KeyEventArgs e)
        {
            if (CurrentState != States.Idle)
            {
                return; // precondition, can't change effect while moving
            }
            // restore to a move effect
            CurrentEffect = DragDropEffects.Move;
        }

        protected void treeViewFolders_KeyDown(object sender, KeyEventArgs e)
        {
            if (CurrentState != States.Idle)
            {
                return; // precondition, can't change effect while moving
            }
            if (e.Control == true)
            {
                CurrentEffect = DragDropEffects.Copy;
            }
            else
            {
                CurrentEffect = DragDropEffects.Move;
            }
        }

        private void treeViewFolders_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode aNode = (TreeNode)e.Item;
           
            // Only drag if it is a file, where not dragging directories in this example  
            if (aNode.ImageIndex == 2)
            {
               
                //The effect was previously set based on whether the ctrl key was pressed.  
                // If it was, the effect will be copy otherwise it will be move  
                // Set the state of the drag-drop operation, the state is a local enumeration in our class  
                if (CurrentEffect == DragDropEffects.Move)
                {
                    CurrentState = States.Copy;
                }
                else
                {
                    CurrentState = States.Copy;
                }
                // DoDragDrop sets things in motion for the drag drop operation by passing the data and the current effect  
                // Data can be a bitmap, string or something that implements the IDataObject interface  
                Bitmap b = new Bitmap(Convert.ToString(aNode.Tag));
                this.selectedImageNode = Convert.ToString(aNode.Tag);
                this.DoDragDrop(b, CurrentEffect);
                this.selectedImageNode = "";
            }
        }

        public void treeViewFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0 || e.Node.ImageIndex == 0 || e.Node.ImageIndex == -1)
            {
                return;
            }
            else
            {
                
                imageList.Add(Convert.ToString(e.Node.Tag));
                picPreview.Image = new Bitmap(Convert.ToString(e.Node.Tag));
                this.selectedImageNode = Convert.ToString(e.Node.Tag);
                picPreview.Tag = this.selectedImageNode;
                SelectBox(picPreview);
            }
            

        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            
            SelectBox((PictureBox)sender);
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            var pb = (PictureBox)sender;
            pb.BackColor = Color.White;
            if (selectedPage.selectedPic == pb)
            {
                ControlPaint.DrawBorder(e.Graphics, pb.ClientRectangle,
                   Color.OrangeRed, 3, ButtonBorderStyle.Solid,  // Left
                   Color.OrangeRed, 3, ButtonBorderStyle.Solid,  // Top
                   Color.OrangeRed, 3, ButtonBorderStyle.Solid,  // Right
                   Color.OrangeRed, 3, ButtonBorderStyle.Solid); // Bottom
            }
        }

        private void ctxMenuItemFillAll_Click(object sender, EventArgs e)
        {
            try
            {
                CopyImageX(9);
                //selectedPage.imageListTemp = new List<Image>();
                //selectedPage.imageList = new List<string>();
                //if (picPreview.Image != null)
                //{
                //    for (int i = 0; i < 9; i++)
                //    {
                //        selectedPage.imageListTemp.Add(picPreview.Image);
                //        selectedPage.imageList.Add(Convert.ToString(picPreview.Tag));
                //    }

                //    selectedPage.LoadTable(selectedPage.imageListTemp);
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void picPreview_MouseDown(object sender, MouseEventArgs e)
        {
            selectedPage.selectedPic = picPreview;
            switch (e.Button)
            {

                case MouseButtons.Left:
                    // Left click
                    break;

                case MouseButtons.Right:
                    ctxPreviewMenu.Show(this, new Point(e.X, e.Y));
                    break;
            }

        }

        private void picPreview_Click(object sender, EventArgs e)
        {
            selectedPage.selectedPic = picPreview;
        }

        private void toolStripMenuItemFillChecked_Click(object sender, EventArgs e)
        {
            try
            {
                if(imageListChecked.Count() > 0)
                {
                    if (selectedPage.imageListTemp == null)
                        selectedPage.imageListTemp = new List<Image>();


                    foreach (var item in imageListChecked)
                    {
                        image = new Bitmap(item);
                        selectedPage.imageListTemp.Add(image);
                    }

                    if (selectedPage.imageListTemp.Count > 9 && imageListChecked.Count > 0)
                        selectedPage.imageListTemp.RemoveRange(0, imageListChecked.Count);

                    

                    if(selectedPage.imageListTemp.Count < 9)
                    {
                        int dif = 9 - selectedPage.imageListTemp.Count;
                        for (int i=0; i < dif; i++)
                        {
                            selectedPage.imageListTemp.Add(picPreviewBottomRight.Image);
                        }
                    }

                    

                    selectedPage.LoadTable(selectedPage.imageListTemp);

                    imageListChecked.Clear();

                    foreach(TreeNode node in treeViewFolders.Nodes)
                    {
                        node.Checked = false;
                        foreach(TreeNode node1 in node.Nodes)
                        {
                            node1.Checked = false;
                            if (node1.Nodes.Count > 0)
                            {
                                foreach (TreeNode node2 in node1.Nodes)
                                {
                                    node2.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                selectedPage.imageListTemp = new List<Image>();
               // imageListChecked.Clear();
                MessageBox.Show(ex.ToString());
            }
        }

        private void treeViewFolders_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Nodes.Count > 0 || e.Node.ImageIndex == 0)
                {
                    return;
                }
                else
                {
                    imageListChecked.Add(Convert.ToString(e.Node.Tag));
                  
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            //imageListChecked.Clear();
            imageList.Clear();
            var selectedPath = @"C:\";

            tabControl1.TabPages.Clear();

            AddnewTabPage();

            //ChooseFolder(selectedPath);

            for (int i = 0; i < 9; i++)
            {
                imageListBacks.Add(picPreview.BackgroundImage);
            }
            picPreview.Image = picPreview.BackgroundImage;

            
        }

        private void toolStripButtonAddPage_Click(object sender, EventArgs e)
        {
            AddnewTabPage();

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            TabPage tabPage = tabControl1.SelectedTab;
            if(tabPage != null)
                selectedPage = (UCTabPage)tabPage.Controls[0];

        }

        private void AddnewTabPage()
        {
            int pageCount = tabControl1.TabPages.Count + 1;
            TabPage tabPage = new TabPage($"Page {pageCount++}");

            UCTabPage ucTab = new UCTabPage(imageListBacks);

            ucTabPages.Add(ucTab);
            tabPage.Controls.Add(ucTab);

            tabControl1.TabPages.Add(tabPage);
        }

        private void CopyImageX(int copyCount)
        {
            try
            {

                if (picPreview.Image == null && selectedPage.selectedPic == null)
                    return;

                if (picPreview.Image != null)
                    selectedPage.selectedPic = picPreview;
                else if (selectedPage.selectedPic != null)
                    picPreview = selectedPage.selectedPic;
                else
                    return;

                if (selectedPage.imageListTemp == null)
                    selectedPage.imageListTemp = new List<Image>();               

                for (int i = 0; i < copyCount; i++)
                    selectedPage.imageListTemp.Add(selectedPage.selectedPic.Image);

                if (selectedPage.imageListTemp.Count > 9 && copyCount > 0)
                    selectedPage.imageListTemp.RemoveRange(0, copyCount);

                if (selectedPage.imageListTemp.Count < 9)
                {
                    int dif = 9 - selectedPage.imageListTemp.Count;
                    for (int i = 0; i < dif; i++)
                    {
                        selectedPage.imageListTemp.Add(picCardBack.Image);
                    }
                }

                selectedPage.LoadTable(selectedPage.imageListTemp);
            
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void toolStripMenuItemCopy3_Click(object sender, EventArgs e)
        {
            CopyImageX(3);
        }

        private void toolStripMenuItemCopy5_Click(object sender, EventArgs e)
        {
            CopyImageX(5);
        }

        private void copyCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyImageX(1);
        }
    }
}
