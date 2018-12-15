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
    public class HUD
    {
        public int score, screenW, screenH;
        public SpriteFont scoreFont;
        public Vector2 scorePosition;
        public bool toggle;
        //Constructor
        public HUD()
        {
            score = 0;
            toggle = true;
            screenH = 650;
            screenW = 1118;
            scoreFont = null;
            scorePosition = new Vector2(screenW / 2, 15);
        }
        public void LoadContent(ContentManager content)
        {
            scoreFont = content.Load<SpriteFont>("Times New Roman");
        }
        public void Update(GameTime gameTime)
        {
            //Get keyboard state
            KeyboardState keyState = Keyboard.GetState();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //if toggle = true then display HUD
            if (toggle)
                spriteBatch.DrawString(scoreFont, "Score: " + score, scorePosition, Color.Black);
        }

        //Method Subscribed to the event
        public void IncreaseScore(int amountToAdd)
        {
            score += amountToAdd;
        }
    }
}
