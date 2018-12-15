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

        //Player damage from Main
        public int dmgReceived;
        
        //Collision variables
        public bool exists;

        //File read variable
        private FileReader fileRead;
        #endregion

        #region Initialization

        //Constructor
        public Enemies(Texture2D newTexture, Vector2 newPosition, Texture2D newProjTexture, Stream enemyInfoPath, int difficultyLevel)
        {
            //Update its texture, random position and projectile texture            
            texture = newTexture;
            position = newPosition;
            projTexture = newProjTexture;

            //Set up an array with the lines from enemy info text files (1.txt, 2.txt etc) read before
            List<string> enemyAttributes;
            fileRead = new FileReader(enemyInfoPath);
            enemyAttributes = fileRead.ReadLinesFromTextFile();

            //Update attributes based on values read
            health = Int32.Parse(enemyAttributes[0])+ 1*difficultyLevel;
            dmg = Int32.Parse(enemyAttributes[1])+2*difficultyLevel;
            speed = Int32.Parse(enemyAttributes[2]);
            pointsWorth = Int32.Parse(enemyAttributes[3]);

            //Damage received initially
            dmgReceived = 0;

            //Alive or not
            exists = true;
        }
        #endregion Initialization

        public void Update(GameTime gameTime)
        {
            //Update collision rectangle
            boundingRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //Update enemy movement (constant speed)
            position.X -= speed*deltaTime/15;

            //Move enemy back to right of screen if he flies off left
            if (position.X <= -200)
                position.X += 1118+200;

            if (!exists) FlaggedForRemoval = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw enemy sprite
            spriteBatch.Draw(texture, position, Color.White);
        }


        #region Collision
        //Collision Detection
        public override bool CollisionTest(Collidable obj)
        {
            if (obj != null)
            {
                return boundingRectangle.Intersects(obj.BoundingRectangle);
            }
            else
                return false;
        }

        //Collision Response
        public override void OnCollision(Collidable obj)
        {
            //Check collision with player
            Player pl = obj as Player;
            if (pl != null)
            {
               exists = false;
               ScoreManager.UpdateScore(pointsWorth);
            }

            Projectile pr = obj as Projectile;
            Console.WriteLine(pr);
            if (pr != null)
            {             
                health -= dmgReceived;
                if (health <= 0)
                {
                    ScoreManager.UpdateScore(pointsWorth);
                    exists = false;
                    FlaggedForRemoval = true;
                }
            }

        }
        #endregion

    }
}
