using Application.Graphics;
using Application.Services;
using GameCore.Map;
using OpenTK.Graphics;
//using OpenTK.Mathematics;
using System;
using System.Windows.Forms;

namespace LabyrinthGame
{
    public partial class Form1 : Form
    {
        private OpenGlRender _render;
        private LabyrinthMap _map;
        private MapGenerator _generator = new MapGenerator();
        private bool _isLoaded = false;

        public Form1()
        {
            InitializeComponent();

            // Таймер для плавной перерисовки
            Timer timer = new Timer();
            timer.Interval = 16;
            timer.Tick += (s, e) => glControl1.Invalidate();
            timer.Start();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            try
            {
                glControl1.MakeCurrent();

                // 1. Генерируем лабиринт (обязательно нечетные!)
                _map = _generator.GenerateMaze(31, 31);

                // 2. Инициализируем рендер
                _render = new OpenGlRender("Shaders/shader.vert", "Shaders/shader.frag");

                _isLoaded = true;

                // 3. Настраиваем проекцию под размер лабиринта
                UpdateProjection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void UpdateProjection()
        {
            if (_isLoaded && _map != null)
            {
                glControl1.MakeCurrent();
                // Передаем реальные размеры: ширина тайлов * размер одного тайла
                _render.SetupView(glControl1.Width, glControl1.Height, _map.Width(), _map.Height(), LabyrinthMap.TileSize);
            }
        }

        //private void glControl1_Paint(object sender, PaintEventArgs e)
        //{
        //    if (!_isLoaded || _map == null) return;

        //    glControl1.MakeCurrent();

        //    // Очищаем фон (темно-серый)
        //    //_render.Clear(new Color4(0.05f, 0.05f, 0.05f, 1.0f));

        //    _render.Clear(new Color4(255f, 0, 0, 1.0f));

        //    // Проходим по сетке лабиринта
        //    for (int x = 0; x < _map.Width(); x++)
        //    {
        //        for (int y = 0; y < _map.Height(); y++)
        //        {
        //            // Используем твой метод конвертации в мировые координаты
        //            var worldPos = _map.ConvertToWorldCoordinates(x, y);

        //            // Выбираем цвет: Стены — светлее, Пол — почти черный
        //            Color4 tileColor = _map.IsWall(x, y)
        //                ? new Color4(0.3f, 0.3f, 0.3f, 1.0f)
        //                : new Color4(0.1f, 0.1f, 0.1f, 1.0f);

        //            // Рисуем тайл. 
        //            // Вычитаем небольшое значение (например, 20), чтобы была видна сетка между блоками
        //            _render.DrawRect(worldPos.X, worldPos.Y, LabyrinthMap.TileSize, tileColor);
        //        }
        //    }

        //    glControl1.SwapBuffers();
        //}

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!_isLoaded || _map == null) return;

            glControl1.MakeCurrent();

            // Очищаем фон
            _render.Clear(new Color4(255f, 0.1f, 0.1f, 1.0f));

            for (int x = 0; x < _map.Width(); x++)
            {
                for (int y = 0; y < _map.Height(); y++)
                {
                    // Получаем мировые координаты (центр тайла)
                    var worldPos = _map.ConvertToWorldCoordinates(x, y);

                    if (_map.IsWall(x, y))
                    {
                        // Стены: рисуем без вычитания отступов для монолитности
                        _render.DrawRect(worldPos.X, worldPos.Y, LabyrinthMap.TileSize, new Color4(0.3f, 0.3f, 0.3f, 1.0f));
                    }
                    else
                    {
                        // Пол: можно сделать чуть темнее фона или вообще не рисовать
                        _render.DrawRect(worldPos.X, worldPos.Y, LabyrinthMap.TileSize, new Color4(0.05f, 0.05f, 0.05f, 1.0f));
                    }
                }
            }
            //_render.DrawRect(0, 0, 50, Color4.White);

            //// Рисуем квадрат в центре лабиринта
            //float centerX = (_map.Width() * LabyrinthMap.TileSize) / 2f;
            //float centerY = (_map.Height() * LabyrinthMap.TileSize) / 2f;
            //_render.DrawRect(centerX, centerY, 60, Color4.Blue);
            glControl1.SwapBuffers();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            UpdateProjection();
            glControl1.Invalidate();
        }
    }
}