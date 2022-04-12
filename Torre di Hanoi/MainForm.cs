namespace Torre_di_Hanoi
{
    public partial class MainForm : Form
    {
        //Base
        private const int baseX = 100;
        private const int baseY = 400;
        private const int baseWidth = 300;
        private const int baseHeight = 20;
        //Rod
        private const int rodWidth = 20;
        private const int rodHeight = baseWidth;
        //Utility
        private readonly Random rnd = new();
        private static readonly Dictionary<Rectangle, Color> pieces = new();
        private bool PanelsPresent = false;

        private readonly Dictionary<Panel, Queue<Panel>> Stacks = new();

        public MainForm()
        {
            InitializeComponent();

            //disksNumber disks
            short disksNumber = 10;
            int diskWidth = 80;
            int diskHeight = 20;
            int diskX = baseX + (baseWidth - diskWidth) / 2;
            int diskY = baseY - baseHeight - ((disksNumber - 1) * diskHeight);
            for (short i = 0; i < disksNumber; i++)
            {
                Panel panel = DrawDisk(diskX, diskY, diskWidth, diskHeight);
                Stacks[Stacks.Keys.First()].Enqueue(panel);
                Controls.Add(panel);
                int newWidth = diskWidth + 20;
                diskX -= (newWidth - diskWidth) / 2;
                diskWidth = newWidth;
                diskY += diskHeight;
            }
        }

        private Panel DrawDisk(int x, int y, int width, int height)
        {
            Panel panel = new();
            panel.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            panel.Location = new(x, y);
            panel.Size = new(width, height);
            panel.MouseDown += new(DiskPanel_MouseDown);
            panel.MouseMove += new(DiskPanel_MouseMove);
            panel.MouseUp += new(DiskPanel_MouseUp);
            return panel;
        }

        private static bool Overlap(Rectangle a, Rectangle b)
        {
            return a.IntersectsWith(b);
        }

        private Point MouseDownLocation;
        private Point Start;
        private Panel? Grabbing = null;
        private Panel? Target = null;

        private void DiskPanel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (sender != null && Grabbing == null && e.Button == MouseButtons.Left)
            {
                Panel panel = (Panel)sender;
                Start = panel.Location;
                Grabbing = panel;
                MouseDownLocation = e.Location;
            }
        }

        private void ResetBackgroundPanels()
        {
            foreach (Panel panel in Stacks.Keys)
                panel.BackColor = Color.LightGray;
        }

        private void DiskPanel_MouseMove(object? sender, MouseEventArgs e)
        {
            if (sender != null && Grabbing != null && e.Button == MouseButtons.Left)
            {
                //Mouse grab
                Panel diskPanel = (Panel)sender;
                diskPanel.Left = e.X + diskPanel.Left - MouseDownLocation.X;
                diskPanel.Top = e.Y + diskPanel.Top - MouseDownLocation.Y;
                //Show center coordinates
                Point center = new(diskPanel.Left + diskPanel.Width / 2, diskPanel.Top + diskPanel.Height / 2);
                lblDebug.Text = center.ToString();
                
                /*Rectangle diskRectangle = new(diskPanel.Location, diskPanel.Size);
                Rectangle diskRectangle = new(e.Location, diskPanel.Size);
                foreach (Panel panel in Panels)
                {
                    Rectangle areaRectangle = new(panel.Location, panel.Size);
                    panel.BackColor = Color.LightGray;
                    if (Overlap(diskRectangle, areaRectangle))
                    {
                        panel.BackColor = Color.Red;
                    }
                }*/
            }
        }

        private void DiskPanel_MouseUp(object? sender, MouseEventArgs e)
        {
            if (sender != null && Grabbing != null && e.Button == MouseButtons.Left)
            {
                Panel diskPanel = (Panel)sender;
                if (Target != null && !Stacks[Target].Contains(Grabbing))
                {
                    Stacks[Target].Enqueue(Grabbing);
                    Target = null;
                }
                else
                    diskPanel.Location = Start;
                Grabbing = null;
            }
            ResetBackgroundPanels();
        }

        private void BackPanel_MouseEnter(object? sender, EventArgs e)
        {
            if (sender != null && Grabbing != null)
            {
                Target = (Panel)sender;
            }
        }

        private void DrawBackgroundPanels()
        {
            for (int i = 0; i < 3; i++)
            {
                Rectangle @base = pieces.Keys.ElementAt(i);
                Panel panel = new();
                Rectangle position = new(@base.X, @base.Y - rodHeight, baseWidth, rodHeight);
                panel.Location = position.Location;
                panel.Size = position.Size;
                panel.BackColor = Color.LightGray;
                panel.MouseEnter += new(BackPanel_MouseEnter);
                Stacks[panel] = new();
                Controls.Add(panel);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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
            //3 backgrounds
            if (!PanelsPresent)
            {
                PanelsPresent = true;
                DrawBackgroundPanels();
            }
        }
    }
}