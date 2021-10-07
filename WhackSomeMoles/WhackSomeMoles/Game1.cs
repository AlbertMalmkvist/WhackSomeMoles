using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WhackSomeMoles
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D Screen;

        SpriteFont Font;

        enum GameState
        {
            Start,
            Play,
            End,
        }

        GameState CurrentGameState;

        public Texture2D holeForeground;
        public Texture2D background;
        public Texture2D lightning;
        public Texture2D moleHole;
        public Texture2D mallet;
        public Texture2D moleKO;
        public Texture2D stone;
        public Texture2D puff;
        public Texture2D moleImg;

        public Vector2 Gameboard;
        public Vector2 SpawnMole;

        Random rand;

        MouseState mouseState, previousMouseState;

        public int PuffTimer = 0;
        public int PuffDelay = 50;
        public int GameTimer = 600;

        bool Hit;

        Point FrameSize;
        Point CurrentFrame = new Point(0, 0);
        Point SheetSize = new Point(4, 2);

        public Mole mole;

        Mole[,] moles;
        Array[,] holes;
        Array[,] grass;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Screen = new RenderTarget2D(GraphicsDevice, 1920, 1920);
            graphics.PreferredBackBufferHeight = 1920;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            CurrentGameState = GameState.Start;

            holeForeground = Content.Load<Texture2D>("hole_foreground");
            background = Content.Load<Texture2D>("background");
            lightning = Content.Load<Texture2D>("lightning");
            moleHole = Content.Load<Texture2D>("mole_hole");
            mallet = Content.Load<Texture2D>("mallet");
            moleKO = Content.Load<Texture2D>("mole_KO");
            puff = Content.Load<Texture2D>("spritesheet_puff");
            moleImg = Content.Load<Texture2D>("mole");
            Font = Content.Load<SpriteFont>("Font");

            Gameboard.Y = background.Height * 3;

            int FrameCutY = puff.Height / 2;
            int FrameCutX = puff.Width / 4;
            FrameSize = new Point(FrameCutY, FrameCutX);

            moles = new Mole[3, 3];
            holes = new Array[3, 3];
            grass = new Array[3, 3];
            for (int i = 0; i < moles.GetLength(0); i++)
            {
                for (int j = 0; j < moles.GetLength(1); j++)
                {
                    float SpawnX = j * moleImg.Width;
                    float SpawnY = i * moleImg.Height + Gameboard.Y;

                    Mole AddMole = new Mole(moleImg, SpawnX, SpawnY);

                    moles[i, j] = AddMole;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (CurrentGameState)
            {
                case GameState.Start:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        CurrentGameState = GameState.Play;
                    }
                    break;
                case GameState.Play:
                    PuffTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (PuffTimer > PuffDelay)
                    {
                        PuffTimer -= PuffDelay;
                        ++CurrentFrame.X;
                        if (CurrentFrame.X >= SheetSize.X)
                        {
                            CurrentFrame.X = 0;
                            ++CurrentFrame.Y;
                            if (CurrentFrame.Y >= SheetSize.Y)
                            {
                                CurrentFrame.Y = 0;
                            }
                        }

                    }
                    break;
                case GameState.End:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        CurrentGameState = GameState.Play;
                    }
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        Exit();
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(112, 208, 72));

            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.Start:
                    break;
                case GameState.Play:
                    break;
                case GameState.End:
                    break;
            }

            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            spriteBatch.Draw(puff, Vector2.Zero, new Rectangle(CurrentFrame.X * FrameSize.X, CurrentFrame.Y * FrameSize.Y, FrameSize.X, FrameSize.Y), Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            for (int i = 0; i < moles.GetLength(0); i++)
            {
                for (int j = 0; j < moles.GetLength(1); j++)
                {
                    spriteBatch.Draw(moleHole, Gameboard, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                    moles[i, j].DrawMole(spriteBatch);
                    spriteBatch.Draw(holeForeground, Gameboard, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                }
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
