using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Figures
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        static List <Shape> shapes = new List<Shape>();
        static float dxm, dym;

        #region Functions
        static int findFarthestPoints()
        {
            double distance = -1;
            int number1 = 0;
            for (int i = 0; i < shapes.Count(); i++)
            {
                for (int j = i + 1; j < shapes.Count(); j++)
                {
                    if (Math.Sqrt(Math.Pow(shapes[i].x - shapes[j].x, 2) + Math.Pow(shapes[i].y - shapes[j].y, 2)) > distance)
                    {
                        distance = Math.Sqrt(Math.Pow(shapes[i].x - shapes[j].x, 2) + Math.Pow(shapes[i].y - shapes[j].y, 2));
                        number1 = i;
                    }
                }
            }
            return number1;
        }

        static List<int> findFigurePoints()
        {
            bool ok = true;
            bool up = false, down = false;
            float k;
            int number1 = findFarthestPoints();
            List<int> figurePoints = new List<int>();

            figurePoints.Add(number1);

            for (int i = 0; i < shapes.Count(); i++)
            {
                up = false; down = false;
                if (figurePoints.Contains(i))
                {
                    continue;
                }
                for (int i1 = 0; i1 < shapes.Count(); i1++)
                {
                    ok = true;
                    if (i1 != number1 && i != i1)
                    {

                        k = (shapes[number1].y - shapes[i].y) / (shapes[number1].x - shapes[i].x);
                        if (k * shapes[i1].x + shapes[i].y - k * shapes[i].x > shapes[i1].y)
                        {
                            up = true;
                            if (down) { ok = false; break; }

                        }
                        else
                        {
                            down = true;
                            if (up) { ok = false; break; }
                        }
                    }
                }
                if (ok)
                {
                    figurePoints.Add(i);
                    number1 = i;
                    if (number1 == findFarthestPoints()) { break; }
                    i = -1;
                }
            }

            return figurePoints;
        }

        static void drawLines(Graphics e)
        {
            Brush b = new SolidBrush(Color.Black);
            Pen p1 = new Pen(b);
            List<int> figurePoints1 = findFigurePoints();
            for (int i = 0; i < figurePoints1.Count(); i++)
            {
                if (i == figurePoints1.Count() - 1)
                {
                    e.DrawLine(p1, shapes[figurePoints1[0]].x, shapes[figurePoints1[0]].y, shapes[figurePoints1[i]].x, shapes[figurePoints1[i]].y);
                }
                else
                {
                    e.DrawLine(p1, shapes[figurePoints1[i]].x, shapes[figurePoints1[i]].y, shapes[figurePoints1[i + 1]].x, shapes[figurePoints1[i + 1]].y);
                }
            }

        }

        static bool isClickedInArea(MouseEventArgs e)
        {
            bool up = false, down = false, left = false, right = false;
            for(int i = 0; i < shapes.Count(); i++)
            {
                if (e.X < shapes[i].x) right = true;
                if (e.X > shapes[i].x) left = true;
                if (e.Y < shapes[i].y) up = true;
                if (e.Y > shapes[i].y) down = true;
            }
            return right && left && up && down;
        }

        static void dragDropFigure(MouseEventArgs e)
        {
            if (isClickedInArea(e))
            {
                for (int i = 0; i < shapes.Count(); i++)
                {
                    shapes[i].x += e.X - dxm;
                    shapes[i].y += e.Y - dym;
                }
            }
        }

        static bool findClickedShape(MouseEventArgs e, Shape s)
        {
            if (e.X >= s.x - Shape.radius && e.X <= s.x + Shape.radius && e.Y >= s.y - Shape.radius && e.Y <= s.y + Shape.radius) return true;
            return false;
        }

        static void deleteInnerPoints()
        {
            List<Shape> s = new List<Shape>();
            List<int> figurePoints = findFigurePoints();
            for (int i = 0; i < shapes.Count(); i++)
            {
                if (figurePoints.Contains(i) || shapes[i].is_moving)
                {
                    s.Add(shapes[i]);
                }
            }
            shapes = s;
        }

        void runPoints()
        {
            Random r = new Random();
            Action action = () => Refresh();
            while (runs)
            {
                for (int i = 0; i < shapes.Count(); i++)
                {
                    shapes[i].x += r.Next(-1, 2);
                    shapes[i].y += r.Next(-1, 2);
                }
                deleteInnerPoints();
                try
                {
                    if (InvokeRequired) Invoke(action);
                    else action();
                }
                catch
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        Shape createFigure(MouseEventArgs e)
        {
            if (type_of_figure.GetType().Name == "Triangle") return new Triangle(e.X, e.Y);
            if (type_of_figure.GetType().Name == "Square") return new Square(e.X, e.Y);
            if (type_of_figure.GetType().Name == "Circle") return new Circle(e.X, e.Y);
            return null;
        }
        #endregion

        Shape type_of_figure = new Triangle();

        bool isMouseDown = false;
        bool isMouseMove = false;
        bool figureIsMoving = false;

        #region WindowMouse
        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            for (int i = shapes.Count() - 1; i >= 0 && shapes.Count() > 0; i--)
            {
                if (findClickedShape(e, shapes[i]))
                {
                    shapes[i].is_moving = true;
                }
            }
            dxm = e.X;
            dym = e.Y;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseMove = true;

                for (int i = 0; i < shapes.Count(); i++)
                {
                    if (shapes[i].is_moving)
                    {
                        shapes[i].x += e.X - dxm;
                        shapes[i].y += e.Y - dym;
                        figureIsMoving = true;
                    }
                }

                if(!figureIsMoving) dragDropFigure(e);

                dxm = e.X;
                dym = e.Y;
                Invalidate();
            }

        }

        private void Window_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            if (!isMouseMove || shapes.Count() == 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    shapes.Add(createFigure(e));
                }
                else
                {
                    for (int i = shapes.Count() - 1; i >= 0 && shapes.Count() > 0; i--)
                    {
                        if (findClickedShape(e, shapes[i]))
                        {
                            shapes.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            deleteInnerPoints();

            for(int i = 0; i < shapes.Count(); i++)
            {
                shapes[i].is_moving = false;
            }
            Invalidate();


            isMouseMove = false;
            figureIsMoving = false;
        }
        #endregion

        #region ToolStrip
        private void TriangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            type_of_figure = new Triangle();
        }

        private void SquareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            type_of_figure = new Square();
        }

        private void CircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            type_of_figure = new Circle();
        }
        #endregion

        static bool runs = false;

        #region PlayStop
        private void Play_Click(object sender, EventArgs e)
        {
            if (!runs && shapes.Count() > 0)
            {
                var runThread = new System.Threading.Thread(runPoints);
                runThread.Start();
                Invalidate();
                runs = true;
            }
            
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            runs = false;
        }
        #endregion

        private void ColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Shape.color = colorDialog1.Color;
            Invalidate();
        }

        private void Window_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            if (shapes.Count() > 2) drawLines(e.Graphics);
            for(int i = 0; i < shapes.Count(); i++)
            {
                shapes[i].Draw(e.Graphics);
            }
        }
    }
}