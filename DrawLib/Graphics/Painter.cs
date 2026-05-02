using GameCore;
using GameCore.Bullets;
using GameCore.Characters;
using GameCore.Map;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.IO;
using System;
using GameCore.Items;

namespace DrawLib.Graphics
{

    /// <summary>
    /// Класс представляющий собой высокоуровневую обертку над GameRenderer для конкретной отрисовки игровых объектов.
    /// </summary>
    public class Painter
    {
        // Ссылка на низкоуровневый движок отрисовки
        private readonly GameRenderer _renderer;

        // Цвета для различных игровых сущностей
        private static readonly Color4 Player1Color = new Color4(0.0f, 0.5f, 1.0f, 1.0f);
        private static readonly Color4 Player2Color = new Color4(0.9f, 0.4f, 0.3f, 1.0f);
        private static readonly Color4 BackgroundColor = new Color4(0.05f, 0.05f, 0.05f, 1.0f);
        private static readonly Color4 Base1Color = new Color4(0.0f, 0.5f, 1.0f, 0.4f);
        private static readonly Color4 Base2Color = new Color4(0.9f, 0.4f, 0.3f, 0.4f);
        private static readonly Color4 WallColor = new Color4(0.1f, 0.5f, 0.35f, 1.0f);
        private static readonly Color4 EmptyColor = new Color4(1f, 1f, 1f, 1f);

        // Цвета бонусов 
        private static readonly Color4 AmmoPrizeColor = new Color4(0.5f, 0.5f, 0.5f, 1f);
        private static readonly Color4 ExplosivePrizeColor = new Color4(0.545f, 0f, 0f, 1f);
        private static readonly Color4 FastPrizeColor = new Color4(1f, 1f, 0.878f, 1f);
        private static readonly Color4 HealthPrizeColor = new Color4(0.564f, 0.933f, 0.564f, 1f);

        // Идентификаторы загруженных текстур
        private int _texPlayer;
        private int _texWall;
        private int _texFloor;
        private int _texEnemy;
        private int _texBullet;
        private int _texAmmoPrize;
        private int _texExplosivePrize;
        private int _texFastPrize;
        private int _texHealthPrize;

        /// <summary>
        /// Создает экземпляр рисовальщика, привязанный к конкретному рендереру.
        /// </summary>
        public Painter(GameRenderer renderer)
        {
            _renderer = renderer;
        }

        /// <summary>
        /// Формирует полный путь к файлу текстуры в папке Textures.
        /// </summary>
        private string GetAssetPath(string fileName) =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Textures", fileName);

        /// <summary>
        /// Загружает все необходимые графические ресурсы в видеопамять.
        /// </summary>
        public void LoadAssets()
        {
            _texPlayer = _renderer.LoadTexture(GetAssetPath("player.png"));
            _texWall = _renderer.LoadTexture(GetAssetPath("wall_blue.png"));
            _texFloor = _renderer.LoadTexture(GetAssetPath("floor.png"));
            _texEnemy = _renderer.LoadTexture(GetAssetPath("enemy.png"));
            _texBullet = _renderer.LoadTexture(GetAssetPath("standartBullet.png"));
            _texAmmoPrize = _renderer.LoadTexture(GetAssetPath("bullet.png"));
            _texExplosivePrize = _renderer.LoadTexture(GetAssetPath("fastPrize.png"));
            _texFastPrize = _renderer.LoadTexture(GetAssetPath("fastPrize.png"));
            _texHealthPrize = _renderer.LoadTexture(GetAssetPath("medkit.png"));
        }

        /// <summary>
        /// Настраивает ортографическую проекцию камеры по размеру карты лабиринта.
        /// </summary>
        public void SetupCamera(LabyrinthMap map)
        {
            float mazeWidth = map.Width() * LabyrinthMap.TileSize;
            float mazeHeight = map.Height() * LabyrinthMap.TileSize;
            var projection = Matrix4.CreateOrthographicOffCenter(0, mazeWidth, 0, mazeHeight, -1, 1);
            _renderer.SetMatrices(Matrix4.Identity, projection);
        }

        /// <summary>
        /// Отрисовывает статичные элементы карты: пол, стены и базы игроков.
        /// </summary>
        public void Draw(LabyrinthMap map)
        {
            for (int x = 0; x < map.Width(); x++)
            {
                for (int y = 0; y < map.Height(); y++)
                {
                    var worldPos = map.ConvertToWorldCoordinates(x, y);
                    bool isWall = map.IsWall(x, y);

                    _renderer.DrawSquare(
                        worldPos.X, worldPos.Y,
                        LabyrinthMap.TileSize, LabyrinthMap.TileSize,
                        isWall ? WallColor : EmptyColor,
                        isWall ? _texWall : _texFloor
                    );
                }
            }

            DrawBase(map, 1, 1, Base1Color);
            DrawBase(map, map.Width() - 2, map.Height() - 2, Base2Color);
        }

