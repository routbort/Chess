using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ChessUI
{
    public partial class Board : UserControl
    {
        private const Boolean ENFORCE_VALID_MOVES = true;
        private const int ROWS = 8;
        private const int COLS = 8;
        private const int MOVE_BORDER = 10;
        private const float MOVE_FUDGE = 0.2f;
        private bool _Initialized = false;
        private bool _WhiteIsMoving = true;
        public bool WhiteIsMoving { get { return _WhiteIsMoving; } }
        private bool _MoveInProgress = false;
        private Square _SourceSquare;
        private int _SquareLength;
        private Square[,] _Squares = new Square[ROWS, COLS];

        private Dictionary<char, string> FENTrans = new Dictionary<char, string>();

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        internal void InitFENTransformDictionary()
        {
            FENTrans.Add('k', "K_Black");
            FENTrans.Add('q', "Q_Black");
            FENTrans.Add('b', "B_Black");
            FENTrans.Add('n', "N_Black");
            FENTrans.Add('r', "R_Black");
            FENTrans.Add('p', "P_Black");
            FENTrans.Add('K', "K_White");
            FENTrans.Add('Q', "Q_White");
            FENTrans.Add('B', "B_White");
            FENTrans.Add('N', "N_White");
            FENTrans.Add('R', "R_White");
            FENTrans.Add('P', "P_White");

        }

        private void HighlightMoves(List<Location> locations)
        {
            foreach (Location location in locations)
            {
                Debug.WriteLine(location.ToString());
                _Squares[location.rowindex, location.colindex].HighlightMove();

            }
        }

        internal ChessUI.Square.SquareColor? GetPieceColor(ChessUI.Square.PieceType sourcePiece)
        {
            if (sourcePiece == Square.PieceType.NONE) return null;

            if (FENTrans[Convert.ToChar(sourcePiece.ToString())].Contains("Black"))
                return Square.SquareColor.Black;
            if (FENTrans[Convert.ToChar(sourcePiece.ToString())].Contains("White"))
                return Square.SquareColor.White;

            return null;
        }

        internal Boolean ContainsOpposition(ChessUI.Square.PieceType sourcePiece, Square target)
        {

            if (!GetPieceColor(sourcePiece).HasValue || !GetPieceColor(target.CurrentPiece).HasValue) return false;

            return (GetPieceColor(sourcePiece) != GetPieceColor(target.CurrentPiece));

        }

        internal Boolean ContainsSame(ChessUI.Square.PieceType sourcePiece, Square target)
        {

            if (!GetPieceColor(sourcePiece).HasValue || !GetPieceColor(target.CurrentPiece).HasValue) return false;

            return (GetPieceColor(sourcePiece) == GetPieceColor(target.CurrentPiece));

        }

        internal Boolean ContainsPiece(Square target)
        {
            return (target.CurrentPiece != Square.PieceType.NONE);
        }

        internal void MoveInitiated(Square source)
        {
            _MoveInProgress = true;
            _SourceSquare = source;
            System.Diagnostics.Debug.WriteLine("move started: " + source.Label.Text);
            System.Diagnostics.Debug.WriteLine(source.CurrentPiece.ToString());


            if (source.CurrentPiece == Square.PieceType.n || source.CurrentPiece == Square.PieceType.N)
                HighlightMoves(CalculateKnightMoves(source));

            if (source.CurrentPiece == Square.PieceType.p || source.CurrentPiece == Square.PieceType.P)
                HighlightMoves(CalculatePawnMoves(source));

            if (source.CurrentPiece == Square.PieceType.r || source.CurrentPiece == Square.PieceType.R)
                HighlightMoves(CalculateRookMoves(source));

            if (source.CurrentPiece == Square.PieceType.b || source.CurrentPiece == Square.PieceType.B)
                HighlightMoves(CalculateBishopMoves(source));

            if (source.CurrentPiece == Square.PieceType.q || source.CurrentPiece == Square.PieceType.Q)
                HighlightMoves(CalculateQueenMoves(source));

            if (source.CurrentPiece == Square.PieceType.k || source.CurrentPiece == Square.PieceType.K)
                HighlightMoves(CalculateKingMoves(source));












        }

        public void LoadRandomPosition()
        {
            this.LoadFEN(GetRandomFen());
        }

        public string GetRandomFen()
        {

            string[] fens = new string[]
            {
            @"r5k1/r4p1p/1pq1pPp1/2b1P2P/2p1PP1Q/4BK2/P1p5/6RR w - - 0 35",
            @"1rb1nrk1/p4q1p/4pBp1/3pP3/2pP4/P3P1R1/1PB1Q2P/1R5K w - - 0 28",
            @"8/3n3p/5p1k/5q2/1p1Pp3/1rp1P1QP/8/6RK w - - 0 47",
            @"1k5r/pp6/2p1K3/4P1pp/5P2/P1Pb1PQ1/3r1q1P/2R4R b - - 0 35",
            @"8/pp4pk/4Qb1p/3p4/2qP4/K7/P2R2PP/3R4 b - - 0 28",
            @"8/1R2N3/6pk/7p/1p5r/5P2/5KP1/2n5 w - - 0 48",
            @"4r3/3k1p1p/p1R2pp1/qp1P4/3Q2P1/P6P/KPP5/7r w - - 0 30",
            @"2r4k/1p1b3p/p3p3/3pPp2/1P3Q2/6RK/1q6/7R w - - 0 44",
            @"4rknQ/p2R1p2/1qp1p3/2p1P1p1/2P3P1/8/P1P2PKP/3R4 w - - 0 27",
            @"4r1r1/ppp1k3/4p2Q/2P3p1/4p3/4q3/PPP5/1K3RR1 w - - 0 29",
@"Q7/5p1k/6p1/7p/4Q2P/6P1/7K/3q1q2 b - - 0 65",
@"8/1p3k2/p1p4R/2P2PP1/3PK3/P5b1/1P6/8 w - - 0 51",            
@"r3br1k/pp5p/4B1p1/4NpP1/P2Pn3/q1PQ3R/7P/3R2K1 w - - 0 28",  
@"8/5R2/1p4p1/4N1k1/2n5/P5P1/5PK1/2r5 w - - 0 46",
@"5k1r/ppq3p1/1b5p/4R3/2Bp4/1Q4P1/P4P1P/6K1 w - - 0 27",  
@"4r1k1/p1b2qp1/2p3Qp/2P2P2/3P4/8/3K3P/6R1 b - - 0 36",
@"8/1b3p1p/p7/1p2kN2/1P4P1/P1R2P1p/r2r4/2R3K1 b - - 0 37",
@"2k5/pp3pp1/2n1p3/3q4/3P1pP1/Q1PR1P2/PPr4r/3K1R2 b - - 0 29",
@"8/6p1/2r1p1k1/3p2p1/3Pn1P1/1p1KP3/1Pr1R3/1R1N4 b - - 0 39",            
@"4rr1k/pq4pp/2bRB3/8/2p2PQ1/P1B3P1/1P5P/2K5 w - - 0 1",  
@"",
@"",  
@"",
@"",
@"",
@"",            
@"",  
@"",
@"",  
@"",
@"",
@"",
@"",            
@"",  
@"",
@"",  
@"",
@"",
@"",
@"",            
@"",  
@"",
@"",  
@"",
@""};



            Random randNum = new Random();
            int index;

            do
            {
                index = randNum.Next(fens.Length);
            } while (fens[index] == "");



            return fens[index];


        }

        public void UnhighlightMoves()
        {
            for (int rowindex = 0; rowindex < ROWS; rowindex++)
                for (int colindex = 0; colindex < COLS; colindex++)
                {
                    _Squares[rowindex, colindex].UnhighlightMove();
                }
        }


        public void Redraw()
        {
            for (int rowindex = 0; rowindex < ROWS; rowindex++)
                for (int colindex = 0; colindex < COLS; colindex++)
                {

                    Square current = _Squares[rowindex,colindex];
                    current.ShowPiece(current.CurrentPiece);
                    
                }
        }

        public void LoadFEN(string fen)
        {

            if (FENTrans.Keys.Count == 0)
                InitFENTransformDictionary();


            /*
            rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            */

            this.Clear();
            string[] FenParts = fen.Split(' ');
            string positions = FenParts[0];
            string activeColor = FenParts[1];
            _WhiteIsMoving = (activeColor == "w");
            int rowindex = 7;
            int colindex = 0;
            char current;
            for (int index = 0; index < positions.Length; index++)
            {
                current = positions[index];

                int colIncrement;

                if (Int32.TryParse(current.ToString(), out colIncrement))
                {
                    colindex += colIncrement;
                }

                if (FENTrans.ContainsKey(current))
                {

                    Square.PieceType p = (Square.PieceType)Enum.Parse(typeof(Square.PieceType), current.ToString(), false);
                    _Squares[rowindex, colindex].ShowPiece(p);

                    colindex++;
                }

                if (current == '/')
                {
                    colindex = 0;
                    rowindex--;
                }



            }





        }

        public void Clear()
        {
            for (int rowindex = 0; rowindex < ROWS; rowindex++)
                for (int colindex = 0; colindex < COLS; colindex++)
                    _Squares[rowindex, colindex].ClearPiece();

        }

        internal void MoveEnded(Square destination)
        {
            if (_MoveInProgress)
            {
                _MoveInProgress = false;
                _SourceSquare.Revert();
                System.Diagnostics.Debug.WriteLine("move ended: " + destination.Label.Text);

                Point PanelLoc = this.pnlMain.PointToClient(MousePosition);
                int row = 7 - PanelLoc.Y / _SquareLength;
                int col = PanelLoc.X / _SquareLength;
                Debug.WriteLine("row:" + row.ToString() + ", c: " + col.ToString());
                if (row >= 0 && col >= 0 && row < ROWS && col < COLS)
                {
                    Point SquareLoc = _Squares[row, col].PointToClient(MousePosition);
                    int move_border = (int)(_SquareLength * MOVE_FUDGE);
                    if (SquareLoc.X >= move_border && SquareLoc.Y >= move_border && SquareLoc.X <= _SquareLength - move_border && SquareLoc.Y <= _SquareLength - move_border)
                    {
                        System.Diagnostics.Debug.WriteLine("move true ended: " + _Squares[row, col].Label.Text);
                        Square _destSquare = _Squares[row, col];
                        if (_SourceSquare.CurrentPiece != Square.PieceType.NONE && _destSquare.BackColor == Color.BlueViolet)
                        {
                            _Squares[row, col].ShowPiece(_SourceSquare.CurrentPiece);
                            if (!_SourceSquare.Equals(_destSquare))
                            {
                                _WhiteIsMoving = !_WhiteIsMoving;
                                Debug.WriteLine("Current mover is " + (_WhiteIsMoving ? "white" : "black"));
                                _SourceSquare.ClearPiece();
                            }

                            _MoveInProgress = false;
                            _SourceSquare = null;
                        }
                    }
                }
            }
            UnhighlightMoves();

        }

        public void DisplayPiece(Square.PieceType piece, Location location)
        {
            _Squares[location.rowindex, location.colindex].ShowPiece(piece);
        }

        private Form GetParentForm(Control parent)
        {
            Form form = parent as Form;
            if (form != null)
                return form;
            return parent != null ? GetParentForm(parent.Parent) : null;
        }

        public void SetLabelVisible(bool visible)
        {
            for (int rowindex = 0; rowindex < ROWS; rowindex++)
                for (int colindex = 0; colindex < COLS; colindex++)
                {
                    _Squares[rowindex, colindex].Label.Visible = visible;
                }

        }

        public Board()
        {
            InitializeComponent();

            for (int rowindex = 0; rowindex < ROWS; rowindex++)
                for (int colindex = 0; colindex < COLS; colindex++)
                {

                    Square current = new Square(this);
                    current.Color = ((rowindex + colindex) % 2 == 0) ? Square.SquareColor.Black : Square.SquareColor.White;
                    current.Label.Text = Convert.ToChar(colindex + 97) + (rowindex + 1).ToString();
                    current.location.rowindex = rowindex;
                    current.location.colindex = colindex;
                    this.pnlMain.Controls.Add(current);
                    _Squares[rowindex, colindex] = current;

                }
            _Initialized = true;
            ResizeBoard(false);

        }

        private void ResizePanel()
        {

            //enforce appropriate dimensions


            int squarewidth = this.Width / COLS;
            int squareheight = this.Height / ROWS;

            //we'll use whichever is smaller, rectangles are ugly so we want to enfore squares

            _SquareLength = Math.Min(squarewidth, squareheight);

            //System.Diagnostics.Debug.WriteLine("");
            //System.Diagnostics.Debug.WriteLine("");
            //System.Diagnostics.Debug.WriteLine("w,h:" + this.Width.ToString() + "," + this.Height.ToString());
            //System.Diagnostics.Debug.WriteLine("bw,bh:" + squarewidth.ToString() + "," + squareheight.ToString());
            //System.Diagnostics.Debug.WriteLine("sl:" + _squarelength.ToString());

            if (_SquareLength * COLS + 2 > this.Width || _SquareLength * ROWS + 2 > this.Height)
            {
                //System.Diagnostics.Debug.WriteLine("decreasing sl by 1 for best fit");
                _SquareLength--;
            }

            this.pnlMain.Width = _SquareLength * COLS + 2;
            this.pnlMain.Height = _SquareLength * ROWS + 2;

            //       System.Diagnostics.Debug.WriteLine("pw,ph:" + this.pnlMain.Width.ToString() + "," + this.pnlMain.Height.ToString());


            this.pnlMain.Left = (this.Width - this.pnlMain.Width) / 2;
            this.pnlMain.Top = (this.Height - this.pnlMain.Height) / 2;


        }

        public void ResizeBoard(bool RecenterPanel)
        {

            ResizePanel();

            for (int rowindex = 0; rowindex < ROWS; rowindex++)
                for (int colindex = 0; colindex < COLS; colindex++)
                {
                    _Squares[rowindex, colindex].Size = new System.Drawing.Size(_SquareLength, _SquareLength);
                    _Squares[rowindex, colindex].Location = new System.Drawing.Point(colindex * _SquareLength, this.pnlMain.Height - _SquareLength - rowindex * _SquareLength - 2);
                }

        }

        private void Board_Resize(object sender, EventArgs e)
        {
            if (_Initialized)
                ResizeBoard(true);
        }

        private void pnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("mm panel " + this.PointToClient(MousePosition).ToString());
            if (e.Button == 0) return;


        }

        private Boolean WithinBounds(Location location)
        {
            return (location.rowindex >= 0 && location.colindex >= 0 && location.rowindex < ROWS && location.colindex < COLS);
        }

        private List<Location> CalculateRookMoves(Square source)
        {
            List<Location> AvailableMoves = new List<Location>();

            Location current = source.location;

            //Rooks can move left or right, up or down, until they capture or are blocked

            Location proposed;

            for (int i = 1; i < 7; i++)
            {
                proposed = new Location();
                proposed.colindex = current.colindex;
                proposed.rowindex = current.rowindex + i;
                if (!WithinBounds(proposed) || ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
                AvailableMoves.Add(proposed);
                if (ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
            }

            for (int i = 1; i < 7; i++)
            {
                proposed = new Location();
                proposed.colindex = current.colindex;
                proposed.rowindex = current.rowindex - i;
                if (!WithinBounds(proposed) || ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
                AvailableMoves.Add(proposed);
                if (ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
            }

            for (int i = 1; i < 7; i++)
            {
                proposed = new Location();
                proposed.rowindex = current.rowindex;
                proposed.colindex = current.colindex + i;
                if (!WithinBounds(proposed) || ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
                AvailableMoves.Add(proposed);
                if (ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
            }

            for (int i = 1; i < 7; i++)
            {
                proposed = new Location();
                proposed.rowindex = current.rowindex;
                proposed.colindex = current.colindex - i;
                if (!WithinBounds(proposed) || ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
                AvailableMoves.Add(proposed);
                if (ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
            }






            return AvailableMoves;

        }

        private List<Location> CalculateBishopMoves(Square source)
        {
            List<Location> AvailableMoves = new List<Location>();

            Location current = source.location;

            //Bishops can move diagonally until they capture or are blocked

            Location proposed;

            for (int i = 1; i < 7; i++)
            {
                proposed = new Location();
                proposed.colindex = current.colindex + i;
                proposed.rowindex = current.rowindex + i;
                if (!WithinBounds(proposed) || ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
                AvailableMoves.Add(proposed);
                if (ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
            }

            for (int i = 1; i < 7; i++)
            {
                proposed = new Location();
                proposed.colindex = current.colindex + i;
                proposed.rowindex = current.rowindex - i;
                if (!WithinBounds(proposed) || ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
                AvailableMoves.Add(proposed);
                if (ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
            }

            for (int i = 1; i < 7; i++)
            {
                proposed = new Location();
                proposed.colindex = current.colindex - i;
                proposed.rowindex = current.rowindex + i;
                if (!WithinBounds(proposed) || ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
                AvailableMoves.Add(proposed);
                if (ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
            }

            for (int i = 1; i < 7; i++)
            {
                proposed = new Location();
                proposed.colindex = current.colindex - i;
                proposed.rowindex = current.rowindex - i;
                if (!WithinBounds(proposed) || ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
                AvailableMoves.Add(proposed);
                if (ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex])) break;
            }

            return AvailableMoves;

        }

        private List<Location> CalculateQueenMoves(Square source)
        {
            //Queen can move as a rook or bishop
            List<Location> merged = new List<Location>(CalculateBishopMoves(source));
            merged.AddRange(CalculateRookMoves(source));
            return merged;
        }

        private List<Location> CalculatePawnMoves(Square source)
        {
            List<Location> AvailableMoves = new List<Location>();

            Location current = source.location;

            //Pawns move vertically by +1 or -1 depending on color, if not blocked by any other piece

            Location proposed;

            proposed = new Location();
            proposed.colindex = current.colindex;
            if (GetPieceColor(source.CurrentPiece) == Square.SquareColor.White)
                proposed.rowindex = current.rowindex + 1;
            if (GetPieceColor(source.CurrentPiece) == Square.SquareColor.Black)
                proposed.rowindex = current.rowindex - 1;
            if (proposed.rowindex != current.rowindex && WithinBounds(proposed) && !ContainsPiece(_Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);


            //Pawns can also move vertically by +2 or -2 if on their home square, if not blocked from doing so

            if (!ContainsPiece(_Squares[proposed.rowindex, proposed.colindex]) && (current.rowindex == 1 || current.rowindex == 6))
            {
                proposed = new Location();
                proposed.colindex = current.colindex;
                if (GetPieceColor(source.CurrentPiece) == Square.SquareColor.White)
                    proposed.rowindex = current.rowindex + 2;
                if (GetPieceColor(source.CurrentPiece) == Square.SquareColor.Black)
                    proposed.rowindex = current.rowindex - 2;
                if (proposed.rowindex != current.rowindex && WithinBounds(proposed) && !ContainsPiece(_Squares[proposed.rowindex, proposed.colindex]))
                    AvailableMoves.Add(proposed);
            }


            //Pawns can capture diagonally

            proposed = new Location();

            proposed.colindex = current.colindex + 1;
            if (GetPieceColor(source.CurrentPiece) == Square.SquareColor.White)
                proposed.rowindex = current.rowindex + 1;
            if (GetPieceColor(source.CurrentPiece) == Square.SquareColor.Black)
                proposed.rowindex = current.rowindex - 1;
            if (proposed.rowindex != current.rowindex
                && WithinBounds(proposed)
                && ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();

            proposed.colindex = current.colindex - 1;
            if (GetPieceColor(source.CurrentPiece) == Square.SquareColor.White)
                proposed.rowindex = current.rowindex + 1;
            if (GetPieceColor(source.CurrentPiece) == Square.SquareColor.Black)
                proposed.rowindex = current.rowindex - 1;
            if (proposed.rowindex != current.rowindex
                && WithinBounds(proposed)
                && ContainsOpposition(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);


            return AvailableMoves;

        }

        private List<Location> CalculateKnightMoves(Square source)
        {
            List<Location> AvailableMoves = new List<Location>();
            Location current = source.location;
            Location proposed;

            proposed = new Location();
            proposed.colindex = current.colindex + 2;
            proposed.rowindex = current.rowindex + 1;
            if (WithinBounds(proposed) &&
                !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex + 2;
            proposed.rowindex = current.rowindex - 1;
            if (WithinBounds(proposed) &&
                  !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex - 2;
            proposed.rowindex = current.rowindex + 1;
            if (WithinBounds(proposed) &&
                  !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex - 2;
            proposed.rowindex = current.rowindex - 1;
            if (WithinBounds(proposed) &&
                  !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex + 1;
            proposed.rowindex = current.rowindex + 2;
            if (WithinBounds(proposed) &&
                  !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex + 1;
            proposed.rowindex = current.rowindex - 2;
            if (WithinBounds(proposed) &&
                  !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex - 1;
            proposed.rowindex = current.rowindex + 2;
            if (WithinBounds(proposed) &&
                  !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex - 1;
            proposed.rowindex = current.rowindex - 2;
            if (WithinBounds(proposed) &&
                  !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            return AvailableMoves;
        }

        private List<Location> CalculateKingMoves(Square source)
        {
            List<Location> AvailableMoves = new List<Location>();

            Location current = source.location;

            //Pawns move 1 square in ANY direction unless blocked

            Location proposed;
            proposed = new Location();
            proposed.colindex = current.colindex;
            proposed.rowindex = current.rowindex + 1;
            if (WithinBounds(proposed) && !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex;
            proposed.rowindex = current.rowindex - 1;
            if (WithinBounds(proposed) && !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex + 1;
            proposed.rowindex = current.rowindex + 1;
            if (WithinBounds(proposed) && !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex + 1;
            proposed.rowindex = current.rowindex - 1;
            if (WithinBounds(proposed) && !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex + 1;
            proposed.rowindex = current.rowindex;
            if (WithinBounds(proposed) && !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex - 1;
            proposed.rowindex = current.rowindex + 1;
            if (WithinBounds(proposed) && !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex - 1;
            proposed.rowindex = current.rowindex - 1;
            if (WithinBounds(proposed) && !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            proposed = new Location();
            proposed.colindex = current.colindex - 1;
            proposed.rowindex = current.rowindex;
            if (WithinBounds(proposed) && !ContainsSame(source.CurrentPiece, _Squares[proposed.rowindex, proposed.colindex]))
                AvailableMoves.Add(proposed);

            return AvailableMoves;



        }

    }
}
