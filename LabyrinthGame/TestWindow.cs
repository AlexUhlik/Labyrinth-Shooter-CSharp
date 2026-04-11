using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics; 
using Application.Services;
using Application.Game;
using GameCore.Map;
using OpenTK.Graphics.OpenGL4;
using DrawLib.Graphics;

namespace LabyrinthGame
{
    public partial class TestWindow : Form
    {
        private GameRenderer _renderer;
        private Painter _painter;
        private LabyrinthMap _map;
        private MapGenerator _generator;
        private GameController _game;

        private bool _isLoaded = false;
        private Keys _lastProcessedKey = Keys.None;

        private Stopwatch _frameClock = new Stopwatch();

        public TestWindow()
        {
            InitializeComponent();

            this.KeyPreview = true;

            Timer timer = new Timer { Interval = 16 };
            timer.Tick += (s, e) => { if (_isLoaded) glControl1.Invalidate(); };
            timer.Start();

            _frameClock.Start();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            try
            {
                glControl1.MakeCurrent();

                _renderer = new GameRenderer();
                _generator = new MapGenerator();
                _renderer.Init();

                _map = _generator.GenerateMaze(31, 31);
                _painter = new Painter(_renderer);
                _game = new GameController(_map);

                _painter.LoadAssets();
                _painter.SetupCamera(_map);

                _isLoaded = true;
                UpdateEverything();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void GameContainer_Layout(object sender, LayoutEventArgs e)
        {
            UpdateEverything();
        }

        private void UpdateEverything()
        {
            int pw = GameContainer.Width;
            int ph = GameContainer.Height;

            if (pw <= 0 || ph <= 0) return;

            int side = Math.Min(pw, ph);

            glControl1.SetBounds(
                (pw - side) / 2,
                (ph - side) / 2,
                side,
                side
            );

            if (_isLoaded && glControl1.Width > 0)
            {
                glControl1.MakeCurrent();
                GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!_isLoaded || _map == null) return;

            float deltaTime = (float)_frameClock.Elapsed.TotalSeconds;
            _frameClock.Restart();

            _game.UpdateWorld(deltaTime);

            glControl1.MakeCurrent();

            _painter.Clear();
            _painter.Draw(_map);

            DrawEnemies();
            DrawBullets();
            

            _painter.Draw(_game.Player1);
            _painter.Draw(_game.Player2);

            glControl1.SwapBuffers();
            UpdateStats();
        }

        private void DrawBullets()
        {
            foreach (var bullet in _game.ActiveBullets) _painter.Draw(bullet);
        }

        private void DrawEnemies()
        {
            foreach (var enemy in _game.Enemies) _painter.Draw(enemy);
        }

        private void UpdateStats()
        {
            pbP1Health.Value = _game.Player1.Health;
            pbP1Armor.Value = _game.Player1.Armor;
            lblP1Ammo.Text = $"{_game.Player1.Ammunition}";
            lblP1Score.Text = $"Score: {_game.Player1.Score}"; 

            pbP2Health.Value = _game.Player2.Health;
            pbP2Armor.Value = _game.Player2.Armor;
            lblP2Ammo.Text = $"{_game.Player2.Ammunition}";
            //lblP2Score.Text = $"Score: {_game.Player2.Score}"; // Новое: вывод очков P2

            pbP1Health.Invalidate();
            pbP1Armor.Invalidate();
            pbP2Health.Invalidate();
            pbP2Armor.Invalidate();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            Keys key = keyData & Keys.KeyCode;
            if (key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right) return false;
            return base.ProcessDialogKey(keyData);
        }

        private void TestWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == _lastProcessedKey) return;

            GameInput gameInput = GameInput.None;
            switch (e.KeyCode)
            {
                case Keys.W: gameInput = GameInput.W; break;
                case Keys.S: gameInput = GameInput.S; break;
                case Keys.A: gameInput = GameInput.A; break;
                case Keys.D: gameInput = GameInput.D; break;
                case Keys.Up: gameInput = GameInput.Up; break;
                case Keys.Down: gameInput = GameInput.Down; break;
                case Keys.Left: gameInput = GameInput.Left; break;
                case Keys.Right: gameInput = GameInput.Right; break;
                case Keys.Space: gameInput = GameInput.Space; break;
                case Keys.Enter: gameInput = GameInput.Enter; break;
            }

            if (gameInput != GameInput.None)
            {
                _lastProcessedKey = e.KeyCode;
                _game.HandleInput(gameInput);
            }
        }

        private void TestWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == _lastProcessedKey) _lastProcessedKey = Keys.None;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _renderer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}