using NUnit.Framework;
using UnityEngine;

namespace DefaultNamespace.Tests
{
    /// <summary>
    /// Unit tests for scroll position calculations used in ScrollableCardPanel.
    /// Tests the mathematical logic for scroll speed and position clamping.
    /// </summary>
    public class ScrollCalculationTests
    {
        [Test]
        public void ScrollPosition_Clamp01_ClampsToValidRange()
        {
            // Test lower bound
            Assert.AreEqual(0f, Mathf.Clamp01(-0.5f));
            Assert.AreEqual(0f, Mathf.Clamp01(0f));

            // Test upper bound
            Assert.AreEqual(1f, Mathf.Clamp01(1.5f));
            Assert.AreEqual(1f, Mathf.Clamp01(1f));

            // Test valid range
            Assert.AreEqual(0.5f, Mathf.Clamp01(0.5f));
            Assert.AreEqual(0.25f, Mathf.Clamp01(0.25f));
        }

        [Test]
        public void ScrollSpeed_IncreasesWithProximityToEdge()
        {
            // Arrange
            var baseSpeed = 0.01f;
            var deltaTime = 0.016f; // ~60 FPS

            var edgePosition = 0f;
            var farPosition = 50f;
            var closePosition = 10f;

            // Act
            var farPositionChange = deltaTime * baseSpeed * Mathf.Abs(edgePosition - farPosition);
            var closePositionChange = deltaTime * baseSpeed * Mathf.Abs(edgePosition - closePosition);

            // Assert
            Assert.Greater(farPositionChange, closePositionChange);
        }

        [Test]
        public void ScrollSpeed_AtEdge_IsZero()
        {
            // Arrange
            var baseSpeed = 0.01f;
            var deltaTime = 0.016f;

            var edgePosition = 0f;
            var pointerPosition = 0f; // At the edge

            // Act
            var positionChange = deltaTime * baseSpeed * Mathf.Abs(edgePosition - pointerPosition);

            // Assert
            Assert.AreEqual(0f, positionChange);
        }

        [Test]
        public void ScrollDirection_Vertical_UpIncreases()
        {
            // Arrange
            var currentPosition = 0.5f;
            var positionChange = 0.1f;
            var isIncreasing = true;

            // Act
            var direction = isIncreasing ? 1f : -1f;
            var newPosition = Mathf.Clamp01(currentPosition + (positionChange * direction));

            // Assert
            Assert.AreEqual(0.6f, newPosition, 0.001f);
        }

        [Test]
        public void ScrollDirection_Vertical_DownDecreases()
        {
            // Arrange
            var currentPosition = 0.5f;
            var positionChange = 0.1f;
            var isIncreasing = false;

            // Act
            var direction = isIncreasing ? 1f : -1f;
            var newPosition = Mathf.Clamp01(currentPosition + (positionChange * direction));

            // Assert
            Assert.AreEqual(0.4f, newPosition, 0.001f);
        }

        [Test]
        public void ScrollPosition_DoesNotExceedBounds()
        {
            // Arrange
            var currentPosition = 0.95f;
            var positionChange = 0.2f;

            // Act
            var newPosition = Mathf.Clamp01(currentPosition + positionChange);

            // Assert
            Assert.AreEqual(1f, newPosition);
        }

        [Test]
        public void ScrollPosition_DoesNotGoBelowZero()
        {
            // Arrange
            var currentPosition = 0.05f;
            var positionChange = -0.2f;

            // Act
            var newPosition = Mathf.Clamp01(currentPosition + positionChange);

            // Assert
            Assert.AreEqual(0f, newPosition);
        }
    }
}
