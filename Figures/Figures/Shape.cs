using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Figures
{
    abstract class Shape
    {
        static public Color color;
        static public int radius;
        public float x, y;
        public bool is_moving;

        static Shape()
        {
            color = Color.Green;
            radius = 40;
        }

        public Shape()
        {
            is_moving = false;
        }

        public abstract void Draw(Graphics g);

    }
}
