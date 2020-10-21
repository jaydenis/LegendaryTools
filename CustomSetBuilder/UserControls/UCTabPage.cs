using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace CustomSetBuilder.UserControls
{
    public partial class UCTabPage : UserControl
    {
        public List<Image> imageListTemp { get; set; }
        public List<string> imageList { get; set; }
        public List<PictureBox> picBoxes { get; set; }
        public string selectedImageNode { get; set; }
        public PictureBox picPreview;
        string path;
        PictureBox activePictureBox;
        protected bool validData;
        protected Thread getImageThread;
        public PictureBox selectedPic { get; set; }

        protected Image image;

        public TableLayoutPanel layoutPanel { get; set; }

        public UCTabPage()
        {
            InitializeComponent();
            layoutPanel = tableLayoutPanel1;
        }

        public UCTabPage(List<Image> imageListTemp)
        {
            InitializeComponent();
            LoadTable(imageListTemp);
            layoutPanel = tableLayoutPanel1;
        }

        public void LoadTable(List<Image> imageListTemp)
        {
            picBoxes = new List<PictureBox>();
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
                pictureBox.Paint += PictureBox_Paint;
                picBoxes.Add(pictureBox);
                tableLayoutPanel1.Controls.Add(pictureBox, colNumber, rowNumber);

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

        private void SwapImages(PictureBox source, PictureBox target)
        {
            if (source.Image == null && target.Image == null)
            {
                return;
            }

            var temp = target.Image;
            target.Tag = source.Tag;
            target.Image = source.Image;
            source.Image = temp;

            if (imageList != null)
            {
                if (imageList.Contains(Convert.ToString(source.Tag)))
                {
                    imageList.Remove(Convert.ToString(source.Tag));
                }
            }
            else
            {
                imageList = new List<string>
                {
                    Convert.ToString(target.Tag)
                };
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
            foreach (var box in picBoxes) box.Invalidate();
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
                    if (e.Data.GetDataPresent(typeof(Bitmap)) || e.Data.GetDataPresent(typeof(PictureBox)))
                    {
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
                        }else if (e.Data.GetDataPresent(typeof(Bitmap)))
                        {
                            var sourceBitmap = (Bitmap)e.Data.GetData(typeof(Bitmap));
                            var source = new PictureBox();
                            source.Image = new Bitmap(sourceBitmap);

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
    }
}
