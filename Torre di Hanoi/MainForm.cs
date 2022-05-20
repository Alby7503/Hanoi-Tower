using System.Diagnostics;

namespace Torre_di_Hanoi
{
    public partial class MainForm : Form
    {
        //Panels
        private const short towersNumber = 3;
        //Base
        private const int baseX = 100;
        private const int baseY = 400;
        private const int baseWidth = 300;
        private const int baseHeight = 20;
        //Rod
        private const int rodWidth = 20;
        private const int rodHeight = baseWidth;
        //Disk
        private short disksNumber = 10;
        private static int diskWidth = 80;
        private static readonly int diskHeight = 20;
        //Utility
        private Panel Smallest;
        private bool PanelsPresent = false;
        private readonly Random rnd = new();
        private static readonly List<Rectangle> Pieces = new();
        private readonly Stack<(Panel, int)> History = new();
        private readonly Dictionary<Panel, Stack<Panel>> Stacks = new();
        //private readonly List<Stack<Panel>> Stacks = new();
        //Handlers
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
            Panel panel = new()
            {
                //Style
                BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)),
                Location = new(x, y),
                Size = new(width, height)
            };
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
                    Disk diskStruct = (Disk)diskPanel.Tag;
                    if (target != null && target != diskStruct.backPanel)
                    {
                        MoveDisk(diskPanel, target);
                    }
                    else
                        diskPanel.Location = Start;
                    Grabbing = null;
                    CheckWinCondition();
                }
            }
        }

        private void ClearDisks()
        {
            disksNumber = 0;
            diskWidth = 80;
            History.Clear();
            foreach (Panel panel in Stacks.Keys)
            {
                foreach (Panel disk in Stacks[panel])
                {
                    disk.Dispose();
                }
                Stacks[panel].Clear();
            }
        }

        private void DrawDisks(int number)
        {
            const int diskWidthIncrement = 20;
            int diskX = baseX + (baseWidth - diskWidth) / 2;
            int diskY = baseY - baseHeight - ((disksNumber - 1) * diskHeight);
            Panel firstPanel = Stacks.Keys.First();
            for (short i = 0; i < number; i++)
            {
                //Create a disk panel
                Panel diskPanel = DrawDisk(diskX, diskY, diskWidth, diskHeight);
                //Create and assign a disk struct, containing index and parent panel
                Disk diskStruct = new(i, firstPanel);
                diskPanel.Tag = diskStruct;
                //Add the disk to the form
                Controls.Add(diskPanel);
                diskPanel.BringToFront();
                //Push the disk in the stack
                Stacks[firstPanel].Push(diskPanel);
                //Calculate the next disk position
                int newWidth = diskWidth + diskWidthIncrement;
                diskX -= (newWidth - diskWidth) / 2;
                diskWidth = newWidth;
                diskY += diskHeight;
            }
            //No, aggiungi i dischi al contrario...
            Stack<Panel> reversed = new();
            while (Stacks[firstPanel].Count != 0)
                reversed.Push(Stacks[firstPanel].Pop());
            Stacks[firstPanel] = reversed;
            Smallest = Stacks[firstPanel].Peek();
            SetGrabbable(Smallest, true);
        }

        private void MoveDisk(Panel diskPanel, Panel target, bool doesHistory = true)
        {
            Disk diskStruct = (Disk)diskPanel.Tag;
            if (Stacks[target].TryPeek(out Panel? oldFirstDisk))
            {
                //Get the struct of the first target stack disk
                Disk oldFirstDiskStruct = (Disk)oldFirstDisk.Tag;
                //Check if it's smaller
                if (oldFirstDiskStruct.index < diskStruct.index)
                {
                    diskPanel.Location = Start;
                    Grabbing = null;
                    return;
                }
            }
            //Remove disk from previous stack
            Stacks[diskStruct.backPanel].Pop();
            //Make the first disk of the old stack grabbable
            if (Stacks[diskStruct.backPanel].TryPeek(out Panel? newFirstDisk))
                SetGrabbable(newFirstDisk, true);
            //Remove the grab events from the first disk of the target stack
            if (oldFirstDisk != null)
                SetGrabbable(oldFirstDisk, false);
            //Enqueue the new disk onto the target stack
            //Move the disk
            Point diskPoint = diskPanel.Location;
            diskPoint.X = target.Location.X + (target.Width / 2) - diskPanel.Width / 2;
            List<Panel> stackList = Stacks.Keys.ToList();
            int indexOfBase = stackList.IndexOf(target);
            diskPoint.Y = Pieces[indexOfBase].Y - (diskHeight * (Stacks[target].Count + 1));
            diskPanel.Location = diskPoint;
            //Log the movement
            int diskIndex = diskStruct.index;
            int originTowerIndex = stackList.IndexOf(diskStruct.backPanel);
            diskStruct.backPanel = target;
            diskPanel.Tag = diskStruct;
            ListEvent($"Moved disk {diskIndex} from tower {originTowerIndex} to tower {indexOfBase}");
            Stacks[target].Push(diskPanel);
            if (doesHistory)
                if (History.LastOrDefault().Item1 != diskPanel)
                    History.Push((diskPanel, originTowerIndex));
        }

        private void MoveDisk(Panel diskPanel, int targetIndex, bool doesHistory = true)
        {
            Panel target = Stacks.Keys.ToList()[targetIndex];
            MoveDisk(diskPanel, target, doesHistory);
        }

        private bool IsGameWon => Stacks[Stacks.Keys.Last()].Count == disksNumber;

        private bool CheckWinCondition()
        {
            if (IsGameWon)
            {
                ListEvent("You win!");
                foreach (Panel panel in Stacks.Keys)
                    foreach (Panel disk in Stacks[panel])
                        SetGrabbable(disk, false);
                btnResolve.Enabled = false;
                cntDisks.Enabled = false;
                return true;
            }
            return false;
        }

        private void DrawBackgroundPanels()
        {
            //towersNumber backrgound panels
            for (int i = 0; i < towersNumber; i++)
            {
                //Create the size based on the base width and the rod height
                Rectangle @base = Pieces[i];
                Rectangle position = new(@base.X, @base.Y - rodHeight, baseWidth, rodHeight);
                Panel backPanel = new()
                {
                    Tag = i,
                    Location = position.Location,
                    Size = position.Size,
                    BackColor = Color.Transparent
                };
                //Initialize a new stack for each panel
                Stacks[backPanel] = new();
                Controls.Add(backPanel);
                backPanel.SendToBack();
            }
            //disksNumber disks
            DrawDisks(disksNumber);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pieces.Clear();
            //towersNumber bases
            Rectangle @base = new(baseX, baseY, baseWidth, baseHeight);
            for (int i = 0; i < towersNumber; i++)
            {
                Pieces.Add(@base);
                @base.X += baseWidth + baseX;
            }
            //towersNumber rods
            int rodX = baseX + (baseWidth / 2) - (rodWidth / 2);
            int rodY = baseY - baseWidth;
            Rectangle rod = new(rodX, rodY, rodWidth, rodHeight);
            for (int i = 0; i < towersNumber; i++)
            {
                Pieces.Add(rod);
                rod.X += baseWidth + baseX;
            }
            //Draw everything
            e.Graphics.FillRectangles(new SolidBrush(Color.Black), Pieces.ToArray());
            //3 backgrounds
            if (!PanelsPresent)
            {
                PanelsPresent = true;
                DrawBackgroundPanels();
            }
        }

        public void Resolve(object sender, EventArgs e)
        {
            int steps = 0;
            //double steps = Math.Pow(2, disksNumber) - 1;

            steps += History.Count;
            //...
            while (History.Count > 0)
            {
                var move = History.Pop();
                MoveDisk(move.Item1, move.Item2, false);
                Thread.Sleep(250);
            }
            History.Clear();

            int nextTarget;
            List<Panel> towers;
            List<Panel> possibleTowers;
            Disk smallestStruct, firstDisk, secondDisk;
            int smallestNextStep = disksNumber % 2 == 0 ? 1 : -1;
            while (!CheckWinCondition())
            {
                towers = Stacks.Keys.ToList();
                //Move smallest disk
                smallestStruct = (Disk)Smallest.Tag;
                nextTarget = towers.IndexOf(smallestStruct.backPanel) + smallestNextStep;
                if (nextTarget == 3)
                    nextTarget = 0;
                else if (nextTarget == -1)
                    nextTarget = 2;
                MoveDisk(Smallest, nextTarget, false);
                //Make next legal move
                smallestStruct = (Disk)Smallest.Tag;
                possibleTowers = towers;
                possibleTowers.Remove(smallestStruct.backPanel);
                if (Stacks[possibleTowers[0]].TryPeek(out Panel? first))
                {
                    if (Stacks[possibleTowers[1]].TryPeek(out Panel? second))
                    {
                        firstDisk = (Disk)first.Tag;
                        secondDisk = (Disk)second.Tag;
                        if (firstDisk.index < secondDisk.index)
                            MoveDisk(first, possibleTowers[1], false);
                        else
                            MoveDisk(second, possibleTowers[0], false);
                    }
                    else
                        MoveDisk(first, possibleTowers[1], false);
                }
                else if (Stacks[possibleTowers[1]].TryPeek(out Panel? second))
                    MoveDisk(second, possibleTowers[0], false);
                steps += 2;
            }
            ListEvent($"Solved in {steps} steps");
        }

        private void DisksNumber_Changed(object sender, EventArgs e)
        {
            ClearDisks();
            disksNumber = (short)cntDisks.Value;
            DrawDisks(disksNumber);
        }
    }
}