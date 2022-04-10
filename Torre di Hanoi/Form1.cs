namespace Torre_di_Hanoi
{
    public partial class Form1 : Form
    {
        private const int baseX = 100;
        private const int baseY = 400;
        private const int baseWidth = 300;
        private const int baseHeight = 20;

        int rodWidth = 20;

        private readonly Random rnd = new();
        private static readonly Dictionary<Rectangle, Color> pieces = new();
        
        public Form1()
        {
            InitializeComponent();

            //disksNumber disks
            short disksNumber = 10;
            int diskWidth = 100;
            int diskHeight = 20;
            int diskX = baseX + (baseWidth - diskWidth) / 2;
            int diskY = baseY - baseHeight - ((disksNumber - 1) * diskHeight);
            for (short i = 0; i < disksNumber; i++)
            {
                Controls.Add(DrawDisk(diskX, diskY, diskWidth, diskHeight));
                int newWidth = diskWidth + 20;//= (int)(diskWidth * 1.35f);
                diskX -= (newWidth - diskWidth) / 2;
                diskWidth = newWidth;
                diskY += diskHeight;
            }
        }

        private Panel DrawDisk(int x, int y, int width, int height)
        {
            Panel panel = new();
            panel.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            panel.Location = new Point(x, y);
            panel.Size = new(width, height);
            panel.MouseDown += new(Panel_MouseDown);
            panel.MouseMove += new(Panel_MouseMove);
            panel.MouseUp += new(Panel_MouseUp);
            return panel;
        }

        private Point MouseDownLocation;
        private Point Start;

        private void Panel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (sender != null && e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
                Start = ((Panel)sender).Location;
            }
        }

        private void Panel_MouseMove(object? sender, MouseEventArgs e)
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

        private void Panel_MouseUp(object? sender, MouseEventArgs e)
        {
            if (sender != null && e.Button == MouseButtons.Left)
                ((Panel)sender).Location = Start;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            pieces.Clear();
            //3 bases
            Rectangle @base = new(baseX, baseY, baseWidth, baseHeight);
            for (int i = 0; i < 3; i++)
            {
                pieces.Add(@base, Color.Black);
                @base.X += baseWidth + baseX;
            }
            //3 rods
            int rodX = baseX + (baseWidth / 2) - (rodWidth / 2);
            int rodHeight = baseWidth;
            int rodY = baseY - baseWidth;
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