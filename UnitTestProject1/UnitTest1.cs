using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Exam;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Critical Cr = new Critical();
            Cr.Reshenie();
            Assert.AreEqual(29, Cr.max);
        }
        [TestMethod]
        public void TestMethod2()
        {
            Critical Cr = new Critical();
            Cr.Reshenie();
            Assert.IsNotNull(Cr.itog);
        }
        [TestMethod]
        public void TestMethod3()
        {
            Critical Cr = new Critical();
            Cr.Reshenie();
            Assert.IsInstanceOfType(Cr.s, typeof(string));
        }
        [TestMethod]
        public void TestMethod4()
        {
            Critical Cr = new Critical();
            Cr.Reshenie();
            Assert.IsTrue(Cr.max == 29);
        }
    }
}
