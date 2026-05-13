using Xunit;
using GameCore;
using System;

namespace GameTests.EntitiesTests
{
    /// <summary>
    /// Набор тестов для проверки базовой логики боевых единиц (Unit), 
    /// включая передвижение, вращение и систему распределения урона.
    /// </summary>
    public class UnitTests
    {
        /// <summary>
        /// Тестовая реализация абстрактного класса <see cref="Unit"/>.
        /// </summary>
        private class MockUnit : Unit
        {
            public MockUnit(float x, float y, float size) : base(x, y, size) { }

            /// <summary>
            /// Заглушка метода стрельбы.
            /// </summary>
            public override GameCore.Bullets.Bullet? Shoot() => null;
        }

        /// <summary>
        /// Проверяет, что метод Move корректно изменяет координаты позиции юнита.
        /// </summary>
        [Fact]
        public void Move_DeltaCoordinates_UpdatesPositionCorrectly()
        {
            // Arrange
            var unit = new MockUnit(10, 10, 5);

            // Act
            unit.Move(5, -3);

            // Assert
            Assert.Equal(15f, unit.Position.X);
            Assert.Equal(7f, unit.Position.Y);
        }

        /// <summary>
        /// Проверяет, что установка направления взгляда корректно вычисляет угол поворота (Rotation).
        /// </summary>
        [Fact]
        public void SetDirection_VectorInput_UpdatesRotationProperty()
        {
            // Arrange
            var unit = new MockUnit(0, 0, 5);

            // Act
            unit.SetDirection(1, 0);

            // Assert
            Assert.Equal(0f, unit.Rotation);
        }

        /// <summary>
        /// Проверяет логику распределения урона между броней и здоровьем.
        /// </summary>
        /// <param name="h">Начальное здоровье.</param>
        /// <param name="a">Начальная броня.</param>
        /// <param name="dmg">Входящий урон.</param>
        /// <param name="expA">Ожидаемая броня после выстрела.</param>
        /// <param name="expH">Ожидаемое здоровье после выстрела.</param>
        [Theory]
        [InlineData(100, 50, 60, 0, 90)] // Кейс: урон пробивает броню полностью и задевает HP
        [InlineData(100, 0, 30, 0, 70)]  // Кейс: брони нет, весь урон уходит в HP
        [InlineData(100, 100, 40, 60, 100)] // Кейс: броня полностью поглощает урон
        public void TakeDamage_VaryingArmorAndHealth_DistributesDamageCorrectly(int h, int a, int dmg, int expA, int expH)
        {
            // Arrange
            var unit = new MockUnit(0, 0, 5);
            unit.Health = h;
            unit.Armor = a;

            // Act
            unit.TakeDamage(dmg, 1);

            // Assert
            Assert.Equal(expA, unit.Armor);
            Assert.Equal(expH, unit.Health);
        }

        /// <summary>
        /// Проверяет работу таймера визуального эффекта получения урона (Flash Effect).
        /// </summary>
        [Fact]
        public void UpdateDamageFlash_TimePasses_ResetsIsDamagedFlag()
        {
            // Arrange
            var unit = new MockUnit(0, 0, 5);
            unit.TakeDamage(10, 1); 

            // Assert 
            Assert.True(unit.IsDamaged);

            // Act
            unit.UpdateDamageFlash(0.2f);

            // Assert
            Assert.False(unit.IsDamaged);
        }
    }
}