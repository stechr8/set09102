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
    public class sirListTests
    {
        [TestMethod()]
        public void addTest()
        {
            sirList list = new sirList();
            string testVar = "Test string";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(sirList.SIRlist.ElementAt(0), mixedStringTest);
            Assert.AreEqual(sirList.SIRlist.ElementAt(1), testVar);
        }

        [TestMethod()]
        public void returnValueTest()
        {
            sirList list = new sirList();
            sirList.SIRlist.Clear();
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
            sirList list = new sirList();
            sirList.SIRlist.Clear();
            string testVar = "Test string";
            string mixedStringTest = "123456ABCDEF_%^";
            list.add(mixedStringTest);
            list.add(testVar);
            Assert.AreEqual(list.count(), 2);
        }
    }
}