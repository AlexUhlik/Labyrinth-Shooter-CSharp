using GameCore.Characters;
using GameCore.Map;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LabyrinthGame
{
    public class GameController
    {
        private LabyrinthMap _map;
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        public GameController(LabyrinthMap map)
        {
            _map = map;

            var p1Pos = _map.ConvertToWorldCoordinates(1, 1);
            Player1 = new Player(p1Pos.X, p1Pos.Y) { Size = 20 };

            var p2Pos = _map.ConvertToWorldCoordinates(_map.Width() - 2, _map.Height() - 2);
            Player2 = new Player(p2Pos.X, p2Pos.Y) { Size = 20 };
        }

        public void HandleInput(Keys keyCode)
        {
            Console.WriteLine($"HandleInput вызван с клавишей: {keyCode}, Время: {DateTime.Now:HH:mm:ss.fff}");

            UpdatePlayerPosition(Player1, keyCode, Keys.W, Keys.S, Keys.A, Keys.D);
            UpdatePlayerPosition(Player2, keyCode, Keys.Up, Keys.Down, Keys.Left, Keys.Right);
        }

        private void UpdatePlayerPosition(Player player, Keys pressedKey, Keys up, Keys down, Keys left, Keys right)
        {
            float dx = 0, dy = 0;
            if (pressedKey == up) dy = LabyrinthMap.TileSize;
            else if (pressedKey == down) dy = -LabyrinthMap.TileSize;
            else if (pressedKey == left) dx = -LabyrinthMap.TileSize;
            else if (pressedKey == right) dx = LabyrinthMap.TileSize;

            if (dx != 0 || dy != 0)
            {
                var nextPos = new GameCore.Point(player.Position.X + dx, player.Position.Y + dy);
                var gridCoords = _map.ConvertToTileCoordinates(nextPos);

                if (!_map.IsWall(gridCoords.X, gridCoords.Y))
                {
                    player.SetDirection(dx, dy);
                    player.Move(dx, dy);
                }
            }
        }

    }
}