using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessUI
{
    public class Piece
    {

        public enum Type
        {
            pawn = 1,
            knight = 2,
            king = 3,
            bishop = 5,
            rook = 6,
            queen = 7
        }

        public static List<string> GetStyles()
        {

            string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Pieces");
            List<string> output = new List<string>();
            foreach (string directory in System.IO.Directory.GetDirectories(path))
                output.Add(System.IO.Path.GetFileName(directory));
            return output;

        }



    }



}
