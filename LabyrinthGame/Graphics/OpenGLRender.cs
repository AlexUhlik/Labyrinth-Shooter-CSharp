//// Presentation/Graphics/OpenGLRenderer.cs

//using GameCore.Map; // Нам нужен LabyrinthMap
//using OpenTK.Graphics.OpenGL;
//using System;
//using System.Drawing;
//using System.Drawing.Imaging;

//namespace LabyrinthGame.Graphics
//{
//    public class OpenGLRenderer
//    {
//        // "ID" текстур, которые мы загрузим в память видеокарты
//        private int wallTextureId;
//        private int floorTextureId;

//        /// <summary>
//        /// Этот метод вызывается один раз при загрузке GLControl.
//        /// Здесь мы загружаем текстуры и настраиваем базовые параметры.
//        /// </summary>
//        public void Load()
//        {
//            // Загружаем наши текстуры из файлов.
//            // Убедитесь, что у вас есть файлы wall.png и floor.png в папке с игрой.
//            wallTextureId = LoadTexture("Textures/wall.png");
//            floorTextureId = LoadTexture("Textures/floor.png");

//            // Включаем поддержку 2D-текстур
//            GL.Enable(EnableCap.Texture2D);
//        }

//        /// <summary>
//        /// Главный метод отрисовки лабиринта.
//        /// </summary>
//        /// <param name="map">Объект карты, который нужно нарисовать.</param>
//        public void DrawLabyrinth(LabyrinthMap map)
//        {
//            if (map == null) return;

//            // Пробегаемся по всей сетке карты
//            for (int x = 0; x < map.Width; x++)
//            {
//                for (int y = 0; y < map.Height; y++)
//                {
//                    // Рассчитываем мировые координаты для текущей клетки
//                    float worldX = x * LabyrinthMap.TileSize;
//                    float worldY = y * LabyrinthMap.TileSize;

//                    // В зависимости от типа клетки, выбираем нужную текстуру
//                    if (map.Grid[x, y] == TileType.Wall)
//                    {
//                        // Рисуем квадрат (квад) с текстурой стены
//                        DrawTexturedQuad(worldX, worldY, LabyrinthMap.TileSize, wallTextureId);
//                    }
//                    else
//                    {
//                        // Рисуем квадрат с текстурой пола
//                        DrawTexturedQuad(worldX, worldY, LabyrinthMap.TileSize, floorTextureId);
//                    }
//                }
//            }
//        }

//        // --- Вспомогательные методы ---

//        /// <summary>
//        /// Рисует один квадрат с наложенной на него текстурой.
//        /// </summary>
//        private void DrawTexturedQuad(float x, float y, float size, int textureId)
//        {
//            // "Активируем" текстуру, которую хотим использовать
//            GL.BindTexture(TextureTarget.Texture2D, textureId);

//            // Начинаем рисовать примитив "Квадрат"
//            // (используем устаревший, но очень простой для 2D-игр "immediate mode")
//            GL.Begin(PrimitiveType.Quads);

//            // Задаем 4 вершины квадрата и для каждой указываем,
//            // какая точка текстуры ей соответствует.

//            // Верхний левый угол
//            GL.TexCoord2(0, 0); // (0,0) - верхний левый угол текстуры
//            GL.Vertex2(x, y);

//            // Верхний правый угол
//            GL.TexCoord2(1, 0); // (1,0) - верхний правый угол текстуры
//            GL.Vertex2(x + size, y);

//            // Нижний правый угол
//            GL.TexCoord2(1, 1); // (1,1) - нижний правый угол текстуры
//            GL.Vertex2(x + size, y + size);

//            // Нижний левый угол
//            GL.TexCoord2(0, 1); // (0,1) - нижний левый угол текстуры
//            GL.Vertex2(x, y + size);

//            GL.End();
//        }

//        /// <summary>
//        /// Загружает изображение из файла и создает для него текстуру OpenGL.
//        /// </summary>
//        private int LoadTexture(string filePath)
//        {
//            if (!File.Exists(filePath))
//                throw new FileNotFoundException("Файл текстуры не найден", filePath);

//            // 1. Генерируем "ID" для нашей будущей текстуры
//            int id = GL.GenTexture();
//            GL.BindTexture(TextureTarget.Texture2D, id);

//            // 2. Загружаем изображение с помощью стандартной библиотеки C#
//            Bitmap bmp = new Bitmap(filePath);
//            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
//                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

//            // 3. Отправляем данные изображения в память видеокарты
//            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
//                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

//            bmp.UnlockBits(data);

//            // 4. Настраиваем параметры текстуры
//            // GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
//            // GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
//            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest); // Четкие пиксели
//            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest); // при увеличении

//            return id;
//        }
//    }
//}
