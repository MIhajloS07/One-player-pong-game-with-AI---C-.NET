using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_player_PONG_game
{
    /// <summary>
    /// Represents a player (paddle) in the Pong game.
    /// Stores size, position, and movement speed.
    /// </summary>
    public class Player
    {
        public int PlayerHeight { get; set; }
        public int PlayerWidth { get; set; }
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }
        public int PlayerSpeed { get; set; }

        /// <summary>
        /// Initializes a new instance of the Player class with given size, position, and speed.
        /// </summary>
        /// <param name="height">Height of the paddle in pixels.</param>
        /// <param name="width">Width of the paddle in pixels.</param>
        /// <param name="x">Initial X coordinate of the paddle.</param>
        /// <param name="speed">Movement speed of the paddle.</param>
        /// <param name="y">Initial Y coordinate of the paddle (default 0).</param>
        public Player(int height, int width, int x,int speed, int y = 0) 
        {
            PlayerHeight = height; PlayerWidth = width; PlayerX = x; PlayerY = y; PlayerSpeed = speed;
        }     
    }
}
