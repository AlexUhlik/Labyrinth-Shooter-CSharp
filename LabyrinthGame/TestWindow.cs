using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using Application.Services;
using Application.Game;
using GameCore.Map;
using GameCore.Characters;
using OpenTK.Graphics.OpenGL4;
using DrawLib.Graphics;

namespace LabyrinthGame
{
    /// <summary>
    /// Главное окно игры, объединяющее графический интерфейс WinForms и рендеринг на OpenTK.
    /// Управляет жизненным циклом игры, вводом пользователя и отображением панелей меню.
    /// </summary>
    public partial class TestWindow : Form
    {
        // Ядро игры и рендеринга
        private GameRenderer _renderer;
        private Painter _painter;
        private LabyrinthMap _map;
        private GameController _game;

        // Состояние системы
        private bool _isLoaded;
        private bool _isPaused;

        /// <summary>
        /// Предотвращает залипание клавиш и повторную обработку в одном кадре.
        /// </summary>
        private Keys _lastProcessedKey = Keys.None;

        /// <summary>
        /// Высокоточный таймер для расчета времени между кадрами (DeltaTime).
        /// </summary>
        private readonly Stopwatch _frameClock = new Stopwatch();

        public TestWindow()
        {
            InitializeComponent();
            SetupWindowStyle();
            StartGameLoop();
        }

        /// <summary>
        /// Настраивает окно на полноэкранный режим без рамок.
        /// </summary>
        private void SetupWindowStyle()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            KeyPreview = true; 
        }

        /// <summary>
        /// Инициализирует и запускает цикл перерисовки окна.
        /// </summary>
        private void StartGameLoop()
        {
            // Интервал 16мс примерно соответствует 60 кадрам в секунду
            var timer = new Timer { Interval = 16 };
            timer.Tick += (s, e) => { if (_isLoaded) glControl1.Invalidate(); };
            timer.Start();
            _frameClock.Start();
        }

        /// <summary>
        /// Обработчик события загрузки OpenGL-контекста. 
        /// Здесь происходит создание игрового мира и загрузка текстур.
        /// </summary>
        private void glControl1_Load(object sender, EventArgs e)
        {
            try
            {
                glControl1.MakeCurrent();

                _renderer = new GameRenderer();
                _renderer.Init();

                // Генерация карты и инициализация контроллера
                _map = new MapGenerator().GenerateMaze(21, 21);
                _painter = new Painter(_renderer);
                _game = new GameController(_map);

                _painter.LoadAssets();
                _painter.SetupCamera(_map);

                _isLoaded = true;
                UpdateUIElements();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации графики: {ex.Message}");
            }
        }

