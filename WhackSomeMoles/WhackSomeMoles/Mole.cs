using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TomtProjektWhackAMole
{
    public class Mole
    {
        public Texture2D MoleImg;

        public Vector2 pos;
        public Vector2 velocity;

        public bool Hittable;

        public Rectangle HitRect;

        public Mole(Texture2D MoleImg, Vector2 pos)
        {
            this.MoleImg = MoleImg;
            this.pos = pos;

            Hittable = false;
        }

        public void UpdateAstroid()
        {
            HitRect = new Rectangle((int)(pos.X), (int)(pos.Y), MoleImg.Width, MoleImg.Height);
        }

        public void DrawAstroid(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MoleImg, pos, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
        }

        public bool IsMoleHit(int x, int y)
        {
            //Checks to make sure its destroyed and changes Active to false, which make the MainGame make a new astroid
            bool Hit = false;
            Rectangle rect = new Rectangle((int)(pos.X), (int)(pos.Y), MoleImg.Width, MoleImg.Height);

            if (rect.Contains(x, y))
            {
                Hit = true;
                Hittable = false;
            }
            return Hit;
        }
    }


}
