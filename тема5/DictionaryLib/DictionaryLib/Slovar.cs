using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DictionaryLib
{
    public class Slovar
    {
        // Храним слова с учетом регистра (ним != Ним)
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

        // 1. ПОИСК ОДИН В ОДИН (Находит все варианты регистра: ним, Ним)
        public List<string> ExactSearch(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return new List<string>();
            string target = word.Trim();
            return wordsSet
                .Where(w => w.Equals(target, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // 2. ПОИСК ПО ЛЕВЕНШТЕЙНУ (Меню "Поиск")
        public List<string> LevenshteinSearch(string target, int maxDistance)
        {
            if (string.IsNullOrEmpty(target)) return new List<string>();
            string t = target.ToLower();

            // Оптимизация: проверяем только слова похожей длины
            return wordsSet
                .Where(w => Math.Abs(w.Length - t.Length) <= maxDistance)
                .Where(w => CalculateDistance(w.ToLower(), t) <= maxDistance)
                .ToList();
        }

        // 3. НЕЧЕТКИЙ ПОИСК (Меню "Нечетный поиск": Длина + Начало/Конец)
        public List<string> SearchVariant4(int length, string part)
        {
            if (string.IsNullOrEmpty(part)) return new List<string>();
            string p = part.ToLower();
            return wordsSet
                .Where(w => w.Length == length &&
                           (w.ToLower().StartsWith(p) || w.ToLower().EndsWith(p)))
                .ToList();
        }

        private int CalculateDistance(string s, string t)
        {
            int n = s.Length, m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            if (n == 0) return m;
            if (m == 0) return n;
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;
            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            return d[n, m];
        }
    }
}