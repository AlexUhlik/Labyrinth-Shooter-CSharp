using Application.Services;
using Application.Game;
using GameCore.Characters;
using GameCore.Map;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using DrawLib.Graphics;

namespace LabyrinthGame
{
    public partial class Form1 : Form
    {
        private GameRenderer _renderer;
        private Painter _painter;

        private LabyrinthMap _map;
        private MapGenerator _generator;
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
                _generator = new MapGenerator();

                //ShowDialog(new TestWindow());
                _renderer.Init();
                _map = _generator.GenerateMaze(31, 31);

                _painter = new Painter(_renderer);
                _game = new GameController(_map);

                _painter.LoadAssets();
                _painter.SetupCamera(_map);

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
            _game.UpdatePhysics();

            glControl1.MakeCurrent();
            _painter.Clear();

            _painter.Draw(_map);

            DrawEnemies();

            _painter.Draw(_game.Player1);
            _painter.Draw(_game.Player2);

            DrawBullets();


            //_painter.DrawBullets(_game.ActiveBullets);




            glControl1.SwapBuffers();
            UpdateStats();
        }

        private void DrawBullets()
        {
            foreach (var bullet in _game.ActiveBullets)
            {
                _painter.Draw(bullet);
            }
        }

        private void DrawEnemies()
        {
            foreach (var enemy in _game.Enemies)
            {
                _painter.Draw(enemy);
            }
        }

        private void UpdateStats()
        {
            pbP1Health.Value = _game.Player1.Health;
            pbP1Armor.Value = _game.Player1.Armor;
            lblP1Ammo.Text = $"{_game.Player1.Ammunition}";

            pbP2Health.Value = _game.Player2.Health;
            pbP2Armor.Value = _game.Player2.Armor;
            lblP2Ammo.Text = $"{_game.Player2.Ammunition}";

            pbP1Health.Invalidate();
            pbP1Armor.Invalidate();

            pbP2Health.Invalidate();
            pbP2Armor.Invalidate();
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

            GameInput gameInput = GameInput.None;

            switch (e.KeyCode)
            {
                case Keys.W:
                    gameInput = GameInput.W;
                    break;
                case Keys.S:
                    gameInput = GameInput.S;
                    break;
                case Keys.A:
                    gameInput = GameInput.A;
                    break;
                case Keys.D:
                    gameInput = GameInput.D;
                    break;
                case Keys.Up:
                    gameInput = GameInput.Up;
                    break;
                case Keys.Down:
                    gameInput = GameInput.Down;
                    break;
                case Keys.Left:
                    gameInput = GameInput.Left;
                    break;
                case Keys.Right:
                    gameInput = GameInput.Right;
                    break;
                case Keys.Space:
                    gameInput = GameInput.Space;
                    //_game.HandleInput(GameInput.Space); 
                    break;
                case Keys.Enter:
                    gameInput = GameInput.Enter;
                    //_game.HandleInput(GameInput.Enter); 
                    break;
            }

            if (gameInput != GameInput.None)
            {
                _lastProcessedKey = e.KeyCode;
                _game.HandleInput(gameInput);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == _lastProcessedKey)
                _lastProcessedKey = Keys.None;
        }
    }
}