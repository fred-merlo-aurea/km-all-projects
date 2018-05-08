using NUnit.Framework;
using Shouldly;

using WpfControls;

namespace UAS.UnitTests.WpfControls
{
    [TestFixture]
    public class AddRemoveDataFetcherTest
    {
        private const int TestPropertyValue = 777;

        private AddRemoveDataFetcher.Totals _totals;
        private string _calledPropertyName;
        private AddRemoveDataFetcher.Totals _calledSender;

        [SetUp]
        public void Setup()
        {
            _totals = new AddRemoveDataFetcher.Totals(0, 0, 0, 0, 0, 0, 0, 0);
            _calledPropertyName = null;
            _calledSender = null;
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void CurrentFreeRecords_PropertyChangedSet_PropertyChangedCalled()
        {
            // Arrange
            SubscribePropertyChanged();

            // Act
            _totals.CurrentFreeRecords = TestPropertyValue;

            // Assert
            _calledSender.ShouldBe(_totals);
            _calledPropertyName.ShouldBe(nameof(_totals.CurrentFreeRecords));
            _totals.CurrentFreeRecords.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void CurrentFreeRecords_PropertyChangedNotSet_PropertyChangedNotCalled()
        {
            // Arrange, Act
            _totals.CurrentFreeRecords = TestPropertyValue;

            // Assert
            _calledSender.ShouldBeNull();
            _calledPropertyName.ShouldBeNull();
            _totals.CurrentFreeRecords.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void CurrentFreeCopies_PropertyChangedSet_PropertyChangedCalled()
        {
            // Arrange
            SubscribePropertyChanged();

            // Act
            _totals.CurrentFreeCopies = TestPropertyValue;
            
            // Assert
            _calledSender.ShouldBe(_totals);
            _calledPropertyName.ShouldBe(nameof(_totals.CurrentFreeCopies));
            _totals.CurrentFreeCopies.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void CurrentFreeCopies_PropertyChangedNotSet_PropertyChangedNotCalled()
        {
            // Arrange, Act
            _totals.CurrentFreeCopies = TestPropertyValue;

            // Assert
            _calledSender.ShouldBeNull();
            _calledPropertyName.ShouldBeNull();
            _totals.CurrentFreeCopies.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void NewFreeRecords_PropertyChangedSet_PropertyChangedCalled()
        {
            // Arrange
            SubscribePropertyChanged();

            // Act
            _totals.NewFreeRecords = TestPropertyValue;

            // Assert
            _calledSender.ShouldBe(_totals);
            _calledPropertyName.ShouldBe(nameof(_totals.NewFreeRecords));
            _totals.NewFreeRecords.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void NewFreeRecords_PropertyChangedNotSet_PropertyChangedNotCalled()
        {
            // Arrange, Act
            _totals.NewFreeRecords = TestPropertyValue;

            // Assert
            _calledSender.ShouldBeNull();
            _calledPropertyName.ShouldBeNull();
            _totals.NewFreeRecords.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void NewFreeCopies_PropertyChangedSet_PropertyChangedCalled()
        {
            // Arrange
            SubscribePropertyChanged();

            // Act
            _totals.NewFreeCopies = TestPropertyValue;

            // Assert
            _calledSender.ShouldBe(_totals);
            _calledPropertyName.ShouldBe(nameof(_totals.NewFreeCopies));
            _totals.NewFreeCopies.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void NewFreeCopies_PropertyChangedNotSet_PropertyChangedNotCalled()
        {
            // Arrange, Act
            _totals.NewFreeCopies = TestPropertyValue;

            // Assert
            _calledSender.ShouldBeNull();
            _calledPropertyName.ShouldBeNull();
            _totals.NewFreeCopies.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void CurrentPaidRecords_PropertyChangedSet_PropertyChangedCalled()
        {
            // Arrange
            SubscribePropertyChanged();

            // Act
            _totals.CurrentPaidRecords = TestPropertyValue;

            // Assert
            _calledSender.ShouldBe(_totals);
            _calledPropertyName.ShouldBe(nameof(_totals.CurrentPaidRecords));
            _totals.CurrentPaidRecords.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void CurrentPaidRecords_PropertyChangedNotSet_PropertyChangedNotCalled()
        {
            // Arrange, Act
            _totals.CurrentPaidRecords = TestPropertyValue;

            // Assert
            _calledSender.ShouldBeNull();
            _calledPropertyName.ShouldBeNull();
            _totals.CurrentPaidRecords.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void CurrentPaidCopies_PropertyChangedSet_PropertyChangedCalled()
        {
            // Arrange
            SubscribePropertyChanged();

            // Act
            _totals.CurrentPaidCopies = TestPropertyValue;

            // Assert
            _calledSender.ShouldBe(_totals);
            _calledPropertyName.ShouldBe(nameof(_totals.CurrentPaidCopies));
            _totals.CurrentPaidCopies.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void CurrentPaidCopies_PropertyChangedNotSet_PropertyChangedNotCalled()
        {
            // Arrange, Act
            _totals.CurrentPaidCopies = TestPropertyValue;

            // Assert
            _calledSender.ShouldBeNull();
            _calledPropertyName.ShouldBeNull();
            _totals.CurrentPaidCopies.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void NewPaidRecords_PropertyChangedSet_PropertyChangedCalled()
        {
            // Arrange
            SubscribePropertyChanged();

            // Act
            _totals.NewPaidRecords = TestPropertyValue;

            // Assert
            _calledSender.ShouldBe(_totals);
            _calledPropertyName.ShouldBe(nameof(_totals.NewPaidRecords));
            _totals.NewPaidRecords.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void NewPaidRecords_PropertyChangedNotSet_PropertyChangedNotCalled()
        {
            // Arrange, Act
            _totals.NewPaidRecords = TestPropertyValue;

            // Assert
            _calledSender.ShouldBeNull();
            _calledPropertyName.ShouldBeNull();
            _totals.NewPaidRecords.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void NewPaidCopies_PropertyChangedSet_PropertyChangedCalled()
        {
            // Arrange
            SubscribePropertyChanged();

            // Act
            _totals.NewPaidCopies = TestPropertyValue;

            // Assert
            _calledSender.ShouldBe(_totals);
            _calledPropertyName.ShouldBe(nameof(_totals.NewPaidCopies));
            _totals.NewPaidCopies.ShouldBe(TestPropertyValue);
        }

        [Test]
        public void NewPaidCopies_PropertyChangedNotSet_PropertyChangedNotCalled()
        {
            // Arrange, Act
            _totals.NewPaidCopies = TestPropertyValue;

            // Assert
            _calledSender.ShouldBeNull();
            _calledPropertyName.ShouldBeNull();
            _totals.NewPaidCopies.ShouldBe(TestPropertyValue);
        }

        private void SubscribePropertyChanged()
        {
            _totals.PropertyChanged += (sender, args) =>
            {
                _calledSender = sender as AddRemoveDataFetcher.Totals;
                _calledPropertyName = args.PropertyName;
            };
        }
    }
}
