using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace _2D_Game_Musical_Theme
{
    public class Enemies:Collidable
    {
        #region Fields
        //Attributes and appearance variables
        public Texture2D texture, projTexture;
        public Vector2 position;
        public int health, dmg, speed, pointsWorth;

        //Collision variables
        public bool exists;

        //File read variable
        private FileReader fileRead;
        #endregion

        #region Initialization
        public Enemies(Texture2D newTexture, Vector2 newPosition, Texture2D newProjTexture, Stream enemyInfoPath, int difficultyLevel)
        {
            //Texture, Random position, projectile texture            
            texture = newTexture;
            position = newPosition;
            projTexture = newProjTexture;

            //Create array with enemy attributes from text files (1.txt, 2.txt etc)
            List<string> enemyAttributes;
            fileRead = new FileReader(enemyInfoPath);
            enemyAttributes = fileRead.ReadLinesFromTextFile();

            //Get attributes from file
            health = Int32.Parse(enemyAttributes[0])+ 1*difficultyLevel;
            dmg = Int32.Parse(enemyAttributes[1])+2*difficultyLevel;
            speed = Int32.Parse(enemyAttributes[2]);
            pointsWorth = Int32.Parse(enemyAttributes[3]);

            //Alive or not
            exists = true;
        }
        #endregion Initialization

        public void Update(GameTime gameTime)
        {
            //Update position & collision rectangle
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            position.X -= speed*deltaTime/15;
            if (position.X <= -200) position.X += 1118 + 200; //Move enemy back to right of screen if he flies off left
            boundingRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            
            if (!exists) FlaggedForRemoval = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        #region Collision
        //Collision Detection
        public override bool CollisionTest(Collidable obj)
        {
            if (obj != null) return boundingRectangle.Intersects(obj.BoundingRectangle);
            else return false;
        }

        //Collision Response
        public override void OnCollision(Collidable obj)
        {
            //Collision with Projectile
            Projectile pr = obj as Projectile;
            if (pr!= null)
            {
                if (health <= 0)
                {
                    ScoreManager.UpdateScore(pointsWorth);
                    exists = false;
                }
            }
            
            //Collision with Player
            Player pl = obj as Player;
            if (pl != null)
            {
               exists = false;
               ScoreManager.UpdateScore(pointsWorth);
            }   
        }
        #endregion
    }
}
