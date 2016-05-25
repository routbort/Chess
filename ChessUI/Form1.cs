using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChessUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.lstPieceStyles.DataSource = Piece.GetStyles();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  Location loc = ChessUI.Location.ToLocation(this.txtLocation.Text);
        //    board1.DisplayPiece(this.cmbPiece.SelectedItem.ToString(), loc);
            board1.LoadFEN(this.textBox1.Text);


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.board1.SetLabelVisible(this.checkBox1.Checked);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            board1.LoadFEN(this.textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = board1.GetRandomFen();
            board1.LoadFEN(this.textBox1.Text);
         }

        private void button4_Click(object sender, EventArgs e)
        {
            board1.Redraw();

        }

        private void lstPieceStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            Renderer.Style = this.lstPieceStyles.SelectedValue.ToString();
            this.board1.Redraw();

        }
    }
}
