using ReaLTaiizor.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomSetBuilder
{
    public partial class ReaLTaiizorUI : Form
    {
        List<string> imageListBacks = new List<string>();
        public ReaLTaiizorUI()
        {
            InitializeComponent();
        }

        private void ReaLTaiizorUI_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                imageListBacks.Add(@"C:\Users\jayte\OneDrive\TableTopGaming\Legendery-Marvel\CustomSets\test\cardback.png");
            }

            LoadTable();
        }

        private void LoadTable()
        {
            hopeRichTextBox1.Text = "";
            int leftMargin = 10;
            int topMargin = 10;
            int x = 10;
            int y = 10;
            int h = 250;
            int w = 180;
            int rowNumber = 0;
            int colNumber = 0;
            int x_offset = 0;
            int y_offset = 0;

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
                    colNumber = 0;
                    rowNumber++;
                }
                else
                {
                    colNumber++;
                }

                HopePictureBox pictureBox = new HopePictureBox();
                pictureBox.Location = new Point(x, y);
                pictureBox.Image = new Bitmap(item);
                pictureBox.Size = new Size(180, 250);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.AllowDrop = true;
                //pictureBox.DragOver += PictureBox_DragOver;
                //pictureBox.MouseMove += PictureBox_MouseMove;
                //pictureBox.DragEnter += PictureBox_DragEnter;
                //pictureBox.DragDrop += PictureBox_DragDrop;
                //pictureBox.DoubleClick += PictureBox_DoubleClick;

                var xy = $"x={x.ToString()}, y={y.ToString()}{System.Environment.NewLine}";
                hopeRichTextBox1.Text += xy;
                

                lostBorderPanel1.Controls.Add(pictureBox);

                
            }
        }
    }
}
