using System;
using System.Collections.Generic;
using GameCore.Map;

namespace Application.Services
{
    public class MapGenerator
    {
        private readonly Random _random = new Random();

        public LabyrinthMap GenerateMaze(int width, int height)
        {
            if (width % 2 == 0 || height % 2 == 0)
                throw new ArgumentException("Размеры должны быть нечетными.");

            var grid = new TileType[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    grid[x, y] = TileType.Wall;

            var path = new Stack<(int x, int y)>();

            // Стартовые точки для двух игроков (зеркальные углы)
            int startX = 1, startY = 1;
            grid[startX, startY] = TileType.Empty;
            grid[width - 1 - startX, height - 1 - startY] = TileType.Empty;

            path.Push((startX, startY));

            while (path.Count > 0)
            {
                var (cx, cy) = path.Peek();
                var neighbors = GetValidSymmetricNeighbors(grid, cx, cy, width, height);

                if (neighbors.Count > 0)
                {
                    var (nx, ny) = neighbors[_random.Next(neighbors.Count)];

                    int wx = cx + (nx - cx) / 2;
                    int wy = cy + (ny - cy) / 2;

                    // Пробиваем основные клетки
                    grid[wx, wy] = TileType.Empty;
                    grid[nx, ny] = TileType.Empty;

                    // Пробиваем зеркальные клетки
                    grid[width - 1 - wx, height - 1 - wy] = TileType.Empty;
                    grid[width - 1 - nx, height - 1 - ny] = TileType.Empty;

                    path.Push((nx, ny));
                }
                else
                {
                    path.Pop();
                }
            }

            grid[width / 2, height / 2] = TileType.Empty; // Гарантируем проход через центр

            return new LabyrinthMap(grid);
        }

        private List<(int x, int y)> GetValidSymmetricNeighbors(TileType[,] grid, int x, int y, int w, int h)
        {
            var neighbors = new List<(int x, int y)>();
            int[] dx = { 0, 0, -2, 2 }, dy = { -2, 2, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i], ny = y + dy[i];

                if (nx > 0 && nx < w - 1 && ny > 0 && ny < h - 1)
                {
                    // Проверяем, чтобы и сама клетка, и её зеркальное отражение были стенами
                    if (grid[nx, ny] == TileType.Wall && grid[w - 1 - nx, h - 1 - ny] == TileType.Wall)
                        neighbors.Add((nx, ny));
                }
            }
            return neighbors;
        }
    }
}