        /// <summary>
        /// Основной метод кадра. Выполняет обновление физики и отрисовку сцены.
        /// </summary>
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!_isLoaded || _map == null) return;

            // Расчет времени кадра
            float deltaTime = (float)_frameClock.Elapsed.TotalSeconds;
            _frameClock.Restart();

            // Обновление логики, если игра не на паузе
            if (!_isPaused && !_game.IsGameOver)
                _game.UpdateWorld(deltaTime);
            else if (_game.IsGameOver)
                ShowEndScreen();

            RenderFrame();

            if (!_isPaused)
                UpdateStats();
        }

        /// <summary>
        /// Отрисовка игровых объектов через Painter.
        /// </summary>
        private void RenderFrame()
        {
            glControl1.MakeCurrent();
            _painter.Clear();
            _painter.Draw(_map);
            _painter.DrawObjects(_game.GameObjects);
            glControl1.SwapBuffers();
        }

        /// <summary>
        /// Переключает видимость экрана завершения игры и выводит результаты.
        /// </summary>
        private void ShowEndScreen()
        {
            if (pnlGameOver.Visible) return;

            _isPaused = false;
            pnlPause.Visible = false;

            var (text, color) = GetWinnerInfo();
            lblWinner.Text = text;
            lblWinner.ForeColor = color;
            lblFinalScore.Text = $"Счёт: {_game.Player1.Score} - {_game.Player2.Score}";

            pnlGameOver.Visible = true;
            pnlGameOver.BringToFront();
        }

        /// <summary>
        /// Определяет победителя на основе текущего счета игроков.
        /// </summary>
        private (string text, Color color) GetWinnerInfo()
        {
            int s1 = _game.Player1.Score;
            int s2 = _game.Player2.Score;

            if (s1 > s2) return ("ПОБЕДИЛ СИНИЙ ИГРОК", Color.LightSkyBlue);
            if (s2 > s1) return ("ПОБЕДИЛ КРАСНЫЙ ИГРОК", Color.Tomato);
            return ("НИЧЬЯ!", Color.White);
        }

        /// <summary>
        /// Переключает состояние паузы и останавливает/запускает игровой таймер.
        /// </summary>
        private void TogglePause()
        {
            if (_game.IsGameOver) return;

            _isPaused = !_isPaused;
            pnlPause.Visible = _isPaused;

            if (_isPaused)
            {
                pnlPause.BringToFront();
                _frameClock.Stop();
            }
            else
            {
                _frameClock.Start();
                glControl1.Focus();
            }
        }

        /// <summary>
        /// Пересчитывает размеры и позиции UI элементов (кнопок, панелей, игрового поля).
        /// Гарантирует, что игровое поле останется квадратным.
        /// </summary>
        private void UpdateUIElements()
        {
            int pw = GameContainer.Width;
            int ph = GameContainer.Height;
            if (pw <= 0 || ph <= 0) return;

            int side = Math.Min(pw, ph);
            glControl1.SetBounds((pw - side) / 2, (ph - side) / 2, side, side);

            if (_isLoaded && glControl1.Width > 0)
            {
                glControl1.MakeCurrent();
                GL.Viewport(0, 0, glControl1.Width, glControl1.Height);

                _painter.SetupCamera(_map);
            }

            UpdatePanelLayouts(pw, ph);
        }

        /// <summary>
        /// Вспомагательный метод позиционирования панелей
        /// </summary>
        private void UpdatePanelLayouts(int pw, int ph)
        {
            int targetWidth = (int)(pw * 0.6);
            int targetHeight = (int)(ph * 0.5);
            pnlGameOver.Width = Math.Max(targetWidth, 380);
            pnlGameOver.Height = Math.Max(targetHeight, 280);
            pnlGameOver.Left = (pw - pnlGameOver.Width) / 2;
            pnlGameOver.Top = (ph - pnlGameOver.Height) / 2;

            lblWinner.Left = (pnlGameOver.Width - lblWinner.Width) / 2;
            lblWinner.Top = 40;
            lblFinalScore.Left = (pnlGameOver.Width - lblFinalScore.Width) / 2;
            lblFinalScore.Top = 100;

            int btnWidth = (pnlGameOver.Width / 2) - 40;
            btnRestart.Width = btnWidth;
            btnExit.Width = btnWidth;

            int buttonsTop = pnlGameOver.Height - btnRestart.Height - 40;
            btnRestart.Top = buttonsTop;
            btnExit.Top = buttonsTop;
            btnRestart.Left = 30;
            btnExit.Left = pnlGameOver.Width - btnExit.Width - 30;

            int pauseWidth = Math.Max(250, (int)(pw * 0.3));
            int pauseHeight = Math.Max(180, (int)(ph * 0.2));
            pnlPause.Width = pauseWidth;
            pnlPause.Height = pauseHeight;
            pnlPause.Left = (pw - pnlPause.Width) / 2;
            pnlPause.Top = (ph - pnlPause.Height) / 2;

            btnContinue.Width = pnlPause.Width - 40;
            btnFinish.Width = pnlPause.Width - 40;
            btnContinue.Left = (pnlPause.Width - btnContinue.Width) / 2;
            btnFinish.Left = (pnlPause.Width - btnFinish.Width) / 2;

            int totalPauseBtnsHeight = btnContinue.Height + btnFinish.Height + 20;
            btnContinue.Top = (pnlPause.Height - totalPauseBtnsHeight) / 2;
            btnFinish.Top = btnContinue.Bottom + 20;
        }

        /// <summary>
        /// Обновляет значения полосок здоровья, брони и текстовых меток для обоих игроков.
        /// </summary>
        private void UpdateStats()
        {
            UpdatePlayerStats(pbP1Health, pbP1Armor, P1Ammo, P1Score, _game.Player1);
            UpdatePlayerStats(pbP2Health, pbP2Armor, P2Ammo, P2Score, _game.Player2);
        }

        private void UpdatePlayerStats(CustomProgressBar hp, CustomProgressBar arm, Label ammo, Label score, Player p)
        {
            hp.Value = p.Health;
            arm.Value = p.Armor;
            ammo.Text = $"Ammo: {p.Ammunition}";
            score.Text = $"Score: {p.Score}";

            // Перерисовка кастомных прогресс-баров
            hp.Invalidate();
            arm.Invalidate();
        }

        /// <summary>
        /// Обрабатывает нажатия клавиш. Поддерживает управление для двух игроков.
        /// </summary>
        private void TestWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { TogglePause(); return; }
            if (_isPaused || _game.IsGameOver || e.KeyCode == _lastProcessedKey) return;

            var input = MapKeyToInput(e.KeyCode);
            if (input != GameInput.None)
            {
                _lastProcessedKey = e.KeyCode;
                _game.HandleInput(input);
            }
        }

        /// <summary>
        /// Конвертирует клавиши Windows Forms в перечисление игровых команд.
        /// </summary>
        private GameInput MapKeyToInput(Keys key)
        {
            GameInput gameInput = GameInput.None;

            switch (key)
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
            return gameInput;
        }

        private void TestWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == _lastProcessedKey) _lastProcessedKey = Keys.None;
        }

        /// <summary>
        /// Переопределение для предотвращения смены фокуса между кнопками при нажатии стрелок.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            var key = keyData & Keys.KeyCode;
            if (key >= Keys.Left && key <= Keys.Down) return false;
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Освобождает ресурсы OpenGL при закрытии приложения.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _renderer?.Dispose();
            base.OnFormClosing(e);
        }

        private void GameContainer_Layout(object sender, LayoutEventArgs e) => UpdateUIElements();
        private void btnContinue_Click_1(object sender, EventArgs e) => TogglePause();
        private void btnFinish_Click(object sender, EventArgs e) => _game.FinishGameManually();
        private void btnExit_Click(object sender, EventArgs e) => Close();

        /// <summary>
        /// Сбрасывает состояние игрового контроллера для начала нового раунда.
        /// </summary>
        private void btnRestart_Click(object sender, EventArgs e)
        {
            pnlGameOver.Visible = false;
            _game.Reset();
            glControl1.Focus();
            _frameClock.Restart();
        }
    }
}