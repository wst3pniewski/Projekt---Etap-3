
namespace Data
{
    public abstract class DataLayerApi
    {
        public abstract Ball GetBall(int ballIndex);
        public abstract void SetBall(Ball ball);
        public abstract int GetNumberOfBalls();
        public abstract int GetCanvasWidth();
        public abstract int GetCanvasHeight();
        public abstract void SetCanvasWidth(int canvasWidth);
        public abstract void SetCanvasHeight(int canvasHeight);
    }
}
