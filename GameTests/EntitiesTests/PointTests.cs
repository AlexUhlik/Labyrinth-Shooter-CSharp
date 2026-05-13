using Xunit;
using GameCore;

namespace GameTests.EntitiesTests
{
    /// <summary>
    /// Набор тестов для проверки математических операций и инициализации структуры <see cref="Point"/>.
    /// </summary>
    public class PointTests
    {
        /// <summary>
        /// Проверяет, что конструктор корректно присваивает значения координатам X и Y.
        /// </summary>
        [Fact]
        public void Constructor_ValidCoordinates_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var point = new Point(10.5f, 20.3f);

            // Assert
            Assert.Equal(10.5f, point.X);
            Assert.Equal(20.3f, point.Y);
        }

        /// <summary>
        /// Проверяет перегрузку оператора сложения для двух точек.
        /// </summary>
        [Fact]
        public void OperatorAdd_TwoPoints_ReturnsSummedCoordinates()
        {
            // Arrange
            var p1 = new Point(10, 20);
            var p2 = new Point(5, -5);

            // Act
            var result = p1 + p2;

            // Assert
            Assert.Equal(15f, result.X);
            Assert.Equal(15f, result.Y);
        }

        /// <summary>
        /// Проверяет перегрузку оператора вычитания числа из точки.
        /// </summary>
        [Fact]
        public void OperatorSubtract_ScalarValue_SubtractsFromBothCoordinates()
        {
            // Arrange
            var p1 = new Point(10, 20);

            // Act
            var result = p1 - 5f;

            // Assert
            Assert.Equal(5f, result.X);
            Assert.Equal(15f, result.Y);
        }

        /// <summary>
        /// Проверяет перегрузку оператора умножения точки на скаляр.
        /// </summary>
        [Fact]
        public void OperatorMultiply_ScalarValue_ScalesCoordinatesCorrectly()
        {
            // Arrange
            var p1 = new Point(4, 8);

            // Act
            var result = p1 * 0.5f;

            // Assert
            Assert.Equal(2f, result.X);
            Assert.Equal(4f, result.Y);
        }
    }
}