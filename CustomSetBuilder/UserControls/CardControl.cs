using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomSetBuilder.UserControls
{
    public partial class CardControl : UserControl
    {
        public int PrintCount { get; set; }
        public CardControl(string imagePath)
        {
            InitializeComponent();

            this.pictureBox1.Image = new Bitmap(imagePath);
            this.pictureBox1.ImageLocation = imagePath;
            this.pictureBox1.Size = new Size(180, 250);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
