namespace CrosswordMaker
{
    public partial class Form1 : Form
    {

        int w, h;
        int cx, cy;
        bool cright;
        Cell[][] grid;

        public Form1()
        {
            InitializeComponent();
            w = 15;
            h = 15;
            cx = 0;
            cy = 0;
            cright = true;
            grid = new Cell[h][];
            for (int i = 0; i < h; i++)
            {
                grid[i] = new Cell[w];
                for (int j = 0; j < w; j++)
                {
                    grid[i][j] = new Cell();
                }
            }

            this.KeyDown += Form1_KeyDown;
            this.Paint += Form1_Paint;
            this.MouseDown += Form1_MouseDown;
            this.MouseDoubleClick += Form1_DoubleClick;

            this.Width = 28 + (w * 30);
            this.Height = 51 + (h * 30);
            this.Text = "Crossword";
        }

        public void SwitchDir()
        {
            cright = !cright;
            RedrawCell(cx, cy);
            DrawCursor();
        }

        public void MoveLeft()
        {
            RedrawCell(cx, cy);
            if (cx > 0)
                cx--;
            RedrawCell(cx, cy);
            DrawCursor();
        }

        public void MoveRight()
        {
            RedrawCell(cx, cy);
            if (cx < w - 1)
                cx++;
            RedrawCell(cx, cy);
            DrawCursor();
        }

        public void MoveUp()
        {
            RedrawCell(cx, cy);
            if (cy > 0)
                cy--;
            RedrawCell(cx, cy);
            DrawCursor();
        }

        public void MoveDown()
        {
            RedrawCell(cx, cy);
            if (cy < h - 1)
                cy++;
            RedrawCell(cx, cy);
            DrawCursor();
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                KeysConverter kc = new KeysConverter();
                grid[cy][cx].C(kc.ConvertToString(e.KeyCode).ToCharArray()[0]);
                if (cright)
                    MoveRight();
                else
                    MoveDown();
            }
            else switch (e.KeyCode)
            {
                case Keys.Tab:
                    SwitchDir();
                    break;
                case Keys.Left:
                    MoveLeft();
                    break;
                case Keys.Right:
                        if (cright)
                            MoveRight();
                        else
                            SwitchDir();
                    break;
                case Keys.Up:
                    MoveUp();
                    break;
                case Keys.Down:
                        if (!cright)
                            MoveDown();
                        else
                            SwitchDir();
                    break;
                case Keys.Back:
                        grid[cy][cx].C(' ');
                        if (cright)
                            MoveLeft();
                        else
                            MoveUp();
                        break;
            }
        }

