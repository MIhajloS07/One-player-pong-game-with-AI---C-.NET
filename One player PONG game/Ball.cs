using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_player_PONG_game
{
    public class Ball
    {
        public int BallX { get; set; }
        public int BallY { get; set; }
        public int BallSpeedX { get; set; }
        public int BallSpeedY { get; set; }
        public int BallWidth { get; set; }
        public Ball(int x, int y, int speedX, int speedY, int width)
        {
            BallX = x; BallY = y; BallSpeedX = speedX; BallSpeedY = speedY; BallWidth = width;
        }
    }
}
