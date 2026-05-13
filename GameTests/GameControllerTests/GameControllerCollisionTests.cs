using Xunit;
using Application.Game;
using GameCore.Map;
using GameCore.Bullets;
using GameCore.Characters;
using System.Reflection;
using System;

namespace GameTests.GameControllerTests
{
    /// <summary>
    /// Тестирование логики обработки столкновений и нанесения урона в игровом контроллере.
    /// </summary>
    public class GameControllerCollisionTests
    {
        /// <summary>
        /// Создает экземпляр <see cref="GameController"/> с принудительно отключенным спавном врагов.
        /// </summary>
        /// <param name="grid">Сетка тайлов для инициализации карты. Если <see langword="null"/>, создается пустая сетка 20x20.</param>
        /// <returns>Инициализированный объект контроллера с очищенными списками сущностей.</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается, если через рефлексию не удалось получить доступ к внутренним полям контроллера.</exception>
        private GameController CreateSafeController(TileType[,]? grid = null)
        {
            var finalGrid = grid ?? new TileType[20, 20];
            var map = new LabyrinthMap(finalGrid);
            var controller = new GameController(map);

            FieldInfo? spawnField = typeof(GameController).GetField("_canSpawnNewEnemies", BindingFlags.NonPublic | BindingFlags.Instance);
            if (spawnField == null)
            {
                throw new InvalidOperationException("Field '_canSpawnNewEnemies' not found.");
            }
            spawnField.SetValue(controller, false);

            controller.Enemies.Clear();
            controller.ActiveBullets.Clear();

            return controller;
        }

        /// <summary>
        /// Проверяет, что при попадании пули во врага без брони, урон вычитается напрямую из здоровья.
        /// </summary>
        [Fact]
        public void ProcessBullets_EnemyHitWithoutArmor_ReducesHealth()
        {
            var controller = CreateSafeController();
            var enemy = new Enemy(200, 200) { Health = 80, Armor = 0 };
            controller.AddEntity(enemy);

            var bullet = new Bullet(199, 200, 0, 0, new StandartBullet(), 1);
            controller.AddEntity(bullet);

            controller.UpdateWorld(0.01f);

            Assert.DoesNotContain(bullet, controller.ActiveBullets);
            Assert.Equal(70, enemy.Health);
        }

        /// <summary>
        /// Проверяет, что броня поглощает урон пули, предотвращая уменьшение здоровья.
        /// </summary>
        [Fact]
        public void ProcessBullets_EnemyHitWithArmor_ReducesArmorInsteadOfHealth()
        {
            var controller = CreateSafeController();
            var enemy = new Enemy(200, 200) { Health = 80, Armor = 20 };
            controller.AddEntity(enemy);

            var bullet = new Bullet(200, 200, 0, 0, new StandartBullet(), 1);
            controller.AddEntity(bullet);

            controller.UpdateWorld(0.01f);

            Assert.Equal(10, enemy.Armor);
            Assert.Equal(80, enemy.Health);
        }

        /// <summary>
        /// Проверяет удаление снаряда из игрового мира при столкновении с непроходимым препятствием (стеной).
        /// </summary>
        [Fact]
        public void ProcessBullets_WallHit_RemovesBullet()
        {
            var grid = new TileType[10, 10];
            grid[5, 5] = TileType.Wall;
            var controller = CreateSafeController(grid);

            FieldInfo? mapField = typeof(GameController).GetField("_map", BindingFlags.NonPublic | BindingFlags.Instance);
            var map = mapField?.GetValue(controller) as LabyrinthMap;

            if (map == null)
            {
                throw new InvalidOperationException("Could not access LabyrinthMap via reflection.");
            }

            var wallPos = map.ConvertToWorldCoordinates(5, 5);
            var bullet = new Bullet(wallPos.X, wallPos.Y, 0, 0, new StandartBullet(), 1);
            controller.AddEntity(bullet);

            controller.UpdateWorld(0.01f);

            Assert.DoesNotContain(bullet, controller.ActiveBullets);
        }

        /// <summary>
        /// Проверяет, что пули, выпущенные вражескими юнитами, не наносят урон другим врагам.
        /// </summary>
        [Fact]
        public void ProcessBullets_FriendlyFire_DoesNotApplyDamage()
        {
            var controller = CreateSafeController();
            var enemy = new Enemy(200, 200) { Health = 80 };
            controller.AddEntity(enemy);

            var bullet = new Bullet(200, 200, 0, 0, new StandartBullet(), 3); 
            controller.AddEntity(bullet);

            controller.UpdateWorld(0.01f);

            Assert.Contains(bullet, controller.ActiveBullets);
            Assert.Equal(80, enemy.Health);
        }
    }
}