using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace RSA_Project
{
    public class RsaCryptography
    {
        private static readonly Random Rnd = new Random((int) DateTime.Now.Ticks);
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        private readonly int[] _fermatNumbers = {17, 257, 65537}; //Числа ферма
        private BigInteger _n; //P,Q -простые числа. N - их произведение
        private BigInteger _p; //P,Q -простые числа. N - их произведение
        private BigInteger _privateKey; //открытый и закрытый ключи
        private BigInteger _publicKey; //открытый и закрытый ключи
        private BigInteger _q; //P,Q -простые числа. N - их произведение

        public RsaCryptography(BigInteger p, BigInteger q)
        {
//Инициализация класса
            _p = p;
            _q = q;
            _n = _p*_q;
            BigInteger eulierFunction = (_p - 1)*(_q - 1);
            _publicKey = _fermatNumbers[Rnd.Next(0, _fermatNumbers.Length)];
            _privateKey = BuildPrivateKey(eulierFunction);
        }

        public RsaCryptography(int log10N)
        {
            GenerateKeys(log10N);
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

        public List<byte[]> EncryptMessage(List<byte[]> p)
        {
            //Зашифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(Encrypt(block));
            }
            return result;
        }

        public List<byte[]> DecryptMessage(List<byte[]> p)
        {
            //Разшифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(Decrypt(block));
            }
            return result;
        }

        public byte[] Encrypt(byte[] message)
        {
//Шифруем сообщение открытым ключом
            var val = new BigInteger(message);
            val = BigInteger.ModPow(val, _publicKey, _n);
            return val.ToByteArray();
        }


        public byte[] Decrypt(byte[] message)
        {
//Расшифруем cообщение открытм ключом
            var val = new BigInteger(message);
            val = BigInteger.ModPow(val, _privateKey, _n);
            return val.ToByteArray();
        }

        private BigInteger BuildPrivateKey(BigInteger E)
        {
//Вычисляем закрытый ключ, находя обратный по модулю элемент кольца
            BigInteger x, y;
            BigInteger g = GCD(_publicKey, E, out x, out y);
            return (x%E + E)%E;
        }


        private static BigInteger GCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
//Расширеный Алгоритм Евклида
            if (a.IsZero) //Находит НОД чисел A и B, и коэфициенты x,y уравнения Ax +By = НОД(A,B)
            {
                x = 0;
                y = 1;
                return b;
            }
            BigInteger x1, y1;
            BigInteger d = GCD(b%a, a, out x1, out y1);
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


        public BigInteger PrivateKey
        {
//значение закрытого ключа
            get { return _privateKey; }
        }

        public BigInteger PublicKey
        {
//значение откртыго ключа
            get { return _publicKey; }
        }

        #endregion

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
                // если простое то
                if (!(BigInteger.ModPow(a, y, x) - 1).IsZero) return false;
            }
            // признаём число простым
            // хотя можем продолжать ошибаться
            return true;
        }

        private BigInteger GeneratePrimary(int bytes)
        {
            BigInteger x = Random(bytes) | 1; // Простые являются нечётными
            while (!IsPrimary(x)) x += 2; // Движемся вперёд пока не встретим простое
            return x;
        }

        public void GenerateKeys(int log10N)
        {
            // 10^x==2^y 
            // x=y*log10(2)
            var bits = (int) Math.Ceiling(log10N/Math.Log10(2));
            int bits1 = bits/2;
            int bits2 = bits - bits1;
            int bytes1 = (bits1 + 7)/8;
            int bytes2 = (bits2 + 7)/8;
            _p = GeneratePrimary(bytes1);
            _q = GeneratePrimary(bytes2);
            _n = _p*_q;
            BigInteger eulierFunction = (_p - 1)*(_q - 1);
            _publicKey = _fermatNumbers[Rnd.Next(0, _fermatNumbers.Length)];
            _privateKey = BuildPrivateKey(eulierFunction);
        }

        #endregion
    }
}