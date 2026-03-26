using System.Collections.Generic;

namespace WindowsFormsApp1 // Проверь, чтобы это имя совпадало с твоим проектом!
{
    public class Question
    {
        public string Text { get; set; }
        public string? Image { get; set; } // Было ImagePath
        public List<string> Answers { get; set; } = new List<string>(); // Был массив string[]
        public int RightIndex { get; set; } // Было RightAnswerIndex
    }
}