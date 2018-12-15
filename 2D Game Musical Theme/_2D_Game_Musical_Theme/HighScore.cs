using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace _2D_Game_Musical_Theme
{
    [Serializable]
    public class HighScore
    {
        public string[] name;
        public int[] score;

        public const int numberScores = 4;

        public HighScore()
        {
            name = new string[numberScores];
            score = new int[numberScores];
        }

    }

}
