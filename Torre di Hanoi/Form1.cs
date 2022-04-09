namespace Torre_di_Hanoi
{
    public partial class Form1 : Form
    {
        Random rnd = new();
        static readonly Dictionary<Rectangle, Color> pieces = new();
        public Form1()
        {
            InitializeComponent();

            int x = 300;
            int y = 10;
            int width = 20;
            int height = 20;
            short discsNumber = 10;
            for (short i = 0; i < discsNumber; i++)
            {
                Controls.Add(DrawDisk(x, y, width, height));
                x -= (int)(((width * 1.3f) - width) / 2);
                width = (int)(width * 1.3f);
                y += height;
            }
        }

        private Panel DrawDisk(int x, int y, int width, int height)
        {
            Panel panel = new();
            Color color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            panel.BackColor = color;
            panel.Location = new Point(x, y);
            panel.Size = new Size(width, height);
            panel.MouseDown += new MouseEventHandler(panel_MouseDown);
            panel.MouseMove += new MouseEventHandler(panel_MouseMove);
            panel.MouseUp += new MouseEventHandler(panel_MouseUp);
            return panel;
        }

        private Point MouseDownLocation;
        private Point Start;

        private void panel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (sender != null && e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
                Start = ((Panel)sender).Location;
            }
        }

        private void panel_MouseMove(object? sender, MouseEventArgs e)
        {
            if (sender != null && e.Button == MouseButtons.Left)
            {
                Panel panel = (Panel)sender;
                panel.Left = e.X + panel.Left - MouseDownLocation.X;
                panel.Top = e.Y + panel.Top - MouseDownLocation.Y;
                Point center = new(panel.Left + panel.Width / 2, panel.Top + panel.Height / 2);
                lblDebug.Text = center.ToString();
            }
        }

        private void panel_MouseUp(object? sender, MouseEventArgs e)
        {
            if (sender != null && e.Button == MouseButtons.Left)
            {
                Panel panel = (Panel)sender;
                panel.Location = Start;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            pieces.Clear();
            //3 bases
            int baseX = 100;
            int baseY = 400;
            int baseWidth = 300;
            int baseHeight = 20;
            Rectangle @base = new(baseX, baseY, baseWidth, baseHeight);
            for (int i = 0; i < 3; i++)
            {
                pieces.Add(@base, Color.Black);
                @base.X += baseWidth + baseX;
            }
            //3 rods
            int rodX = baseX + (baseWidth / 2);
            int rodHeight = baseWidth;
            int rodY = baseY - baseWidth;
            int rodWidth = 20;
            Rectangle rod = new(rodX, rodY, rodWidth, rodHeight);
            for (int i = 0; i < 3; i++)
            {
                pieces.Add(rod, Color.Black);
                rod.X += baseWidth + baseX;
            }
            //Draw everything
            foreach (KeyValuePair<Rectangle, Color> piece in pieces)
                e.Graphics.FillRectangle(new SolidBrush(piece.Value), piece.Key);
        }
    }
}