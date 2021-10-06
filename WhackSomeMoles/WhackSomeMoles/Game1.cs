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

        public Texture2D holeForeground;
        public Texture2D background;
        public Texture2D lightning;
        public Texture2D moleHole;
        public Texture2D mallet;
        public Texture2D moleKO;
        public Texture2D stone;
        public Texture2D puff;
        public Texture2D mole;

        public Vector2 Gameboard;

        public int PuffTimer = 0;
        public int PuffDelay = 50;

        Point FrameSize;
        Point CurrentFrame = new Point(0, 0);
        Point SheetSize = new Point(4, 2);

        public Mole mole;

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

            holeForeground = Content.Load<Texture2D>("hole_foreground");
            background = Content.Load<Texture2D>("background");
            lightning = Content.Load<Texture2D>("lightning");
            moleHole = Content.Load<Texture2D>("mole_hole");
            mallet = Content.Load<Texture2D>("mallet");
            moleKO = Content.Load<Texture2D>("mole_KO");
            stone = Content.Load<Texture2D>("spritesheet_stone");
            puff = Content.Load<Texture2D>("spritesheet_puff");
            mole = Content.Load<Texture2D>("mole");

            Gameboard.Y = background.Height * 3;

            int FrameCutY = puff.Height / 2;
            int FrameCutX = puff.Width / 4;
            FrameSize = new Point(FrameCutY, FrameCutX);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            int Holes = moleHole.Width + moleHole.Width + moleHole.Width / 2;
            GraphicsDevice.Clear(new Color(112, 208, 72));

            spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            spriteBatch.Draw(puff, Vector2.Zero, new Rectangle(CurrentFrame.X * FrameSize.X, CurrentFrame.Y * FrameSize.Y, FrameSize.X, FrameSize.Y), Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            spriteBatch.Draw(holeForeground, Gameboard, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            spriteBatch.Draw(moleHole, Gameboard, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
