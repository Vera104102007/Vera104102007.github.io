public class Question
{
    public string Text { get; set; } = "";
    public string Image { get; set; } = "";

    // Инициализируем список сразу
    public List<string> Answers { get; set; } = new List<string>();

    public int RightIndex { get; set; }
}