using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace _2D_Game_Musical_Theme
{
    public class SoundManager
    {
        public SoundEffect playerShoots, enemyDies;
        public Song backgroundMusic;
        public SoundManager()
        {
            enemyDies = null;
            backgroundMusic = null;
        }
        public void LoadContent(ContentManager content)
        {
            enemyDies = content.Load<SoundEffect>("Note A 2");
            backgroundMusic = content.Load<Song>("paris game mp3"); //Has to be mp3
        }
            
    }
}
