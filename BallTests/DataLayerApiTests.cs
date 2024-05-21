using Data;
using System.Numerics;

namespace BallTests
{
    [TestFixture]
    public class DataLayerApiTests
    {
        private DataLayerApi _testDataLayerApi;

        [SetUp]
        public void Setup()
        {
            _testDataLayerApi = new BallManager();
        }

        [Test]
        public void GetBall_ReturnsCorrectBall()
        {
            var ball = new Ball(new Vector2(1, 1), new Vector2(1, 1), 25, 1.0f);
            _testDataLayerApi.SetBall(ball);

            var result = _testDataLayerApi.GetBall(0);

            Assert.That(result, Is.EqualTo(ball));
        }

        [Test]
        public void GetNumberOfBalls_ReturnsCorrectCount()
        {
            var ball = new Ball(new Vector2(1, 1), new Vector2(1, 1), 25, 1.0f);
            _testDataLayerApi.SetBall(ball);

            var count = _testDataLayerApi.GetNumberOfBalls();

            Assert.That(count, Is.EqualTo(1));
        }
    }
}
