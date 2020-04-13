namespace Figures
{
    partial class Radius
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.raduis_trackbar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.raduis_trackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // raduis_trackbar
            // 
            this.raduis_trackbar.BackColor = System.Drawing.Color.Maroon;
            this.raduis_trackbar.Location = new System.Drawing.Point(12, 47);
            this.raduis_trackbar.Maximum = 250;
            this.raduis_trackbar.Minimum = 50;
            this.raduis_trackbar.Name = "raduis_trackbar";
            this.raduis_trackbar.Size = new System.Drawing.Size(499, 45);
            this.raduis_trackbar.TabIndex = 0;
            this.raduis_trackbar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.raduis_trackbar.Value = 50;
            this.raduis_trackbar.Scroll += new System.EventHandler(this.Raduis_trackbar_Scroll);
            // 
            // Radius
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Maroon;
            this.ClientSize = new System.Drawing.Size(523, 137);
            this.Controls.Add(this.raduis_trackbar);
            this.MaximumSize = new System.Drawing.Size(539, 175);
            this.MinimumSize = new System.Drawing.Size(539, 175);
            this.Name = "Radius";
            this.Text = "Radius";
            this.Load += new System.EventHandler(this.Radius_Load);
            ((System.ComponentModel.ISupportInitialize)(this.raduis_trackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TrackBar raduis_trackbar;
    }
}