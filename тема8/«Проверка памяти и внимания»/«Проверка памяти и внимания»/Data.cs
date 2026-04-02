using System;
using System.Collections.Generic;
using System.Drawing;

namespace Name
{
    [Serializable]
    public struct GameResult
    {
        public string Login;
        //hello
        public int Score;
        public int SeqScore;
        public DateTime Date;
    }

    public static class GameState
    {
        public static string PlayerName = "";
        public static int Interval = 2000;
        public static Color BackgroundColor = SystemColors.Control;

        public static List<string> SessionHistory = new List<string>();

        public static List<int> Sequence = new List<int>();
        public static string[] Items = { "чашка кофе", "настольная лампа", "наручные часы", "солнцезащитные очки", "чемодан", "гитара", "кроссовки", "яблоко", "книга", "карандаш", "телефон", "стул", "чайник", "подушка", "велосипед" };
    }
}