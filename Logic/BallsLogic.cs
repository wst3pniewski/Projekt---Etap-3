using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public class BallsLogic : LogicLayerApi
    {
        #region privateProperties

        private int Radius = 25;

        private BallsLogger logger;
        private Timer? timer;

        #endregion


        #region publicProperties
        public DataLayerApi BallCollectionManager { get; }
        public CancellationTokenSource CancellationTokenSourceInstance { get; private set; }

        #endregion


        #region constructors

        public BallsLogic(int width, int height)
        {
            BallCollectionManager = new BallManager();
            BallCollectionManager.SetCanvasWidth(width);
            BallCollectionManager.SetCanvasHeight(height);
            CancellationTokenSourceInstance = new CancellationTokenSource();
            logger = new BallsLogger($"..\\..\\..\\..\\logs\\log.json");
        }

        #endregion


        #region publicMethods

        public override int GetNumberOfBalls()
        {
            return BallCollectionManager.GetNumberOfBalls();
        }

        public override Ball GetBall(int index)
        {
            return BallCollectionManager.GetBall(index);
        }

        public override void AddBalls(int numberOfBalls)
        {
            for (var i = 0; i < numberOfBalls; i++)
            {
                int maxRadius = 50;
                var x = (float)((new Random().NextDouble() * 0.95 + 0.05) * (BallCollectionManager.GetCanvasWidth() - maxRadius));
                var y = (float)((new Random().NextDouble() * 0.95 + 0.05) * (BallCollectionManager.GetCanvasHeight() - maxRadius));
                var vx = (float)(new Random().NextDouble() * 10 + 1);
                var vy = (float)(new Random().NextDouble() * 10 + 1);
                int weight = new Random().Next(10, 60);
                int radius = new Random().Next(25, 51);
                BallCollectionManager.SetBall(new Ball(new Vector2(x, y), new Vector2(vx, vy), radius, weight));
            }
        }

        public override void StartProgram()
        {
            if (CancellationTokenSourceInstance.IsCancellationRequested) return;
            CancellationTokenSourceInstance = new CancellationTokenSource();
            timer = new Timer(LogBallsMovement, null, 0, 1000);
            for (var i = 0; i < BallCollectionManager.GetNumberOfBalls(); i++)
            {
                var ball = new BallMotion(BallCollectionManager.GetBall(i), i, this, BallCollectionManager, BallCollectionManager.GetCanvasWidth(), BallCollectionManager.GetCanvasHeight());
                ball.PositionChanged += (_, args) => OnPositionChange(args);
                Task.Factory.StartNew(ball.MoveBall, CancellationTokenSourceInstance.Token);
            }
        }

        public void LogBallsMovement(object? state)
        {
            for (var i = 0; i < BallCollectionManager.GetNumberOfBalls(); i++)
            {
                logger.makeLog($"Ball {i} is at position {BallCollectionManager.GetBall(i).BallPosition} with velocity {BallCollectionManager.GetBall(i).BallSpeed}");
            }
        }

        public override void StopProgram()
        {
            timer.Dispose();
            CancellationTokenSourceInstance.Cancel();
        }
        

        #endregion


        protected override void OnPositionChange(BallMotion args)
        {
            base.OnPositionChange(args);
        }

    }
}
