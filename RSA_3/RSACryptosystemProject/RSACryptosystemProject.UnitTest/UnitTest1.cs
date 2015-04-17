using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RSACryptosystemProject.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            const int count = 20;
            Console.WriteLine(@"# N GeneratePairs");
            for (int decimals = 20; decimals < 50; decimals++)
            {
                var rsa = new Cryptosystem(decimals);
                DateTime t = DateTime.Now;
                for (int i = 0; i < count; i++)
                    rsa.GeneratePairs(decimals);
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", decimals, ts.TotalMilliseconds/count);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            const int count = 20;
            Console.WriteLine(@"# N GeneratePrimary");
            for (int decimals = 20; decimals < 50; decimals++)
            {
                var bits = (int) Math.Ceiling(decimals/Math.Log10(2));
                int bytes = (bits + 7)/8;
                DateTime t = DateTime.Now;
                for (int i = 0; i < count; i++)
                    Cryptosystem.GeneratePrimary(bytes);
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", decimals, ts.TotalMilliseconds/count);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            const int count = 20;
            Console.WriteLine(@"# N IsPrimary");
            for (int decimals = 20; decimals < 50; decimals++)
            {
                var bits = (int) Math.Ceiling(decimals/Math.Log10(2));
                int bytes = (bits + 7)/8;
                DateTime t = DateTime.Now;
                int total = 0;
                for (int i = 0; i < count; i++)
                {
                    BigInteger x = Cryptosystem.Random(bytes) | 1; // Простые являются нечётными
                    total += Cryptosystem.NumberOfTests(x);
                    Cryptosystem.IsPrimary(x);
                }
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", decimals, ts.TotalMilliseconds/total);
            }
        }
    }
}