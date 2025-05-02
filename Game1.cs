using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.Encodings.Web;

namespace Monogame_Summative___Breakout
{

    enum Screen
    {
        Title,
        Tutorial,
        Main,
        End
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle window, ballRect, paddleRect, fastPowerUpRect, slowPowerUpRect, hundredPowerUpRect, fiftyPowerUpRect, twoFiftyPowerUpRect, fiveHundredPowerUpRect;
        Screen screenState;
        Texture2D titleBackgroundTexture, tutorialBackgroundTexture, mainBackgroundTexture, endBackgroundTexture, ballTexture, fastPowerUpTexture, slowPowerUpTexture, hundredPowerUpTexture, fiftyPowerUpTexture, twoFiftyPowerUpTexture, fiveHundredPowerUpTexture;
        KeyboardState currentKeyboardState, prevKeyboardState;
        Ball ball;
        Brick bricks;
        Paddle paddle;
        Vector2 ballSpeed;

        Song introSong, mainSong, endSong;
        SoundEffect bounceSound, powerUpSound, deathSound, scoreSound;
        SoundEffectInstance bounceSoundInstance, powerUpSoundInstance, deathSoundInstance, scoreSoundInstance;

        bool fastPowerUp, slowPowerUp, hundredPowerUp, fiftyPowerUp, twoFiftyPowerUp, fiveHundredPowerUp;

        SpriteFont instructionFont, titleFont, tutorialFont; 

        List<Rectangle> brickRects;
        List<Texture2D> brickTextures, paddleTextures;
        List<Color> brickColours;
        List<int> powerUpRequirements;
        Random generator;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "Welcome to Breakout!";
            //Power Up Bools
            fastPowerUp = false;
            slowPowerUp = false;
            fiftyPowerUp = false;
            hundredPowerUp = false;
            twoFiftyPowerUp = false;
            fiveHundredPowerUp =false;

            screenState = Screen.Title;

            //Lists
            brickRects = new List<Rectangle>();
            brickTextures = new List<Texture2D>();
            brickColours = new List<Color>();
            paddleTextures = new List<Texture2D>();
            powerUpRequirements = new List<int>();

            generator = new Random();

            //Rectangles
            window = new Rectangle(0,0,800,600);
            ballRect = new Rectangle(350, 500, 25, 25);
            paddleRect = new Rectangle(300, 550, 100, 25);

            //Rectangles - Power Ups
            fastPowerUpRect = new Rectangle(40, 400, 80, 20);
            slowPowerUpRect = new Rectangle(160, 400, 80, 20);
            hundredPowerUpRect = new Rectangle(280,400,80,20);
            fiftyPowerUpRect = new Rectangle(400,400,80,20);
            twoFiftyPowerUpRect = new Rectangle(520,400,80,20);
            fiveHundredPowerUpRect = new Rectangle(640, 400, 80, 20);


            ballSpeed = new Vector2(2, 2); 

            //Loop - generates Bricks
            for (int x = 0; x < window.Width; x += 100)
            {
                for (int y = 0; y < 363; y += 33)
                {
                    brickRects.Add(new Rectangle(x, y, 100, 33));
                }
            }
            for (int i = 0; i < brickRects.Count; i++)
            {
                brickColours.Add(new Color((float)generator.NextDouble(), (float)generator.NextDouble(), (float)generator.NextDouble()));
            }

            for (int i = 0; i <= 5; i++)
            {
                powerUpRequirements.Add(generator.Next(10, 81));
            }
            
            //Window
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.ApplyChanges();

            base.Initialize();

            ball = new Ball(ballRect, ballSpeed, ballTexture, window);
            bricks = new Brick(brickRects, brickTextures, brickColours);
            paddle = new Paddle(paddleRect, paddleTextures, window);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Lists & Their Textures
            for (int i = 0; i < brickRects.Count; i++)
            {
                brickTextures.Add(Content.Load<Texture2D>("Images/brick"));
            }

            for (int i = 1; i <=3; i++)
            {
                paddleTextures.Add(Content.Load<Texture2D>("Images/paddle_" + i));
            }

            //Background Textures
            titleBackgroundTexture = Content.Load<Texture2D>("Images/titleBackground");
            tutorialBackgroundTexture = Content.Load<Texture2D>("Images/tutorialBackground");
            mainBackgroundTexture = Content.Load<Texture2D>("Images/mainBackground");
            endBackgroundTexture = Content.Load<Texture2D>("Images/endBackground");

