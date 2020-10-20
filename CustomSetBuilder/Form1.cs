using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ReaLTaiizor.Controls;
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
        List<string> imageListBacks = new List<string>();
        List<string> imageList = new List<string>();
        protected bool validData;
        string path;
        protected Image image;
        protected Thread getImageThread;
        PictureBox activePictureBox;

        private XVector borderWidth;
        private XColor shadowColor;
        private XForm xform;
        PdfDocument document;
        bool linkSpacing = false;

        int x_offset = 1;
        int y_offset = 1;

        private enum States { Idle, Move, Copy };
        private States CurrentState = States.Idle;
        private System.Windows.Forms.DragDropEffects CurrentEffect = DragDropEffects.Move;
        private TreeNode RootNode = new TreeNode("C:\\", 1, 0);
        private TreeNode OriginalNode = new TreeNode("C:\\", 1, 0);
        private TreeNode StartNode = null;
        private TreeNode EndNode = null;
        private TreeNode OldNode = null;

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
                imageListBacks.Add(@"C:\Users\jayte\OneDrive\TableTopGaming\Legendery-Marvel\CustomSets\test\cardback.png");
                
            }
            ListDirectory(treeViewFolders, @"C:\Users\jayte\OneDrive\TableTopGaming\Legendery-Marvel\CustomSets\");
            LoadTable();
        }

        private void LoadTable()
        {
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
                    page = document.AddPage();
                    DrawPage(page, imageListBacks);
                }

                xform = new XForm(document,
                        XUnit.FromMillimeter(70),
                        XUnit.FromMillimeter(55));

                XGraphics formGfx = XGraphics.FromForm(xform);

                saveFileDialog1.ShowDialog();
                //string fileName = $"test{numX.Value}{numY.Value}.pdf";
                //document.Save(@"C:\Users\jayte\OneDrive\TableTopGaming\Legendery-Marvel\CustomSets\Test\" + fileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
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
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));

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
            }
        }

        private void MoveFile(TreeNode Node1, TreeNode Node2)
        {
            string strdir1 = Node1.Parent.FullPath; // get the path of the source item  
            string strdir2 = Node2.FullPath; // get the path of the drop target  
            strdir1 = strdir1 + "\\" + Node1.Text; // create file paths using the name of the source item  
            strdir2 = strdir2 + "\\" + Node1.Text;
            Directory.Move(strdir1, strdir2);// use the static Directory Move command to move a file  
            TreeNode aNode = new TreeNode(Node1.Text, 2, 2); // create a new file node  
            Node1.Remove(); // remove the old file node  
            Node2.Nodes.Add(aNode); // add the new file node under the target directory node  
        }

        private void treeViewFolders_DoubleClick(object sender, EventArgs e)
        {

        }

        private void treeViewFolders_DragDrop(object sender, DragEventArgs e)
        {
            // set a flag indicating if we are moving or not. If we are not moving, then we are copying  
            bool movingFile = (CurrentEffect == DragDropEffects.Move);
            // determine the node we are dropping into from the coordinates of the DragEvent Arguement  
            //TreeNode DropNode = FindTreeNode(e.X, e.Y);
            //// Reset the local state of the drag and the effect field  
            //CurrentState = States.Idle;
            //CurrentEffect = DragDropEffects.Move;
            //// If the drop target is a folder (not a file), then perform drop operations  
            //if (DropNode.ImageIndex != 2)
            //{
            //    // it's a folder, drop the file  
            //    if (movingFile)
            //    {
            //        MoveFile(StartNode, DropNode);// move the file from the startnode to the dropnode  
            //    }
            //    else
            //    {
            //        CopyFile(StartNode, DropNode);// copy the file from the startnode to the dropnode  
            //    }
            //    this.Invalidate(new Region(this.ClientRectangle)); // redraw the treeview  
            //    treeViewFolders.SelectedNode = DropNode; // select the drop node as the current selection  
            //}
        }

        private void treeViewFolders_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        protected TreeNode FindTreeNode(int x, int y)
        {
            TreeNode aNode = this.RootNode;

            Point pt = new Point(x, y);
            pt = PointToClient(pt);

            while (aNode != null)
            {
                if (aNode.Bounds.Contains(pt))
                {
                    return aNode;
                }
                aNode = aNode.NextVisibleNode;
            }

            return null;

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

        private void CopyFile(TreeNode Node1, TreeNode Node2)
        {
            string strdir1 = Node1.Parent.FullPath;
            string strdir2 = Node2.FullPath;
            strdir1 = strdir1 + "\\" + Node1.Text;
            strdir2 = strdir2 + "\\" + Node1.Text;
            File.Copy(strdir1, strdir2);
            TreeNode aNode = new TreeNode(Node1.Text, 2, 2);
            Node2.Nodes.Add(aNode);
        }

        private void treeViewFolders_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode aNode = (TreeNode)e.Item;
           
            // Only drag if it is a file, where not dragging directories in this example  
            if (aNode.ImageIndex == 2)
            {
                StartNode = aNode;
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

        private void treeViewFolders_DragOver(object sender, DragEventArgs e)
        {
            // set the effect again to maintain the effect we set in the DoDragDrop, (Seems you have to do this, may be a bug)  
            e.Effect = CurrentEffect;
            // Determine the node we are dragging over  
            // FindTreeNode is a local function of this class that determines the node from a point  
            TreeNode aNode = FindTreeNode(e.X, e.Y);
            if (aNode != null)
            {
                // If the node is a folder, change the color of the background to dark blue to simulate selection  
                // Be sure to return the previous node to its original color by copying from a blank node  
                if ((aNode.ImageIndex == 1) || (aNode.ImageIndex == 0))
                {
                    aNode.BackColor = Color.DarkBlue;
                    aNode.ForeColor = Color.White;
                    if ((OldNode != null) && (OldNode != aNode))
                    {
                        OldNode.BackColor = OriginalNode.BackColor;
                        OldNode.ForeColor = OriginalNode.ForeColor;
                    }
                    OldNode = aNode;
                }
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
                //selectedPic = picPreview;
                SelectBox(picPreview);
            }
            

        }

        protected void DirTree_Resize(object sender, System.EventArgs e)
        {
            treeViewFolders.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height);
        }

        protected string BuildDirectory(TreeNode aNode)
        {
            TreeNode theNode = aNode;
            string strDir = "";
            while (theNode != null)
            {
                if (theNode.Text[theNode.Text.Length - 1] != '\\')
                {
                    strDir = "\\" + strDir;
                }
                strDir = theNode.Text + strDir;
                theNode = theNode.Parent;
            }

            return strDir;

        }

        private void InitializeTree()
        {
            try
            {
                treeViewFolders.Nodes.Add(RootNode);
                string fileName = "DirectoryTree.File.bmp";
                //				DragCursor = new Cursor(pictureBox1.);
                ClearTree();
            }
            catch (Exception exx)
            {
                System.Console.WriteLine(exx.Message.ToString());
            }
        }

        public void ClearTree()
        {
            RootNode.Nodes.Clear();
        }

        private string m_strPath = "C:\\";
        public string Path
        {
            get
            {
                if (treeViewFolders.SelectedNode != null)
                {
                    m_strPath = BuildDirectory(treeViewFolders.SelectedNode);
                }
                return m_strPath;
            }
        }

        string m_strFileName = "";
        public string FileName
        {
            get
            {
                if (treeViewFolders.SelectedNode != null)
                {
                    if (treeViewFolders.SelectedNode.ImageIndex == 2)
                        m_strFileName = treeViewFolders.SelectedNode.Text;
                }
                return m_strFileName;
            }
        }


        private string m_strFilter = "*.*";
        public string Filter
        {
            set
            {
                m_strFilter = value;
            }

            get
            {
                return m_strFilter;
            }
        }

        private string m_strDrive = "C:\\";
        public string Drive
        {
            set
            {
                m_strDrive = value;
                RootNode.Text = m_strDrive;
                ClearTree();
            }
            get
            {
                return m_strDrive;
            }
        }

        private void treeViewFolders_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Bitmap bmp = (Bitmap)imageList1.Images[2];
            Graphics g = Graphics.FromImage(bmp);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            this.Cursor.Draw(g, rect);

        }




        /// <summary>
        /// Handle mouse click on picture box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            SelectBox((PictureBox)sender);
        }



        /// <summary>
        /// Override paint so we can draw a border on a selected image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Set the selected image, and trigger repaint on all boxes.
        /// </summary>
        /// <param name="pb"></param>
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

        /// <summary>
        /// Swap images between two PictureBoxes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private void SwapImages(PictureBox source, PictureBox target)
        {
            if (source.Image == null && target.Image == null)
            {
                return;
            }

            var temp = target.Image;
            target.Image = source.Image;
            source.Image = temp;
        }

        private void treeViewFolders_DragLeave(object sender, EventArgs e)
        {

            //TreeNode treeNode = (TreeNode)sender;
            //if(treeNode.ImageIndex == 2)
            //    this.selectedImageNode = Convert.ToString(treeNode.Tag);
        }

        private void treeViewFolders_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

        }
    }
}
