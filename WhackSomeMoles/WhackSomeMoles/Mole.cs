using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WhackSomeMoles
{
    public class Mole
    {
        public Texture2D MoleImg;
        float posX;
        float posY;


        public Vector2 Peakpos;
        public Vector2 Bottompos;
        public Vector2 pos;
        public Vector2 velocity;

        public bool Hittable;

        public Rectangle HitRect;

        public Mole(Texture2D MoleImg, float posX, float posY, Vector2 velocity, bool Hittable)
        {
            this.MoleImg = MoleImg;
            this.posX = posX;
            this.posY = posY;

            this.velocity = velocity;
            pos = new Vector2(posX, posY);
            Bottompos = pos;
            Peakpos.Y = pos.Y - MoleImg.Height* 2;

            this.Hittable = Hittable;
        }
        public void UpdateMole()
        {
            if (pos.Y > Peakpos.Y)
            {
                pos = pos + velocity;
            }
            HitRect = new Rectangle((int)(posX), (int)(posY), MoleImg.Width *3, MoleImg.Height*5/2);
        }

        public void DrawMole(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MoleImg, pos, null, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
        }

        public bool IsMoleHittable(int x, int y)
        {
            bool Hit = false;
            Rectangle rect = new Rectangle((int)(pos.X), (int)(pos.Y), MoleImg.Width, MoleImg.Height);

            if (HitRect.Contains(x, y))
            {
                Hit = true;
                Hittable = false;
            }
            return Hit;
        }

        public bool IsMoleHit(int x, int y)
        {
            bool Hit = false;
            Rectangle rect = new Rectangle((int)(pos.X), (int)(pos.Y), MoleImg.Width, MoleImg.Height);

            if (HitRect.Contains(x, y) && Hittable == true)
            {
                Hit = true;
                Hittable = false;
            }
            return Hit;
        }
    }


}