            //Item Textures
            ballTexture = Content.Load<Texture2D>("Images/ball");
            fastPowerUpTexture = Content.Load<Texture2D>("Images/powerUpFast");
            slowPowerUpTexture = Content.Load<Texture2D>("Images/powerUpSlow");
            hundredPowerUpTexture = Content.Load<Texture2D>("Images/powerUp100");
            fiftyPowerUpTexture = Content.Load<Texture2D>("Images/powerUp50");
            twoFiftyPowerUpTexture = Content.Load<Texture2D>("Images/powerUp250");
            fiveHundredPowerUpTexture = Content.Load<Texture2D>("Images/powerUp500");

            //Fonts
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            instructionFont = Content.Load<SpriteFont>("Fonts/InstructionFont");
            tutorialFont = Content.Load<SpriteFont>("Fonts/TutorialFont");

            //Sounds & Their Instances
            bounceSound = Content.Load<SoundEffect>("SoundFX/bouncing");
            bounceSoundInstance = bounceSound.CreateInstance();
            bounceSoundInstance.IsLooped = false;

            deathSound = Content.Load<SoundEffect>("SoundFX/deathSound");
            deathSoundInstance = deathSound.CreateInstance();
            deathSoundInstance.IsLooped = false;

            powerUpSound = Content.Load<SoundEffect>("SoundFX/powerUp");
            powerUpSoundInstance = powerUpSound.CreateInstance();
            powerUpSoundInstance.IsLooped = false;

            scoreSound = Content.Load<SoundEffect>("SoundFX/score");
            scoreSoundInstance = scoreSound.CreateInstance();
            scoreSoundInstance.IsLooped = false;

            //Songs
            introSong = Content.Load<Song>("SoundFX/introMusic");
            mainSong = Content.Load<Song>("SoundFX/mainMusic");
            endSong = Content.Load<Song>("SoundFX/endMusic");

