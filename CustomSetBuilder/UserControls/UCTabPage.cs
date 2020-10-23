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
        public List<Image> imageListPopulated { get; set; }
        public List<string> imageList { get; set; }
        public List<PictureBox> picBoxes { get; set; }
        public string selectedImageNode { get; set; }

        public int ImageCount = 0;

        public PictureBox picPreview;
        string path;
        PictureBox activePictureBox;
        protected bool validData;
        protected Thread getImageThread;
        public PictureBox selectedPic { get; set; }

        protected Image image;
        public Image picCardBackImage { get; set; }

        public TableLayoutPanel layoutPanel { get; set; }

        public UCTabPage()
        {
            InitializeComponent();
            layoutPanel = tableLayoutPanel1;
            AllowDrop = true;
            LoadTable();

            imageListPopulated = new List<Image>();
        }

 
        public void LoadTable()
        {
            picBoxes = new List<PictureBox>();
            tableLayoutPanel1.Controls.Clear();
            int x = 20;
            int y = 30;
            int h = 250;
            int w = 180;
            int rowNumber = 0;
            int colNumber = 0;
            int x_offset = 0;
            int y_offset = 0;
            int leftMargin = 30;
            int topMargin = 20;

            for(int i=0;i<9;i++) 
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
                //pictureBox.Image = new Bitmap(item);
                pictureBox.Size = new Size(180, 250);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.BackColor = Color.LightSteelBlue;
                pictureBox.BorderStyle = BorderStyle.FixedSingle;
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

        public void LoadTable(List<Image> imageList)
        {
            int i = 0;

            foreach (PictureBox pictureBox1 in tableLayoutPanel1.Controls)
            {
                if (imageList.ElementAtOrDefault(i) != null)
                {
                    ImageCount++;
                    pictureBox1.Image = new Bitmap(imageList[i]);
                    imageListPopulated.Add(pictureBox1.Image);
                }
                i++;
            }

        }

        public List<Image> GetImages()
        {
            var list = new List<Image>();
            foreach (PictureBox pictureBox1 in tableLayoutPanel1.Controls)
            {
                list.Add(pictureBox1.Image);               
            }

            return list;
        }

        private bool GetFilename(out string filename, DragEventArgs e)
        {
            bool ret = false;

            filename = string.Empty;

            if (e.AllowedEffect == DragDropEffects.Copy || e.AllowedEffect == DragDropEffects.Move)
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

        public void SwapImages(PictureBox source, PictureBox target)
        {
            if (source.Image == null && target.Image == null)
            {
                return;
            }

            var temp = target.Image;
            target.Tag = source.Tag;
            target.Image = source.Image;
            source.Image = temp;

            if (imageListTemp != null)
            {
                if (imageListTemp.Contains(source.Image))
                    imageListTemp.Remove(source.Image);

                imageListTemp.Add(target.Image);
            }
            else
            {
                imageListTemp = new List<Image>
                {
                    target.Image
                };
            }

            imageListPopulated.Add(target.Image);

            //LoadTable(imageListTemp);

        }

        public void SelectBox(PictureBox pb)
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

      

        public void CopyImageX(int copyCount, Image currentImage)
        {
            try
            {

                if (currentImage == null)
                    return;
                

                if (imageListTemp == null)
                    imageListTemp = new List<Image>();

                for (int i = 0; i < copyCount; i++)
                    imageListTemp.Add(currentImage);

                if (imageListTemp.Count > 9 && copyCount > 0)
                    imageListTemp.RemoveRange(0, copyCount);

                if (imageListTemp.Count < 9)
                {
                    int dif = 9 - imageListTemp.Count;
                    for (int i = 0; i < dif; i++)
                    {
                        imageListTemp.Add(picCardBackImage);
                    }
                }

                LoadTable(imageListTemp);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                    pb.DoDragDrop(pb, DragDropEffects.Copy);
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
               

                    var target = (PictureBox)sender;
                    if (e.Data.GetDataPresent(typeof(Bitmap)) || e.Data.GetDataPresent(typeof(PictureBox)))
                    {
                        picPreview = target;
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
