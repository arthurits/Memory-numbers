﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace Controls
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(System.ComponentModel.Design.IDesigner))]
    public partial class RoundButton : UserControl
    {
        #region GDI API functions

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
                    int nLeftRect,      // x-coordinate of upper-left corner
                    int nTopRect,       // y-coordinate of upper-left corner
                    int nRightRect,     // x-coordinate of lower-right corner
                    int nBottomRect,    // y-coordinate of lower-right corner
                    int nWidthEllipse,  // height of ellipse
                    int nHeightEllipse  // width of ellipse
            );
        [DllImport("Gdi32.dll", EntryPoint = "FillRgn")]
        private static extern IntPtr FillRgn
            (IntPtr hdc,
            IntPtr hrgn,
            IntPtr hbr
            );
        [DllImport("Gdi32.dll", EntryPoint = "FrameRgn")]
        public static extern int FrameRgn(
            IntPtr hDC,
            IntPtr hRgn,
            IntPtr hBrush,
            int nWidth,
            int nHeight
            );

        [DllImport("Gdi32.dll", EntryPoint = "CreateSolidBrush")]
        private static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("Gdi32.dll", EntryPoint = "DeleteObject")]
        private static extern bool DeleteObject(IntPtr hObject);

        #endregion #region GDI API functions

        #region Private variables

        private float _fBorderWidth = 1f;
        private float _xRadius = 0f;
        private float _yRadius = 0f;
        private Color _cBorderColor = Color.Black;
        private Color _cFillColor = Color.Transparent;
        private int _nWidth;
        private int _nHeight;
        private string _sText ="";
        private bool _showText = true;
        private bool _showBorder = true;

        #endregion Private variables

        #region Public interface

        [Description("Width of the border (0 means no border)"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float BorderWidth
        {
            get { return _fBorderWidth; }
            set { _fBorderWidth = value < 0 ? 0f : value; Invalidate(); }
        }

        [Description("Border radius (0 means no rounded corner)"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float xRadius
        {
            get { return _xRadius; }
            set { _xRadius = value < 0 ? 0f : value; Invalidate(); }
        }

        [Description("Border radius (0 means no rounded corner)"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float yRadius
        {
            get { return _yRadius; }
            set { _yRadius = value < 0 ? 0f : value; Invalidate(); }
        }

        [Description("Border color"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get { return _cBorderColor; }
            set { _cBorderColor = value; Invalidate(); }
        }

        [Description("Fill color"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color FillColor
        {
            get { return _cFillColor; }
            set { _cFillColor = value; Invalidate(); }
        }

        [Description("Inner width inside stroke"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int InnerWidth
        {
            get { return (int)(this.ClientRectangle.Width - 1 - 2 * _fBorderWidth); }
        }

        [Description("Inner height inside stroke"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int InnerHeight
        {
            get { return (int)(this.ClientRectangle.Height - 1 - 2 * _fBorderWidth); }
        }

        [Description("Text to display"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return _sText; }
            set { _sText = value.ToString(); lblText.Text = _sText; }
        }

        [Description("Show text"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool VisibleText
        {
            get { return _showText; }
            set { _showText = value; lblText.Visible = _showText; }
        }

        [Description("Show border"),
        Category("Rounded properties"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool VisibleBorder
        {
            get { return _showBorder; }
            set { _showBorder = value; Invalidate(); }
        }

        #endregion Public interface

        #region Public events
        public event EventHandler<ButtonClickEventArgs> ButtonClick;
        protected virtual void OnButtonClick(ButtonClickEventArgs e)
        {
            if (ButtonClick != null) ButtonClick(this, e);
        }
        public class ButtonClickEventArgs : EventArgs
        {
            public readonly int ButtonValue;
            public ButtonClickEventArgs(int button) { ButtonValue = button; }
        }
        #endregion Public events

        public RoundButton()
        {
            InitializeComponent();

            // To ensure that your control is redrawn every time it is resized
            // https://msdn.microsoft.com/en-us/library/b818z6z6(v=vs.110).aspx
            SetStyle(ControlStyles.ResizeRedraw, true);

            this.DoubleBuffered = true;

            // Set default size
            //this.ClientSize = new System.Drawing.Size(100, 100);

            //this.AutoSize = false;
            //this.BackColor= Color.Transparent;
            //this.TextAlign = ContentAlignment.MiddleCenter;
            //this.BorderStyle = BorderStyle.None;

            //this.OnMouseClick += new System.Windows.Forms.MouseEventHandler(this.RoundButton_MouseClick);
            //this.Click += new EventHandler(OnMouseClick);

        }

        private void RoundButton_Load(object sender, EventArgs e)
        {
            // Some default label properties
            lblText.Text = Text;
            lblText.Text = _sText;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            /*
            //30 is width, height of ellipse
            IntPtr hrgn = CreateRoundRectRgn(0, 0, this.Width, this.Height, 30, 30);
            IntPtr brush = CreateSolidBrush(0x0); // black, of format : //0x00bbggrr
            FillRgn(this.Handle, hrgn, brush);
            FrameRgn(e.Graphics.GetHdc(), hrgn, brush, 2, 2);

            DeleteObject(hrgn);
            DeleteObject(brush);

            Region = Region.FromHrgn(hrgn);

            Graphics.FromHdc(this.Handle).FillRegion(, Region);
            Graphics.FromHdc(this.Handle).;
            */

            using (Pen pen = new Pen(new SolidBrush(_cBorderColor), _fBorderWidth))
            {
                RectangleF rect = new RectangleF(0.5f, 0.5f, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
                GraphicsPath path = MakeRoundedRect(rect, _xRadius, _yRadius, true, true, true, true);

                pen.Alignment = PenAlignment.Inset;
                Graphics dc = e.Graphics;
                dc.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                dc.FillPath(new SolidBrush(_cFillColor), path);
                if (_showBorder) dc.DrawPath(pen, path);
                this.Region = new Region(path);
                this.lblText.Padding = new Padding((int)(this.lblText.Font.SizeInPoints / 7), 0, 0, 0);
                this.lblText.Region = this.Region;
            }
            base.OnPaint(e);
            
        }

        private void lblText_Click(object sender, EventArgs e)
        {
            //_showBorder = !_showBorder;
            //Invalidate();
            //if (ButtonClick != null) OnButtonClick(new ButtonClickEventArgs(int.Parse(lblText.Text)));
        }


        protected override void OnClick(EventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine("Click button");
            _showBorder = !_showBorder;
            Invalidate();
            if (ButtonClick != null) OnButtonClick(new ButtonClickEventArgs(int.Parse(lblText.Text)));

            //_showBorder = !_showBorder;
            //Invalidate();
            //base.OnClick(e);
        }

        /// <summary>
        /// Draw a rectangle in the indicated Rectangle rounding the indicated corners.
        /// http://csharphelper.com/blog/2016/01/draw-rounded-rectangles-in-c/
        /// </summary>
        /// <param name="rect">Rectangle structure to be rounded</param>
        /// <param name="xradius">Horizonal radius in pixels</param>
        /// <param name="yradius">Vertical radius in pixels</param>
        /// <param name="round_ul">True if upper left corner is to be rounded</param>
        /// <param name="round_ur">True if upper right corner is to be rounded</param>
        /// <param name="round_lr">True if lower right corner is to be rounded</param>
        /// <param name="round_ll">True if lower left corner is to be rounded</param>
        /// <returns></returns>
        private GraphicsPath MakeRoundedRect(RectangleF rect, float xradius, float yradius, bool round_ul, bool round_ur, bool round_lr, bool round_ll)
        {
            // Make a GraphicsPath to draw the rectangle.
            PointF point1, point2;
            GraphicsPath path = new GraphicsPath();

            // Upper left corner.
            if (round_ul)
            {
                if (xradius>0 && yradius>0)
                {
                    RectangleF corner = new RectangleF(
                        rect.X, rect.Y,
                        2 * xradius, 2 * yradius);
                    path.AddArc(corner, 180, 90);
                }
                point1 = new PointF(rect.X + xradius, rect.Y);
            }
            else point1 = new PointF(rect.X, rect.Y);

            // Top side.
            if (round_ur)
                point2 = new PointF(rect.Right - xradius, rect.Y);
            else
                point2 = new PointF(rect.Right, rect.Y);
            path.AddLine(point1, point2);

            // Upper right corner.
            if (round_ur)
            {
                if (xradius>0 && yradius>0)
                {
                    RectangleF corner = new RectangleF(
                        rect.Right - 2 * xradius, rect.Y,
                        2 * xradius, 2 * yradius);
                    path.AddArc(corner, 270, 90);
                }
                point1 = new PointF(rect.Right, rect.Y + yradius);
            }
            else point1 = new PointF(rect.Right, rect.Y);

            // Right side.
            if (round_lr)
                point2 = new PointF(rect.Right, rect.Bottom - yradius);
            else
                point2 = new PointF(rect.Right, rect.Bottom);
            path.AddLine(point1, point2);

            // Lower right corner.
            if (round_lr)
            {
                if (xradius > 0 && yradius>0)
                {
                    RectangleF corner = new RectangleF(
                    rect.Right - 2 * xradius,
                    rect.Bottom - 2 * yradius,
                    2 * xradius, 2 * yradius);
                    path.AddArc(corner, 0, 90);
                }
                point1 = new PointF(rect.Right - xradius, rect.Bottom);
            }
            else point1 = new PointF(rect.Right, rect.Bottom);

            // Bottom side.
            if (round_ll)
                point2 = new PointF(rect.X + xradius, rect.Bottom);
            else
                point2 = new PointF(rect.X, rect.Bottom);
            path.AddLine(point1, point2);

            // Lower left corner.
            if (round_ll)
            {
                if (xradius>0 && yradius > 0)
                {
                    RectangleF corner = new RectangleF(
                    rect.X, rect.Bottom - 2 * yradius,
                    2 * xradius, 2 * yradius);
                    path.AddArc(corner, 90, 90);
                }
                point1 = new PointF(rect.X, rect.Bottom - yradius);
            }
            else point1 = new PointF(rect.X, rect.Bottom);

            // Left side.
            if (round_ul)
                point2 = new PointF(rect.X, rect.Y + yradius);
            else
                point2 = new PointF(rect.X, rect.Y);
            path.AddLine(point1, point2);

            // Join with the start point.
            path.CloseFigure();

            return path;
        }


    }
}
