using GameCore.Map;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class MapGenerator
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Генерирует объект LabyrinthMap с процедурно созданным лабиринтом.
        /// </summary>
        public LabyrinthMap GenerateMaze(int width, int height)
        {
            if (width % 2 == 0 || height % 2 == 0)
            {
                throw new ArgumentException("Ширина и высота лабиринта должны быть нечетными для корректной генерации.");
            }

            // 1. Создаем сетку, заполненную стенами
            var grid = new TileType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = TileType.Wall;
                }
            }

            // Алгоритм Recursive Backtracker (используем стек)
            var path = new Stack<(int x, int y)>();

            // 2. Выбираем случайную стартовую точку
            int startX = _random.Next(0, width / 2) * 2 + 1;
            int startY = _random.Next(0, height / 2) * 2 + 1;

            grid[startX, startY] = TileType.Empty;
            path.Push((startX, startY));

            while (path.Count > 0)
            {
                var (currentX, currentY) = path.Peek();
                var neighbors = GetUnvisitedNeighbors(grid, currentX, currentY, width, height);

                if (neighbors.Count > 0)
                {
                    // Выбираем случайного соседа
                    var (nextX, nextY) = neighbors[_random.Next(neighbors.Count)];

                    // Убираем стену между текущей и следующей клеткой
                    int wallX = currentX + (nextX - currentX) / 2;
                    int wallY = currentY + (nextY - currentY) / 2;

                    grid[wallX, wallY] = TileType.Empty;
                    grid[nextX, nextY] = TileType.Empty;

                    path.Push((nextX, nextY));
                }
                else
                {
                    path.Pop();
                }
            }

            // ВОЗВРАЩАЕМ ОБЪЕКТ ВАШЕГО КЛАССА
            return new LabyrinthMap(grid);
        }

        private List<(int x, int y)> GetUnvisitedNeighbors(TileType[,] grid, int x, int y, int w, int h)
        {
            var neighbors = new List<(int x, int y)>();

            // Проверка во всех 4 направлениях на расстоянии 2 клеток
            if (y - 2 >= 0 && grid[x, y - 2] == TileType.Wall) neighbors.Add((x, y - 2));
            if (y + 2 < h && grid[x, y + 2] == TileType.Wall) neighbors.Add((x, y + 2));
            if (x - 2 >= 0 && grid[x - 2, y] == TileType.Wall) neighbors.Add((x - 2, y));
            if (x + 2 < w && grid[x + 2, y] == TileType.Wall) neighbors.Add((x + 2, y));

            return neighbors;
        }
    }
}