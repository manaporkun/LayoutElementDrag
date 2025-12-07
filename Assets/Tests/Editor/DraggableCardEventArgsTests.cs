using NUnit.Framework;
using DefaultNamespace;

namespace DefaultNamespace.Tests
{
    /// <summary>
    /// Unit tests for the DraggableCardEventArgs struct.
    /// </summary>
    public class DraggableCardEventArgsTests
    {
        [Test]
        public void DraggableCardEventArgs_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var args = new DraggableCardEventArgs();

            // Assert
            Assert.AreEqual(0, args.StartIndex);
            Assert.AreEqual(0, args.EndIndex);
            Assert.IsNull(args.Card);
        }

        [Test]
        public void DraggableCardEventArgs_CanSetStartIndex()
        {
            // Arrange
            var args = new DraggableCardEventArgs();

            // Act
            args.StartIndex = 5;

            // Assert
            Assert.AreEqual(5, args.StartIndex);
        }

        [Test]
        public void DraggableCardEventArgs_CanSetEndIndex()
        {
            // Arrange
            var args = new DraggableCardEventArgs();

            // Act
            args.EndIndex = 10;

            // Assert
            Assert.AreEqual(10, args.EndIndex);
        }

        [Test]
        public void DraggableCardEventArgs_InitializerSyntax_SetsValues()
        {
            // Arrange & Act
            var args = new DraggableCardEventArgs
            {
                StartIndex = 3,
                EndIndex = 7,
                Card = null
            };

            // Assert
            Assert.AreEqual(3, args.StartIndex);
            Assert.AreEqual(7, args.EndIndex);
            Assert.IsNull(args.Card);
        }

        [Test]
        public void DraggableCardEventArgs_NegativeIndices_AreAllowed()
        {
            // Arrange & Act
            var args = new DraggableCardEventArgs
            {
                StartIndex = -1,
                EndIndex = -1
            };

            // Assert
            Assert.AreEqual(-1, args.StartIndex);
            Assert.AreEqual(-1, args.EndIndex);
        }
    }
}
