using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSA_Project;

namespace ZI_Lab_Lab_3_1.UnitTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            const int count = 20;
            Console.WriteLine(@"# N GenerateKeys");
            for (int log10N = 20; log10N < 50; log10N++)
            {
                var rsa = new RsaCryptography(log10N);
                DateTime t = DateTime.Now;
                for (int i = 0; i < count; i++)
                    rsa.GenerateKeys(log10N);
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", log10N, ts.TotalMilliseconds/count);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            const int count = 20;
            Console.WriteLine(@"# N GeneratePrimary");
            for (int log10N = 20; log10N < 50; log10N++)
            {
                var bits = (int) Math.Ceiling(log10N/Math.Log10(2));
                int bytes = (bits + 7)/8;
                DateTime t = DateTime.Now;
                for (int i = 0; i < count; i++)
                    RsaCryptography.GeneratePrimary(bytes);
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", log10N, ts.TotalMilliseconds/count);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            const int count = 20;
            Console.WriteLine(@"# N IsPrimary");
            for (int log10N = 20; log10N < 50; log10N++)
            {
                var bits = (int) Math.Ceiling(log10N/Math.Log10(2));
                int bytes = (bits + 7)/8;
                DateTime t = DateTime.Now;
                int total = 0;
                for (int i = 0; i < count; i++)
                {
                    BigInteger x = RsaCryptography.Random(bytes) | 1; // Простые являются нечётными
                    total += RsaCryptography.NumberOfTests(x);
                    RsaCryptography.IsPrimary(x);
                }
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", log10N, ts.TotalMilliseconds / count);
            }
        }
    }
}