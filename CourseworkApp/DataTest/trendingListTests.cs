using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Tests
{
    [TestClass()]
    public class trendingListTests
    {

        [TestMethod()]
        public void addTest()
        {
            trendingList list = new trendingList();
            string testVar = "Test string";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(trendingList.Trendings.ElementAt(0), mixedStringTest);
            Assert.AreEqual(trendingList.Trendings.ElementAt(1), testVar);
        }

        [TestMethod()]
        public void returnValueTest()
        {
            trendingList list = new trendingList();
            trendingList.Trendings.Clear();
            string testVar = "Test string with many characters in it";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(list.returnValue(0), mixedStringTest);
            Assert.AreEqual(list.returnValue(1), testVar);
        }

        [TestMethod()]
        public void countTest()
        {
            trendingList list = new trendingList();
            trendingList.Trendings.Clear();
            string testVar = "Test string";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(list.count(), 2);
        }
    }
}