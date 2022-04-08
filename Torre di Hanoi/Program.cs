namespace Torre_di_Hanoi
{
    internal static class Program
    {
        static Rectangle[] rectangles = new Rectangle[] {
            //Base 1
            new Rectangle()
        };
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static void OnPaint(object sender, PaintEventArgs e)
        {
            Color[] colors = new Color[] { Color.Red, Color.Green, Color.Blue };

            int x = 0;
            int y = 0;
            int width = 200;
            int height = 20;
            Rectangle rectangle = new(x, y, width, height);
            foreach (Color color in colors)
            {
                using (SolidBrush myBrush = new(color))
                {
                    e.Graphics.FillRectangle(myBrush, rectangle);
                }
                rectangle.X += width;
            }
        }
    }


}