using Xunit;
using GameCore.Characters;
using GameCore.Bullets;
using GameCore;
using System;

namespace GameTests.EntitiesTests
{
    /// <summary>
    /// Набор тестов для проверки характеристик, механик стрельбы и системы возрождения игрока.
    /// </summary>
    public class PlayerTests
    {
        private const float DeltaTime = 0.016f;

        /// <summary>
        /// Проверяет, что конструктор устанавливает верные начальные характеристики игрока.
        /// </summary>
        [Fact]
        public void Constructor_InitialParameters_SetsCorrectDefaultStats()
        {
            // Arrange
            int id = 1;
            float x = 100f;
            float y = 200f;

            // Act
            var player = new Player(id, x, y);

            // Assert
            Assert.Equal(id, player.Id);
            Assert.Equal(x, player.Position.X);
            Assert.Equal(y, player.Position.Y);
            Assert.Equal(100, player.Health);
            Assert.Equal(50, player.Armor);
            Assert.Equal(45, player.Ammunition);
            Assert.Equal(0, player.Score);
            Assert.NotNull(player.CurrentBullet);
            Assert.True(player.CanShoot);
        }

        /// <summary>
        /// Проверяет, что при выстреле расходуются боеприпасы и создается объект пули с корректным владельцем.
        /// </summary>
        [Fact]
        public void Shoot_ValidState_DecreasesAmmunitionAndReturnsBullet()
        {
            // Arrange
            var player = new Player(1, 0, 0);
            int initialAmmo = player.Ammunition;

            // Act
            var bullet = player.Shoot();

            // Assert
            Assert.NotNull(bullet);
            Assert.Equal(initialAmmo - 1, player.Ammunition);
            Assert.Equal(player.Id, bullet.OwnerId);
        }

        /// <summary>
        /// Проверяет, что игрок не может стрелять, если боеприпасы закончились.
        /// </summary>
        [Fact]
        public void Shoot_NoAmmunition_ReturnsNull()
        {
            // Arrange
            var player = new Player(1, 0, 0);
            player.Ammunition = 0;

            // Act
            var bullet = player.Shoot();

            // Assert
            Assert.Null(bullet);
        }

        /// <summary>
        /// Проверяет работу таймера перезарядки: возможность стрельбы должна блокироваться до истечения времени задержки.
        /// </summary>
        [Fact]
        public void UpdateCooldown_AfterReset_BlocksShootingUntilTimePasses()
        {
            // Arrange
            var player = new Player(1, 0, 0);

            // Act
            player.ResetShootTimer();
            bool canShootImmediately = player.CanShoot;

            player.UpdateCooldown(0.31f);

            // Assert
            Assert.False(canShootImmediately);
            Assert.True(player.CanShoot);
        }

        /// <summary>
        /// Проверяет систему возрождения: сброс позиции, характеристик и применение штрафа к очкам.
        /// </summary>
        [Fact]
        public void Respawn_PlayerDied_ResetsPositionAndAppliesScorePenalty()
        {
            // Arrange
            float startX = 10f, startY = 10f;
            var player = new Player(1, startX, startY);
            player.Position = new Point(500, 500);
            player.Score = 1000;
            player.Ammunition = 10;

            // Act
            player.Respawn();

            // Assert
            Assert.Equal(startX, player.Position.X);
            Assert.Equal(startY, player.Position.Y);
            Assert.Equal(100, player.Health);
            Assert.Equal(45, player.Ammunition);
            Assert.Equal(500, player.Score); 
        }

        /// <summary>
        /// Проверяет, что штраф за смерть не может сделать количество очков игрока отрицательным.
        /// </summary>
        [Fact]
        public void Respawn_LowScore_DoesNotMakeScoreNegative()
        {
            // Arrange
            var player = new Player(1, 0, 0);
            player.Score = 100; 

            // Act
            player.Respawn();

            // Assert
            Assert.Equal(0, player.Score);
        }

        /// <summary>
        /// Проверяет комплексное обновление игрока: завершение перезарядки и окончание визуального эффекта урона.
        /// </summary>
        [Fact]
        public void Update_PassingTime_ProcessesCooldownAndFlashEffects()
        {
            // Arrange
            var player = new Player(1, 0, 0);
            player.ResetShootTimer();
            player.TakeDamage(20, 1); 

            // Act
            player.Update(0.5f);

            // Assert
            Assert.True(player.CanShoot);
            Assert.False(player.IsDamaged);
        }
    }
}