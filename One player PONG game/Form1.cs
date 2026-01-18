using System.Diagnostics.Eventing.Reader;
using System.Media;
using Microsoft.VisualBasic;

namespace One_player_PONG_game
{
    public partial class PongForm : Form
    {
        private Ball ball = new Ball(100, 100, 3, 3, 20);
        private Player player = new Player(15, 100, 0, 13);
        private Player aiPlayer = new Player(15, 100, 0, 20);
        private SoundPlayer sound = new SoundPlayer(Path.Combine(Application.StartupPath, "assets", "pong.wav"));
        private Random rand;
        private DialogResult playAgainDialog;
        private readonly Font scoreFont = new Font("Calibri", 17, FontStyle.Regular);
        private readonly Font pauseFont = new Font("Nirmala UI", 23, FontStyle.Bold);
        private readonly Font miniPauseFont = new Font("Nirmala UI", 12, FontStyle.Regular);
        private readonly Pen penHalfLine = new Pen(Color.FromArgb(100, 255, 255, 255), 1);
        private readonly Pen penPauseMenu = new Pen(Color.Blue, 4);
        private bool GameActive = true;
        private bool ballHitPlayer = false;
        private bool ballHitAI = false;
        private bool menuPause = false;
        private bool gameExit = false;
        private int GamePointsPlayer = 0;
        private int GamePointsAi = 0;
        private int aiSpeed = 4;
        private int maxBallSpeedX = 4;
        private int maxBallSpeedY = 4;
        private int MaxPoints;
               
        public PongForm()
        {
            InitializeComponent();
            #region Form-settings
            this.DoubleBuffered = true;
            this.Width = 640;
            this.Height = 480;
            this.BackColor = Color.Black;
            #endregion
            // Set Player to center X
            player.PlayerX = (this.ClientSize.Width - player.PlayerWidth) / 2;
            aiPlayer.PlayerX = (this.ClientSize.Width - player.PlayerWidth) / 2;
            this.Focus();
            this.KeyDown += Move;
            rand = new Random();
            Input();
        }

