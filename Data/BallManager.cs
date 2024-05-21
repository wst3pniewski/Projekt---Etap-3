using System.Collections.Generic;


namespace Data
{
    public class BallManager : DataLayerApi
    {
        #region privateProperties

        private readonly List<Ball> _ballsCollection = new List<Ball>();
        private BallsCanvas _ballCanvas = new BallsCanvas(0,0);

        #endregion

        #region publicMethods

        public override int GetCanvasWidth()
        {
            return _ballCanvas.Width;
        }

        public override int GetCanvasHeight()
        {
            return _ballCanvas.Height;
        }

        public override void SetCanvasWidth(int canvasWidth)
        {
            _ballCanvas.Width = canvasWidth;
        }

        public override void SetCanvasHeight(int canvasHeight)
        {
            _ballCanvas.Height = canvasHeight;
        }
        public override Ball GetBall(int ballIndex)
        {
            return _ballsCollection[ballIndex];
        }

        public override void SetBall(Ball ball)
        {
            _ballsCollection.Add(ball);
        }

        public override int GetNumberOfBalls()
        {
            return _ballsCollection.Count;
        }

        #endregion

    }
}
