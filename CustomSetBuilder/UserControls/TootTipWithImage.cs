using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomSetBuilder.UserControls
{
    public class ToolTipWithImage : Control

    {

        private Image _img;

        private Control _ctl;

        private Timer _timer;

        string _imgfilename;

        string _tiptext;

        public String TipText

        {

            get { return _tiptext; }

            set { _tiptext = value; }

        }

        public String ImageFile

        {

            get

            {

                return _imgfilename;

            }

            set

            {

                if (_imgfilename == value)

                {

                }

                else

                {

                    _imgfilename = value;

                    try

                    {

                        _img = Image.FromFile(_imgfilename);

                        this.Size = new Size(180, 250);

                    }

                    catch

                    {

                        _img = null;

                    }

                }

            }

        }

        public ToolTipWithImage()

        {

            this.Location = new Point(0, 0);

            this.Visible = false;

            _timer = new Timer();

            _timer.Interval = 1000;

            _timer.Tick += new EventHandler(ShowTipOff);

        }

        public void SetToolTip(Control ctl)

        {

            _ctl = ctl;

            ctl.Parent.Controls.Add(this);

            ctl.Parent.Controls.SetChildIndex(this, 0);

            ctl.MouseMove += new MouseEventHandler(ShowTipOn);

        }

        protected override void OnPaint(PaintEventArgs e)
        {

            if (_img != null)
            {
                _img = FixedSize(_img, 180, 250);
                e.Graphics.DrawImage(_img, 0, 0);
                e.Graphics.DrawString(TipText, this.Font, Brushes.Black, _img.Width, 0);
            }
        }

        public void ShowTipOn(object sender, MouseEventArgs e)
        {
            if (!this.Visible)
            {
                _timer.Start();
                
                this.Left = _ctl.Left + e.X - 100;

                this.Top = _ctl.Top + e.Y+10;

                this.Visible = true;

            }
        }

        public void ShowTipOff(Object sender, EventArgs e)

        {

            _timer.Stop();

            this.Visible = false;

        }

        static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

    }


}
