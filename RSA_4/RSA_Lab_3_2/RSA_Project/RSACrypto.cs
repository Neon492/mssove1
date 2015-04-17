using System;
using System.Numerics;
using System.Security.Cryptography;

namespace RSA_Project
{ /*
     * Алгоритм шифрования с открытм ключом
     */

    public class RsaCrypto
    {
        private static readonly Random Rnd = new Random((int) DateTime.Now.Ticks);
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        #region  Методы для генерации случайного ключа

        private static BigInteger Random(int bytes)
        {
            if (bytes == 0) return new BigInteger(0);
            var buffer = new byte[bytes];
            Rng.GetBytes(buffer);
            buffer[bytes - 1] = (byte) ((buffer[bytes - 1] & 127) | 64); // старший байт
            return new BigInteger(buffer);
        }

        private static int NumberOfTests(BigInteger x)
        {
            return 2 * x.ToByteArray().Length + 100; // Чем больше тестов тем меньше вероятность ошибиться
        }

        private static bool IsPrimary(BigInteger x)
        {
            if (x < 2) return false; // отбрасываем отрицательные и единицу
            int len = x.ToByteArray().Length;
            int tests = NumberOfTests(x); // Чем больше тестов тем меньше вероятность ошибиться
            BigInteger y = x - 1;
            for (int i = 0; i < tests; i++)
            {
                BigInteger a = (Random((int) (len*Rnd.NextDouble()))%y) + 1; // берём ненулевое
                // проверяем выполнение малой теоремы Ферма
                // если простое то a^(x-1)==1 mod x
                if (!(BigInteger.ModPow(a, y, x) - 1).IsZero) return false;
            }
            // признаём число простым
            // хотя можем продолжать ошибаться
            // и в случае ошибки стойкость рушится
            return true;
        }

        private static BigInteger GeneratePrimary(int bytes)
        {
            BigInteger x = Random(bytes) | 1; // Простые являются нечётными
            while (!IsPrimary(x)) x += 2; // Движемся вперёд пока не встретим простое
            return x;
        }

        public void GeneratePairs(int decimals)
        {
            // 10^x==2^y 
            // x=y*log10(2)
            var bits = (int) Math.Ceiling(decimals/Math.Log10(2));
            int bits1 = bits/2;
            int bits2 = bits - bits1;
            int bytes1 = (bits1 + 7)/8;
            int bytes2 = (bits2 + 7)/8;
            _p = GeneratePrimary(bytes1);
            _q = GeneratePrimary(bytes2);
            _n = _p*_q;
            BigInteger eulierFunction = (_p - 1)*(_q - 1);
            _e = _primeNumbers[Rnd.Next(0, _primeNumbers.Length)];
            _d = GeneratePrivateKey(eulierFunction);
        }

        #endregion

        private readonly int[] _primeNumbers = {17, 257, 65537}; //Простые числа для открытого ключа(взаимо простые с N)

        private BigInteger _d,
            //private key
            _e; //public key

        private BigInteger _n; //p,q,n
        private BigInteger _p; //p,q,n
        private BigInteger _q; //p,q,n

        public RsaCrypto(int decimals)
        {
            GeneratePairs(decimals);
        }

        public RsaCrypto(BigInteger p, BigInteger q)
        {
/*
          * _p,_q два простых числа которые инициализируют класс
          */
            _p = p;
            _q = q;
            _n = _p*_q; //Вычисляем число n как произведение целых  чисел
            BigInteger eF = (_p - 1)*(_q - 1); //Считаем  функцию эйлера
            _e = _primeNumbers[Rnd.Next(0, _primeNumbers.Length)]; //Находим взаимо-простое с ним
            _d = GeneratePrivateKey(eF);
        }

        public BigInteger N
        {
            get { return _n; }
        }

        public BigInteger PrivateKey
        {
            get { return _d; }
        }

        public BigInteger PublicKey
        {
            get { return _e; }
        }


        public long NbyteLength
        {
//Длина числа  байт-масссива числа n
            get { return _n.ToByteArray().Length; }
        }

        public byte[] EncryptMessage(byte[] message)
        {
/*Шифруем сообщение открытым ключом
          * message - сообщение (в виде байт массива)
         */
            return BigInteger.ModPow(new BigInteger(message), _d, _n).ToByteArray();
        }


        public byte[] DecryptMessage(byte[] message)
        {
//Расшифруем cообщение открытм ключом
            return BigInteger.ModPow(new BigInteger(message), _e, _n).ToByteArray();
        }

        private BigInteger GeneratePrivateKey(BigInteger E)
        {
//Вычисляем закрытый ключ, находя обратный по модулю элемент кольца
            BigInteger x, y;
            BigInteger g = NOD(_e, E, out x, out y);
            return (x%E + E)%E;
        }


        private BigInteger NOD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
/*Расширеный Алгоритм Евклида
          * Решает диофантово уравнение
          * */
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
            BigInteger x1, y1;
            BigInteger d = NOD(b%a, a, out x1, out y1);
            x = y1 - (b/a)*x1;
            y = x1;
            return d;
        }
    }
}