using System;
using Data;

namespace Logic
{
    public abstract class LogicLayerApi
    {
        public abstract Ball GetBall(int index);
        public abstract void AddBalls(int numberOfBalls);
        public abstract int GetNumberOfBalls();
        public abstract void StopProgram();
        public abstract void StartProgram();

        public event EventHandler<BallMotion>? PositionChangedEvent;
        protected virtual void OnPositionChange(BallMotion b)
        {
            PositionChangedEvent?.Invoke(this, b);
        }
    }
}
