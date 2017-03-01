using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Core
{
    public class BitmapScreen: IScreen
    {
        //implememtation of a screen as a bitmap image.
        public object screenRepresentation
        {
            get { return screen; }
            set { screen = value as Bitmap; }

        }

        Bitmap screen;

        
    }
}
