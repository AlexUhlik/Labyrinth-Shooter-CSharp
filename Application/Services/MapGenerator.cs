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

        private static readonly int[] Dx = { 0, 0, -2, 2 };
        private static readonly int[] Dy = { -2, 2, 0, 0 };

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

            var grid = InitializeGrid(width, height);
            var stack = new Stack<(int x, int y)>();

            SetSymmetricEmpty(grid, 1, 1, width, height);
            stack.Push((1, 1));

            while (stack.Count > 0)
            {
                var (currentX, currentY) = stack.Peek();
                var neighbors = GetUnvisitedNeighbors(grid, currentX, currentY, width, height);

                if (neighbors.Count > 0)
                {
                    var (nextX, nextY) = neighbors[_random.Next(neighbors.Count)];

                    int wallX = currentX + (nextX - currentX) / 2;
                    int wallY = currentY + (nextY - currentY) / 2;

                    SetSymmetricEmpty(grid, wallX, wallY, width, height);
                    SetSymmetricEmpty(grid, nextX, nextY, width, height);

                    stack.Push((nextX, nextY));
                }
                else
                {
                    stack.Pop();
                }
            }

            RemoveRandomWallsSymmetrically(grid, width, height, pairsCount: 12);

            grid[width / 2, height / 2] = TileType.Empty;

            return new LabyrinthMap(grid);
        }

        /// <summary>
        /// Удаляет случайные стены, превращая идеальный лабиринт в игровую карту с петлями.
        /// </summary>
        /// <param name="grid">Сетка лабиринта.</param>
        /// <param name="width">Ширина карты.</param>
        /// <param name="height">Высота карты.</param>
        /// <param name="pairsCount">Количество пар стен для удаления.</param>
        private void RemoveRandomWallsSymmetrically(TileType[,] grid, int width, int height, int pairsCount)
        {
            int removed = 0;
            int attempts = 0;
            const int maxAttemptsMultiplier = 20;
            int maxAttempts = pairsCount * maxAttemptsMultiplier;

            while (removed < pairsCount && attempts < maxAttempts)
            {
                attempts++;

                int x = _random.Next(1, width - 1);
                int y = _random.Next(1, height - 1);

                if (grid[x, y] != TileType.Wall) continue;

                var (mirrorX, mirrorY) = GetMirrorCoordinates(x, y, width, height);

                if (CanRemoveWall(grid, x, y, width, height) &&
                    CanRemoveWall(grid, mirrorX, mirrorY, width, height))
                {
                    grid[x, y] = TileType.Empty;
                    grid[mirrorX, mirrorY] = TileType.Empty;
                    removed++;
                }
            }
        }

        /// <summary>
        /// Проверяет возможность превращения стены в проход.
        /// Удаление возможно, если у стены есть минимум 2 пустых соседа,
        /// и после удаления ни один из соседей не окажется изолированным.
        /// </summary>
        private bool CanRemoveWall(TileType[,] grid, int x, int y, int width, int height)
        {
            var emptyNeighbors = GetEmptyNeighbors(grid, x, y, width, height);

            if (emptyNeighbors.Count < 2) return false;

            grid[x, y] = TileType.Empty;

            bool createsIsolation = false;
            foreach (var (nx, ny) in emptyNeighbors)
            {
                if (GetExitCount(grid, nx, ny, width, height) == 0)
                {
                    createsIsolation = true;
                    break;
                }
            }

            grid[x, y] = TileType.Wall;

            return !createsIsolation;
        }

        /// <summary>
        /// Возвращает список пустых соседей для указанной клетки.
        /// </summary>
        private List<(int x, int y)> GetEmptyNeighbors(TileType[,] grid, int x, int y, int width, int height)
        {
            var neighbors = new List<(int x, int y)>();

            if (x > 0 && grid[x - 1, y] == TileType.Empty) neighbors.Add((x - 1, y));
            if (x < width - 1 && grid[x + 1, y] == TileType.Empty) neighbors.Add((x + 1, y));
            if (y > 0 && grid[x, y - 1] == TileType.Empty) neighbors.Add((x, y - 1));
            if (y < height - 1 && grid[x, y + 1] == TileType.Empty) neighbors.Add((x, y + 1));

            return neighbors;
        }

        /// <summary>
        /// Подсчитывает количество выходов из клетки (соседних пустых клеток).
        /// </summary>
        private int GetExitCount(TileType[,] grid, int x, int y, int width, int height)
        {
            int exits = 0;
            if (x > 0 && grid[x - 1, y] == TileType.Empty) exits++;
            if (x < width - 1 && grid[x + 1, y] == TileType.Empty) exits++;
            if (y > 0 && grid[x, y - 1] == TileType.Empty) exits++;
            if (y < height - 1 && grid[x, y + 1] == TileType.Empty) exits++;
            return exits;
        }

        /// <summary>
        /// Проверяет корректность размеров сетки (должны быть нечётными).
        /// </summary>
        private void ValidateDimensions(int width, int height)
        {
            if (width % 2 == 0 || height % 2 == 0)
                throw new ArgumentException($"Ширина и высота должны быть нечётными. Получено: {width}x{height}");
        }

        /// <summary>
        /// Создаёт сетку и заполняет её стенами.
        /// </summary>
        private TileType[,] InitializeGrid(int width, int height)
        {
            var grid = new TileType[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    grid[x, y] = TileType.Wall;
            return grid;
        }

        /// <summary>
        /// Устанавливает значение Empty для клетки и её симметричного отражения.
        /// </summary>
        private void SetSymmetricEmpty(TileType[,] grid, int x, int y, int width, int height)
        {
            grid[x, y] = TileType.Empty;
            var (mirrorX, mirrorY) = GetMirrorCoordinates(x, y, width, height);
            grid[mirrorX, mirrorY] = TileType.Empty;
        }

        /// <summary>
        /// Вычисляет зеркальные координаты относительно центра карты.
        /// Формула: mirrorX = width - 1 - x, mirrorY = height - 1 - y.
        /// </summary>
        private (int x, int y) GetMirrorCoordinates(int x, int y, int width, int height)
        {
            return (width - 1 - x, height - 1 - y);
        }

        /// <summary>
        /// Возвращает список непосещённых соседей на расстоянии 2 клетки.
        /// </summary>
        private List<(int x, int y)> GetUnvisitedNeighbors(TileType[,] grid, int x, int y, int width, int height)
        {
            var neighbors = new List<(int x, int y)>();

            for (int i = 0; i < 4; i++)
            {
                int nx = x + Dx[i];
                int ny = y + Dy[i];

                if (!IsInsideBounds(nx, ny, width, height)) continue;

                var (mirrorX, mirrorY) = GetMirrorCoordinates(nx, ny, width, height);

                if (grid[nx, ny] == TileType.Wall && grid[mirrorX, mirrorY] == TileType.Wall)
                {
                    neighbors.Add((nx, ny));
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Проверяет, находятся ли координаты внутри границ карты (с отступом 1).
        /// </summary>
        private bool IsInsideBounds(int x, int y, int width, int height)
        {
            return x > 0 && x < width - 1 && y > 0 && y < height - 1;
        }
    }
}