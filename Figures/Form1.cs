using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace Figures
{
    public partial class Window : Form
    {
        public Window()
        {
            Shape s;
            InitializeComponent();
            shapes = new List<Shape>();
            type_of_figure = new Triangle();
            Shape.RadiusChanged += new EventHandler(RadiusChanged);

            string str1 = "";
            try
            {
                for (int i = 0; i < Plugins.getClasses().Count; i++)
                {
                    for (int j = 0; j < Plugins.getClasses()[i].Length; j++)
                        str1 += "Class: " + Plugins.getClasses()[i][j].Name + "; Library: " + Plugins.getLibs()[i] + "\n";
                }
            }
            catch { MessageBox.Show(str1); }
        }

        static string filename;
        static string format = ".poo";
        static bool is_changed = false;
        static List<Shape> shapes;
        List<Changes> changes = new List<Changes>();
        enum alg { Def, Jar};
        alg MyAlgorithm = alg.Def;

        #region Functions
        static void defAlg(Graphics e)
        {
            foreach (Shape s in shapes) s.points = false;
            for (int i = 0; i < shapes.Count - 1; i++)
            {
                for (int j = i + 1; j < shapes.Count; j++)
                {
                    bool sign = true;
                    bool t = true;
                    for (int b = 0, k = 0; b < shapes.Count; b++)
                    {
                        if (b != i && b != j)
                        {
                            k++;
                            if (k > 1)
                            {
                                if ((shapes[i].X - shapes[b].X) * (shapes[j].Y - shapes[i].Y) - (shapes[j].X - shapes[i].X) * (shapes[i].Y - shapes[b].Y) <= 0 && sign == true)
                                {
                                    t = false;
                                    break;
                                }
                                if ((shapes[i].X - shapes[b].X) * (shapes[j].Y - shapes[i].Y) - (shapes[j].X - shapes[i].X) * (shapes[i].Y - shapes[b].Y) > 0 && sign == false)
                                {
                                    t = false;
                                    break;
                                }
                            }
                            else
                            {
                                if ((shapes[i].X - shapes[b].X) * (shapes[j].Y - shapes[i].Y) - (shapes[j].X - shapes[i].X) * (shapes[i].Y - shapes[b].Y) <= 0)
                                {
                                    sign = false;
                                }
                                else
                                {
                                    sign = true;
                                }
                            }
                        }
                    }
                    if (t == true)
                    {
                        shapes[i].points = true;
                        shapes[j].points = true;
                    }
                }
            }
        }

        static int findMostLeftPoint(List<Shape> s)
        {
            double x = double.MaxValue;
            int number = 0;
            for (int i = 0; i < s.Count(); i++)
            {
                if (s[i].X < x)
                {
                    x = s[i].X;
                    number = i;
                }
            }
            return number;
        }

        static List<int> findFigurePoints(List<Shape> s)
        {
            bool isItPoint = true;
            bool up = false, down = false;
            float k;
            int number1 = findMostLeftPoint(s);
            List<int> figurePoints = new List<int>();

            figurePoints.Add(number1);

            for (int i = 0; i < s.Count(); i++)
            {
                up = false; down = false;
                if (figurePoints.Contains(i)) continue;

                for (int i1 = 0; i1 < s.Count(); i1++)
                {
                    isItPoint = true;
                    if (i1 != number1 && i != i1)
                    {
                        k = (s[number1].Y - s[i].Y) / (s[number1].X - s[i].X);
                        if (s[number1].X == s[i].X) k = 0;
                        if (k * s[i1].X + s[i].Y - k * s[i].X > s[i1].Y)
                        {
                            up = true;
                            if (down) { isItPoint = false; break; }

                        }
                        else
                        {
                            down = true;
                            if (up) { isItPoint = false; break; }
                        }
                    }
                }
                if (isItPoint)
                {
                    figurePoints.Add(i);
                    number1 = i;
                    if (number1 == findMostLeftPoint(s)) { break; }
                    i = -1;
                }
            }

            return figurePoints;
        }

        static void drawLines(Graphics e, List<int> figurePoints)
        {
            Brush b = new SolidBrush(Shape.shell_color);
            Point[] p = new Point[3];
            p[0].X = (int)shapes[0].X;
            p[0].Y = (int)shapes[0].Y;
            for (int i = 1; i < shapes.Count - 1; i++)
            {
                for (int j = i + 1; j < shapes.Count; j++)
                {
                    p[1].X = (int)shapes[i].X;
                    p[1].Y = (int)shapes[i].Y;
                    p[2].X = (int)shapes[j].X;
                    p[2].Y = (int)shapes[j].Y;
                    e.FillPolygon(new SolidBrush(Shape.shell_color), p);
                }
            }
        }

        static bool isClickedInArea(MouseEventArgs e)
        {
            if (shapes.Count > 2)
            {
                List<Shape> s = new List<Shape>(shapes);
                Shape sample = new Triangle(e.X, e.Y);
                s.Add(sample);
                if (findFigurePoints(s).Contains(s.Count - 1)) return false;
                return true;
            }
            return false;
        }

        static void dragDropFigure(MouseEventArgs e)
        {
            if (shapes.Count() > 2 && isClickedInArea(e))
            {
                for (int i = 0; i < shapes.Count(); i++)
                {
                    shapes[i].X += e.X - shapes[i].X - shapes[i].rx;
                    shapes[i].Y += e.Y - shapes[i].Y - shapes[i].ry;
                }
            }
        }

        static List<Shape> deleteInnerPoints(List<Shape> s)
        {
            List<Shape> _shapes = new List<Shape>();
            for (int i = 0; i < s.Count; i++)
            {
                if (findFigurePoints(s).Contains(i))
                {
                    _shapes.Add(s[i]);
                }
            }
            return _shapes;
        }

        Shape createFigure(MouseEventArgs e)
        {
            if (type_of_figure.GetType().Name == "Triangle") return new Triangle(e.X, e.Y);
            if (type_of_figure.GetType().Name == "Square") return new Square(e.X, e.Y);
            if (type_of_figure.GetType().Name == "Circle") return new Circle(e.X, e.Y);
            return null;
        }
        # endregion

        Shape type_of_figure;

        #region WindowMouse

        bool isMouseDown = false;
        bool isMouseMove = false;
        bool figuresMoving;
        bool figuresClicked;
        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            figuresMoving = false;
            figuresClicked = false;
            List<Shape> move_s = new List<Shape>();
            is_changed = true;

            for (int i = shapes.Count() - 1; i >= 0 && shapes.Count() > 0; i--)
            {
                shapes[i].rx = e.X - shapes[i].X;
                shapes[i].ry = e.Y - shapes[i].Y;
                if (shapes[i].findClickedShape(e))
                {
                    shapes[i].is_moving = true;
                    figuresClicked = true;
                    move_s.Add(shapes[i]);
                }
            }

            if(!figuresClicked)
            {
                if (e.Button == MouseButtons.Left && !isClickedInArea(e))
                {
                    List<Shape> s = new List<Shape>(shapes);
                    List<Shape> add = new List<Shape>();
                    shapes.Add(createFigure(e));
                    if(s.Count + 1 == findFigurePoints(shapes).Count) changes.Insert(Changes.index_global, new Add());
                    else
                    {
                        foreach(Shape s1 in s)
                        {
                            if (!shapes.Contains(s1))
                            {
                                MessageBox.Show("");
                                add.Add(s1);
                            }
                        }
                        changes.Insert(Changes.index_global, new Add(add));
                    }
                    shapes[shapes.Count - 1].is_moving = true;
                }
                Refresh();
            }

            //if (isClickedInArea(e) || move_s.Count > 0) changes.Insert(Changes.index_global, new Move(shapes));
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
                        shapes[i].X += e.X - shapes[i].X - shapes[i].rx;
                        shapes[i].Y += e.Y - shapes[i].Y - shapes[i].ry;
                        figuresMoving = true;
                        
                    }
                }

                if (!figuresMoving)
                {
                    dragDropFigure(e);
                }
                Refresh();
            }

        }

        private void Window_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            List<Shape> del_fig = new List<Shape>();

            if (!isMouseMove || shapes.Count() == 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    for (int i = shapes.Count() - 1; i >= 0 && shapes.Count() > 0; i--)
                    {
                        if (shapes[i].findClickedShape(e))
                        {
                            del_fig.Add(shapes[i]);
                            
                            shapes.RemoveAt(i);
                            break;
                        }
                    }
                    if (del_fig.Count != 0) changes.Insert(Changes.index_global, new Del(del_fig));
                }
            }

            foreach (Shape s in shapes)
            {

                s.is_moving = false;
            }

            Refresh();
            del_fig = new List<Shape>();

            if (shapes.Count > 2 && MyAlgorithm == alg.Def)
            {
                
                for (int i = 0; i < shapes.Count; i++)
                {
                    if (!shapes[i].points)
                    {
                        del_fig.Add(shapes[i]);
                        shapes.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (shapes.Count > 2 && MyAlgorithm == alg.Jar)
            {
                for(int i = 0; i < shapes.Count; i++)
                {
                    if (!deleteInnerPoints(shapes).Contains(shapes[i])) del_fig.Add(shapes[i]);
                }
                shapes = deleteInnerPoints(shapes);
            }
            //if (del_fig.Count != 0) changes.Insert(Changes.index_global, new Del(del_fig));
            Refresh();

            isMouseMove = false;
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

        Radius radius_window;

        #region Radius
        private void RadiusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(radius_window == null || radius_window.Visible == false)
            {
                radius_window = new Radius();
                radius_window.Show();
            }
            else
            {
                radius_window.Activate();
                radius_window.WindowState = FormWindowState.Normal;
            }
        }

        public void RadiusChanged(object sender, EventArgs args)
        {
            is_changed = true;
            //radius_window.raduis_trackbar.Value = Shape.Radius / 5 - 1;
            Refresh();
        }
        #endregion

        private void Window_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (MyAlgorithm == alg.Jar && shapes.Count > 2) 
                drawLines(e.Graphics, findFigurePoints(shapes));
            else
            {
                if (shapes.Count > 2)
                {
                    Point[] p = new Point[3];
                    p[0].X = (int)shapes[0].X;
                    p[0].Y = (int)shapes[0].Y;
                    defAlg(e.Graphics);
                    for (int i = 1; i < shapes.Count - 1; i++)
                    {
                        for (int j = i + 1; j < shapes.Count; j++)
                        {
                            p[1].X = (int)shapes[i].X;
                            p[1].Y = (int)shapes[i].Y;
                            p[2].X = (int)shapes[j].X;
                            p[2].Y = (int)shapes[j].Y;
                            e.Graphics.FillPolygon(new SolidBrush(Shape.shell_color), p);
                        }
                    }
                }
            }
            foreach (Shape s in shapes)
            {
                s.Draw(e.Graphics);
            }

            shellToolStripMenuItem.Enabled = shapes.Count < 3 ? false : true;
            defaultToolStripMenuItem.Checked = MyAlgorithm == alg.Def ? true : false;
            jarvisToolStripMenuItem.Checked = MyAlgorithm == alg.Jar ? true : false;
        }

        #region Algoritms
        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyAlgorithm = alg.Def;
            shellToolStripMenuItem.Enabled = true;
            Refresh();
        }

        private void jarvisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyAlgorithm = alg.Jar;
            shellToolStripMenuItem.Enabled = false;
            Refresh();
        }
        #endregion

        #region Colors
        private void pointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Shape.color = colorDialog1.Color;
                is_changed = true;
            }
            Refresh();
        }

        private void shellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Shape.shell_color = colorDialog1.Color;
                is_changed = true;
            }
            Refresh();
        }
        #endregion

        #region RunStop
        private void timer1_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            foreach (Shape s in shapes)
            {
                s.X += r.Next(-1, 2);
                s.Y += r.Next(-1, 2);
            }

            if (shapes.Count > 0 && shapes != deleteInnerPoints(shapes))
            {
                List<Shape> _shapes = new List<Shape>();
                foreach(Shape s in shapes)
                {
                    if(!(shapes.Contains(s) && deleteInnerPoints(shapes).Contains(s)))
                    {
                        if (!s.is_moving) _shapes.Add(s);
                    }
                }
                foreach(Shape s in _shapes)
                {
                    shapes.Remove(s);
                }
            }
            is_changed = true;

            Refresh();
        }
        
        private void RunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
        #endregion

        #region SAVING
        static void saveState(string path)
        {
            if (path != "")
            {
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                new BinaryFormatter().Serialize(fs, Shape.color);
                new BinaryFormatter().Serialize(fs, Shape.shell_color);
                new BinaryFormatter().Serialize(fs, Shape.Radius);
                new BinaryFormatter().Serialize(fs, shapes);
                fs.Close();
                is_changed = false;
            }
        }

        static void openState(string path)
        {
            if (path != "")
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                Shape.color = (Color)new BinaryFormatter().Deserialize(fs);
                Shape.shell_color = (Color)new BinaryFormatter().Deserialize(fs);
                Shape.Radius = (int)new BinaryFormatter().Deserialize(fs);
                shapes = (List<Shape>)new BinaryFormatter().Deserialize(fs);
                fs.Close();
                filename = path;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filename == null || filename == "")
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.ShowDialog();
                saveState(sf.FileName + format);
                filename = sf.FileName + format;
            }
            else saveState(filename);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.ShowDialog();
            saveState(sf.FileName);
            filename = sf.FileName;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialres = DialogResult.Abort;
            if (is_changed)
            {
                dialres = MessageBox.Show("Save?", "Open file", MessageBoxButtons.YesNoCancel);
                if (shapes.Count > 0)
                {
                    if (dialres == DialogResult.Yes)
                    {
                        if (filename == null)
                        {
                            SaveFileDialog sf = new SaveFileDialog();
                            sf.ShowDialog();
                            saveState(sf.FileName + format);
                            filename = sf.FileName + format;
                        }
                        else saveState(filename);
                    }
                }
            }

            if (is_changed || dialres != DialogResult.Cancel)
            {
                if (filename != format)
                {
                    OpenFileDialog of = new OpenFileDialog();
                    of.ShowDialog();
                    openState(of.FileName);
                    Refresh();
                }
            }
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialres = DialogResult.Abort;
            if (is_changed)
            {
                dialres = MessageBox.Show("Save?", "New file", MessageBoxButtons.YesNoCancel);

                if (shapes.Count > 0)
                {
                    if (dialres == DialogResult.Yes)
                    {
                        if (filename == null)
                        {
                            SaveFileDialog sf = new SaveFileDialog();
                            sf.ShowDialog();
                            saveState(sf.FileName + format);
                            filename = sf.FileName + format;
                        }
                        else saveState(filename);
                    }
                }
            }

            if (is_changed || dialres != DialogResult.Cancel)
            {
                if (filename != format)
                {
                    shapes = new List<Shape>();
                    Shape.color = Color.Red;
                    Shape.shell_color = Color.Aquamarine;
                    Refresh();
                }
            }
        }
        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialres = DialogResult.Abort;
            if (is_changed)
            {
                //dialres = MessageBox.Show("Save?", "Save file", MessageBoxButtons.YesNoCancel);

                if (shapes.Count > 0)
                {
                    if (dialres == DialogResult.Yes)
                    {
                        if (filename == null)
                        {
                            SaveFileDialog sf = new SaveFileDialog();
                            sf.ShowDialog();
                            saveState(sf.FileName + format);
                            filename = sf.FileName + format;
                        }
                        else saveState(filename);
                    }
                }
            }
        }
        #endregion

        #region UndoRedo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Changes.index_global > 0) changes[Changes.index_global - 1].Undo(shapes);
            shapes = deleteInnerPoints(shapes);
            //MessageBox.Show(Changes.index_global.ToString());
            Refresh();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Changes.index_global >= 0) changes[Changes.index_global].Redo(shapes);
            shapes = deleteInnerPoints(shapes);
            Refresh();
        }
        #endregion
    }
}