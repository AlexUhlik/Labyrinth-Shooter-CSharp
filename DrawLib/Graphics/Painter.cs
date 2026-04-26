using GameCore;
using GameCore.Bullets;
using GameCore.Characters;
using GameCore.Map;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;
using GameCore.Items;

namespace DrawLib.Graphics
{
    public class Painter
    {
        private readonly GameRenderer _renderer;

        private static readonly Color4 Player1Color = new Color4(0.0f, 0.5f, 1.0f, 1.0f);
        private static readonly Color4 Player2Color = new Color4(1.0f, 0.2f, 0.2f, 1.0f);

        private static readonly Color4 BackgroundColor = new Color4(0.05f, 0.05f, 0.05f, 1.0f);
        private static readonly Color4 Base1Color = new Color4(0.0f, 0.5f, 1.0f, 0.4f);
        private static readonly Color4 Base2Color = new Color4(1.0f, 0.2f, 0.2f, 0.4f);

        private static readonly Color4 WallColor = new Color4(0.3f, 0.3f, 0.3f, 1.0f);
        private static readonly Color4 EmptyColor = new Color4(1f, 1f, 1f, 1f);

        private int _texPlayer1;
        private int _texPlayer2;
        private int _texWall;
        private int _texFloor;
        private int _texEnemy;
        private int _texBullet;
        private int _texAmmoPrize;
        private int _texExplosivePrize;
        private int _texFastPrize;
        private int _texHealthPrize;

        public Painter(GameRenderer renderer)
        {
            _renderer = renderer;
        }

        private string GetAssetPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Textures", fileName);
        }

        public void LoadAssets()
        {
            _texPlayer1 = _renderer.LoadTexture(GetAssetPath("player.png"));
            _texPlayer2 = _renderer.LoadTexture(GetAssetPath("magician.png"));
            _texWall = _renderer.LoadTexture(GetAssetPath("wall.png"));
            _texFloor = _renderer.LoadTexture(GetAssetPath("floor.png"));
            _texEnemy = _renderer.LoadTexture(GetAssetPath("enemy.png"));
            _texBullet = _renderer.LoadTexture(GetAssetPath("standartBullet.png"));

            _texAmmoPrize = _renderer.LoadTexture(GetAssetPath("bullet.png"));
            _texExplosivePrize = _renderer.LoadTexture(GetAssetPath("fastPrize.png"));
            _texFastPrize = _renderer.LoadTexture(GetAssetPath("fastPrize.png"));
            _texHealthPrize = _renderer.LoadTexture(GetAssetPath("medkit.png"));
        }
        public void SetupCamera(LabyrinthMap map)
        {
            float mazeWidth = map.Width() * LabyrinthMap.TileSize;
            float mazeHeight = map.Height() * LabyrinthMap.TileSize;
            var projection = Matrix4.CreateOrthographicOffCenter(0, mazeWidth, 0, mazeHeight, -1, 1);
            _renderer.SetMatrices(Matrix4.Identity, projection);
        }

        public void Draw(LabyrinthMap map)
        {
            for (int x = 0; x < map.Width(); x++)
            {
                for (int y = 0; y < map.Height(); y++)
                {
                    var worldPos = map.ConvertToWorldCoordinates(x, y);
                    if (map.IsWall(x, y))
                    {
                        _renderer.DrawSquare(worldPos.X, worldPos.Y, LabyrinthMap.TileSize, LabyrinthMap.TileSize,
                                      WallColor, _texWall);
                    }
                    else

                    {
                        _renderer.DrawSquare(worldPos.X, worldPos.Y, LabyrinthMap.TileSize, LabyrinthMap.TileSize,
                                      EmptyColor, _texFloor);
                    }
                }
            }

            DrawBase(map, 1, 1, Base1Color);
            DrawBase(map, map.Width() - 2, map.Height() - 2, Base2Color);
        }

        public void Draw(GameObject entity)
        {
            if (entity == null) return;

            switch (entity)
            {
                case Player p:
                    DrawPlayer(p);
                    break;

                case Enemy e:
                    DrawEnemy(e);
                    break;

                case Bullet b:
                    //_renderer.DrawSquare(b.Position.X, b.Position.Y, b.Size, b.Size, Color4.OrangeRed, _texBullet);
                    DrawBullet(b);
                    break;

                case Prize pr:
                    Debug.WriteLine($"PRIZE!!!: {_texFastPrize}");
                    //_renderer.DrawSquare(pr.Position.X, pr.Position.Y, pr.Size, pr.Size, Color4.Yellow);
                    DrawPrize(pr);
                    break;

            }
        }

