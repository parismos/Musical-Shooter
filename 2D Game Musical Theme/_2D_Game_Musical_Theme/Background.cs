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
    class Background
    {
        public Texture2D texture;
        public Vector2 img1, img2; //2nd one start off the screen
        public int speed;

        public Background()
        {
            texture = null;
            img1 = new Vector2(0, 0);
            img2 = new Vector2(1118, 0);
            speed = 2;
        }

        //Load Content
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("penta_laptop");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, img1, Color.White);
            spriteBatch.Draw(texture, img2, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            img1.X = img1.X - speed;
            img2.X = img2.X - speed;

            //Looping background
            if (img1.X <= -1118)
            {
                img1.X = 1118;
            }
            if (img2.X <= -1118)
                img2.X = 1118;
        }

       

    }
}
