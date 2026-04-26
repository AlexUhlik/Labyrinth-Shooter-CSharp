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
            ValidateDimensions(width, height);

            var grid = InitializeGrid(width, height);
            var stack = new Stack<(int x, int y)>();

            int startX = 1;
            int startY = 1;

            SetSymmetricEmpty(grid, startX, startY, width, height);
            stack.Push((startX, startY));

            while (stack.Count > 0)
            {
                var (currentX, currentY) = stack.Peek();
                var neighbors = GetValidSymmetricNeighbors(grid, currentX, currentY, width, height);

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

            grid[width / 2, height / 2] = TileType.Empty;

            return new LabyrinthMap(grid);
        }

        private void ValidateDimensions(int width, int height)
        {
            if (width % 2 == 0 || height % 2 == 0)
            {
                throw new ArgumentException("Размеры должны быть нечетными.");
            }
        }

        private TileType[,] InitializeGrid(int width, int height)
        {
            var grid = new TileType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = TileType.Wall;
                }
            }
            return grid;
        }

        private void SetSymmetricEmpty(TileType[,] grid, int x, int y, int w, int h)
        {
            grid[x, y] = TileType.Empty;
            var (mirrorX, mirrorY) = GetMirrorCoordinates(x, y, w, h);
            grid[mirrorX, mirrorY] = TileType.Empty;
        }

        private (int x, int y) GetMirrorCoordinates(int x, int y, int w, int h)
        {
            return (w - 1 - x, h - 1 - y);
        }

        private List<(int x, int y)> GetValidSymmetricNeighbors(TileType[,] grid, int x, int y, int w, int h)
        {
            var neighbors = new List<(int x, int y)>();
            int[] dx = { 0, 0, -2, 2 };
            int[] dy = { -2, 2, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (IsInsideBounds(nx, ny, w, h))
                {
                    var (mx, my) = GetMirrorCoordinates(nx, ny, w, h);

                    if (grid[nx, ny] == TileType.Wall && grid[mx, my] == TileType.Wall)
                    {
                        neighbors.Add((nx, ny));
                    }
                }
            }
            return neighbors;
        }

        private bool IsInsideBounds(int x, int y, int w, int h)
        {
            return x > 0 && x < w - 1 && y > 0 && y < h - 1;
        }
    }
}