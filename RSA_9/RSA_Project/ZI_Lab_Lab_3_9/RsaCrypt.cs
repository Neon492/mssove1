using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace RSA_Project
{
    public class RsaCrypt
    {
        private static readonly Random Rnd = new Random((int) DateTime.Now.Ticks);
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        private readonly int[] _fermat = {17, 257, 65537}; //Числа ферма
        private BigInteger _n; //P,Q -простые числа. N - их произведение
        private BigInteger _p; //P,Q -простые числа. N - их произведение
        private BigInteger _priKey; //открытый и закрытый ключи
        private BigInteger _pubKey; //открытый и закрытый ключи
        private BigInteger _q; //P,Q -простые числа. N - их произведение

        public RsaCrypt(BigInteger p, BigInteger q)
        {
//Инициализация класса
            _p = p;
            _q = q;
            _n = _p*_q;
            BigInteger eulierFunction = (_p - 1)*(_q - 1);
            _pubKey = _fermat[Rnd.Next(0, _fermat.Length)];
            _priKey = BuildPrivateKey(eulierFunction);
        }

        public RsaCrypt(int n)
        {
            GenerateKeys(n);
        }

        public long ModuleLength
        {
//Длина модуля в байтах
            get
            {
                long l = _n.ToByteArray().Length;
                return l;
            }
        }

        public List<byte[]> EncryptMessage(IEnumerable<byte[]> p)
        {
            //Зашифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(Encrypt(block));
            }
            return result;
        }

        public List<byte[]> DecryptMessage(IEnumerable<byte[]> p)
        {
            //Разшифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(Decrypt(block));
            }
            return result;
        }

        private byte[] Encrypt(byte[] message)
        {
//Шифруем сообщение открытым ключом
            var val = new BigInteger(message);
            val = BigInteger.ModPow(val, _pubKey, _n);
            return val.ToByteArray();
        }


        private byte[] Decrypt(byte[] message)
        {
//Расшифруем cообщение открытм ключом
            var val = new BigInteger(message);
            val = BigInteger.ModPow(val, _priKey, _n);
            return val.ToByteArray();
        }

        private BigInteger BuildPrivateKey(BigInteger E)
        {
//Вычисляем закрытый ключ, находя обратный по модулю элемент кольца
            BigInteger x, y;
            BigInteger g = Gcd(_pubKey, E, out x, out y);
            return (x%E + E)%E;
        }


        private static BigInteger Gcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
//Расширеный Алгоритм Евклида
            if (a.IsZero) //Находит НОД чисел A и B, и коэфициенты x,y уравнения Ax +By = НОД(A,B)
            {
                x = 0;
                y = 1;
                return b;
            }
            BigInteger x1, y1;
            BigInteger d = Gcd(b%a, a, out x1, out y1);
            x = y1 - (b/a)*x1;
            y = x1;
            return d;
        }

        #region  Свойства

        public BigInteger Module
        {
//Возвращает значения модуля(N)
            get { return _n; }
        }


        public BigInteger PriKey
        {
//значение закрытого ключа
            get { return _priKey; }
        }

        public BigInteger PubKey
        {
//значение откртыго ключа
            get { return _pubKey; }
        }

        #endregion

        #region  Методы для генерации случайного ключа

        public static BigInteger Random(int bytes)
        {
            if (bytes == 0) return new BigInteger(0);
            var buffer = new byte[bytes];
            Rng.GetBytes(buffer);
            buffer[bytes - 1] = (byte) ((buffer[bytes - 1] & 127) | 64); // старший байт
            return new BigInteger(buffer);
        }

        public static int NumberOfTests(BigInteger x)
        {
            return 2*x.ToByteArray().Length + 100; // Чем больше тестов тем меньше вероятность ошибиться
        }

        public static bool IsPrimary(BigInteger x)
        {
            if (x < 2) return false; // отбрасываем отрицательные и единицу
            int len = x.ToByteArray().Length;
            int tests = NumberOfTests(x); // Чем больше тестов тем меньше вероятность ошибиться
            BigInteger y = x - 1;
            for (int i = 0; i < tests; i++)
            {
                BigInteger a = (Random((int) (len*Rnd.NextDouble()))%y) + 1; // берём ненулевое
                // проверяем выполнение малой теоремы Ферма
                // если простое то
                if (!(BigInteger.ModPow(a, y, x) - 1).IsZero) return false;
            }
            // признаём число простым
            // хотя можем продолжать ошибаться
            return true;
        }

        public static BigInteger GeneratePrimary(int bytes)
        {
            BigInteger x = Random(bytes) | 1; // Простые являются нечётными
            while (!IsPrimary(x)) x += 2; // Движемся вперёд пока не встретим простое
            return x;
        }

        public void GenerateKeys(int n)
        {
            var bits = (int) Math.Ceiling(n/Math.Log10(2));
            int bits1 = bits/2;
            int bits2 = bits - bits1;
            int bytes1 = (bits1 + 7)/8;
            int bytes2 = (bits2 + 7)/8;
            _p = GeneratePrimary(bytes1);
            _q = GeneratePrimary(bytes2);
            _n = _p*_q;
            BigInteger eulierFunction = (_p - 1)*(_q - 1);
            _pubKey = _fermat[Rnd.Next(0, _fermat.Length)];
            _priKey = BuildPrivateKey(eulierFunction);
        }

        #endregion
    }
}