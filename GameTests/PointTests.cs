using Xunit;
using GameCore;

namespace LabyrinthGame.Tests
{
    public class PointTests
    {
        [Fact]
        public void Constructor_ShouldSetCoordinates()
        {
            var point = new Point(10.5f, 20.3f); 

            Assert.Equal(10.5f, point.X); 
            Assert.Equal(20.3f, point.Y); 
        }

        [Fact]
        public void OperatorPlus_Points_ShouldAddCoordinates()
        {
            var p1 = new Point(10, 20); 
            var p2 = new Point(5, -5); 

            var result = p1 + p2; 

            Assert.Equal(15, result.X);
            Assert.Equal(15, result.Y);
        }

        [Fact]
        public void OperatorMinus_Number_ShouldSubtractFromBothCoordinates()
        {
            var p1 = new Point(10, 20); 

            var result = p1 - 5f; 

            Assert.Equal(5, result.X);
            Assert.Equal(15, result.Y);
        }

        [Fact]
        public void OperatorMultiply_Scalar_ShouldScalePoint()
        {
            var p1 = new Point(4, 8); 

            var result = p1 * 0.5f; 

            Assert.Equal(2, result.X);
            Assert.Equal(4, result.Y);
        }
    }
}