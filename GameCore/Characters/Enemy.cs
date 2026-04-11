using System;
using GameCore.Bullets;
using GameCore.Map;

namespace GameCore.Characters
{
    public class Enemy : Unit
    {
        private static Random _rnd = new Random();

        private int _moveCooldown = 40;
        private int _moveTimer = 0;

        private int _shootCooldown = 30;
        private int _shootTimer = 0;

        public IBullet CurrentBullet { get; set; }

        public Enemy(float x, float y) : base(x, y, 60)
        {
            Health = 80;
            Armor = 50;
            Speed = 2.0f;
            CurrentBullet = new StandartBullet();
        }

        public bool UpdatePosition(LabyrinthMap map, Player p1, Player p2)
        {
            bool wantsToShoot = false;

            bool seesPlayer = CanSeeTarget(p1, map) || CanSeeTarget(p2, map);

            if (seesPlayer)
            {
                _shootTimer++;

                if (_shootTimer >= _shootCooldown)
                {
                    wantsToShoot = true;
                    _shootTimer = 0;
                }

                return wantsToShoot;
            }
            else
            {
                _shootTimer = 0;

                _moveTimer++;
                if (_moveTimer >= _moveCooldown)
                {
                    _moveTimer = 0;

                    int dir = _rnd.Next(4);
                    float dx = 0, dy = 0;

                    if (dir == 0) dy = LabyrinthMap.TileSize;
                    else if (dir == 1) dy = -LabyrinthMap.TileSize;
                    else if (dir == 2) dx = -LabyrinthMap.TileSize;
                    else if (dir == 3) dx = LabyrinthMap.TileSize;

                    var nextPos = new Point(Position.X + dx, Position.Y + dy);
                    var gridCoords = map.ConvertToTileCoordinates(nextPos);

                    if (!map.IsWall(gridCoords.X, gridCoords.Y))
                    {
                        SetDirection(dx, dy);
                        Move(dx, dy);
                    }
                }
            }

            return wantsToShoot;
        }

        private bool CanSeeTarget(Player target, LabyrinthMap map)
        {
            if (target.Health <= 0) return false;

            var enemyTile = map.ConvertToTileCoordinates(Position);
            var targetTile = map.ConvertToTileCoordinates(target.Position);

            if (enemyTile.X == targetTile.X && enemyTile.Y == targetTile.Y) return true;

            int maxDistance = 5;
            if (Math.Abs(enemyTile.X - targetTile.X) > maxDistance ||
                Math.Abs(enemyTile.Y - targetTile.Y) > maxDistance) return false;

            if (Math.Abs(DirectionX) > 0.01f && Math.Abs(DirectionY) < 0.01f)
            {
                if (enemyTile.Y == targetTile.Y) 
                {
                    int deltaX = targetTile.X - enemyTile.X;
                    if (Math.Sign(deltaX) == Math.Sign(DirectionX))
                    {
                        int minX = Math.Min(enemyTile.X, targetTile.X);
                        int maxX = Math.Max(enemyTile.X, targetTile.X);
                        for (int x = minX; x <= maxX; x++)
                            if (map.IsWall(x, enemyTile.Y)) return false;

                        return true;
                    }
                }
            }

            if (Math.Abs(DirectionY) > 0.01f && Math.Abs(DirectionX) < 0.01f)
            {
                if (enemyTile.X == targetTile.X) 
                {
                    int deltaY = targetTile.Y - enemyTile.Y;
                    if (Math.Sign(deltaY) == Math.Sign(DirectionY))
                    {
                        int minY = Math.Min(enemyTile.Y, targetTile.Y);
                        int maxY = Math.Max(enemyTile.Y, targetTile.Y);
                        for (int y = minY; y <= maxY; y++)
                            if (map.IsWall(enemyTile.X, y)) return false;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}