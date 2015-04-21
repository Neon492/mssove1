using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSA_Project;

namespace ZI_Lab_Lab_3_9.UnitTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            const int count = 20;
            Console.WriteLine(@"# N GenerateKeys");
            for (int n = 20; n < 50; n++)
            {
                var rsa = new RsaCrypt(n);
                DateTime t = DateTime.Now;
                for (int i = 0; i < count; i++)
                    rsa.GenerateKeys(n);
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", n, ts.TotalMilliseconds/count);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            const int count = 20;
            Console.WriteLine(@"# N GeneratePrimary");
            for (int n = 20; n < 50; n++)
            {
                var bits = (int) Math.Ceiling(n/Math.Log10(2));
                int bytes = (bits + 7)/8;
                DateTime t = DateTime.Now;
                for (int i = 0; i < count; i++)
                    RsaCrypt.GeneratePrimary(bytes);
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", n, ts.TotalMilliseconds/count);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            const int count = 20;
            Console.WriteLine(@"# N IsPrimary");
            for (int n = 20; n < 50; n++)
            {
                var bits = (int) Math.Ceiling(n/Math.Log10(2));
                int bytes = (bits + 7)/8;
                DateTime t = DateTime.Now;
                int total = 0;
                for (int i = 0; i < count; i++)
                {
                    BigInteger x = RsaCrypt.Random(bytes) | 1; // Простые являются нечётными
                    total += RsaCrypt.NumberOfTests(x);
                    RsaCrypt.IsPrimary(x);
                }
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", n, ts.TotalMilliseconds / count);
            }
        }
    }
}