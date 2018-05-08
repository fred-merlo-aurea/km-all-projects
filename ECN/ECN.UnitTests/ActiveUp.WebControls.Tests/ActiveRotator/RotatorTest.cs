using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ActiveUp.WebControls.Tests.Helper;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveRotator
{
    /// <summary>
    /// Unit Tests for <see cref="Rotator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RotatorTest
    {
        private const string BuildParams = "BuildParams";
        private Rotator _rotator;

        [SetUp]
        public void Setup()
        {
            _rotator = new Rotator();
        }

        [Test]
        [TestCaseSource(nameof(TransitionProperties))]
        public void BuildParams_WhenCalledWithTransitions_VerifyExpectedOutput(
            TransitionProperties properties,
            Transition transition,
            string expected)
        {
            // Arrange
            _rotator.Transition = transition;
            _rotator.Params = properties;

            // Act
            string result = _rotator.GetType().CallMethod(BuildParams, new object[0], _rotator) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBe(expected),
                () => _rotator.IsContentMatched(new Rotator
                {
                    Transition = transition,
                    Params = properties
                }, nameof(Rotator.Params)).ShouldBeTrue()
            );
        }

        private static readonly IEnumerable<TestCaseData> TransitionProperties = new[]
        {
               new TestCaseData(null, Transition.Barn, string.Empty),
               new TestCaseData(new TransitionProperties { Motion = Motion.In, Orientation = Orientation.Horizontal }, Transition.Barn, "motion=In,orientation=Horizontal"),
               new TestCaseData(new TransitionProperties { Direction = Direction.Down, Bands = 0 }, Transition.Blinds, "direction=Down,bands=0"),
               new TestCaseData(new TransitionProperties { Direction = Direction.Down, SquaresX = 0, SquaresY = 0 }, Transition.CheckerBoard, "direction=Down,squaresx=0,squaresy=0"),
               new TestCaseData(new TransitionProperties { Overlap = 0 }, Transition.Fade, "overlap=0"),
               new TestCaseData(new TransitionProperties { Motion = Motion.In, GradientSize = 0, Orientation = Orientation.Horizontal },
                   Transition.GradientWipe, "motion=In,gradientsize=0,wipestyle=0"),
               new TestCaseData(new TransitionProperties { Motion = Motion.In, IrisStyle = IrisStyle.Circle }, Transition.Iris, "motion=In,irisstyle=Circle"),
               new TestCaseData(new TransitionProperties { MaxSquare = 0 }, Transition.Pixelate, "maxsquare=0"),
               new TestCaseData(new TransitionProperties { WipeStyle = WipeStyle.Clock }, Transition.RadialWipe, "wipestyle=Clock"),
               new TestCaseData(new TransitionProperties { Orientation = Orientation.Horizontal }, Transition.RandomBars, "orientation=Horizontal"),
               new TestCaseData(new TransitionProperties { Bands = 0, SlideStyle = SlideStyle.Hide }, Transition.Slide, "bands=0,slidestyle=Hide"),
               new TestCaseData(new TransitionProperties { Direction = Direction.Down, SmoothStyle = SmoothStyle.None }, Transition.SmoothScroll, "direction=Down,smoothstyle=None"),
               new TestCaseData(new TransitionProperties { StretchStyle = StretchStyle.Push }, Transition.Stretch, "stretchstyle=Push"),
               new TestCaseData(new TransitionProperties { Direction = Direction.DownLeft }, Transition.Strips, "motion=leftdown"),
               new TestCaseData(new TransitionProperties { Direction = Direction.DownRight }, Transition.Strips, "motion=rightdown"),
               new TestCaseData(new TransitionProperties { Direction = Direction.UpLeft }, Transition.Strips, "motion=leftup"),
               new TestCaseData(new TransitionProperties { Direction = Direction.UpRight }, Transition.Strips, "motion=rightup"),
               new TestCaseData(new TransitionProperties { Spokes = 1 }, Transition.Wheel, "spokes=1"),
               new TestCaseData(new TransitionProperties { GridSizeX = 0, GridSizeY = 1 }, Transition.Zigzag, "gridsizex=0,gridsizey=1"),
               new TestCaseData(new TransitionProperties { GridSizeX = 1, GridSizeY = 2 }, Transition.Spiral, "gridsizex=1,gridsizey=2")
        };
    }
}
