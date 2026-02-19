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

        public const int TileSize = 500;
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
            int x = (int)(position.X / TileSize);
            int y = (int)(position.Y / TileSize);
            return (x, y);
        }
    }
}
