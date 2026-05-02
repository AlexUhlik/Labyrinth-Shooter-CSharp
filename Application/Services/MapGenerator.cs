using System;
using System.Collections.Generic;
using GameCore.Map;

namespace Application.Services
{
    /// <summary>
    /// Сервис процедурной генерации игрового лабиринта. 
    /// Реализует модифицированный алгоритм "Recursive Backtracker", 
    /// адаптированный для обеспечения центральной симметрии игрового поля.
    /// </summary>
    public class MapGenerator
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Выполняет полную генерацию симметричного лабиринта.
        /// </summary>
        /// <param name="width">Ширина карты (должна быть нечетным числом).</param>
        /// <param name="height">Высота карты (должна быть нечетным числом).</param>
        /// <returns>Экземпляр LabyrinthMap с заполненной сеткой тайлов.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если размеры четные.</exception>
        public LabyrinthMap GenerateMaze(int width, int height)
        {
            ValidateDimensions(width, height);

            // Инициализация сетки (все клетки изначально — стены)
            // grid - двумерный массив типов тайлов, представляющий каркас карты.
            var grid = InitializeGrid(width, height);

            // stack - хранит координаты посещенных ячеек, из которых еще можно пойти вглубь.
            var stack = new Stack<(int x, int y)>();

            // Точка начала генерации
            int startX = 1;
            int startY = 1;

            SetSymmetricEmpty(grid, startX, startY, width, height);
            stack.Push((startX, startY));

            // Основной цикл генерации
            while (stack.Count > 0)
            {
                var (currentX, currentY) = stack.Peek();

                // neighbors - список доступных соседних ячеек, которые еще не были посещены ни игроком, ни генератором.
                var neighbors = GetValidSymmetricNeighbors(grid, currentX, currentY, width, height);

                if (neighbors.Count > 0)
                {
                    // Выбираем случайного соседа из доступных
                    var (nextX, nextY) = neighbors[_random.Next(neighbors.Count)];

                    // Координаты стены, которую нужно убрать между текущей и следующей клеткой.
                    int wallX = currentX + (nextX - currentX) / 2;
                    int wallY = currentY + (nextY - currentY) / 2;

                    // Убираем стену и саму следующую клетку симметрично
                    SetSymmetricEmpty(grid, wallX, wallY, width, height);
                    SetSymmetricEmpty(grid, nextX, nextY, width, height);

                    // Переходим в следующую ячейку
                    stack.Push((nextX, nextY));
                }
                else
                {
                    // Если у текущей ячейки нет доступных соседей, возвращаемся назад по стеку
                    stack.Pop();
                }
            }

            // Удаляем 12 дополнительных пар стен для создания нелинейных маршрутов.
            RemoveRandomWallsSymmetrically(grid, width, height, 12);

            grid[width / 2, height / 2] = TileType.Empty;

            return new LabyrinthMap(grid);
        }

        /// <summary>
        /// Удаляет случайные стены, превращая идеальный лабиринт в игровую карту с петлями.
        /// </summary>
        /// <param name="pairsCount">Количество пар стен для удаления.</param>
        private void RemoveRandomWallsSymmetrically(TileType[,] grid, int w, int h, int pairsCount)
        {
            int removed = 0;
            int attempts = 0;
            int maxAttempts = pairsCount * 20;

            while (removed < pairsCount && attempts < maxAttempts)
            {
                attempts++;
                int x = _random.Next(1, w - 1);
                int y = _random.Next(1, h - 1);

                if (grid[x, y] == TileType.Wall)
                {
                    var (mx, my) = GetMirrorCoordinates(x, y, w, h);

                    // Проверка, что удаление стены не создаст изолированную пустую клетку, окруженную стенами.
                    if (CanRemoveWall(grid, x, y, w, h) && CanRemoveWall(grid, mx, my, w, h))
                    {
                        grid[x, y] = TileType.Empty;
                        grid[mx, my] = TileType.Empty;
                        removed++;
                    }
                }
            }
        }

        /// <summary>
        /// Проверяет возможность превращения стены в проход.
        /// </summary>
        private bool CanRemoveWall(TileType[,] grid, int x, int y, int w, int h)
        {
            // emptyNeighbors - счетчик соседних пустых клеток.
            int emptyNeighbors = 0;
            if (x > 0 && grid[x - 1, y] == TileType.Empty) emptyNeighbors++;
            if (x < w - 1 && grid[x + 1, y] == TileType.Empty) emptyNeighbors++;
            if (y > 0 && grid[x, y - 1] == TileType.Empty) emptyNeighbors++;
            if (y < h - 1 && grid[x, y + 1] == TileType.Empty) emptyNeighbors++;

            return emptyNeighbors >= 2;
        }

        /// <summary> Проверяет корректность размеров сетки. </summary>
        private void ValidateDimensions(int width, int height)
        {
            if (width % 2 == 0 || height % 2 == 0)
                throw new ArgumentException("Для работы алгоритма требуются нечетные значения ширины и высоты.");
        }

        /// <summary> Создает массив и заполняет его тайлами типа Wall. </summary>
        private TileType[,] InitializeGrid(int width, int height)
        {
            var grid = new TileType[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    grid[x, y] = TileType.Wall;
            return grid;
        }

        /// <summary> Устанавливает значение Empty для клетки и её зеркального отражения. </summary>
        private void SetSymmetricEmpty(TileType[,] grid, int x, int y, int w, int h)
        {
            grid[x, y] = TileType.Empty;
            var (mirrorX, mirrorY) = GetMirrorCoordinates(x, y, w, h);
            grid[mirrorX, mirrorY] = TileType.Empty;
        }

        /// <summary> 
        /// Вычисляет зеркальные координаты.
        /// Формула: mx = (W-1) - x; my = (H-1) - y;
        /// </summary>
        private (int x, int y) GetMirrorCoordinates(int x, int y, int w, int h)
        {
            return (w - 1 - x, h - 1 - y);
        }

        /// <summary> Возвращает список доступных для посещения соседей на расстоянии 2 тайлов. </summary>
        private List<(int x, int y)> GetValidSymmetricNeighbors(TileType[,] grid, int x, int y, int w, int h)
        {
            var neighbors = new List<(int x, int y)>();
            // dx/dy - векторы смещения для проверки четырех направлений
            int[] dx = { 0, 0, -2, 2 };
            int[] dy = { -2, 2, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (IsInsideBounds(nx, ny, w, h))
                {
                    var (mx, my) = GetMirrorCoordinates(nx, ny, w, h);
                    // Ячейка считается валидной, если и она, и её отражение еще покрыты стенами
                    if (grid[nx, ny] == TileType.Wall && grid[mx, my] == TileType.Wall)
                        neighbors.Add((nx, ny));
                }
            }
            return neighbors;
        }

        /// <summary> Проверяет, не выходят ли координаты за пределы карты. </summary>
        private bool IsInsideBounds(int x, int y, int w, int h)
        {
            return x > 0 && x < w - 1 && y > 0 && y < h - 1;
        }
    }
}