        void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs pe)
        {
            Pen pen = new Pen(Color.Black);
            Graphics g = pe.Graphics;
            g.DrawLine(pen, 5 + (30 * w), 5, 5 + (30 * w), 5 + (30 * h));
            g.DrawLine(pen, 5, 5 + (30 * h), 5 + (30 * w), 5 + (30 * h));
            for (int i = 0; i < w; i++)
            {
                g.DrawLine(pen, 5 + (30 * i), 5, 5 + (30 * i), 5 + (30 * h));
            }
            for (int i = 0; i < h; i++)
            {
                g.DrawLine(pen, 5, 5 + (30 * i), 5 + (30 * w), 5 + (30 * i));
            }

            SolidBrush myBrush = new SolidBrush(Color.Black);
            Font letFont = new Font("TimesNewRoman", 14);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (!grid[i][j].IsWhite())
                    {
                        g.FillRectangle(myBrush, 5 + (30 * j), 5 + (30 * i), 30, 30);
                    }
                    else
                    {
                        if (grid[i][j].C() != ' ')
                            g.DrawString(grid[i][j].C().ToString(), letFont, myBrush, 21 + (30 * j), 11 + (30 * i), sf);
                    }
                }
            }
            RedrawNums();
            DrawCursor();

            myBrush.Dispose();
            letFont.Dispose();
            sf.Dispose();
            pen.Dispose();
        }

        void Form1_DoubleClick(object sender, EventArgs e)
        {
            grid[cy][cx].IsWhite(!grid[cy][cx].IsWhite());
            grid[h - 1 - cy][w - 1 - cx].IsWhite(grid[cy][cx].IsWhite());
            RedrawCell(cx, cy);
            RedrawCell(w - 1 - cx, h - 1 - cy);
            RedrawNums();
            DrawCursor();
        }

        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X >= 5 && e.X <= 5 + (30 * w) && e.Y >= 5 && e.Y <= 5 + (30 * h))
            {
                RedrawCell(cx, cy);
                cx = (e.X - 5) / 30;
                cy = (e.Y - 5) / 30;
                RedrawCell(cx, cy);
                DrawCursor();
            }
        }

        void RedrawCell(int x, int y)
        {
            Graphics g = this.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(Color.Black);
            SolidBrush urBrush = new SolidBrush(Color.White);
            Font letFont = new Font("TimesNewRoman", 14);
            Font numFont = new Font("TimesNewRoman", 7);
            StringFormat sf = new StringFormat();
            StringFormat nsf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            if (grid[y][x].IsWhite())
            {
                g.FillRectangle(urBrush, 6 + (30 * x), 6 + (30 * y), 29, 29);
                g.DrawString(grid[y][x].C().ToString(), letFont, myBrush, 21 + (30 * x), 11 + (30 * y), sf);
                int n = grid[y][x].N();
                if (n > 0)
                    g.DrawString("" + n, numFont, myBrush, 5 + (30 * x), 5 + (30 * y), nsf);
            }
            else
            {
                g.FillRectangle(myBrush, 5 + (30 * x), 5 + (30 * y), 30, 30);
            }

            myBrush.Dispose();
            urBrush.Dispose();
            letFont.Dispose();
            numFont.Dispose();
            sf.Dispose();
            nsf.Dispose();
            g.Dispose();
        }

        void RedrawNums()
        {
            Graphics g = this.CreateGraphics();
            SolidBrush urBrush = new SolidBrush(Color.White);
            SolidBrush myBrush = new SolidBrush(Color.Black);
            Font numFont = new Font("TimesNewRoman", 7);
            StringFormat nsf = new StringFormat();
            int num = 1;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (grid[i][j].IsWhite())
                    {
                        g.FillRectangle(urBrush, 6 + (30 * j), 6 + (30 * i), 29, 9);
                        if (j == 0 || i == 0 || !grid[i - 1][j].IsWhite() || !grid[i][j - 1].IsWhite())
                        {
                            g.DrawString("" + num, numFont, myBrush, 5 + (30 * j), 5 + (30 * i), nsf);
                            grid[i][j].N(num);
                            num++;
                        }
                    }
                }
            }
            g.Dispose();
            myBrush.Dispose();
            urBrush.Dispose();
            numFont.Dispose();
            nsf.Dispose();
        }

        void DrawCursor()
        {
            Graphics g = this.CreateGraphics();
            Pen pen = new Pen(Color.Red);
            if (!grid[cy][cx].IsWhite())
                pen.Color = Color.White;
            if (cright)
            {
                g.DrawLine(pen, 7 + (30 * cx), 7 + (30 * cy), 7 + (30 * cx), 32 + (30 * cy));
                g.DrawLine(pen, 7 + (30 * cx), 7 + (30 * cy), 28 + (30 * cx), 7 + (30 * cy));
                g.DrawLine(pen, 28 + (30 * cx), 7 + (30 * cy), 33 + (30 * cx), 20 + (30 * cy));
                g.DrawLine(pen, 28 + (30 * cx), 32 + (30 * cy), 33 + (30 * cx), 20 + (30 * cy));
                g.DrawLine(pen, 7 + (30 * cx), 32 + (30 * cy), 28 + (30 * cx), 32 + (30 * cy));
            }
            else
            {
                g.DrawLine(pen, 7 + (30 * cx), 7 + (30 * cy), 32 + (30 * cx), 7 + (30 * cy));
                g.DrawLine(pen, 7 + (30 * cx), 7 + (30 * cy), 7 + (30 * cx), 28 + (30 * cy));
                g.DrawLine(pen, 7 + (30 * cx), 28 + (30 * cy), 20 + (30 * cx), 33 + (30 * cy));
                g.DrawLine(pen, 32 + (30 * cx), 28 + (30 * cy), 20 + (30 * cx), 33 + (30 * cy));
                g.DrawLine(pen, 32 + (30 * cx), 7 + (30 * cy), 32 + (30 * cx), 28 + (30 * cy));
            }
            g.Dispose();
            pen.Dispose();
        }
    }
}