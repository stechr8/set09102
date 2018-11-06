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
    public class urlQuarantinedListTests
    {
        [TestMethod()]
        public void addTest()
        {
            urlQuarantinedList list = new urlQuarantinedList();
            string testVar = "<Test string>";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(urlQuarantinedList.UrlQuarantines.ElementAt(0), mixedStringTest);
            Assert.AreEqual(urlQuarantinedList.UrlQuarantines.ElementAt(1), testVar);
        }

        [TestMethod()]
        public void returnValueTest()
        {
            urlQuarantinedList list = new urlQuarantinedList();
            urlQuarantinedList.UrlQuarantines.Clear();
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
            urlQuarantinedList list = new urlQuarantinedList();
            urlQuarantinedList.UrlQuarantines.Clear();
            string testVar = "Test string";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(list.count(), 2);
        }
    }
}