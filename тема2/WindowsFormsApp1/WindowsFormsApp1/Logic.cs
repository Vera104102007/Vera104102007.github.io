using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1
{
    public static class NaturalNumbersLogic
    {
        // 1. Сумма делителей числа
        /// <summary>
        /// Метод для вычисления суммы всех делителей числа
        /// </summary>
        /// <param name="n">Натуральное число</param>
        /// <returns>Возвращает сумму делителей типа long</returns>
        public static long GetSumOfDivisors(int n)
        {
            if (n <= 0) throw new ArgumentException("Число должно быть натуральным.");
            long sum = 0;
            for (int i = 1; i <= n; i++)
            {
                if (n % i == 0) sum += i;
            }
            return sum;
        }

        // 2. Проверка на совершенное число
        /// <summary>
        /// Метод для проверки, является ли число совершенным
        /// </summary>
        /// <param name="n">Натуральное число</param>
        /// <returns>Возвращает true, если число равно сумме своих делителей (кроме самого себя)</returns>
        public static bool IsPerfect(int n)
        {
            if (n <= 1) return false;
            long sum = 0;
            for (int i = 1; i <= n / 2; i++)
            {
                if (n % i == 0) sum += i;
            }
            return sum == n;
        }

        // 3. Поиск чисел с максимальной суммой делителей
        /// <summary>
        /// Поиск всех чисел с максимальной суммой делителей в заданном интервале
        /// </summary>
        /// <param name="a">Начало интервала</param>
        /// <param name="b">Конец интервала</param>
        /// <returns>Список чисел с наибольшей суммой делителей</returns>
        public static List<int> GetMaxSumDivisorsList(int a, int b)
        {
            long maxSum = -1;
            List<int> numbers = new List<int>();

            for (int i = a; i <= b; i++)
            {
                long currentSum = GetSumOfDivisors(i);
                if (currentSum > maxSum)
                {
                    maxSum = currentSum;
                    numbers.Clear();
                    numbers.Add(i);
                }
                else if (currentSum == maxSum)
                {
                    numbers.Add(i);
                }
            }
            return numbers;
        }

        // 4. Поиск двух ближайших чисел
        /// <summary>
        /// Метод для поиска двух ближайших друг к другу чисел из списка
        /// </summary>
        /// <param name="numbers">Список целых чисел</param>
        /// <returns>Кортеж из двух ближайших чисел или null, если чисел меньше двух</returns>
        public static (int, int)? FindTwoClosest(List<int> numbers)
        {
            if (numbers.Count < 2) return null;
            numbers.Sort();
            int minDiff = int.MaxValue;
            int n1 = 0, n2 = 0;

            for (int i = 0; i < numbers.Count - 1; i++)
            {
                int diff = numbers[i + 1] - numbers[i];
                if (diff < minDiff)
                {
                    minDiff = diff;
                    n1 = numbers[i];
                    n2 = numbers[i + 1];
                }
            }
            return (n1, n2);
        }
    }
}