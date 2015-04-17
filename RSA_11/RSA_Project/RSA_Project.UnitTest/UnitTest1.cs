using System;
using System.Numerics;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RSA_Project.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        [TestMethod]
        public void TestMethod1()
        {
            const int count = 20;
            Console.WriteLine(@"# N GeneratePrimesPair");
            for (byte decimals = 20; decimals < 50; decimals++)
            {
                var rsa = new RSA(decimals);
                DateTime t = DateTime.Now;
                for (int i = 0; i < count; i++)
                    rsa.GeneratePrimesPair(decimals);
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", decimals, ts.TotalMilliseconds/count);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            const int count = 20;
            Console.WriteLine(@"# N Primary");
            for (int decimals = 20; decimals < 50; decimals++)
            {
                var bits = (int) Math.Ceiling(decimals/Math.Log10(2));
                int bytes = (bits + 7)/8;
                DateTime t = DateTime.Now;
                for (int i = 0; i < count; i++)
                {
                    var data = new byte[bytes];
                    Rng.GetBytes(data);
                    data[bytes - 1] = (byte) ((data[bytes - 1] & 127) | 64);
                    var x = new BigInteger(data);
                    while (!RSA.IsProbablePrime(x, RSA.NumberOfTests(x)))
                    {
                        x += 1;
                    }
                }
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", decimals, ts.TotalMilliseconds/count);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            const int count = 20;
            Console.WriteLine(@"# N IsProbablePrime");
            for (int decimals = 20; decimals < 50; decimals++)
            {
                var bits = (int) Math.Ceiling(decimals/Math.Log10(2));
                int bytes = (bits + 7)/8;
                DateTime t = DateTime.Now;
                int total = 0;
                for (int i = 0; i < count; i++)
                {
                    var data = new byte[bytes];
                    Rng.GetBytes(data);
                    data[bytes - 1] = (byte) ((data[bytes - 1] & 127) | 64);
                    BigInteger x = new BigInteger(data) | 1; // Простые являются нечётными
                    total += RSA.NumberOfTests(x);
                    RSA.IsProbablePrime(x, RSA.NumberOfTests(x));
                }
                var ts = new TimeSpan(DateTime.Now.Ticks - t.Ticks);
                Console.WriteLine(@"{0} {1}", decimals, ts.TotalMilliseconds/count);
            }
        }
    }
}