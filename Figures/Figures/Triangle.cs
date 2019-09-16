using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Figures
{
    class Triangle : Shape
    {
        public Triangle() { }

        public Triangle(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override void Draw(Graphics g)
        {
            Brush b = new SolidBrush(color);
            Point[] p = {
                    new Point((int)x, (int)y - radius),
                    new Point((int)x - Convert.ToInt32(radius * Math.Cos(3)), (int)y + Convert.ToInt32(radius * Math.Sin(3))),
                    new Point((int)x + Convert.ToInt32(radius * Math.Cos(3)), (int)y + Convert.ToInt32(radius * Math.Sin(3)))
                };
            g.FillPolygon(b, p);
        }
    }
}
