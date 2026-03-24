using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DictionaryLib
{
    public class Slovar
    {
        private HashSet<string> wordsSet = new HashSet<string>(StringComparer.Ordinal);
        private string _filePath;

        public int Count => wordsSet.Count;

        public Slovar(string path)
        {
            _filePath = path;
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            if (!File.Exists(_filePath)) return;
            wordsSet.Clear();
            foreach (var line in File.ReadLines(_filePath, Encoding.UTF8))
            {
                if (!string.IsNullOrWhiteSpace(line))
                    wordsSet.Add(line.Trim());
            }
        }

        public void AddWord(string word) => wordsSet.Add(word.Trim());
        public void DeleteWord(string word) => wordsSet.Remove(word.Trim());
        public List<string> GetAll() => wordsSet.ToList();

        // 1. Точный поиск (Кнопка "Найти")
        public List<string> ExactSearch(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return new List<string>();
            return wordsSet.Where(w => w.Equals(word.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // 2. Метод для нечетного поиска (Вариант 4: Длина + Часть слова)
        public List<string> SearchVariant4(int length, string part)
        {
            if (string.IsNullOrEmpty(part)) return new List<string>();
            string p = part.ToLower();
            return wordsSet
                .Where(w => w.Length == length && (w.ToLower().StartsWith(p) || w.ToLower().EndsWith(p)))
                .ToList();
        }

        // 3. Поиск Левенштейна с расчетом дистанции
        public List<string> LevenshteinSearch(string target, int maxDistance)
        {
            if (string.IsNullOrEmpty(target)) return new List<string>();
            string t = target.ToLower();

            return wordsSet
                .Where(w => Math.Abs(w.Length - t.Length) <= maxDistance)
                .Select(w => new { Word = w, Dist = GetDistance(w.ToLower(), t) })
                .Where(x => x.Dist <= maxDistance)
                .OrderBy(x => x.Dist) // Самые похожие (молоко) будут первыми
                .Select(x => x.Word)
                .ToList();
        }

        public int GetDistance(string s, string t)
        {
            int n = s.Length, m = t.Length;
            int[] v0 = new int[m + 1];
            int[] v1 = new int[m + 1];
            for (int i = 0; i <= m; i++) v0[i] = i;
            for (int i = 0; i < n; i++)
            {
                v1[0] = i + 1;
                for (int j = 0; j < m; j++)
                {
                    int cost = (s[i] == t[j]) ? 0 : 1;
                    v1[j + 1] = Math.Min(Math.Min(v1[j] + 1, v0[j + 1] + 1), v0[j] + cost);
                }
                Array.Copy(v1, v0, v1.Length);
            }
            return v0[m];
        }
    }
}