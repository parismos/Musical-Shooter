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
        public Texture2D texture;
        public Vector2 position, origin;
        public int speed;

        //Player player = new Player();

        //Collision Variables
        public bool exists;
        public Rectangle collider;


        //Constructor
        public Projectile(Texture2D newTexture)
        {
            speed = 6;
            texture = newTexture;
            exists = false;
            collider = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    


        //Collision Detection
        public override bool CollisionTest(Collidable obj)
        {
            if (obj != null)
            {
                return collider.Intersects(obj.BoundingRectangle);
            }
            else
                return false;
        }

        //Collision Response
        public override void OnCollision(Collidable obj)
        {
            Enemies enemy = obj as Enemies;
            if (enemy != null)
            {
                exists = false;
                if (!exists) FlaggedForRemoval = true;
            }                               
        }
    }
}
