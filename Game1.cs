using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        Rectangle window, ballRect, paddleRect, fastBallRect, slowBallRect;
        Screen screenState;
        Texture2D titleBackgroundTexture, tutorialBackgroundTexture, mainBackgroundTexture, endBackgroundTexture, ballTexture, damagedTexture, fastBallTexture, slowBallTexture;
        KeyboardState currentKeyboardState, prevKeyboardState;
        Ball ball;
        Brick bricks;
        Paddle paddle;
        Vector2 ballSpeed;

        bool fastBool, slowBool;

        SpriteFont instructionFont, titleFont, tutorialFont; 

        List<Rectangle> brickRects;
        List<Texture2D> brickTextures, paddleTextures;
        List<Color> brickColours;
        Random generator;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            fastBool = false;
            slowBool = false;
            screenState = Screen.Title;  // NEED TO CHANGE

            brickRects = new List<Rectangle>();
            brickTextures = new List<Texture2D>();
            brickColours = new List<Color>();
            paddleTextures = new List<Texture2D>();

            generator = new Random();

            window = new Rectangle(0,0,800,600);

            ballRect = new Rectangle(350, 500, 25, 25);
            ballSpeed = new Vector2(2, 2);

            paddleRect = new Rectangle(300, 550, 100, 25);
            fastBallRect = new Rectangle(350, 420, 80, 20);
            slowBallRect = new Rectangle(200, 500, 80, 20);

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

            for (int i = 0; i < brickRects.Count; i++)
            {
                brickTextures.Add(Content.Load<Texture2D>("Images/brick"));
            }

            for (int i = 1; i <=3; i++)
            {
                paddleTextures.Add(Content.Load<Texture2D>("Images/paddle_" + i));
            }

            titleBackgroundTexture = Content.Load<Texture2D>("Images/titleBackground");
            tutorialBackgroundTexture = Content.Load<Texture2D>("Images/tutorialBackground");
            mainBackgroundTexture = Content.Load<Texture2D>("Images/mainBackground");
            endBackgroundTexture = Content.Load<Texture2D>("Images/endBackground");
            ballTexture = Content.Load<Texture2D>("Images/ball");

            fastBallTexture = Content.Load<Texture2D>("Images/powerUpFast");
            slowBallTexture = Content.Load<Texture2D>("Images/powerUpSlow");
           
            damagedTexture = Content.Load<Texture2D>("Images/damaged_1");

            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            instructionFont = Content.Load<SpriteFont>("Fonts/InstructionFont");
            tutorialFont = Content.Load<SpriteFont>("Fonts/TutorialFont");
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
                    paddle.Bounds = new Rectangle(300, 350, 100, 25);
                    screenState = Screen.Tutorial;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                {
                    screenState = Screen.Main;
                }
            }
            //Tutorial
            else if (screenState == Screen.Tutorial)
            {
                paddle.Update(currentKeyboardState, gameTime);
                if (currentKeyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                {
                    paddle.Bounds = new Rectangle(300, 550, 100, 25);
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
                    fastBool = false;
                }

                if (ball.Slow)
                {
                    ball.Speed /= 1.5f;
                    slowBool = false;
                }

                ball.Update(bricks.GetBricks, paddle, bricks.GetTextures, fastBallRect, fastBool, slowBool, slowBallRect);
                List<Rectangle> hitBricks = ball.HitBricks;
                //List<int> damagedBricks = ball.DamagedBricks;

                //for (int i = 0; i < hitBricks.Count; i++)
                //{
                //    bricks.GetTextures[damagedBricks[i]] = damagedTexture;
                //}
                bricks.RemoveBricks(hitBricks);
                if (bricks.GetBricks.Count == 64)
                {
                    fastBool = true;
                }

                if (bricks.GetBricks.Count == 40)
                {
                    slowBool = true;
                }
               

                if (currentKeyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                {
                    screenState = Screen.End;
                }

                if (ball.Speed == Vector2.Zero)
                {
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
                _spriteBatch.DrawString(tutorialFont, "Your goal is to make sure the ball doesn't \ntouch the ground AND to destroy all the bricks", new Vector2(20, 90), Color.White);
                paddle.Draw(_spriteBatch);

                _spriteBatch.DrawString(instructionFont, "Press SPACE to go to the MAIN game", new Vector2(20, 550), Color.PaleTurquoise);
            }
            //Main
            else if (screenState == Screen.Main)
            {
                _spriteBatch.Draw(mainBackgroundTexture, window, Color.White);

                for (int i = 0; i < brickRects.Count; i++)
                {
                    bricks.Draw(_spriteBatch);
                }

                if (fastBool == true)
                {
                    _spriteBatch.Draw(fastBallTexture, fastBallRect, Color.White);
                }

                if (slowBool == true)
                {
                    _spriteBatch.Draw(slowBallTexture, slowBallRect, Color.White);
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
