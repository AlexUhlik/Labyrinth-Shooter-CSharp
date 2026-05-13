using GameCore.Characters;
using GameCore.Map;
using GameCore;
using Xunit;

namespace GameTests.EntitiesTests
{
    /// <summary>
    /// Набор тестов для проверки логики поведения, характеристик и состояния вражеских юнитов.
    /// </summary>
    public class EnemyTests
    {
        private const float DeltaTime = 0.016f;

        /// <summary>
        /// Создает пустую карту заданного размера для тестирования навигации.
        /// </summary>
        /// <param name="width">Ширина сетки тайлов.</param>
        /// <param name="height">Высота сетки тайлов.</param>
        /// <returns>Экземпляр <see cref="LabyrinthMap"/> с тайлами типа Empty.</returns>
        private LabyrinthMap CreateEmptyMap(int width, int height)
        {
            var grid = new TileType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = TileType.Empty;
                }
            }
            return new LabyrinthMap(grid);
        }

        /// <summary>
        /// Проверяет, что конструктор устанавливает корректные значения характеристик врага по умолчанию.
        /// </summary>
        [Fact]
        public void Constructor_InitialParameters_SetsCorrectDefaults()
        {
            var enemy = new Enemy(100, 100);

            Assert.Equal(80, enemy.Health);
            Assert.Equal(50, enemy.Armor);
            Assert.Equal(2.0f, enemy.Speed);
            Assert.Equal(60f, enemy.Size);
            Assert.True(enemy.IsActive);
        }

        /// <summary>
        /// Проверяет, что враг обнаруживает игрока и переходит в состояние готовности к стрельбе, 
        /// если игрок находится в зоне видимости и на прямой линии обзора.
        /// </summary>
        [Fact]
        public void UpdatePosition_PlayerIsVisible_ReturnsReadyToShootAfterCooldown()
        {
            // Arrange
            var map = CreateEmptyMap(10, 10);
            var enemy = new Enemy(32, 32);      
            var player = new Player(1, 96, 32); 
            var dummy = new Player(2, 0, 0);

            enemy.SetDirection(1, 0); // Направляем врага в сторону игрока

            // Act
            bool readyToShoot = false;
            // Имитируем несколько игровых кадров для накопления задержки выстрела
            for (int i = 0; i < 30; i++)
            {
                readyToShoot = enemy.UpdatePosition(map, player, dummy, DeltaTime);
            }

            // Assert
            Assert.True(readyToShoot);
        }

        /// <summary>
        /// Проверяет, что враг не открывает огонь, если между ним и игроком находится стена.
        /// </summary>
        [Fact]
        public void UpdatePosition_PlayerBehindWall_ReturnsNotReadyToShoot()
        {
            // Arrange
            var grid = new TileType[10, 10];
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++) grid[i, j] = TileType.Empty;

            grid[2, 1] = TileType.Wall; 

            var map = new LabyrinthMap(grid);
            var enemy = new Enemy(32, 32);
            var player = new Player(1, 96, 32);
            enemy.SetDirection(1, 0);

            // Act
            bool readyToShoot = enemy.UpdatePosition(map, player, player, DeltaTime);

            // Assert
            Assert.False(readyToShoot);
        }

        /// <summary>
        /// Проверяет, что враг не видит игрока, если тот находится за пределами максимального радиуса обзора.
        /// </summary>
        [Fact]
        public void UpdatePosition_PlayerOutOfVisionRange_ReturnsNotReadyToShoot()
        {
            // Arrange
            var map = CreateEmptyMap(20, 20);
            var enemy = new Enemy(32, 32);
            var player = new Player(1, 224, 32); 
            enemy.SetDirection(1, 0);

            // Act
            bool readyToShoot = enemy.UpdatePosition(map, player, player, DeltaTime);

            // Assert
            Assert.False(readyToShoot);
        }

        /// <summary>
        /// Проверяет активацию и автоматическое отключение визуального эффекта получения урона (вспышки).
        /// </summary>
        [Fact]
        public void TakeDamage_ReceivingHit_TogglesDamageFlashEffect()
        {
            // Arrange
            var enemy = new Enemy(0, 0);

            // Act
            enemy.TakeDamage(10, 1);
            bool isDamagedInitially = enemy.IsDamaged;

            enemy.UpdateDamageFlash(0.2f); 
            bool isDamagedAfterUpdate = enemy.IsDamaged;

            // Assert
            Assert.True(isDamagedInitially);
            Assert.False(isDamagedAfterUpdate);
        }

        /// <summary>
        /// Проверяет, что при достижении нулевого здоровья статус IsActive изменяется на false.
        /// </summary>
        [Fact]
        public void TakeDamage_HealthDropsToZero_DeactivatesEnemy()
        {
            // Arrange
            var enemy = new Enemy(0, 0)
            {
                Health = 10,
                Armor = 0
            };

            // Act
            enemy.TakeDamage(20, 1);

            // Assert
            Assert.Equal(0, enemy.Health);
            Assert.False(enemy.IsActive);
        }

        /// <summary>
        /// Проверяет, что создаваемый врагом снаряд имеет корректный OwnerId (3) и начальную позицию.
        /// </summary>
        [Fact]
        public void Shoot_CreateBullet_SetsCorrectOwnerAndPosition()
        {
            // Arrange
            var enemy = new Enemy(100, 200);
            enemy.SetDirection(0, 1);

            // Act
            var bullet = enemy.Shoot();

            // Assert
            Assert.NotNull(bullet);
            Assert.Equal(100, bullet.Position.X);
            Assert.Equal(3, bullet.OwnerId); 
        }
    }
}