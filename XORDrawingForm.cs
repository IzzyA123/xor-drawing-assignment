using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace XORDrawing
{
    public partial class XORDrawingForm : Form
    {
        // Shapes
        private Rectangle aRect;
        private Rectangle anEllipse;
        private Rectangle moving;

        // Starting coords for the moving square (stored here mainly because the worksheet mentions x,y)
        private int x = 0, y = 0;

        public XORDrawingForm()
        {
            InitializeComponent();

            // Set up shapes (client/form coordinates)
            aRect = new Rectangle(100, 100, 200, 200);
            anEllipse = new Rectangle(150, 150, 200, 100);
            moving = new Rectangle(x, y, 10, 10);

            // Form setup
            this.Width = 500;
            this.Height = 500;
            this.BackColor = Color.White;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            // Start the animation once the form is visible
            this.Shown += XORDrawingForm_Shown;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            // Red square
            using (Brush redBrush = new SolidBrush(Color.Red))
            {
                g.FillRectangle(redBrush, aRect);
            }

            // Green ellipse
            using (Brush greenBrush = new SolidBrush(Color.Green))
            {
                g.FillEllipse(greenBrush, anEllipse);
            }
        }

        private void XORDrawingForm_Shown(object sender, EventArgs e)
        {
            // Run animation after the form has finished showing/painting
            this.BeginInvoke((Action)AnimateMovingSquare);
        }

        private void AnimateMovingSquare()
        {
            // Top-right -> bottom-left (diagonal)
            int localX = this.ClientSize.Width - 20; // near right edge
            int localY = 10;                         // near top edge

            while (localX > 0 && localY < this.ClientSize.Height - 10)
            {
                // b) Update moving rectangle location in SCREEN coordinates
                moving.Location = this.PointToScreen(new Point(localX, localY));

                // c) Draw reversible rectangle in red (XOR-like effect)
                ControlPaint.FillReversibleRectangle(moving, Color.Red);

                // d) Pause so you can see it
                Thread.Sleep(10);

                // e) Draw again to erase it (reversible)
                ControlPaint.FillReversibleRectangle(moving, Color.Red);

                // f) Move diagonally: left + down
                localX -= 2;
                localY += 2;
            }
        }
    }
}
