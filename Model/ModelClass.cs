using System;
using System.Numerics;
using Logic;

namespace Model
{
    public class OnPositionChangedUiEventArgs : EventArgs
    {
        public Vector2 Position;
        public int Id;

        public OnPositionChangedUiEventArgs(Vector2 position, int id)
        {
            Position = position;
            Id = id;
        }
    }

    public class ModelClass
    {
        #region publicProperties

        public int CanvasWidth;
        public int CanvasHeight;
        public int NumberOfBalls;
        public LogicLayerApi? LogicLayer;
        public event EventHandler<OnPositionChangedUiEventArgs>? OnBallPositionChanged;

        #endregion

        #region constructors

        public ModelClass()
        {
            NumberOfBalls = 0;
            CanvasWidth = 1000;
            CanvasHeight = 650;
            LogicLayer = new BallsLogic(CanvasWidth, CanvasHeight);


            LogicLayer.PositionChangedEvent += (sender, ballMotion) =>
            {
                OnBallPositionChanged?.Invoke(this, new OnPositionChangedUiEventArgs(ballMotion.GetBall().BallPosition, ballMotion.Id));
            };

        }

        #endregion

        #region publicMethods

        public void StartProgram()
        {
            if (LogicLayer == null)
            {
                return;
            }

            //LogicLayer.AddBalls(NumberOfBalls);
            LogicLayer.StartProgram();
        }

        public void StopProgram()
        {
            if (LogicLayer == null)
            {
                return;
            }

            LogicLayer.StopProgram();
            LogicLayer = new BallsLogic(CanvasWidth, CanvasHeight);
            LogicLayer.PositionChangedEvent += (sender, b) =>
            {
                OnBallPositionChanged?.Invoke(this, new OnPositionChangedUiEventArgs(b.GetBall().BallPosition, b.Id));
            };
        }

        public void SetBallsNumber(int numberOfBalls)
        {
            NumberOfBalls = numberOfBalls;
        }

        public void AddBallsToLogicLayer(int numberOfBalls)
        {
            LogicLayer.AddBalls(NumberOfBalls);
        }

        public int GetBallsNumber()
        {
            return NumberOfBalls;
        }

        public int GetBallRadius(int index)
        {
            return LogicLayer.GetBall(index).BallRadius;
        }

        #endregion

    }
}
