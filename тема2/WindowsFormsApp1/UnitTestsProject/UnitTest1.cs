using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1; 
using System.Collections.Generic;

namespace UnitTestsProject
{
    [TestClass]
    public class NaturalNumbersTests
    {
        // Тест 1: Проверка суммы делителей для числа 6 (1+2+3+6 = 12)
        [TestMethod]
        public void TestGetSumOfDivisors_Input6_Returns12()
        {
            int n = 6;
            long expected = 12;

            long actual = NaturalNumbersLogic.GetSumOfDivisors(n);

            Assert.AreEqual(expected, actual, "Сумма делителей числа 6 должна быть равна 12.");
        }

        // Тест 2: Проверка на совершенное число (6 - совершенное)
        [TestMethod]
        public void TestIsPerfect_Input6_ReturnsTrue()
        {
            int n = 6;

            bool result = NaturalNumbersLogic.IsPerfect(n);

            Assert.IsTrue(result, "Число 6 является совершенным (1+2+3=6).");
        }

        // Тест 3: Проверка на несовершенное число (10 - не совершенное)
        [TestMethod]
        public void TestIsPerfect_Input10_ReturnsFalse()
        {
            int n = 10;

            bool result = NaturalNumbersLogic.IsPerfect(n);

            Assert.IsFalse(result, "Число 10 не является совершенным.");
        }

        // Тест 4: Поиск двух ближайших чисел из списка
        [TestMethod]
        public void TestFindTwoClosest_ValidList_ReturnsClosestPair()
        {
            List<int> numbers = new List<int> { 10, 50, 12, 100 };

            var result = NaturalNumbersLogic.FindTwoClosest(numbers);

            Assert.IsNotNull(result, "Результат не должен быть null.");
            Assert.AreEqual(10, result.Value.Item1);
            Assert.AreEqual(12, result.Value.Item2);
        }

        // Тест 5: Поиск списка чисел с максимальной суммой делителей в интервале [1, 10]
        // Числа и их суммы: 1(1), 2(3), 3(4), 4(7), 5(6), 6(12), 7(8), 8(15), 9(13), 10(18)
        // Максимальная сумма у 10 (сумма 18).
        [TestMethod]
        public void TestGetMaxSumDivisorsList_Range1to10_Returns10()
        {
            int a = 1;
            int b = 10;

            List<int> result = NaturalNumbersLogic.GetMaxSumDivisorsList(a, b);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(10, result[0]);
        }
    }
}