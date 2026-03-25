using Microsoft.VisualStudio.TestTools.UnitTesting;
using DictionaryLib;
using System.Collections.Generic;

namespace DictionaryLib.Tests
{
    [TestClass]
    public class SlovarTests
    {
        private Slovar _slovar;

        [TestInitialize]
        public void Setup()
        {
            // Инициализация перед каждым тестом
            _slovar = new Slovar();
            _slovar.AddWord("молоко");
            _slovar.AddWord("мама");
            _slovar.AddWord("компьютер");
        }

        [TestMethod]
        public void TestAddWord()
        {
            _slovar.AddWord("тест");
            Assert.AreEqual(4, _slovar.Count);
        }

        [TestMethod]
        public void TestExactSearch()
        {
            var results = _slovar.ExactSearch("молоко");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("молоко", results[0]);
        }

        [TestMethod]
        public void TestLevenshteinSearch_WithError()
        {
            // Проверка: найдет ли "молоко", если ввели "малако" (1 правка)
            var results = _slovar.LevenshteinSearch("малако", 2);
            Assert.IsTrue(results.Contains("молоко"));
        }

        [TestMethod]
        public void TestSearchVariant4_ByLengthAndPart()
        {
            // Ищем слово длиной 4, которое начинается/заканчивается на "ма"
            var results = _slovar.SearchVariant4(4, "ма");
            Assert.IsTrue(results.Contains("мама"));
        }

        [TestMethod]
        public void TestDeleteWord()
        {
            _slovar.DeleteWord("мама");
            Assert.AreEqual(2, _slovar.Count);
            Assert.IsFalse(_slovar.ExactSearch("мама").Count > 0);
        }

        [TestMethod]
        public void TestClearAll()
        {
            _slovar.ClearAll();
            Assert.AreEqual(0, _slovar.Count);
        }
    }
}