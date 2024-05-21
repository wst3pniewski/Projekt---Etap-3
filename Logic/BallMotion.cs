using System;
using System.Numerics;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public class BallMotion
    {
        #region privateProperties

        private readonly Ball _ball;
        private readonly BallsLogic _ballsLogic;

        #endregion

        #region publicProperties

        public int Id;
        public double XEnd;
        public double YEnd;
        public DataLayerApi Balls { get; set; }
        public EventHandler<BallMotion>? PositionChanged;


        #endregion

        #region constructors
        public BallMotion(Ball ball, int id, BallsLogic ballsLogic, DataLayerApi ballManager, double xend, double yend)
        {
            _ball = ball;
            Id = id;
            _ballsLogic = ballsLogic;
            Balls = ballManager;
            XEnd = xend;
            YEnd = yend;
        }


        #endregion

        #region publicMethods
        public Ball GetBall()
        {
            return _ball;
        }

        public async void MoveBall()
        {
            while (!_ballsLogic.CancellationTokenSourceInstance.Token.IsCancellationRequested)
            {
                var futurePosition = _ball.BallPosition + _ball.BallSpeed;

                lock (_ballsLogic)
                {
                    for (int i = 0; i< _ballsLogic.GetNumberOfBalls(); i++)
                    {
                        Ball otherBall = _ballsLogic.GetBall(i);
                        if(Vector2.Distance(futurePosition, otherBall.BallPosition) < (this._ball.BallRadius / 2 + otherBall.BallRadius / 2))
                        {
                            
                            if (_ball.BallWeight == otherBall.BallWeight)
                            {
                                Vector2 previousSpeed = this._ball.BallSpeed;
                                this._ball.BallSpeed = otherBall.BallSpeed;
                                otherBall.BallSpeed = previousSpeed;
                            }
                            else
                            {
                                Vector2 previousSpeed = this._ball.BallSpeed;
                                _ball.BallSpeed = (_ball.BallSpeed * (_ball.BallWeight - otherBall.BallWeight) + 2 * otherBall.BallWeight * otherBall.BallSpeed) / (_ball.BallWeight + otherBall.BallWeight);
                                otherBall.BallSpeed = (otherBall.BallSpeed * (otherBall.BallWeight - _ball.BallWeight) + 2*_ball.BallWeight* previousSpeed) /(_ball.BallWeight+otherBall.BallWeight);
                            }
                        }
                    }
                }

                if (futurePosition.X + _ball.BallRadius >= XEnd || futurePosition.X - _ball.BallRadius <= 0)
                {
                    _ball.BallSpeed = new Vector2(-_ball.BallSpeed.X, _ball.BallSpeed.Y);
                }

                if (futurePosition.Y + _ball.BallRadius >= YEnd || futurePosition.Y - _ball.BallRadius <= 0)
                {
                    _ball.BallSpeed = new Vector2(_ball.BallSpeed.X, -_ball.BallSpeed.Y);
                }

                _ball.BallPosition += _ball.BallSpeed;
                PositionChanged?.Invoke(this, this);

                await Task.Delay(15, _ballsLogic.CancellationTokenSourceInstance.Token).ContinueWith(t => { });
            }
        }


        #endregion

    }
}
