using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _2D_Game_Musical_Theme
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum State
        {
            Menu,
            Playing,
            GameOver
        }

        

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        
        Background img;
        HUD hud;
        CollisionManager collisionManager;
        Random rand;
        SoundManager sm;
        public int enemyDamage, enemyValue;
        public Texture2D menu;
        public Texture2D gameOver;
        string[] enemyArray = { "A", "B", "C", "D", "E", "F", "G" };
        public int difficultyLevel;

        CommandManager commandManager;

        //Lists Needed
        List<Enemies> enemyList; // = new List<Enemies>(5);
        List<Explosion> explosionList = new List<Explosion>();

        //Set first game state
        State gameState = State.Menu;

        //Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1118; //Width of game window (laptop = 1280 x 720)
            graphics.PreferredBackBufferHeight = 650; //Height of game window (laptop = 1280 x 720)
            Window.Title = "2D Musical Shooter";

            Content.RootDirectory = "Content";

            player = new Player();
            enemyList = new List<Enemies>();
            enemyDamage = 10;
            menu = null;
            gameOver = null;
            
            img = new Background();
            hud = new HUD();
            rand = new Random();

            commandManager = new CommandManager();
            collisionManager = new CollisionManager();
            sm = new SoundManager();

            //Subscribe Methods
            ScoreManager.IncreaseScoreMethods += hud.IncreaseScore;

            //Temporary array to read difficulty level
            List<String> tempArray;

            FileReader fileReader;
            //Define path to get enemy info using randIndex to randomly choose enemy type to spawn
            string difficultyLevelPath = "Content/Difficulty/difficultyLevel.txt";
            //Open stream using path defined above
            using (Stream fileStream = TitleContainer.OpenStream(difficultyLevelPath))
            {
               fileReader = new FileReader(fileStream);
                tempArray = fileReader.ReadLinesFromTextFile();
            }
            difficultyLevel = Int32.Parse(tempArray[0]);

        }

        protected override void Initialize()
        {
            //Check to see if we can open the highscore file
            if(!File.Exists("highscores.lst"))
            {
                //Create a new one
                HighScore highscore = new HighScore();
                highscore.name[0] = "Paris";
                highscore.score[0] = 50;

                highscore.name[1] = "John";
                highscore.score[1] = 40;

                highscore.name[2] = "Maria";
                highscore.score[2] = 20;

                highscore.name[3] = "Elias";
                highscore.score[3] = 10;

                SaveHighScores(highscore, "highscore.lst");
            }


            base.Initialize();
            InitializeBindings();
            LoadEnemies();
            InitializeCollidableObjects();
            this.IsFixedTimeStep = false;
        }

        private void InitializeBindings()
        {
            commandManager.AddKeyboardBinding(Keys.W, player.Up);
            commandManager.AddKeyboardBinding(Keys.S, player.Down);
            commandManager.AddKeyboardBinding(Keys.D, player.Right);
            commandManager.AddKeyboardBinding(Keys.A, player.Left);
            commandManager.AddKeyboardBinding(Keys.Space, player.PlayerShoots);
            commandManager.AddKeyboardBinding(Keys.Escape, this.ExitGame);
        }

        private void InitializeCollidableObjects()
        {
            collisionManager.AddCollidable(player);   
            //Enemies initialized as they are created in the list   
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            img.LoadContent(Content);
            hud.LoadContent(Content);
            menu = Content.Load<Texture2D>("menu");
            gameOver = Content.Load<Texture2D>("gameover");
            sm.LoadContent(Content);
            MediaPlayer.Play(sm.backgroundMusic);
            MediaPlayer.IsRepeating = true;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            commandManager.Update();
            
            
            // Updating States
            switch (gameState)
            {
                //Update playing state
                case State.Playing:
                    {
                        //Check for collisions
                        collisionManager.Update();
                        //Player Update
                        player.Update(gameTime);
                        //Player projectiles
                        player.UpdateProjectile();

                        ResolveRemovals();

                        foreach (Enemies e in enemyList)
                        {
                            e.Update(gameTime);
                            e.dmgReceived = player.dmg;
                            
                           
                            /* CODE THAT WORKS WITH PROJECTILE COLLISION
                            for (int i = 0; i < player.projList.Count; i++)
                            {
                                if (player.projList[i].collider.Intersects(e.BoundingRectangle))
                                {

                                    e.health -= e.dmgReceived;
                                    player.projList[i].exists = false;
                                    if (e.health <= 0)
                                    {
                                        sm.enemyDies.Play();
                                        explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(e.position.X, e.position.Y)));
                                        e.exists = false;
                                    }
                                }
                            }
                            */

                            /*
                            for (int i = 0; i < player.projList.Count; i++)
                            {
                                if (player.projList[i].collider.Intersects(e.BoundingRectangle))
                                {
                                    //If collision with bullet happens then dmgReceived is activated
                                    player.projList[i].exists = false; //remove projectile
                                }
                            }
                            */

                            if (e.exists == false)
                            {
                                sm.enemyDies.Play();
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(e.position.X, e.position.Y)));
                            }
                            

                            for (int i = 0; i<player.projList.Count; i++)
                                if(!player.projList[i].exists)
                                {
                                    player.projList.RemoveAt(i);
                                    i--;
                                }
                        }

                        //Update Eplosions
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }

                        //if player health = 0 go to GameOver state
                        if (player.health <= 0)
                            gameState = State.GameOver;

                        //Background update
                        img.Update(gameTime);

                        //Enemy update
                        LoadEnemies();

                        //Manage explosion
                        ManageExplosions();
                       
                        //HUD update
                        hud.Update(gameTime); // COULD ADD KEY To TOGGLE SCOREBOARD
                        break;
                    }
                //Updating Menu state
                case State.Menu:
                    {
                        //Get keyboard Inputs
                        KeyboardState keyState = Keyboard.GetState();
                        if(keyState.IsKeyDown(Keys.Enter))
                        {
                            gameState = State.Playing;
                        }
                        break;
                    }
                //Updating GameOver state
                case State.GameOver:
                    {
                        MediaPlayer.Stop();
                        KeyboardState keyState = Keyboard.GetState();
                        SaveScore();
                        if(keyState.IsKeyDown(Keys.Enter))
                        {
                            player.health = player.startingHealth;
                            player.position = player.startingPosition;
                            player.velocity = Vector2.Zero;
                            hud.score = 0;
                            enemyList.Clear();
                            explosionList.Clear();
                            player.projList.Clear();
                            gameState = State.Menu;
                            MediaPlayer.Play(sm.backgroundMusic);
                        }
                        break;
                    }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            switch(gameState)
            {
                //Drawing playstate
                case State.Playing:
                    {
                        img.Draw(spriteBatch);
                        player.Draw(spriteBatch);
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);
                        }
                        foreach (Enemies e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }
                        hud.Draw(spriteBatch);
                        break;
                    }
                    //Drawing Menu State
                case State.Menu:
                    {
                        img.Draw(spriteBatch);
                        spriteBatch.Draw(menu, new Vector2(0, 0), Color.White);
                        break;
                    }
                    //Drawing Game Over State
                case State.GameOver:
                    {
                        spriteBatch.Draw(gameOver, new Vector2(0, 0), Color.White);
                        DisplayHighscores();
                        break;
                    }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void LoadEnemies()
        {
            //Creating random posiitons for our enemies and random array entry
            int randX = rand.Next(1118, 2236);
            int randY = rand.Next(0, 550);
            int randIndex = rand.Next(0, difficultyLevel); // use level number for upper limit ?
            //Keep up the number of enemies on screen
            if(enemyList.Count()<5)
            {
                //Define path to get enemy info using randIndex to randomly choose enemy type to spawn
                string enemyInfoPath = string.Format("Content/EnemyAttributes/{0}.txt", randIndex);
                //Open stream using path defined above
                using (Stream fileStream = TitleContainer.OpenStream(enemyInfoPath))
                {
                    //Load random enemy sprite from array
                    string enemyTexture = string.Format("{0}", enemyArray[randIndex]);
                    Enemies newEnemy = new Enemies(Content.Load<Texture2D>(enemyTexture), new Vector2(randX, randY), Content.Load<Texture2D>("note3_trans2"), fileStream, difficultyLevel);
                    
                    enemyList.Add(newEnemy);
                    collisionManager.AddCollidable(newEnemy);
                }
            }
        }

        public void ManageExplosions()
        {
            //as above to remove explosion from list
            for(int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].exists)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        } 

        private void ResolveRemovals()
        {
            List<Enemies> toDelete = new List<Enemies>();
            foreach (Enemies e in enemyList)
            {
                if(e.FlaggedForRemoval)
                {
                    toDelete.Add(e);
                }
            }

            foreach(Enemies en in toDelete)
            {
                enemyList.Remove(en);
                collisionManager.RemoveCollidable(en);
            }

        }


        #region Highscore

        public static void SaveHighScores(HighScore highScore, string filename)
        {
            //Saved file location
            FileStream stream = File.Open("highscores.lst", FileMode.OpenOrCreate);
            try
            {
                //Convert to XML and use on stream
                XmlSerializer ser = new XmlSerializer(typeof(HighScore));
                ser.Serialize(stream, highScore);
            }
            finally
            {
                //close stream
                stream.Close();
            }

        }

        public static HighScore LoadScores(string filename)
        {
            HighScore highScore;
            FileStream stream = File.Open("highscores.lst", FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                //Read file
                XmlSerializer ser = new XmlSerializer(typeof(HighScore));
                highScore = (HighScore)ser.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }

            return highScore;
        }

        public void SaveScore()
        {
            HighScore highScore = LoadScores("highscores.lst");

            int index = -1;
            for(int i = 0; i < 4 ;i++)
            {
                if (hud.score>highScore.score[i])
                {
                    index = i;
                    break;
                }
            }

            if(index>-1)
            {
                //Found greater highscore, time to swap!
                for (int i = 4-1; i > index; i--)
                {
                    highScore.name[i] = highScore.name[i - 1];
                    highScore.score[i] = highScore.score[i - 1];
                }

                highScore.name[index] = "Yours";
                highScore.score[index] = hud.score;

                SaveHighScores(highScore, "highscores.lst");
            }
        }

        public void DisplayHighscores()
        {
            HighScore highScore = LoadScores("highscores.lst");
            
            spriteBatch.Draw(gameOver, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(hud.scoreFont, "Highscores: ", new Vector2(500, 160), Color.Black);
            spriteBatch.DrawString(hud.scoreFont, highScore.name[0].ToString() + " , " + highScore.score[0].ToString(), new Vector2(500, 180), Color.Black);
            spriteBatch.DrawString(hud.scoreFont, highScore.name[1].ToString() + " , " + highScore.score[1].ToString(), new Vector2(500, 200), Color.Black);
            spriteBatch.DrawString(hud.scoreFont, highScore.name[2].ToString() + " , " + highScore.score[2].ToString(), new Vector2(500, 220), Color.Black);
            spriteBatch.DrawString(hud.scoreFont, highScore.name[3].ToString() + " , " + highScore.score[3].ToString(), new Vector2(500, 240), Color.Black);
        }

        #endregion

        public void ExitGame(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                this.Exit();
            }

        }
    }
}
