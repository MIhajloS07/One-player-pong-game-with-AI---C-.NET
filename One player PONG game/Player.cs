using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_player_PONG_game
{
    public class Player
    {
        public int PlayerHeight { get; set; }
        public int PlayerWidth { get; set; }
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }
        public int PlayerSpeed { get; set; }
        public Player(int height, int width, int x,int speed, int y = 0) 
        {
            PlayerHeight = height; PlayerWidth = width; PlayerX = x; PlayerY = y; PlayerSpeed = speed;
        }
    }
}
