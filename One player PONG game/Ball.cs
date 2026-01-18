using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_player_PONG_game
{
    /// <summary>
    /// Represents a Ball in the Pong game.
    /// Stores size, position, and movement speed of the Ball.
    /// </summary>
    public class Ball
    {
        public int BallX { get; set; }
        public int BallY { get; set; }
        public int BallSpeedX { get; set; }
        public int BallSpeedY { get; set; }
        public int BallWidth { get; set; }

        /// <summary>
        /// Initializes a new instance of the Ball class with given size ,position and speed.
        /// </summary>
        /// <param name="x">Initial X coordinate of the Ball.</param>
        /// <param name="y">Initial Y coordinate of the Ball </param>
        /// <param name="speedX">Movement speed for X coordinate of the Ball.</param>
        /// <param name="speedY">Movement speed for Y coordinate of the Ball.</param>
        /// <param name="width">Width of the Ball in pixels.</param>
        public Ball(int x, int y, int speedX, int speedY, int width)
        {
            BallX = x; BallY = y; BallSpeedX = speedX; BallSpeedY = speedY; BallWidth = width;
        }
    }
}
