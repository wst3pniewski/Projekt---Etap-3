using System.Numerics;

namespace Data
{
    public class Ball
    {
        #region publicProperties

        public Vector2 BallSpeed { get; set; }

        public Vector2 BallPosition { get; set; }

        public int BallRadius { get; set; }
        public float BallWeight { get; set; }

        #endregion

        #region constructors

        public Ball(Vector2 position, Vector2 speed, int radius, float weight)
        {
            BallPosition = position;
            BallSpeed = speed;
            BallRadius = radius;
            BallWeight = weight;
        }

        #endregion

    }
}
