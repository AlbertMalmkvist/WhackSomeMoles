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

        public Texture2D holeForeground1;
        public Texture2D holeForeground2;
        public Texture2D holeForeground3;
        public Texture2D holeForeground4;
        public Texture2D holeForeground5;
        public Texture2D holeForeground6;
        public Texture2D holeForeground7;
        public Texture2D holeForeground8;
        public Texture2D holeForeground9;
        public Texture2D background;
        public Texture2D lightning;
        public Texture2D moleHole1;
        public Texture2D moleHole2;
        public Texture2D moleHole3;
        public Texture2D moleHole4;
        public Texture2D moleHole5;
        public Texture2D moleHole6;
        public Texture2D moleHole7;
        public Texture2D moleHole8;
        public Texture2D moleHole9;
        public Texture2D mallet;
        public Texture2D moleKO;
        public Texture2D stone;
        public Texture2D puff;
        public Texture2D moleImg;

        public Vector2 Gameboard;
        public Vector2 SpawnMole;
        public Vector2 SpawnHole;
        public Vector2 velocity;

        Random rand;

        MouseState mouseState, previousMouseState;

        public int PuffTimer = 0;
        public int PuffDelay = 50;

        public int GameTimer = 0;
        public int GameDelay = 1000;
        public int GameTimeLeft = 60;
        public int MoveMoleTimer = 2;
        public int LeaveMoleTimer = 0;

        public int Score = 0;



        bool Hit;
        bool Hitting;

        Point FrameSize;
        Point CurrentFrame = new Point(0, 0);
        Point SheetSize = new Point(4, 2);

        public Mole mole;

        Mole[,] moles;

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
            mouseState = Mouse.GetState();

            rand = new Random();

            holeForeground1 = Content.Load<Texture2D>("hole_foreground");
            holeForeground2 = Content.Load<Texture2D>("hole_foreground");
            holeForeground3 = Content.Load<Texture2D>("hole_foreground");
            holeForeground4 = Content.Load<Texture2D>("hole_foreground");
            holeForeground5 = Content.Load<Texture2D>("hole_foreground");
            holeForeground6 = Content.Load<Texture2D>("hole_foreground");
            holeForeground7 = Content.Load<Texture2D>("hole_foreground");
            holeForeground8 = Content.Load<Texture2D>("hole_foreground");
            holeForeground9 = Content.Load<Texture2D>("hole_foreground");

            moleHole1 = Content.Load<Texture2D>("mole_hole");
            moleHole2 = Content.Load<Texture2D>("mole_hole");
            moleHole3 = Content.Load<Texture2D>("mole_hole");
            moleHole4 = Content.Load<Texture2D>("mole_hole");
            moleHole5 = Content.Load<Texture2D>("mole_hole");
            moleHole6 = Content.Load<Texture2D>("mole_hole");
            moleHole7 = Content.Load<Texture2D>("mole_hole");
            moleHole8 = Content.Load<Texture2D>("mole_hole");
            moleHole9 = Content.Load<Texture2D>("mole_hole");
            background = Content.Load<Texture2D>("background");
            lightning = Content.Load<Texture2D>("lightning");
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
            for (int i = 0; i < moles.GetLength(0); i++)
            {
                for (int j = 0; j < moles.GetLength(1); j++)
                {
                    float SpawnX = j * moleImg.Width * 3;
                    float SpawnY = i * moleImg.Height * 5 / 2 + Gameboard.Y;
                    velocity = new Vector2(0, 0);

                    Mole AddMole = new Mole(moleImg, SpawnX, SpawnY, velocity, false);

                    moles[i, j] = AddMole;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            previousMouseState = mouseState;
            mouseState = Mouse.GetState();


            // TODO: Add your update logic here
            switch (CurrentGameState)
            {
                case GameState.Start:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        GameTimeLeft = 60;
                        CurrentGameState = GameState.Play;
                    }
                    break;
                case GameState.Play:
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        Hitting = false;
                    }
                    for (int i = 0; i < moles.GetLength(0); i++)
                    {
                        for (int j = 0; j < moles.GetLength(1); j++)
                        {
                            moles[i, j].UpdateMole();
                            if (moles[i, j].HitRect.Contains(mouseState.X, mouseState.Y))
                            {
                                if (mouseState.LeftButton == ButtonState.Pressed && Hitting == false)
                                {
                                    Hitting = true;
                                    if (moles[i, j].IsMoleHit(mouseState.X, mouseState.Y))
                                    {
                                        Score += 10;

                                        float SpawnX = j * moleImg.Width * 3;
                                        float SpawnY = i * moleImg.Height * 5 / 2;
                                        velocity = new Vector2(0, 2);

                                        Mole AddMole = new Mole(moleImg, SpawnX, SpawnY, velocity, false);

                                        moles[i, j] = AddMole;
                                    }
                                }
                            }
                        }
                    }

                    GameTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (GameTimer > GameDelay)
                    {
                        GameTimer -= GameDelay;
                        GameTimeLeft -= 1;
                        MoveMoleTimer += 1;
                        if (GameTimeLeft == 0)
                        {
                            CurrentGameState = GameState.End;
                        }
                    }
                    for (int i = 0; i < moles.GetLength(0); i++)
                    {
                        for (int j = 0; j < moles.GetLength(1); j++)
                        {
                            if (moles[i,j].HittableCheck())
                            {
                                if (moles[i,j].MoleLeave(1))
                                {
                                float SpawnX = j * moleImg.Width * 3;
                                float SpawnY = i * moleImg.Height * 5 / 2;
                                velocity = new Vector2(0, 4);

                                Mole AddMole = new Mole(moleImg, SpawnX, SpawnY, velocity, false);

                                moles[i, j] = AddMole;
                                }

                            }
                        }
                    }

                    if (MoveMoleTimer == 3)
                    {
                        MoveMoleTimer -= 3;
                        int Yran = rand.Next(0, 2);
                        int Xran = rand.Next(0, 2);
                        for (int i = 0; i < moles.GetLength(0); i++)
                        {
                            for (int j = 0; j < moles.GetLength(1); j++)
                            {
                                if (Yran == j && Xran == i)
                                {
                                    if (moles[i, j].IsMoleHittable())
                                    {
                                        float SpawnX = j * moleImg.Width * 3;
                                        float SpawnY = i * moleImg.Height * 5 / 2 + Gameboard.Y;
                                        int MoveRan = rand.Next(-3, -1);
                                        velocity = new Vector2(0, MoveRan);

                                        Mole AddMole = new Mole(moleImg, SpawnX, SpawnY, velocity, true);

                                        moles[i, j] = AddMole;
                                    }

                                }

                            }
                        }
                    }

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
                        GameTimeLeft = 60;
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


            Vector2 SpawnHole = new Vector2(0, 0);
            SpawnHole = Gameboard;
            spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            spriteBatch.Draw(puff, Vector2.Zero, new Rectangle(CurrentFrame.X * FrameSize.X, CurrentFrame.Y * FrameSize.Y, FrameSize.X, FrameSize.Y), Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            Vector2 timepiece = new Vector2(background.Width, Gameboard.Y / 4);
            switch (CurrentGameState)
            {
                case GameState.Start:
                    spriteBatch.DrawString(Font, "Press Left mouse button to start", timepiece / 3, Color.Black);
                    spriteBatch.DrawString(Font, "Press Left mouse button to start", timepiece / 3, Color.Black);
                    break;
                case GameState.Play:
                    spriteBatch.DrawString(Font, "time:" + GameTimeLeft, timepiece, Color.Black);
                    timepiece.Y -= background.Height;
                    spriteBatch.DrawString(Font, "Score:" + Score, timepiece, Color.Black);
                    for (int i = 0; i < moles.GetLength(0); i++)
                    {
                        for (int j = 0; j < moles.GetLength(1); j++)
                        {
                            if (i == 0 && j == 0)
                            {
                                spriteBatch.Draw(moleHole1, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 1 && j == 0)
                            {
                                spriteBatch.Draw(moleHole2, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 2 && j == 0)
                            {
                                spriteBatch.Draw(moleHole3, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 0 && j == 1)
                            {
                                spriteBatch.Draw(moleHole4, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 1 && j == 1)
                            {
                                spriteBatch.Draw(moleHole5, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 2 && j == 1)
                            {
                                spriteBatch.Draw(moleHole6, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 0 && j == 2)
                            {
                                spriteBatch.Draw(moleHole7, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 1 && j == 2)
                            {
                                spriteBatch.Draw(moleHole8, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 2 && j == 2)
                            {
                                spriteBatch.Draw(moleHole9, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }

                            moles[i, j].DrawMole(spriteBatch);

                            if (i == 0 && j == 0)
                            {
                                spriteBatch.Draw(holeForeground1, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 1 && j == 0)
                            {
                                spriteBatch.Draw(holeForeground2, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 2 && j == 0)
                            {
                                spriteBatch.Draw(holeForeground3, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 0 && j == 1)
                            {
                                spriteBatch.Draw(holeForeground4, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 1 && j == 1)
                            {
                                spriteBatch.Draw(holeForeground5, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 2 && j == 1)
                            {
                                spriteBatch.Draw(holeForeground6, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 0 && j == 2)
                            {
                                spriteBatch.Draw(holeForeground7, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 1 && j == 2)
                            {
                                spriteBatch.Draw(holeForeground8, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }
                            if (i == 2 && j == 2)
                            {
                                spriteBatch.Draw(holeForeground9, SpawnHole, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            }

                            SpawnHole.X += moleImg.Width * 3;
                        }
                        SpawnHole.Y += moleImg.Height * 5 / 2;
                        SpawnHole.X = Gameboard.X;
                    }
                    break;
                case GameState.End:
                    break;
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
