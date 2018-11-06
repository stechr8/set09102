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
    public class mentionsListTests
    {
        [TestMethod()]
        public void addTest()
        {
            mentionsList list = new mentionsList();
            string testVar = "@Test string";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(mentionsList.Mentions.ElementAt(0), mixedStringTest);
            Assert.AreEqual(mentionsList.Mentions.ElementAt(1), testVar);
        }

        [TestMethod()]
        public void returnValueTest()
        {
            mentionsList list = new mentionsList();
            mentionsList.Mentions.Clear();
            string testVar = "@Test string with many characters in it";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(list.returnValue(0), mixedStringTest);
            Assert.AreEqual(list.returnValue(1), testVar);
        }

        [TestMethod()]
        public void countTest()
        {
            mentionsList list = new mentionsList();
            mentionsList.Mentions.Clear();
            string testVar = "@Test string";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(list.count(), 2);
        }
    }
}