        /// <summary>
        /// Определяет тип объекта и вызывает соответствующий метод отрисовки.
        /// </summary>
        public void Draw(GameObject entity)
        {
            if (entity == null) return;

            switch (entity)
            {
                case Player p: DrawPlayer(p); break;
                case Enemy e: DrawEnemy(e); break;
                case Bullet b: DrawBullet(b); break;
                case Prize pr: DrawPrize(pr); break;
            }
        }

        /// <summary>
        /// Выполняет пакетную отрисовку списка игровых объектов.
        /// </summary>
        public void DrawObjects(IEnumerable<GameObject> entities)
        {
            foreach (var entity in entities) Draw(entity);
        }

        /// <summary>
        /// Возвращает цвет пули в зависимости от её активных модификаторов (декораторов).
        /// </summary>
        private Color4 GetBulletColor(IBullet bullet)
        {
            if (BulletTools.IsDecoratorActive<ExplosiveAmmo>(bullet)) return Color4.OrangeRed;
            if (BulletTools.IsDecoratorActive<FastAmmo>(bullet)) return Color4.Cyan;
            return Color4.White;
        }

        /// <summary>
        /// Отрисовывает пулю с учетом её текущих характеристик.
        /// </summary>
        private void DrawBullet(Bullet b)
        {
            Color4 bulletColor = GetBulletColor(b.Stats);
            _renderer.DrawSquare(b.Position.X, b.Position.Y, b.Size, b.Size, bulletColor, _texBullet);
        }

        /// <summary>
        /// Отрисовывает игрока, учитывая его ID (для цвета) и состояние получения урона.
        /// </summary>
        private void DrawPlayer(Player p)
        {
            Color4 pColor = (p.Id == 1) ? Player1Color : Player2Color;
            if (p.IsDamaged) pColor = Color4.Red;

            _renderer.DrawSquare(p.Position.X, p.Position.Y, p.Size, p.Size, pColor, _texPlayer, p.Rotation);
        }

        /// <summary>
        /// Подбирает текстуру и базовый цвет для конкретного типа приза.
        /// </summary>
        private int GetPrizeTextureAndColor(Prize prize, out Color4 color)
        {
            switch (prize)
            {
                case ExplosivePrize e:
                    color = ExplosivePrizeColor;
                    return _texExplosivePrize;

                case AmmunitionPrize ap:
                    color = AmmoPrizeColor;
                    return _texAmmoPrize;

                case FastPrize fp:
                    color = FastPrizeColor;
                    return _texFastPrize;

                case HealthPrize hp:
                    color = HealthPrizeColor;
                    return _texHealthPrize;

                default:
                    color = Color4.White;
                    return -1;
            }
        }

        /// <summary>
        /// Отрисовывает бонус на карте, включая эффект мигания перед исчезновением.
        /// </summary>
        private void DrawPrize(Prize prize)
        {
            int texId = GetPrizeTextureAndColor(prize, out Color4 prizeColor);

            if (prize.Age > prize.MaxLifetime * 0.65f)
            {
                if ((int)(prize.Age * 3) % 2 == 0) return;

                float remainingTime = prize.MaxLifetime - prize.Age;
                float warningPhase = prize.MaxLifetime * 0.35f;
                prizeColor.A = remainingTime / warningPhase;
            }

            _renderer.DrawSquare(prize.Position.X, prize.Position.Y, prize.Size, prize.Size, prizeColor, texId);
        }

        /// <summary>
        /// Отрисовывает врага с его уникальным цветом и поворотом.
        /// </summary>
        private void DrawEnemy(Enemy enemy)
        {
            Color4 enemyColor = enemy.IsDamaged ? Color4.Red : ToColor4(enemy.DisplayColor);

            _renderer.DrawSquare(
                enemy.Position.X, enemy.Position.Y,
                enemy.Size, enemy.Size,
                enemyColor, _texEnemy, enemy.Rotation
            );
        }

        /// <summary>
        /// Вспомогательный метод конвертации системного цвета в формат OpenTK.
        /// </summary>
        private Color4 ToColor4(System.Drawing.Color color) =>
            new Color4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);

        /// <summary>
        /// Рисует подложку базы игрока на карте.
        /// </summary>
        private void DrawBase(LabyrinthMap map, int gridX, int gridY, Color4 color)
        {
            var pos = map.ConvertToWorldCoordinates(gridX, gridY);
            _renderer.DrawSquare(pos.X, pos.Y, LabyrinthMap.TileSize, LabyrinthMap.TileSize, color);
        }

        /// <summary>
        /// Очищает экран, заполняя его фоновым цветом.
        /// </summary>
        public void Clear()
        {
            GL.ClearColor(BackgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}