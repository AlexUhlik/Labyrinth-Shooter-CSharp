using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Map
{
    public class LabyrinthMap
    {

        public const int TileSize = 50;
        public TileType[,] Grid {  get; private set; }

        public int Width()
        {
            return Grid.GetLength(0);
        }

        public int Height()
        {
            return Grid.GetLength(1);
        }

        public LabyrinthMap(TileType[,] grid)
        {
            if (grid == null)
                throw new ArgumentNullException("Сетка не может быть null");
            Grid = grid;
        }

        public bool IsWall(int x, int y)
        {
            if (x < 0 || x >= Width() && y < 0 || y >= Height())
            {
                return true;
            }
            return Grid[x, y] == TileType.Wall;
        }

        public (int X, int Y) ConvertToTileCoordinates(Point position)
        {
            // Используем Floor, чтобы 499.99 давало 0, а 500.01 давало 1
            int x = (int)Math.Floor(position.X / (float)TileSize);
            int y = (int)Math.Floor(position.Y / (float)TileSize);
            return (x, y);
        }

        public Point ConvertToWorldCoordinates(int gridX, int gridY)
        {
            float worldX = gridX * TileSize + (TileSize / 2f);
            float worldY = gridY * TileSize + (TileSize / 2f);
            return new Point(worldX, worldY);
        }
    }
}