            MediaPlayer.Volume = 0.8f;
            MediaPlayer.Play(introSong);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Title
            if (screenState == Screen.Title)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                {
                    Window.Title = "Breakout - Tutorial";
                    MediaPlayer.Volume = 0.7f;
                    paddle.Bounds = new Rectangle(300, 350, 100, 25);
                    screenState = Screen.Tutorial;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                {
                    Window.Title = "Breakout - Main Game";
                    MediaPlayer.Stop();
                    MediaPlayer.Play(mainSong);
                    MediaPlayer.Volume = 0.3f;
                    fastPowerUpRect = new Rectangle(generator.Next(0, window.Width-80), generator.Next(0, window.Width-80), 80, 20);
                    slowPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    hundredPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    fiftyPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    twoFiftyPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    fiveHundredPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    screenState = Screen.Main;
                }
            }
            //Tutorial
            else if (screenState == Screen.Tutorial)
            {
                
                paddle.Update(currentKeyboardState, gameTime);
                if (currentKeyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                {
                    Window.Title = "Breakout - Main Game";
                    MediaPlayer.Stop();
                    MediaPlayer.Play(mainSong);
                    MediaPlayer.Volume = 0.3f;
                    paddle.Bounds = new Rectangle(300, 550, 100, 25);
                    fastPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    slowPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    hundredPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    fiftyPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    twoFiftyPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    fiveHundredPowerUpRect = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Width - 80), 80, 20);
                    screenState = Screen.Main;
                }
            }
            //Main
            else if (screenState == Screen.Main)
            {
                paddle.Update(currentKeyboardState, gameTime);

                if (ball.Fast == true)
                {
                    ball.Speed *=1.5f;
                    fastPowerUp = false;
                }

                if (ball.Slow)
                {
                    ball.Speed /= 1.5f;
                    slowPowerUp = false;
                }

                ball.Update(bricks.GetBricks, paddle, bricks.GetTextures, fastPowerUpRect, fastPowerUp, slowPowerUp, slowPowerUpRect, bounceSoundInstance, deathSoundInstance, powerUpSoundInstance, scoreSoundInstance);
                List<Rectangle> hitBricks = ball.HitBricks;
                
                bricks.RemoveBricks(hitBricks);
                if (bricks.GetBricks.Count == powerUpRequirements[0] && !bricks.GetBricks.Contains(fastPowerUpRect))
                {
                    fastPowerUp = true;
                }

                if (bricks.GetBricks.Count == powerUpRequirements[1] && !!bricks.GetBricks.Contains(slowPowerUpRect))
                {
                    slowPowerUp = true;
                }

                if (bricks.GetBricks.Count == powerUpRequirements[2] && !bricks.GetBricks.Contains(fiftyPowerUpRect))
                {
                    fiftyPowerUp = true;
                }

                if (bricks.GetBricks.Count == powerUpRequirements[3] && !bricks.GetBricks.Contains(hundredPowerUpRect))
                {
                    hundredPowerUp = true;
                }

                if (bricks.GetBricks.Count == powerUpRequirements[4] && !bricks.GetBricks.Contains(twoFiftyPowerUpRect))
                {
                    twoFiftyPowerUp = true;
                }

                if (bricks.GetBricks.Count == powerUpRequirements[5] && !bricks.GetBricks.Contains(fiveHundredPowerUpRect))
                {
                    fiveHundredPowerUp = true;
                }


                if (ball.Speed == Vector2.Zero || bricks.GetBricks.Count == 0)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(endSong);
                    MediaPlayer.Volume = 0.8f;
                    Window.Title = "Breakout - Game Over";
                    screenState = Screen.End;
                }
            }
            //End
            else
            {
                if (currentKeyboardState.IsKeyDown(Keys.Q) && prevKeyboardState.IsKeyUp(Keys.Q))
                {
                    Exit();
                }
            }

            prevKeyboardState = currentKeyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            //Title
            if (screenState == Screen.Title)
            {
                _spriteBatch.Draw(titleBackgroundTexture, window, Color.White);

                _spriteBatch.DrawString(titleFont, "Breakout", new Vector2(20, 10), Color.White);
                _spriteBatch.DrawString(instructionFont, "Press ENTER to go to the TUTORIAL", new Vector2(20, 500), Color.LightSeaGreen);
                _spriteBatch.DrawString(instructionFont, "Press SPACE to go to the MAIN game", new Vector2(20, 550), Color.PaleTurquoise);
            }

            //Tutorial
            else if (screenState == Screen.Tutorial)
            {
                _spriteBatch.Draw(tutorialBackgroundTexture, window, Color.White);

                _spriteBatch.DrawString(tutorialFont, "You control the paddle with the keys A & D, \nor the left and right arrow keys.", new Vector2(20, 10), Color.White);
                _spriteBatch.DrawString(tutorialFont, "Your goal is to make sure the ball doesn't \ntouch the ground AND to destroy all the bricks.", new Vector2(20, 90), Color.White);
                _spriteBatch.DrawString(tutorialFont, "There are some Power Ups that will pop up \nthroughout. Bounce the ball on them and you'll \ngain the specified power!", new Vector2(20, 170), Color.White);
                paddle.Draw(_spriteBatch);

                _spriteBatch.Draw(fiftyPowerUpTexture, fiftyPowerUpRect, Color.White);
                _spriteBatch.Draw(hundredPowerUpTexture, hundredPowerUpRect, Color.White);
                _spriteBatch.Draw(twoFiftyPowerUpTexture, twoFiftyPowerUpRect, Color.White);
                _spriteBatch.Draw(fiveHundredPowerUpTexture, fiveHundredPowerUpRect, Color.White);
                _spriteBatch.Draw(fastPowerUpTexture, fastPowerUpRect, Color.White);
                _spriteBatch.Draw(slowPowerUpTexture, slowPowerUpRect, Color.White);

                _spriteBatch.DrawString(instructionFont, "Press SPACE to go to the MAIN game", new Vector2(20, 550), Color.PaleTurquoise);
            }

            //Main
            else if (screenState == Screen.Main)
            {
                _spriteBatch.Draw(mainBackgroundTexture, window, Color.White);

                _spriteBatch.DrawString(instructionFont, $"Score: {bricks.Score}", new Vector2(600, 500), Color.White);

                for (int i = 0; i < brickRects.Count; i++)
                {
                    bricks.Draw(_spriteBatch);
                }

                if (fastPowerUp == true)
                {
                    _spriteBatch.Draw(fastPowerUpTexture, fastPowerUpRect, Color.White);
                }

                if (slowPowerUp == true)
                {
                    _spriteBatch.Draw(slowPowerUpTexture, slowPowerUpRect, Color.White);
                }

                ball.Draw(_spriteBatch);
                paddle.Draw(_spriteBatch);
            }

            //End
            else
            {
                _spriteBatch.Draw(endBackgroundTexture, window, Color.White);

                _spriteBatch.DrawString(titleFont, "THE END", new Vector2(225, 250), Color.White);

                _spriteBatch.DrawString(instructionFont, "Press ESCAPE to Quit", new Vector2(225, 550), Color.PaleTurquoise);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
