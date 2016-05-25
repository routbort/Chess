using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;


namespace ChessUI
{
    public static class Renderer
    {
         static Renderer() { Style = "Default"; }


        public static string Style { get; set; }

        private static Dictionary<string, Dictionary<string,Bitmap>> _icons = new Dictionary<string,Dictionary<string,Bitmap>>();

        public static Bitmap GetIcon( string name)
        {
            if (_icons.ContainsKey (Style) && _icons[Style].ContainsKey(name)) return _icons [Style][name];

            if (!_icons.ContainsKey(Style)) _icons[Style] = new Dictionary<string,Bitmap>();


            Bitmap icon = null;
            try
            {
                string filename =  Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),"Pieces",Style,name + ".png");
                icon =  new Bitmap(filename);

               // icon = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ChessUI.Pieces.Default." + name + ".png"));

            }
            catch
            {

                //  icon = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ChessUI.Pieces.Default.test.gif"));

            }


            if (icon != null)
                _icons[Style][name] = icon;

            return icon ?? null;



        }




    }
}
