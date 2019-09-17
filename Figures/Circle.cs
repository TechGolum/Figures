using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Figures
{
    class Circle : Shape
    {
        public Circle() { }

        public Circle(float x, float y) : base()
        {
            this.x = x;
            this.y = y;
        }

        public override void Draw(Graphics g)
        {
            Brush b = new SolidBrush(color);
            g.FillEllipse(b, x - Radius, y - Radius, Radius * 2, Radius * 2);
        }
    }
}
