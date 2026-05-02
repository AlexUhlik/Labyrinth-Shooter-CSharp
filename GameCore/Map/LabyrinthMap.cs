using System;

namespace GameCore.Map
{
    /// <summary>
    /// Представляет логическую структуру лабиринта.
    /// Отвечает за хранение сетки тайлов, проверку коллизий с окружением 
    /// и преобразование координат между миром и сеткой.
    /// </summary>
    public class LabyrinthMap
    {
        /// <summary> Размер стороны одного квадратного тайла. </summary>
        public const int TileSize = 50;

        /// <summary> Двумерный массив, представляющий логическую сетку уровней. </summary>
        public TileType[,] Grid { get; private set; }

        /// <summary> Возвращает количество тайлов по горизонтали. </summary>
        public int Width() => Grid.GetLength(0);

        /// <summary> Возвращает количество тайлов по вертикали. </summary>
        public int Height() => Grid.GetLength(1);

        /// <summary>
        /// Инициализирует новую карту лабиринта на основе заданной сетки.
        /// </summary>
        /// <param name="grid">Двумерный массив типов тайлов.</param>
        public LabyrinthMap(TileType[,] grid)
        {
            Grid = grid ?? throw new ArgumentNullException("Сетка не может быть null");
        }

        /// <summary>
        /// Проверяет, является ли указанная ячейка стеной.
        /// </summary>
        /// <param name="x">Индекс ячейки по горизонтали.</param>
        /// <param name="y">Индекс ячейки по вертикали.</param>
        /// <returns>True, если ячейка — стена или находится за пределами карты.</returns>
        public bool IsWall(int x, int y)
        {
            if (x < 0 || x >= Width() || y < 0 || y >= Height())
            {
                return true;
            }
            return Grid[x, y] == TileType.Wall;
        }

        /// <summary>
        /// Преобразует мировые координаты (Point) в индексы ячеек сетки.
        /// Используется для определения, в каком тайле находится объект.
        /// </summary>
        /// <param name="position">Позиция в мировых координатах.</param>
        /// <returns>Кортеж с целочисленными координатами сетки.</returns>
        public (int X, int Y) ConvertToTileCoordinates(Point position)
        {
            int x = (int)Math.Floor(position.X / (float)TileSize);
            int y = (int)Math.Floor(position.Y / (float)TileSize);
            return (x, y);
        }

        /// <summary>
        /// Преобразует индексы сетки в мировые координаты центра соответствующего тайла.
        /// Используется для спавна объектов точно в центре проходов.
        /// </summary>
        /// <param name="gridX">Индекс по X.</param>
        /// <param name="gridY">Индекс по Y.</param>
        /// <returns>Точка в мировых координатах.</returns>
        public Point ConvertToWorldCoordinates(int gridX, int gridY)
        {
            float worldX = gridX * TileSize + (TileSize / 2f);
            float worldY = gridY * TileSize + (TileSize / 2f);
            return new Point(worldX, worldY);
        }
    }
}