using NUnit.Framework;
using UnityEngine;

namespace DefaultNamespace.Tests
{
    /// <summary>
    /// Unit tests for pivot-aware distance calculations.
    /// These tests verify the mathematical logic used in DraggableCard for calculating
    /// distances between elements with different pivot points.
    /// </summary>
    public class PivotCalculationTests
    {
        /// <summary>
        /// Calculates the world center position of a rect given its position, size, and pivot.
        /// This mirrors the logic in DraggableCard.GetWorldCenter without requiring a RectTransform.
        /// </summary>
        private Vector3 CalculateWorldCenter(Vector3 position, Vector2 size, Vector2 pivot)
        {
            var offsetX = (0.5f - pivot.x) * size.x;
            var offsetY = (0.5f - pivot.y) * size.y;
            return position + new Vector3(offsetX, offsetY, 0);
        }

        [Test]
        public void CalculateWorldCenter_CenteredPivot_ReturnsPosition()
        {
            // Arrange
            var position = new Vector3(100, 100, 0);
            var size = new Vector2(100, 50);
            var pivot = new Vector2(0.5f, 0.5f);

            // Act
            var center = CalculateWorldCenter(position, size, pivot);

            // Assert
            Assert.AreEqual(position, center);
        }

        [Test]
        public void CalculateWorldCenter_BottomLeftPivot_OffsetsCorrectly()
        {
            // Arrange
            var position = new Vector3(0, 0, 0);
            var size = new Vector2(100, 50);
            var pivot = new Vector2(0, 0); // Bottom-left

            // Act
            var center = CalculateWorldCenter(position, size, pivot);

            // Assert
            // Center should be at half the width and height from position
            Assert.AreEqual(new Vector3(50, 25, 0), center);
        }

        [Test]
        public void CalculateWorldCenter_TopRightPivot_OffsetsCorrectly()
        {
            // Arrange
            var position = new Vector3(100, 100, 0);
            var size = new Vector2(100, 50);
            var pivot = new Vector2(1, 1); // Top-right

            // Act
            var center = CalculateWorldCenter(position, size, pivot);

            // Assert
            // Center should be at negative half the width and height from position
            Assert.AreEqual(new Vector3(50, 75, 0), center);
        }

        [Test]
        public void CalculateWorldCenter_CustomPivot_OffsetsCorrectly()
        {
            // Arrange
            var position = new Vector3(100, 100, 0);
            var size = new Vector2(100, 100);
            var pivot = new Vector2(0.25f, 0.75f);

            // Act
            var center = CalculateWorldCenter(position, size, pivot);

            // Assert
            // Offset X: (0.5 - 0.25) * 100 = 25
            // Offset Y: (0.5 - 0.75) * 100 = -25
            Assert.AreEqual(new Vector3(125, 75, 0), center);
        }

        [Test]
        public void Distance_SamePosition_DifferentPivots_CalculatesCorrectly()
        {
            // Arrange
            var size = new Vector2(100, 50);

            // Two elements at same position but different pivots
            var pos1 = new Vector3(100, 100, 0);
            var pivot1 = new Vector2(0.5f, 0.5f); // Centered

            var pos2 = new Vector3(100, 100, 0);
            var pivot2 = new Vector2(0, 0); // Bottom-left

            // Act
            var center1 = CalculateWorldCenter(pos1, size, pivot1);
            var center2 = CalculateWorldCenter(pos2, size, pivot2);
            var distance = Vector3.Distance(center1, center2);

            // Assert
            // center1 = (100, 100, 0)
            // center2 = (150, 125, 0)
            // distance = sqrt(50^2 + 25^2) = sqrt(2500 + 625) = sqrt(3125) ≈ 55.9
            Assert.AreEqual(Mathf.Sqrt(3125), distance, 0.01f);
        }

        [Test]
        public void Distance_AdjacentElements_CenteredPivots_IsElementHeight()
        {
            // Arrange
            var size = new Vector2(100, 50);
            var pivot = new Vector2(0.5f, 0.5f);

            // Two vertically adjacent elements
            var pos1 = new Vector3(100, 100, 0);
            var pos2 = new Vector3(100, 150, 0); // 50 units above (one element height)

            // Act
            var center1 = CalculateWorldCenter(pos1, size, pivot);
            var center2 = CalculateWorldCenter(pos2, size, pivot);
            var distance = Vector3.Distance(center1, center2);

            // Assert
            Assert.AreEqual(50f, distance, 0.01f);
        }

        [Test]
        public void Distance_OverlappingElements_IsZero()
        {
            // Arrange
            var size = new Vector2(100, 50);
            var pivot = new Vector2(0.5f, 0.5f);
            var position = new Vector3(100, 100, 0);

            // Act
            var center1 = CalculateWorldCenter(position, size, pivot);
            var center2 = CalculateWorldCenter(position, size, pivot);
            var distance = Vector3.Distance(center1, center2);

            // Assert
            Assert.AreEqual(0f, distance);
        }
    }
}
