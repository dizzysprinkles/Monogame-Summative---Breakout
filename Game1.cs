using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text.Encodings.Web;

namespace Monogame_Summative___Breakout
{
    //TODO - need to fix requirements things for powerups... need to fix it so when it's hit it disappears
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

        Rectangle window, ballRect, paddleRect;
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

        List<Rectangle> brickRects, powerUpRects;
        List<Texture2D> brickTextures, paddleTextures, powerUpTextures;
        List<Color> brickColours;
        List<int> powerUpRequirements;
        List<bool> powerUpBools;
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
            powerUpRects = new List<Rectangle>();
            powerUpTextures = new List<Texture2D>();
            powerUpBools = new List<bool>();

            generator = new Random();

            //Rectangles
            window = new Rectangle(0,0,800,600);
            ballRect = new Rectangle(350, 500, 25, 25);
            paddleRect = new Rectangle(300, 550, 100, 25);

            //List power ups
            for (int x = 40; x <= 640; x += 120)
            {
                powerUpRects.Add(new Rectangle(x, 400, 80, 20));
            }

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

            for (int i = 0; i < powerUpRects.Count; i++)
            {
                powerUpRequirements.Add(generator.Next(10, 81));
            }

            for (int i = 0; i < powerUpRects.Count; i++)
            {
                powerUpBools.Add(false);
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

            for (int i = 1; i <= 6; i++)
            {
                powerUpTextures.Add(Content.Load<Texture2D>("Images/powerUp_"+i));
            }

            //Background Textures
            titleBackgroundTexture = Content.Load<Texture2D>("Images/titleBackground");
            tutorialBackgroundTexture = Content.Load<Texture2D>("Images/tutorialBackground");
            mainBackgroundTexture = Content.Load<Texture2D>("Images/mainBackground");
            endBackgroundTexture = Content.Load<Texture2D>("Images/endBackground");

            //Item Textures
            ballTexture = Content.Load<Texture2D>("Images/ball");

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
                    paddle.Bounds = new Rectangle(300, 550, 100, 25);
                    for (int i = 0; i < powerUpRects.Count; i++)
                    {
                        powerUpRects[i] = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Height-20), 80, 20);
                    }
                    
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
                    for (int i = 0; i < powerUpRects.Count; i++)
                    {
                        powerUpRects[i] = new Rectangle(generator.Next(0, window.Width - 80), generator.Next(0, window.Height - 20), 80, 20);
                    }
                    screenState = Screen.Main;
                }
            }
            //Main
            else if (screenState == Screen.Main)
            {
                paddle.Update(currentKeyboardState, gameTime);

                if (ball.PowerUp[0])
                {
                    bricks.Score += 50;
                    powerUpBools[0] = false;
                }
                else if (ball.PowerUp[1])
                {
                    bricks.Score += 100;
                    powerUpBools[1] = false;
                }
                else if (ball.PowerUp[2])
                {
                    bricks.Score += 250;
                    powerUpBools[2] = false;
                }
                else if (ball.PowerUp[3])
                {
                    bricks.Score += 500;
                    powerUpBools[3] = false;
                }
                else if (ball.PowerUp[4])
                {
                    ball.Speed *= 1.5f;
                    powerUpBools[4] = false;
                }
                else if (ball.PowerUp[5])
                {
                    ball.Speed /= 1.5f;
                    powerUpBools[5] = false;
                }

                ball.Update(bricks.GetBricks, paddle, bounceSoundInstance, deathSoundInstance, powerUpSoundInstance, scoreSoundInstance, powerUpRects, powerUpBools);
                List<Rectangle> hitBricks = ball.HitBricks;
                
                bricks.RemoveBricks(hitBricks);
               
                for (int i = 0; i < powerUpRects.Count; i++)
                {
                    if (bricks.GetBricks.Count <= powerUpRequirements[i] && !bricks.Intersects(powerUpRects[i]))
                    {
                        powerUpBools[i] = true;
                    }
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
                if (currentKeyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
                {
                    Exit();
                }
                if (currentKeyboardState.IsKeyDown(Keys.R) && prevKeyboardState.IsKeyUp(Keys.R))
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Volume = 0.8f;
                    MediaPlayer.Play(introSong);
                    Window.Title = "Welcome to Breakout!";
                    brickColours.Clear();
                    brickRects.Clear();
                    brickTextures.Clear();
                    ball.Speed = new Vector2(2, 2);
                    ball.Location = new Rectangle(350, 500, 25, 25);
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
                    for (int i = 0; i < brickRects.Count; i++)
                    {
                        brickTextures.Add(Content.Load<Texture2D>("Images/brick"));
                    }
                    bricks.Score = 0;
                    screenState = Screen.Title;

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
                for (int i = 0; i < powerUpRects.Count; i++)
                {
                    _spriteBatch.Draw(powerUpTextures[i], powerUpRects[i], Color.White);
                }


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
                
                for (int i = 0; i < powerUpRects.Count; i++)
                {
                    if (powerUpBools[i] == true)
                    {
                        _spriteBatch.Draw(powerUpTextures[i], powerUpRects[i], Color.White);
                    }
                }

                ball.Draw(_spriteBatch);
                paddle.Draw(_spriteBatch);
            }

            //End
            else
            {
                _spriteBatch.Draw(endBackgroundTexture, window, Color.White);

                _spriteBatch.DrawString(titleFont, "THE END", new Vector2(225, 250), Color.White);

                _spriteBatch.DrawString(instructionFont, $"You scored {bricks.Score} points!", new Vector2(225, 450), Color.White);

                _spriteBatch.DrawString(instructionFont, "Press R to Restart", new Vector2(225, 500), Color.LightSeaGreen);

                _spriteBatch.DrawString(instructionFont, "Press ESCAPE to Quit", new Vector2(225, 550), Color.PaleTurquoise);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