        private void TimerGameLoop_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!GameActive) return; // If game is not active, exit
                // Ball movement
                ball.BallX += ball.BallSpeedX;
                ball.BallY += ball.BallSpeedY;
                // HardCore AI -> unbeatable
                // Ai player set to follow ball
                //aiPlayer.PlayerX = ball.BallX - (aiPlayer.PlayerWidth / 2);
                //if (aiPlayer.PlayerX < ball.BallX - aiPlayer.PlayerWidth / 2) 
                //    aiPlayer.PlayerX += aiSpeed;
                //else if (aiPlayer.PlayerX > ball.BallX - aiPlayer.PlayerWidth / 2)
                //    aiPlayer.PlayerX -= aiSpeed;    
                //aiPlayer.PlayerX<targetX? aiPlayer.PlayerX += aiSpeed : aiPlayer.PlayerX -= aiSpeed;
                #region CollisionsWithBordersPlayerAIBall
                // left/Right collision
                if (ball.BallX <= 0 || ball.BallX + ball.BallWidth >= this.ClientSize.Width)
                    ball.BallSpeedX *= -1; // ball.BallSpeedX = -ball.BallSpeedX
                // Top collision
                if (ball.BallY <= 0)
                    ball.BallSpeedY *= -1; // ball.BallSpeedY = -ball.BallSpeedY
                // Ball/Player collision
                if (ball.BallY + ball.BallWidth >= this.ClientSize.Height - player.PlayerHeight
                    && ball.BallX + ball.BallWidth >= player.PlayerX
                    && ball.BallX <= player.PlayerX + player.PlayerWidth
                    && !ballHitPlayer)
                {
                    ballHitPlayer = true;
                    sound.Play();
                    ball.BallY = this.ClientSize.Height - player.PlayerHeight - ball.BallWidth;
                    ball.BallSpeedY *= -1;
                    ball.BallSpeedX += Math.Sign(ball.BallSpeedX);
                    ball.BallSpeedY += Math.Sign(ball.BallSpeedY);
                    ball.BallSpeedX = Math.Clamp(ball.BallSpeedX, -maxBallSpeedX, maxBallSpeedX);
                    ball.BallSpeedY = Math.Clamp(ball.BallSpeedY, -maxBallSpeedY, maxBallSpeedY);
                }
                if (ball.BallSpeedY < 0)
                    ballHitPlayer = false;
                if (ball.BallSpeedY > 0)
                    ballHitAI = false;
                // Collision Ball with aiPlayer
                if (ball.BallY <= aiPlayer.PlayerY + aiPlayer.PlayerHeight
                    && ball.BallY > aiPlayer.PlayerY
                    && ball.BallX + ball.BallWidth >= aiPlayer.PlayerX
                    && ball.BallX <= aiPlayer.PlayerX + aiPlayer.PlayerWidth
                    && !ballHitAI)
                {
                    ballHitAI = true;
                    sound.Play();
                    ball.BallY = aiPlayer.PlayerY + aiPlayer.PlayerHeight;
                    ball.BallSpeedY *= -1;
                    ball.BallSpeedX += Math.Sign(ball.BallSpeedX);
                    ball.BallSpeedY += Math.Sign(ball.BallSpeedY);
                    ball.BallSpeedX = Math.Clamp(ball.BallSpeedX, -maxBallSpeedX, maxBallSpeedX);
                    ball.BallSpeedY = Math.Clamp(ball.BallSpeedY, -maxBallSpeedY, maxBallSpeedY);
                }
                // If ball get under clientSize Height -> AI points ++
                if (ball.BallY >= this.ClientSize.Height)
                {
                    GamePointsAi++;
                    ResetBallPosition();
                }
                if (ball.BallY <= 0)
                {
                    GamePointsPlayer++;
                    ResetBallPosition();
                }
                #endregion
                #region AiMovement
                int targetX = ball.BallX - aiPlayer.PlayerWidth / 2 + rand.Next(-45, 45);
                int diff = targetX - aiPlayer.PlayerX; // Difference between targetX and position X of AI
                if (Math.Abs(diff) > aiSpeed)
                    aiPlayer.PlayerX += Math.Sign(diff) * aiSpeed;
                #endregion
                #region Player/aiPlayer-Borders
                if (player.PlayerX + player.PlayerWidth > this.ClientSize.Width)
                    player.PlayerX = this.ClientSize.Width - player.PlayerWidth;
                if (aiPlayer.PlayerX + aiPlayer.PlayerWidth > this.ClientSize.Width)
                    aiPlayer.PlayerX = Math.Clamp(aiPlayer.PlayerX + Math.Sign(diff) * aiSpeed, 0, this.ClientSize.Width - aiPlayer.PlayerWidth);
                if (player.PlayerX < 0)
                    player.PlayerX = 0;
                if (aiPlayer.PlayerX < 0)
                    aiPlayer.PlayerX = 0;
                #endregion
                #region Points
                if (GamePointsPlayer >= MaxPoints)
                {
                    GameActive = false;
                    playAgainDialog = MessageBox.Show(
                        $"You Win," +
                        $"\npoints earned by Player: {GamePointsPlayer}" +
                        $"\npoints earned by AI: {GamePointsAi}" +
                        $"\nDo u want to play again?",
                        "Win!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );
                    if (playAgainDialog == DialogResult.Yes)
                        ResetGame();
                    else
                    {
                        gameExit = true;
                        this.Close();
                    }
                }
                if (GamePointsAi >= MaxPoints)
                {
                    GameActive = false;
                    playAgainDialog = MessageBox.Show(
                        $"AI Wins,\n" +
                        $"points earned by Player: {GamePointsPlayer}" +
                        $"\npoints earned by AI: {GamePointsAi}" +
                        $"\nDo u want to play again?",
                        "Lose!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );
                    if (playAgainDialog == DialogResult.Yes)
                        ResetGame();
                    else
                    {
                        gameExit = true;
                        this.Close();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Problem: {ex.Message}",
                    "Error 404",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            this.Invalidate();
        }

        private void Move(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && player.PlayerX > 0)
                player.PlayerX -= player.PlayerSpeed;
            if (e.KeyCode == Keys.Right && player.PlayerX < this.ClientSize.Width - player.PlayerWidth)
                player.PlayerX += player.PlayerSpeed;
            if (e.KeyCode == Keys.Escape)
            {
                menuPause = !menuPause;
                this.Invalidate();
            }
        }

        private void Input()
        {
            string input = Interaction.InputBox(
                "Enter the number of points you want to play to:",
                "The objective of the game",
                "5"
            );
            if (int.TryParse(input, out int result) && result > 0)
            {
                MaxPoints = result;
            }
            else
            {
                MessageBox.Show(
                    "Entered value are not valid. Default is 5 points",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                MaxPoints = 5;
            }
            TimerGameLoop.Start();
        }

        private void ResetBallPosition()
        {
            // Start X position of ball
            ball.BallX = (this.ClientSize.Width - ball.BallWidth) / 2;
            ball.BallY = aiPlayer.PlayerHeight + 10;
            ball.BallSpeedX = 3; ball.BallSpeedY = 3;
            ballHitPlayer = false;
            ballHitAI = false;
            GameActive = true;
        }

        private void ResetGame()
        {
            TimerGameLoop.Stop();
            ResetBallPosition();
            player.PlayerX = (this.ClientSize.Width - player.PlayerWidth) / 2;
            aiPlayer.PlayerX = (this.ClientSize.Width - aiPlayer.PlayerWidth) / 2;
            GamePointsAi = 0; GamePointsPlayer = 0;          
            Input();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // AntiAliasing -> On
            // Fill circle for ball
            graphics.FillEllipse(Brushes.White, ball.BallX, ball.BallY, ball.BallWidth, ball.BallWidth);
            // Fill rect for player
            graphics.FillRectangle(Brushes.GhostWhite, player.PlayerX, this.ClientSize.Height - player.PlayerHeight, player.PlayerWidth, player.PlayerHeight);
            // Fill rect for AI
            graphics.FillRectangle(Brushes.Gray, aiPlayer.PlayerX, aiPlayer.PlayerHeight, aiPlayer.PlayerWidth, aiPlayer.PlayerHeight);
            // Draw half screen line
            graphics.DrawLine(penHalfLine, 0, this.ClientSize.Height / 2, this.ClientSize.Width, this.ClientSize.Height / 2);
            // Draw string for player points
            graphics.DrawString($"{GamePointsPlayer}", scoreFont, Brushes.GhostWhite, this.ClientSize.Width - 60, this.ClientSize.Height / 2);
            // Draw string for AI points
            graphics.DrawString($"{GamePointsAi}", scoreFont, Brushes.Gray, this.ClientSize.Width - 60, this.ClientSize.Height / 2 - 40);
            if (menuPause)
            {
                // Draw Rectangle for pause game
                graphics.FillRectangle(Brushes.WhiteSmoke, (this.ClientSize.Width - 450) / 2, this.ClientSize.Height / 4, 450, 200);
                // Draw borders for pause game rectangle
                graphics.DrawRectangle(penPauseMenu, (this.ClientSize.Width - 450) / 2, this.ClientSize.Height / 4, 450, 200);
                graphics.DrawString("GAME PAUSE", pauseFont, Brushes.Black, (this.ClientSize.Width - 200) / 2, this.ClientSize.Height / 2.4f);        
                graphics.DrawString("Press ESCAPE to continue", miniPauseFont, Brushes.Black, (this.ClientSize.Width - 182) / 2, this.ClientSize.Height / 1.75f);        
                TimerGameLoop.Stop();
            }
            else
                TimerGameLoop.Start();
        }

        private void PongForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!gameExit)
            {
                TimerGameLoop.Stop();
                DialogResult result = MessageBox.Show(
                    "Are u sure u want to quit?",
                    "Exit game",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    TimerGameLoop.Start();
                }
            }            
        }
    }
}