using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Game_Musical_Theme
{
    public class Explosion
    {
        #region Fields
        public Texture2D texture;
        public Vector2 position, origin;
        public float timer;
        public float interval;
        public int current_frame, spriteW, spriteH;
        public Rectangle sourceRec; //the rectangle that will grab each sprite
        public bool exists;
        #endregion

        #region Initialization
        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 20f; //time between frame switches to speed down or up the animation 
            current_frame = 1;
            spriteW = 50; //Width of each individual image on the spritesheet
            spriteH = 128; //Height of each individual image on the spritesheet
            exists = true;
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Check timer is more than the chosen interval
            if (timer > interval)
            {
                current_frame++; //show next frame
                timer = 0f; //reset timer
            }

            //if last frame on spritesheet then reset and make explosion exist = false
            if (current_frame == 18) //depending how many sprites we have on the spritesheet
            {
                exists = false;
                current_frame = 0;
            }

            sourceRec = new Rectangle(current_frame * spriteW, 0, spriteW, spriteH);
            origin = new Vector2(sourceRec.Width / 2, sourceRec.Height / 2);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (exists)
                spriteBatch.Draw(texture, position, sourceRec, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}