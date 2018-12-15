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
    public class Player:Collidable
    {
        #region Fields
        //Appearance variables
        public Texture2D texture, projTexture, healthTexture;

        //Attribute variables + health bar position
        public int health, startingHealth, dmg, projDelay;
        public Vector2 healthBarPosition;

        //Motion variables
        public Vector2 force, forceEnable, velocity, acceleration, startingPosition, position;
        public int mass;
        public float maxVelocity, drag, time;

        //Collision Variables
        public bool isColliding;
        public Rectangle collider, healthBar;

        //Projectile list        
        public List<Projectile> projList;

        //SoundManager for sounds and music
        SoundManager sm = new SoundManager();

        //File read variable
        private FileReader fileRead;

        CollisionManager collisionManager;
      
        #endregion  

        #region Initialization
        public Player()
        {
            //Set up an array with the lines from playerInfo text file 
            string playerInfoPath = "Content/PlayerAttributes/playerInfo.txt";
            Stream fileStream = TitleContainer.OpenStream(playerInfoPath);
            fileRead = new FileReader(fileStream);
            List<string> playerAttributes;
            playerAttributes = fileRead.ReadLinesFromTextFile();

            //Player attribute variables
            startingHealth = Int32.Parse(playerAttributes[0]); //health bar square = 30x30. So multiple of 30 for a full bar
            health = startingHealth;
            dmg = Int32.Parse(playerAttributes[1]);
            mass = 1;

            //Player texture
            texture = null;

            //Health bar position
            healthBarPosition = new Vector2(0, 0);

            //Player starting position
            startingPosition = new Vector2(1, 360);
            position = startingPosition;
            
            //Motion variables
            velocity = Vector2.Zero;
            maxVelocity = 25.0f;
            acceleration = Vector2.Zero;
            drag = 0.9f;
            time = 0f;
            forceEnable = new Vector2(0,0);
            force = new Vector2(1000, 1000);
            
            //Collision variable
            collisionManager = new CollisionManager();

            //Projectile list and delay variable
            projList = new List<Projectile>();
            projDelay = 1;
        }
        #endregion

        //Load contents
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("pick_trans2"); //Player sprite
            projTexture = content.Load<Texture2D>("note3_trans2"); //Projectile sprite
            healthTexture = content.Load<Texture2D>("health bar");
            sm.LoadContent(content);
        }  

        //Draw on screen
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(healthTexture, healthBar, Color.White);

            foreach(Projectile p in projList)
            {
                p.Draw(spriteBatch);
            }
        }


        //Update
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState(); //checking for keyboard state per frame
            //Player collider - updating every frame
            boundingRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            healthBar = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, health, 30);

            Vector2 previousPosition = position;



            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            position += velocity * deltaTime/2;

            //Keep player within screen boundaries (coordinates of sprites are based on top left corner
            if (position.Y <= 0)
                position.Y = 0;
            if (position.Y >= (650 - texture.Height))
                position.Y = 650-texture.Height;
            if (position.X <= 0)
                position.X = 0;
            if (position.X >= 1118 - texture.Width)
                position.X = 1118 -texture.Width;

            //Reset
            velocity = Vector2.Zero;
            
            UpdateProjectile();
        }

        #region Controls
        public void PlayerShoots(eButtonState buttonState, Vector2 amount)
        {
            if(buttonState == eButtonState.DOWN)
            {
                PlayerShoots();
            }
            if(buttonState ==eButtonState.UP)
            {
                projDelay = 1;
            }
        }

        public void Up(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                velocity.Y = -amount.X;
            }
        }

        public void Down(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                velocity.Y = amount.X; 
            }
        }

        public void Right(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                velocity.X = amount.X;
            }
        }

        public void Left(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                velocity.X = -amount.X;
            }
        }
        #endregion

        //Shooting function
        public void PlayerShoots()
        {
            //Fire only if projectile delay resets
            if (projDelay >= 0)
                projDelay--;

            // if projectile delay is at 0 then create a new projectile(add to list)
            if(projDelay <=0)
            {
                
                Projectile newProj = new Projectile(projTexture);
                collisionManager.AddCollidable(newProj);
                newProj.position = new Vector2(position.X + 100 - newProj.texture.Width / 2, position.Y ); //+ newProj.texture.Height/10
                newProj.collider = new Rectangle((int)newProj.position.X, (int)newProj.position.Y, newProj.texture.Width, newProj.texture.Height);
                newProj.exists = true;


                if (projList.Count() < 80)
                {
                    projList.Add(newProj);
                    
                }
            }

            //reset projectile delay
            if (projDelay == 0)
                projDelay = 50;
        }


        // update projectile functioN
        public void UpdateProjectile()
        {
            // for each projectile in the list update its position and remove it if it reaches right screen boundary
            foreach(Projectile p in projList)
            {
                //Update collider for every projectile in our list
                p.collider = new Rectangle((int)p.position.X, (int)p.position.Y, p.texture.Width, p.texture.Height);
               //Projectile motion
                p.position.X = p.position.X + p.speed+1;
                if (p.position.X >= 1118)
                    p.exists = false;
            }

            for(int i = 0; i<projList.Count; i++)
            {
                if(!projList[i].exists)
                {
                    projList.RemoveAt(i);
                    i--;
                }
            }
        }

        #region Collision
        //Collision Detection
        public override bool CollisionTest(Collidable obj)
        {
            if (obj != null)
            {
                return BoundingRectangle.Intersects(obj.BoundingRectangle);
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
                health -= enemy.dmg; //if player collides with enemy, player takes enemy's damage  
            } 
                         
        }
        #endregion  
    }
}
