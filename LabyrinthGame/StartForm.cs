using System;
using System.Drawing;
using System.Windows.Forms;

namespace LabyrinthGame
{
    /// <summary>
    /// Главное стартовое меню игры. 
    /// Отвечает за инициализацию приложения, настройку визуального стиля и запуск игрового процесса.
    /// </summary>
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            ApplyCustomStyles();
        }

        /// <summary>
        /// Настраивает внешний вид формы: убирает рамки, максимизирует и задает фон.
        /// </summary>
        private void ApplyCustomStyles()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;

            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            AlignElements();
        }

        /// <summary>
        /// Динамически выравнивает все UI-элементы по центру и краям экрана 
        /// в зависимости от текущего разрешения монитора.
        /// </summary>
        private void AlignElements()
        {
            if (lblTitle == null) return;

            // Центральные кнопки
            btnStart.Left = (this.Width - btnStart.Width) / 2;
            btnStart.Top = (this.Height - btnStart.Height) / 2 + 50;

            btnExit.Left = (this.Width - btnExit.Width) / 2;
            btnExit.Top = btnStart.Bottom + 25;

            // Заголовок 
            lblTitle.Left = (this.Width - lblTitle.Width) / 2;
            lblTitle.Top = (int)(this.Height * 0.15f);

            // Параметры изображений управления
            int controlWidth = (int)(this.Width * 0.25f);
            int controlHeight = (int)(this.Height * 0.4f);
            int edgePadding = 80;
            int yPos = btnStart.Top - 50;

            // Игрок 1 (Слева)
            picControlsP1.Size = new Size(controlWidth, controlHeight);
            picControlsP1.Left = edgePadding;
            picControlsP1.Top = yPos;

            // Игрок 2 (Справа)
            picControlsP2.Size = new Size(controlWidth, controlHeight);
            picControlsP2.Left = this.Width - picControlsP2.Width - edgePadding;
            picControlsP2.Top = yPos;

            // Центрирование подписей над картинками
            lblPlayer1Info.Left = picControlsP1.Left + (picControlsP1.Width - lblPlayer1Info.Width) / 2 + 7;
            lblPlayer1Info.Top = picControlsP1.Top;

            lblPlayer2Info.Left = picControlsP2.Left + (picControlsP2.Width - lblPlayer2Info.Width) / 2 + 7;
            lblPlayer2Info.Top = picControlsP2.Top;
        }

        /// <summary>
        /// Обработчик запуска игры. Создает окно TestWindow и скрывает текущее меню.
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            TestWindow gameWindow = new TestWindow();

            // Передаем параметры текущего окна, чтобы переход был бесшовным
            gameWindow.Owner = this;
            gameWindow.StartPosition = FormStartPosition.Manual;
            gameWindow.Location = this.Location;
            gameWindow.Size = this.Size;

            gameWindow.FormClosed += (s, args) =>
            {
                this.Show();
                this.Update();
                AlignElements(); 
            };

            gameWindow.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Пересчитывает позиции элементов при изменении размера формы.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AlignElements();
        }

        /// <summary>
        /// Оптимизация отрисовки формы на уровне ОС.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // Флаг WS_EX_COMPOSITED (0x02000000) включает двойную буферизацию для всех дочерних элементов формы.
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}