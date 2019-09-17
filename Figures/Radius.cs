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
    public partial class Radius : Form
    {
        public Radius()
        {
            InitializeComponent();
        }

        private void Radius_Load(object sender, EventArgs e)
        {

        }
        
        private void Raduis_trackbar_Scroll(object sender, EventArgs e)
        {
            Shape.Radius = raduis_trackbar.Value * 5 + 5;
        }
    }
}