        public void DrawObjects(IEnumerable<GameObject> entities)
        {
            foreach (var entity in entities)
            {
                Draw(entity);
            }
        }

        private Color4 GetBulletColor(IBullet bullet)
        {
            if (BulletTools.IsDecoratorActive<ExplosiveAmmo>(bullet))
            {
                return Color4.OrangeRed; 
            }

            if (BulletTools.IsDecoratorActive<FastAmmo>(bullet))
            {
                return Color4.Cyan; 
            }
            return Color4.White;
        }

        private void DrawBullet(Bullet b)
        {
            Color4 bulletColor = GetBulletColor(b._stats); 
            _renderer.DrawSquare(b.Position.X, b.Position.Y, b.Size, b.Size, bulletColor, _texBullet);
        }

        private void DrawPlayer(Player p)
        {
            int texId = (p.Id == 1) ? _texPlayer1 : _texPlayer2;

            Color4 pColor = Color4.White;
            _renderer.DrawSquare(p.Position.X, p.Position.Y, p.Size, p.Size, pColor, texId, p.Rotation);
        }

        private int GetPrizeTexture(Prize prize)
        {
            Debug.WriteLine($"Проверяю приз: {prize.GetType().FullName}");
            switch (prize)
            {

                case ExplosivePrize e:
                    return _texExplosivePrize;

                case AmmunitionPrize ap:
                    return _texAmmoPrize;

                case FastPrize fp:
                    return _texFastPrize;

                case HealthPrize hp:
                    return _texHealthPrize;

                default:
                    return -1;
            }

        }

        private void DrawPrize(Prize prize)
        {
            Color4 prizeColor = ToColor4(prize.DisplayColor);


            if (prize.Age > prize.MaxLifetime * 0.65f)
            {
                bool isVisible = (int)(prize.Age * 5) % 2 == 0;
                if (!isVisible) return;

                float remainingTime = prize.MaxLifetime - prize.Age;
                float warningPhaseDuration = prize.MaxLifetime * 0.35f;

                prizeColor.A = remainingTime / warningPhaseDuration;
            }

            int texId = GetPrizeTexture(prize);
            Debug.WriteLine($"------------------- приз: {texId}");
            _renderer.DrawSquare(prize.Position.X, prize.Position.Y, prize.Size, prize.Size, prizeColor, texId);
        }

        //private void DrawPrize(Prize prize)

        //{

        //    if (prize.Age > prize.MaxLifetime * 0.65f)

        //    {

        //        bool isVisible = (int)(prize.Age * 5) % 2 == 0;



        //        if (!isVisible) return;

        //    }



        //    //Color4 prizeColor = ToColor4(prize.DisplayColor);
        //    Color4 prizeColor = Color4.White;

        //    float alpha = 1.0f - (prize.Age / prize.MaxLifetime);

        //    prizeColor.A = alpha;

        //    int texId = GetPrizeTexture(prize);

        //    _renderer.DrawSquare(prize.Position.X, prize.Position.Y, prize.Size, prize.Size, prizeColor);

        //}

        private void DrawEnemy(Enemy enemy)
        {
            Color4 enemyColor = ToColor4(enemy.DisplayColor);

            _renderer.DrawSquare(
                enemy.Position.X,
                enemy.Position.Y,
                enemy.Size,
                enemy.Size,
                enemyColor,
                _texEnemy,
                enemy.Rotation
            );

            
        }

        private Color4 ToColor4(System.Drawing.Color color)
        {
            return new Color4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        private void DrawBase(LabyrinthMap map, int gridX, int gridY, Color4 color)
        {
            var pos = map.ConvertToWorldCoordinates(gridX, gridY);
            _renderer.DrawSquare(pos.X, pos.Y, LabyrinthMap.TileSize, LabyrinthMap.TileSize, color);
        }   

        public void Clear()
        {
            GL.ClearColor(BackgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}