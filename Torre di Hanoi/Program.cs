namespace Torre_di_Hanoi
{
    internal static class Program
    {
        static readonly Dictionary<Rectangle, Color> pieces = new();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }


        public static void OnPaint(PaintEventArgs e)
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
            int rodY = baseY - baseHeight * 20;
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