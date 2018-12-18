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
    public class Projectile : Collidable
    {
        #region Fields
        public Texture2D texture;
        public Vector2 position, origin;
        public int speed, dmg;
        public bool exists;
        #endregion

        #region Initialization
        public Projectile(Texture2D newTexture, Player pl)
        {
            speed = 6;
            texture = newTexture;
            exists = false;
            boundingRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            dmg = pl.dmg;
        }
        #endregion 

        public void Update()
        {
            position.X = position.X + speed + 1;
            if (position.X >= 1118)  exists = false; //Remove if end of screen is reached
            boundingRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        #region Collision
        //Collision Detection
        public override bool CollisionTest(Collidable obj)
        {
            if (obj != null)   return boundingRectangle.Intersects(obj.BoundingRectangle);
            else return false;
        }

        //Collision Response
        public override void OnCollision(Collidable obj)
        {
            Enemies enemy = obj as Enemies;
            if (enemy != null)
            {
                enemy.health -= dmg;
                exists = false;
                if (!exists) FlaggedForRemoval = true;
            }                             
        }
        #endregion
    }
}
