using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Figures
{
    abstract class Changes
    {
        static public int index_global = 0;
        protected int index_local = 0;
        public Changes() {  }
        public abstract void Undo(List<Shape> s);
        public abstract void Redo(List<Shape> s);
    }

    class Add : Changes
    {
        private Shape figure;
        private List<Shape> add = new List<Shape>();
        public Add() : base() { index_global++; }
        public Add(List<Shape> _add)
        {
            add = new List<Shape>(_add);
        }
        public override void Undo(List<Shape> s)
        {
            if (s.Count > 0 && index_local < 1)
            {
                if (s[s.Count - 1].GetType().Name == "Triangle")
                    figure = new Triangle(s[s.Count - 1].X, s[s.Count - 1].Y);
                if (s[s.Count - 1].GetType().Name == "Square")
                    figure = new Square(s[s.Count - 1].X, s[s.Count - 1].Y);
                if (s[s.Count - 1].GetType().Name == "Circle")
                    figure = new Circle(s[s.Count - 1].X, s[s.Count - 1].Y);
                s.RemoveAt(s.Count - 1);
                add.Add(figure);
                index_local++;
                index_global--;
            }
        }
        public override void Redo(List<Shape> s)
        {
            if (index_local > 0)
            {
                foreach (Shape s1 in add)
                {
                    s.Add(s1);
                }
                index_local--;
                index_global++;
            }
        }
    }

    class Del : Changes
    {
        private List<Shape> figures = new List<Shape>();
        public Del(List<Shape> del_shapes) : base()
        {
            index_global++;
            for (int i = 0; i < del_shapes.Count; i++)
            {
                if (del_shapes[i].GetType().Name == "Triangle")
                    figures.Add(new Triangle(del_shapes[i].X, del_shapes[i].Y));
                if (del_shapes[i].GetType().Name == "Square")
                    figures.Add(new Square(del_shapes[i].X, del_shapes[i].Y));
                if (del_shapes[i].GetType().Name == "Circle")
                    figures.Add(new Circle(del_shapes[i].X, del_shapes[i].Y));
            }
        }
        public override void Undo(List<Shape> s)
        {
            if (index_local < 1)
            {
                foreach(Shape s1 in figures) s.Add(s1);
                index_local++;
                index_global--;
            }
        }
        public override void Redo(List<Shape> s)
        {
            if (s.Count > 0 && index_local > 0)
            {
                foreach (Shape s1 in figures) s.Remove(s1);
                index_local--;
                index_global++;
            }
        }
    }

    class Move : Changes
    {
        private List<Point> figures = new List<Point>();
        private List<Shape> shapes;
        public Move(List<Shape> move_s) : base()
        {
            index_global++;
            foreach (Shape s in move_s)
            {
                figures.Add(new Point((int)s.X, (int)s.Y));
            }
            shapes = new List<Shape>(move_s);
        }
        public override void Undo(List<Shape> s)
        {
            if (index_local < 1)
            {
                if (shapes != s)
                {
                    foreach (Shape s1 in shapes)
                    {
                        if (!s.Contains(s1))
                        {
                            s.Add(s1);
                        }
                    }
                }
                for (int i = 0; i < s.Count; i++)
                {
                    //MessageBox.Show(s[s.IndexOf(figures[i])].Y.ToString());
                    float x = s[i].X;
                    float y = s[i].Y;
                    s[i].X = figures[i].X;
                    s[i].Y = figures[i].Y;
                    figures[i] = new Point((int)x, (int)y);
                }
                index_global--;
                index_local++;
            }
        }

        public override void Redo(List<Shape> s)
        {
            if(index_local > 0)
            {
                for (int i = 0; i < s.Count; i++)
                {
                    //MessageBox.Show(s[s.IndexOf(figures[i])].Y.ToString());
                    float x = s[i].X;
                    float y = s[i].Y;
                    s[i].X = figures[i].X;
                    s[i].Y = figures[i].Y;
                    figures[i] = new Point((int)x, (int)y);
                }
                index_global++;
                index_local--;
            }
        }
    }

    //class ColorChange : Changes
    //{
    //    public override void Undo()
    //    {

    //    }
    //}

    //class RadiusChange : Changes
    //{
    //    public override void Undo()
    //    {

    //    }
    //}
}
