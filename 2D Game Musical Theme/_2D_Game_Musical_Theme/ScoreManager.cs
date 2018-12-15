using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D_Game_Musical_Theme
{

    public class ScoreManager
    {

        public delegate void IncreaseScoreDelegate(int x);
        public static event IncreaseScoreDelegate IncreaseScoreMethods;
        

        public ScoreManager()
        {
        }

        //To trigger event
        public static void UpdateScore(int amount)
        {
            if (IncreaseScoreMethods != null) IncreaseScoreMethods(amount);
        }

    }
}
