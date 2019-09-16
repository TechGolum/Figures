using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Figures
{
    class Square : Shape
    {
        public Square() { }

        public Square(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override void Draw(Graphics g)
        {
            Brush b = new SolidBrush(color);
            g.FillRectangle(b, x - (float)Math.Sqrt(2) / 2 * radius, y - (float)Math.Sqrt(2) / 2 * radius, (float)Math.Sqrt(2) * radius, (float)Math.Sqrt(2) * radius);
        }
    }
}
