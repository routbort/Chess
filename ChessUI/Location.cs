using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessUI
{
    public class Location
    {

        public int rowindex = 0;
        public int colindex = 0;

        public override string ToString()
        {
            return Convert.ToChar(colindex + 97) + (rowindex + 1).ToString();
        }


        public static Location ToLocation(string notation)
        {
            if (notation.Length > 2) 
                throw new Exception("Invalid notation");
            notation = notation.ToLower();
            int row = int.Parse(notation.Substring(1, 1))-1;
            int col =  (int)notation[0]-97;
            Location loc = new Location();
            loc.rowindex = row;
            loc.colindex = col;
            return loc;


        }


    }
}
