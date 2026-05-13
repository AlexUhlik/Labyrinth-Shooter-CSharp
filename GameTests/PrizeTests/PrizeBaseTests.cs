using Xunit;
using GameCore.Items;
using GameCore.Characters;

namespace GameTests.PrizeTests
{
    /// <summary>
    /// Тестирование базового функционала абстрактного класса Prize.
    /// Проверяет логику старения, время жизни и физические характеристики бонусов.
    /// </summary>
    public class PrizeBaseTests
    {
        /// <summary>
        /// Минимальная реализация Prize для тестирования защищенных и абстрактных членов базового класса.
        /// </summary>
        private class TestPrize : Prize
        {
            public TestPrize(float x, float y) : base(x, y) { }

            /// <summary>
            /// Пустая реализация эффекта для тестов базового класса.
            /// </summary>
            public override void ApplyEffect(Player player) { }
        }

        /// <summary>
        /// Проверяет, что вызов Update корректно увеличивает значение Age (возраст) объекта.
        /// </summary>
        [Fact]
        public void Update_DeltaTimeInput_IncreasesInternalAge()
        {
            // Arrange
            var prize = new TestPrize(0, 0);
            float deltaTime = 1.5f;

            // Act
            prize.Update(deltaTime);

            // Assert
            Assert.Equal(deltaTime, prize.Age);
        }

        /// <summary>
        /// Проверяет, что флаг IsExpired становится истинным, когда возраст объекта превышает MaxLifetime.
        /// </summary>
        [Fact]
        public void IsExpired_AgeExceedsLifetime_ReturnsTrue()
        {
            // Arrange
            var prize = new TestPrize(0, 0);
            prize.MaxLifetime = 5f;

            // Act
            prize.Update(6f); 

            // Assert
            Assert.True(prize.IsExpired, "Объект должен считаться истекшим после превышения MaxLifetime.");
        }

        /// <summary>
        /// Проверяет, что объект не считается истекшим, если его время жизни еще не вышло.
        /// </summary>
        [Fact]
        public void IsExpired_AgeWithinLifetime_ReturnsFalse()
        {
            // Arrange
            var prize = new TestPrize(0, 0);
            prize.MaxLifetime = 10f;

            // Act
            prize.Update(5f);

            // Assert
            Assert.False(prize.IsExpired);
        }

        /// <summary>
        /// Проверяет, что начальное состояние IsActive всегда равно true.
        /// </summary>
        [Fact]
        public void IsActive_Initially_ReturnsTrue()
        {
            // Arrange & Act
            var prize = new TestPrize(0, 0);

            // Assert
            Assert.True(prize.IsActive);
        }
    }
}