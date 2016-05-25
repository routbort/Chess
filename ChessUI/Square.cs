using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ChessUI
{
    public partial class Square : UserControl
    {
        public enum SquareColor
        { Black, White }

        public enum PieceType
        {
            NONE,
            p, n, k, b, r, q,
            P, N, K, B, R, Q
        }

        public Square()
        {
            InitializeComponent();
            _iconbox = new PictureBox();
            _iconbox.Dock = DockStyle.Fill;
            _iconbox.MouseDown += new MouseEventHandler(_iconbox_MouseDown);
            _iconbox.MouseUp += new MouseEventHandler(_iconbox_MouseUp);
         //   _iconbox.MouseMove += new MouseEventHandler(_iconbox_MouseMove);
            _iconbox.MouseEnter += new EventHandler(_iconbox_MouseEnter);
            this.Controls.Add(_iconbox);

        }

        void _iconbox_MouseEnter(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Mouse enter" + this.location.colindex.ToString()   + "," + this.location.rowindex.ToString());
            if (this.ValidDropLocation || Control.MouseButtons== System.Windows.Forms.MouseButtons.Left)
                this.BackColor = System.Drawing.Color.Green;
        }

        void _iconbox_MouseMove(object sender, MouseEventArgs e)
        {

            System.Diagnostics.Debug.WriteLine("mouse move");
            //aklsdfa 

            //if (this.ValidDropLocation && Control.MouseButtons == System.Windows.Forms.MouseButtons.Left)
            if ( Control.MouseButtons == System.Windows.Forms.MouseButtons.Left)
         
                this.BackColor = System.Drawing.Color.Green;

        }

        public Location location = new Location();
        static Dictionary<PieceType, string> Trans = new Dictionary<PieceType, string>();

        public bool ValidDropLocation { get; set; }


        public static string TranslateEnumToFilename(PieceType piece)
        {
            if (Trans.Count == 0)
            {
                Trans.Add(PieceType.p, "P_Black");
                Trans.Add(PieceType.r, "R_Black");
                Trans.Add(PieceType.n, "N_Black");
                Trans.Add(PieceType.b, "B_Black");
                Trans.Add(PieceType.q, "Q_Black");
                Trans.Add(PieceType.k, "K_Black");
                Trans.Add(PieceType.P, "P_White");
                Trans.Add(PieceType.R, "R_White");
                Trans.Add(PieceType.N, "N_White");
                Trans.Add(PieceType.B, "B_White");
                Trans.Add(PieceType.Q, "Q_White");
                Trans.Add(PieceType.K, "K_White");
            }

            return Trans[piece];
        }

        public Square(Board board)
            : this()
        {
            _board = board;
        }

        SquareColor _color;
        PictureBox _iconbox;
        Board _board;

        public PictureBox Icon { get { return _iconbox; } }
        public PieceType CurrentPiece { get { return _currentpiecetype; } }

        string _currentpiece = "";
        PieceType _currentpiecetype;

        public void ClearPiece()
        {
            _iconbox.Image = null;
            _currentpiece = "";
            _currentpiecetype = PieceType.NONE;
        }

        public void ShowPiece(PieceType piece)
        {


            if (piece == PieceType.p && this.location.rowindex == 0)
            {
                piece = PieceType.q;
            }

            if (piece == PieceType.P && this.location.rowindex == 7)
            {
                piece = PieceType.Q;
            }

            _currentpiecetype = piece;

            if (piece == PieceType.NONE)
                ClearPiece();
            else
                ShowPiece(TranslateEnumToFilename(piece));

        }

        public void ShowPiece(string name)
        {
            Bitmap icon = Renderer.GetIcon(name);
            if (icon != null)
            {
                _currentpiece = name;
                _iconbox.Image = icon;
                _iconbox.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        void _iconbox_MouseUp(object sender, MouseEventArgs e)
        {
            HandleMouseUp();
        }

        void _iconbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_currentpiece != "")
            {
                if ((_board.WhiteIsMoving && _currentpiece.Contains("White")) ||
                    (!_board.WhiteIsMoving && _currentpiece.Contains("Black")))
                {
                    System.Diagnostics.Debug.WriteLine("MD icon at: " + this.PointToClient(MousePosition).ToString());
                    Point clickspot = this.PointToClient(MousePosition);
                    this.Cursor = CreateCursor((Bitmap)resizeImage(_iconbox.Image, _iconbox.Size), (int)(clickspot.X * ((double)_iconbox.Image.Width / _iconbox.Image.Height)), clickspot.Y);
                    this._iconbox.Visible = false;
                    _board.MoveInitiated(this);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Not your turn");
                }


            }



        }




        public void Revert()
        {
            if (this._iconbox != null)
                this._iconbox.Visible = true;
        }

        public SquareColor Color
        {

            get { return _color; }

            set
            {
                _color = value;
                UnhighlightMove();
            }


        }

        public Label Label { get { return this.label1; } }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Entered lbl: " + this.label1.Text);
        }

        public void HighlightMove()
        {
            this.BackColor = System.Drawing.Color.BlueViolet;
            this.ValidDropLocation = true;

        }

        public void UnhighlightMove()
        {
            this.BackColor = (_color == SquareColor.Black ? System.Drawing.Color.DarkGoldenrod : System.Drawing.Color.LightGray);
            this.ValidDropLocation = false;
        }

        private void Square_MouseMove(object sender, MouseEventArgs e)
        {
            //  System.Diagnostics.Debug.WriteLine("mm square pre");
            //  if (MouseMove != null)
            //         MouseMove(sender, e);
        }

        private void Square_MouseEnter(object sender, EventArgs e)
        {

              System.Diagnostics.Debug.WriteLine("enter square");

        }

        private void Square_MouseDown(object sender, MouseEventArgs e)
        {


            System.Diagnostics.Debug.WriteLine("mouse down");
        }

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IconInfo tmp = new IconInfo();
            GetIconInfo(bmp.GetHicon(), ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            return new Cursor(CreateIconIndirect(ref tmp));
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            if (imgToResize == null) return null;
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        private void Square_MouseUp(object sender, MouseEventArgs e)
        {
            HandleMouseUp();
        }

        private void HandleMouseUp()
        {
            this.Cursor = Cursors.Default;
            this._board.MoveEnded(this);

        }


    }
}
