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
        PictureBox selectedPic;

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
                imageListBacks.Add(picPreview.BackgroundImage);
                
            }
            //ListDirectory(treeViewFolders, @"C:\\");
            LoadTable();
        }

        private void LoadTable()
        {
            tableLayoutPanel1.Controls.Clear();
            int x = 10;
            int y = 10;
            int h = 250;
            int w = 180;
            int rowNumber = 0;
            int colNumber = 0;
            int x_offset =0;
            int y_offset = 0;
            int leftMargin = 10;
            int topMargin = 10;

            foreach (var item in imageListBacks)
            {
                if (rowNumber == 0)
                    y = topMargin + y_offset;

                if (rowNumber == 1)
                    y = h + topMargin + (y_offset * 2);

                if (rowNumber == 2)
                {
                    y = (h * 2) + topMargin + (y_offset * 3);                    
                }

                if (colNumber == 0)
                    x = leftMargin + x_offset;

                if (colNumber == 1)
                    x = w + leftMargin + (x_offset * 2);

                if (colNumber == 2)
                {
                    x = (w * 2) + leftMargin + (x_offset * 3);                    
                }
                
                

                PictureBox pictureBox = new PictureBox();
                pictureBox.Location = new Point(x, y);
                pictureBox.Image = new Bitmap(item);
                pictureBox.Size = new Size(180, 250);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.AllowDrop = true;
                pictureBox.DragOver += PictureBox_DragOver;
                pictureBox.MouseMove += PictureBox_MouseMove;
                pictureBox.DragEnter += PictureBox_DragEnter;
                pictureBox.DragDrop += PictureBox_DragDrop;
                pictureBox.DoubleClick += PictureBox_DoubleClick;
                pictureBox.Paint += PictureBox_Paint;
                boxes.Add(pictureBox);
                tableLayoutPanel1.Controls.Add(pictureBox, colNumber, rowNumber);

               // string xy = $"row ={rowNumber},col={colNumber} / y={y.ToString()}, x={x.ToString()}{System.Environment.NewLine}";
                //richTextBox1.Text += xy;

                if (colNumber == 2)
                {
                    colNumber = 0;
                    rowNumber++;

                    //xy = $"[row={rowNumber},col={colNumber} / y={y.ToString()}, x={x.ToString()}]{System.Environment.NewLine}";
                    //richTextBox1.Text += xy;
                }
                else
                {
                    colNumber++;
                }

                
            }
        }

        private void LoadTable(List<string> imageListTemp)
        {
            tableLayoutPanel1.Controls.Clear();
            int x = 10;
            int y = 10;
            int h = 250;
            int w = 180;
            int rowNumber = 0;
            int colNumber = 0;
            int x_offset = 0;
            int y_offset = 0;
            int leftMargin = 10;
            int topMargin = 10;

            foreach (var item in imageListTemp)
            {
                if (rowNumber == 0)
                    y = topMargin + y_offset;

                if (rowNumber == 1)
                    y = h + topMargin + (y_offset * 2);

                if (rowNumber == 2)
                {
                    y = (h * 2) + topMargin + (y_offset * 3);
                }

                if (colNumber == 0)
                    x = leftMargin + x_offset;

                if (colNumber == 1)
                    x = w + leftMargin + (x_offset * 2);

                if (colNumber == 2)
                {
                    x = (w * 2) + leftMargin + (x_offset * 3);
                }



                PictureBox pictureBox = new PictureBox();
                pictureBox.Location = new Point(x, y);
                pictureBox.Image = new Bitmap(item);
                pictureBox.Size = new Size(180, 250);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.AllowDrop = true;
                pictureBox.DragOver += PictureBox_DragOver;
                pictureBox.MouseMove += PictureBox_MouseMove;
                pictureBox.DragEnter += PictureBox_DragEnter;
                pictureBox.DragDrop += PictureBox_DragDrop;
                pictureBox.DoubleClick += PictureBox_DoubleClick;
                pictureBox.Paint += PictureBox_Paint;
                pictureBox.Tag = item;
                boxes.Add(pictureBox);
                tableLayoutPanel1.Controls.Add(pictureBox, colNumber, rowNumber);

                // string xy = $"row ={rowNumber},col={colNumber} / y={y.ToString()}, x={x.ToString()}{System.Environment.NewLine}";
                //richTextBox1.Text += xy;

                if (colNumber == 2)
                {
                    colNumber = 0;
                    rowNumber++;

                    //xy = $"[row={rowNumber},col={colNumber} / y={y.ToString()}, x={x.ToString()}]{System.Environment.NewLine}";
                    //richTextBox1.Text += xy;
                }
                else
                {
                    colNumber++;
                }


            }
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

                            selectedPic = null;
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

                                selectedPic = null;
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

        public void DrawPage(PdfPage page, List<string> currentImageList)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);

            DrawImageScaled(gfx, currentImageList);
        }

        void DrawImageScaled(XGraphics gfx, List<string> currentImageList)
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

                var fileName = item;
                XImage image = XImage.FromFile(fileName);
                gfx.DrawImage(image, x, y, w, h);

            }
        }

        private void btnCreatePDF_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GeneratePDF();
            this.Cursor = Cursors.Default;
        }

        private void GeneratePDF()
        {
            try
            {
                document = new PdfDocument();
                PdfPage page = document.AddPage();
                imageList = new List<string>();
                foreach (var item in tableLayoutPanel1.Controls)
                {
                    PictureBox pictureBox = (PictureBox)item;
                    imageList.Add(pictureBox.Tag.ToString());
                }

                DrawPage(page, imageList);

                if (chkIncludeCardBacks.Checked)
                {
                    imageList.Clear();
                    page = document.AddPage();
                    foreach (var imgBack in imageListBacks) {
                        Image pictureBox = (Image)imgBack;
                        imageList.Add(pictureBox.Tag.ToString());
                    }
                    DrawPage(page, imageList);
                }

                XForm xform = new XForm(document,
                        XUnit.FromMillimeter(70),
                        XUnit.FromMillimeter(55));

                //XGraphics formGfx = XGraphics.FromForm(xform);

                saveFileDialog1.ShowDialog();
                //string fileName = $"test{numX.Value}{numY.Value}.pdf";
                //document.Save(@"C:\Test\" + fileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            var selectedPath = string.Empty;
            UserSettings settings = UserSettings.Load();

            if (!string.IsNullOrEmpty(settings.lastFolder))
            {
                saveFileDialog1.InitialDirectory = settings.lastFolder;
            }         
            if (document != null)
            {

                string name = saveFileDialog1.FileName;
                document.Save(name);

                settings.Save();
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

        private void btnDirectorySelector_Click(object sender, EventArgs e)
        {            
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
               
                ChooseFolder(folderBrowserDialog1.SelectedPath);

                UserSettings settings = UserSettings.Load();
                settings.lastFolder = folderBrowserDialog1.SelectedPath;
                settings.Save();

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
            if (e.Node.Nodes.Count > 0 || e.Node.ImageIndex == 0)
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
            if (selectedPic == pb)
            {
                ControlPaint.DrawBorder(e.Graphics, pb.ClientRectangle,
                   Color.OrangeRed, 3, ButtonBorderStyle.Solid,  // Left
                   Color.OrangeRed, 3, ButtonBorderStyle.Solid,  // Top
                   Color.OrangeRed, 3, ButtonBorderStyle.Solid,  // Right
                   Color.OrangeRed, 3, ButtonBorderStyle.Solid); // Bottom
            }
        }

        private void SelectBox(PictureBox pb)
        {
            if (selectedPic != pb)
            {
                selectedPic = pb;
            }
            else
            {
                selectedPic = null;
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

            if (imageList.Contains(Convert.ToString(source.Tag))){
                imageList.Remove(Convert.ToString(source.Tag));
            }

            var temp = target.Image;
            target.Tag = source.Tag;
            target.Image = source.Image;
            source.Image = temp;
        }



        private void ctxMenuItemFillAll_Click(object sender, EventArgs e)
        {
            //imageListBacks = new List<Image>();
            imageList = new List<string>();
            if (picPreview.Image != null)
            {
                for (int i = 0; i < 9; i++)
                {
                    //imageListBacks.Add(picPreview.Image);
                    imageList.Add(Convert.ToString(picPreview.Tag));
                }

                LoadTable(imageList);
            }
        }

        private void picPreview_MouseDown(object sender, MouseEventArgs e)
        {
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

        }

        private void toolStripMenuItemFillChecked_Click(object sender, EventArgs e)
        {
            try
            {
                if(imageListChecked.Count() > 0)
                {
                    LoadTable(imageListChecked);
                    imageListChecked.Clear();

                    foreach(TreeNode node in treeViewFolders.Nodes)
                    {
                        node.Checked = false;
                    }
                }
            }
            catch(Exception ex)
            {
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
            imageListChecked.Clear();
            imageList.Clear();
            var selectedPath = @"C:\\";
            UserSettings settings = UserSettings.Load();

            if (!string.IsNullOrEmpty(settings.lastFolder))
            {
                selectedPath = settings.lastFolder;
            }
            ChooseFolder(selectedPath);

            for (int i = 0; i < 9; i++)
            {
                imageListBacks.Add(picPreview.BackgroundImage);
            }
            picPreview.Image = picPreview.BackgroundImage;
            LoadTable();
        }
    }
}
