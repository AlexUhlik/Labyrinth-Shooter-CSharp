using System;
using System.Drawing;
using System.Windows.Forms;

namespace LabyrinthGame
{
    public class CustomProgressBar : Control
    {
        public int Value { get; set; } = 100;
        public int MaxValue { get; set; } = 100;
        public Color BarColor { get; set; } = Color.Red;

        public CustomProgressBar()
        {
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.FillRectangle(Brushes.DimGray, 0, 0, this.Width, this.Height);

            if (MaxValue <= 0) return;
            float percent = (float)Value / MaxValue;
            float fillWidth = percent * this.Width;
            using (Brush brush = new SolidBrush(BarColor))
            {
                e.Graphics.FillRectangle(brush, 0, 0, fillWidth, this.Height);
            }

            e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}