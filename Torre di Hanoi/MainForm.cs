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
        //Disk
        private static int diskWidth = 80;
        private static readonly int diskHeight = 20;
        private static readonly short disksNumber = 10;
        //Utility
        private bool PanelsPresent = false;
        private readonly Random rnd = new();
        private static readonly Dictionary<Rectangle, Color> pieces = new();
        private readonly Dictionary<Panel, Stack<Panel>> Stacks = new();

        private readonly MouseEventHandler MouseDownHandler;
        private readonly MouseEventHandler MouseMoveHandler;
        private readonly MouseEventHandler MouseUpHandler;

        public MainForm()
        {
            InitializeComponent();
            MouseDownHandler = new(DiskPanel_MouseDown);
            MouseMoveHandler = new(DiskPanel_MouseMove);
            MouseUpHandler = new(DiskPanel_MouseUp);
        }

        private struct Disk
        {
            public int index;
            public Panel backPanel;

            public Disk(int index, Panel backPanel)
            {
                this.index = index;
                this.backPanel = backPanel;
            }
        }

        private Panel DrawDisk(int x, int y, int width, int height)
        {
            Panel panel = new();
            //Style
            panel.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            panel.Location = new(x, y);
            panel.Size = new(width, height);
            return panel;
        }

        private Panel? FindSelected() => Stacks.Keys.Where(panel => panel.ClientRectangle.Contains(panel.PointToClient(Cursor.Position))).FirstOrDefault();

        private Point MouseDownLocation;
        private Point Start;
        private Panel? Grabbing = null;

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
            }
        }

        private void SetGrabbable(Panel p, bool Grabbable)
        {
            if (p == null)
            {
                throw new Exception();
            }
            if (Grabbable)
            {
                p.MouseDown += MouseDownHandler;
                p.MouseMove += MouseMoveHandler;
                p.MouseUp += MouseUpHandler;
            }
            else
            {
                p.MouseDown -= MouseDownHandler;
                p.MouseMove -= MouseMoveHandler;
                p.MouseUp -= MouseUpHandler;
            }
        }

        private void ListEvent(string listEvent)
        {
            lbEvents.Items.Add(listEvent);
            lbEvents.SelectedIndex = lbEvents.Items.Count - 1; //= -1
        }

        private void DiskPanel_MouseUp(object? sender, MouseEventArgs e)
        {
            if (sender != null)
            {
                Panel? diskPanel = (Panel)sender;
                if (Grabbing == diskPanel && e.Button == MouseButtons.Left)
                {
                    //Find the target tower, if any (if not, return to original position)
                    Panel? target = FindSelected();
                    /*if (target != null && !Stacks[target].Contains(Grabbing))
                    {
                        //Iterate over panels to find where the disk was
                        foreach (Panel origin in Stacks.Keys)
                            if (!origin.Equals(target))
                                if (Stacks[origin].Contains(diskPanel))
                                {
                                    string localEvent = "Grab - " + diskPanel.Tag.ToString() + " - origin: " + Stacks.Keys.ToList().IndexOf(origin).ToString() + " - taget: " + Stacks.Keys.ToList().IndexOf(target).ToString();
                                    //Remove disk from previous stack
                                    Stacks[origin].Pop();
                                    //Make the first disk of the old stack grabbable
                                    if (Stacks[origin].TryPeek(out Panel? newOriginFirst))
                                        SetGrabbable(newOriginFirst, true);
                                    //Remove the grab events from the first disk of the target stack
                                    if (Stacks[target].TryPeek(out Panel? oldTargetFirst))
                                        SetGrabbable(oldTargetFirst, false);
                                    //Enqueue the new disk onto the target stack
                                    Stacks[target].Push(diskPanel);
                                    //Add to listbox
                                    ListEvent(localEvent);
                                    break;
                                }
                    }
                    else
                        diskPanel.Location = Start;*/
                    Disk diskStruct = (Disk)diskPanel.Tag;
                    if (target != null && target != diskStruct.backPanel)
                    {
                        //Remove disk from previous stack
                        Stacks[diskStruct.backPanel].Pop();
                        //Make the first disk of the old stack grabbable
                        if (Stacks[diskStruct.backPanel].TryPeek(out Panel? newFirstDisk))
                            SetGrabbable(newFirstDisk, true);
                        //Remove the grab events from the first disk of the target stack
                        if (Stacks[target].TryPeek(out Panel? oldFirstDisk))
                            SetGrabbable(oldFirstDisk, false);
                        //Enqueue the new disk onto the target stack
                        diskStruct.backPanel = target;
                        diskPanel.Tag = diskStruct;
                        Stacks[target].Push(diskPanel);
                    }
                    else
                        diskPanel.Location = Start;
                    Grabbing = null;
                }
            }
        }

        private void DrawBackgroundPanels()
        {
            //3 backrgound panels
            for (int i = 0; i < 3; i++)
            {
                Rectangle @base = pieces.Keys.ElementAt(i);
                Panel panel = new();
                Rectangle position = new(@base.X, @base.Y - rodHeight, baseWidth, rodHeight);
                panel.Location = position.Location;
                panel.Size = position.Size;
                panel.BackColor = Color.Transparent;
                Stacks[panel] = new();
                Controls.Add(panel);
                panel.SendToBack();
            }
            //disksNumber disks
            const int diskWidthIncrement = 20;
            int diskX = baseX + (baseWidth - diskWidth) / 2;
            int diskY = baseY - baseHeight - ((disksNumber - 1) * diskHeight);
            Panel firstPanel = Stacks.Keys.First();
            for (short i = 0; i < disksNumber; i++)
            {
                //Create a disk, add it to the form and enqueue it on the first stack
                Panel diskPanel = DrawDisk(diskX, diskY, diskWidth, diskHeight);
                Disk diskStruct = new(i, firstPanel);
                diskPanel.Tag = diskStruct;
                Controls.Add(diskPanel);
                diskPanel.BringToFront();
                Stacks[firstPanel].Push(diskPanel);
                int newWidth = diskWidth + diskWidthIncrement;
                diskX -= (newWidth - diskWidth) / 2;
                diskWidth = newWidth;
                diskY += diskHeight;
            }
            //No, aggiungi i pannelli al contrario...
            Stack<Panel> reversed = new();
            while (Stacks[firstPanel].Count != 0)
                reversed.Push(Stacks[firstPanel].Pop());
            Stacks[firstPanel] = reversed;
            SetGrabbable(Stacks[firstPanel].Peek(), true);
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
            e.Graphics.FillRectangles(new SolidBrush(Color.Black), pieces.Keys.ToArray());
            //3 backgrounds
            if (!PanelsPresent)
            {
                PanelsPresent = true;
                DrawBackgroundPanels();
            }
        }
    }
}