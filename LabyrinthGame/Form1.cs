using Application.Services;
using GameCore.Characters;
using GameCore.Map;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LabyrinthGame
{
    public partial class Form1 : Form
    {
        private static readonly Color4 BackgroundColor = new Color4(0.05f, 0.05f, 0.05f, 1.0f);
        private static readonly Color4 Player1Color = new Color4(0.0f, 0.5f, 1.0f, 1.0f);
        private static readonly Color4 Player2Color = new Color4(1.0f, 0.2f, 0.2f, 1.0f);
        private static readonly Color4 Base1Color = new Color4(0.0f, 0.5f, 1.0f, 0.4f);
        private static readonly Color4 Base2Color = new Color4(1.0f, 0.2f, 0.2f, 0.4f);

        private static readonly Color4 WallColor = new Color4(0.3f, 0.3f, 0.3f, 1.0f);
        private static readonly Color4 EmptyColor = new Color4(0.1f, 0.1f, 0.1f, 1.0f);

        private GameRenderer _renderer;
        private LabyrinthMap _map;
        private MapGenerator _generator = new MapGenerator();
        private GameController _game;
        private bool _isLoaded = false;
        private Keys _lastProcessedKey = Keys.None;


        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            this.Resize += Form1_Resize;

            Timer timer = new Timer { Interval = 16 };
            timer.Tick += (s, e) => { if (_isLoaded) glControl1.Invalidate(); };
            timer.Start();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            try
            {
                glControl1.MakeCurrent();
                _renderer = new GameRenderer();
                _renderer.Init();

                _map = _generator.GenerateMaze(31, 31);
                _game = new GameController(_map);

                _isLoaded = true;

                CenterSquareControl();
                UpdateViewport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!_isLoaded || _map == null) return;

            glControl1.MakeCurrent();
            GL.ClearColor(BackgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            DrawMap(_map, WallColor, EmptyColor);

            DrawBase(1, 1, Base1Color);
            DrawBase(_map.Width() - 2, _map.Height() - 2, Base2Color);

            RenderPlayer(_game.Player1, Player1Color);
            RenderPlayer(_game.Player2, Player2Color);

            glControl1.SwapBuffers();
        }

        private void DrawMap(LabyrinthMap map, Color4 wallColor, Color4 emptyColor)
        {
            float mazeWidth = _map.Width() * LabyrinthMap.TileSize;
            float mazeHeight = _map.Height() * LabyrinthMap.TileSize;
            var projection = Matrix4.CreateOrthographicOffCenter(0, mazeWidth, 0, mazeHeight, -1, 1);

            _renderer.Draw(_map,wallColor, emptyColor, Matrix4.Identity, projection);
        }

        private void RenderPlayer(Player p, Color4 color)
        {
            if (p != null && p.IsActive)
            {
                _renderer.DrawSquare(p.Position.X, p.Position.Y, p.Size, p.Size, color);
                float indicatorSize = p.Size / 3f;
                var (indicatorX, indicatorY) = p.GetIndicatorPosition(indicatorSize);
                _renderer.DrawSquare(indicatorX, indicatorY, indicatorSize, indicatorSize, new Color4(1f, 1f, 0f, 1f));


            }
        }

        

        private void DrawBase(int gridX, int gridY, Color4 color)
        {
            var pos = _map.ConvertToWorldCoordinates(gridX, gridY);
            _renderer.DrawSquare(pos.X, pos.Y, LabyrinthMap.TileSize, LabyrinthMap.TileSize, color);
        }

        private void UpdateViewport()
        {
            if (glControl1.ClientSize.Width > 0 && glControl1.ClientSize.Height > 0)
            {
                glControl1.MakeCurrent();
                GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            }
        }

        private void CenterSquareControl()
        {
            int side = Math.Min(this.ClientSize.Width, this.ClientSize.Height);
            glControl1.Size = new System.Drawing.Size(side, side);
            glControl1.Location = new System.Drawing.Point((this.ClientSize.Width - side) / 2, (this.ClientSize.Height - side) / 2);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CenterSquareControl();
            if (_isLoaded) UpdateViewport();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _renderer?.Dispose();
            base.OnFormClosing(e);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
            {
                return false;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == _lastProcessedKey)
                return;

            _lastProcessedKey = e.KeyCode;
            _game.HandleInput(e.KeyCode);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == _lastProcessedKey)
                _lastProcessedKey = Keys.None;
        }
    }
}