using Logic;

namespace BallTests
{
    [TestFixture]
    public class BallsLogicTests
    {
        private LogicLayerApi _ballsLogic;

        [SetUp]
        public void Setup()
        {
            _ballsLogic = new BallsLogic(500, 500);
        }

        [Test]
        public void GetNumberOfBalls_ReturnsCorrectCount()
        {
            _ballsLogic.AddBalls(5);
            Assert.That(_ballsLogic.GetNumberOfBalls(), Is.EqualTo(5));
        }

        [Test]
        public void GetBall_ReturnsCorrectBall()
        {
            _ballsLogic.AddBalls(1);
            var ball = _ballsLogic.GetBall(0);
            Assert.That(ball, Is.Not.Null);
        }

        [Test]
        public void AddBalls_IncreasesBallCount()
        {
            var initialCount = _ballsLogic.GetNumberOfBalls();
            _ballsLogic.AddBalls(3);
            Assert.That(_ballsLogic.GetNumberOfBalls(), Is.EqualTo(initialCount + 3));
        }
    }
}