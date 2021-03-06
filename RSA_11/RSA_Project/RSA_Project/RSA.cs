﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace RSA_Project
{
    public class RSA
    {
        private static readonly Random Rnd = new Random((int) DateTime.Now.Ticks);
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();
        private readonly BigInteger P; //P,Q -простые числа. N - их произведение
        private readonly BigInteger Q; //P,Q -простые числа. N - их произведение
        private readonly int[] _fermatNumbers = {17, 257, 65537}; //Числа ферма
        private readonly byte _lengthN; //длина числа n
        private readonly BigInteger private_key; //откртый и закрытый ключи
        private readonly BigInteger public_key; //откртый и закрытый ключи
        private BigInteger N; //P,Q -простые числа. N - их произведение

        public RSA(byte lengthN)
        {
//Инициализация класса
            _lengthN = lengthN;
            KeyValuePair<BigInteger, BigInteger> pair = GeneratePrimesPair(lengthN/2 - lengthN/10);
            //Генерирует пару простых чисел
            P = pair.Key;
            Q = pair.Value;
            N = P*Q;
            BigInteger eulierFunction = (P - 1)*(Q - 1);
            public_key = _fermatNumbers[Rnd.Next(0, _fermatNumbers.Length)];
            private_key = CalcPrivateKey(eulierFunction);
        }

        public RSA(BigInteger P, BigInteger Q)
        {
            this.P = P;
            this.Q = Q;
            N = P*Q;
            BigInteger eulierFunction = (P - 1)*(Q - 1);
            public_key = 11;
            private_key = CalcPrivateKey(eulierFunction);
        }

        public BigInteger Module
        {
            get { return N; }
        }

        public KeyValuePair<BigInteger, BigInteger> PrimePair
        {
            get { return new KeyValuePair<BigInteger, BigInteger>(P, Q); }
        }

        public BigInteger PrivateKey
        {
            get { return private_key; }
        }

        public BigInteger PublicKey
        {
            get { return public_key; }
        }

        public long ModuleLength
        {
//Длина модуля в байтах
            get
            {
                long l = N.ToByteArray().Length;
                return l;
            }
        }

        public byte[] Encrypt(byte[] message)
        {
//Шифруем сообщение открытым ключом
            var val = new BigInteger(message);
            val = BigInteger.ModPow(val, public_key, N);
            return val.ToByteArray();
        }


        public byte[] Decrypt(byte[] message)
        {
//Расшифруем cообщение открытм ключом
            var val = new BigInteger(message);
            val = BigInteger.ModPow(val, private_key, N);
            return val.ToByteArray();
        }

        private BigInteger CalcPrivateKey(BigInteger E)
        {
//Вычисляем закрытый ключ, находя обратный по модулю элемент кольца
            BigInteger x, y;
            BigInteger g = GCD(public_key, E, out x, out y);
            return (x%E + E)%E;
        }


        private BigInteger GCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
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

        public static int NumberOfTests(BigInteger x)
        {
            return 2 * x.ToByteArray().Length + 100; // Чем больше тестов тем меньше вероятность ошибиться
        }

        public KeyValuePair<BigInteger, BigInteger> GeneratePrimesPair(int length)
        {
//Генерирует пару простых чисел, таких что их произведение имеет ровно lengthN знаков
            var resList = new List<BigInteger>();
            BigInteger p = BigInteger.One;
            int curLength = 1;
            while (curLength < length)
            {
                curLength++;
                p *= 10;
            }
            BigInteger lowLimit = p;
            BigInteger upLimit = p*10;
            //Генерируем число в заданном диапазоне
            var bytes = new byte[p.ToByteArray().LongLength];
            while (p <= lowLimit || p > upLimit)
            {
                Rng.GetBytes(bytes);
                p = new BigInteger(bytes);
            }
            //Ищем ближайшее простое
            while (!IsProbablePrime(p, NumberOfTests(p)))
            {
                p += 1;
            }
            // Генерируем число в диапазоне 10^lengthN - 10^(lengthN+1)
            while (curLength < _lengthN)
            {
                curLength++;
                lowLimit *= 10;
            }
            upLimit = lowLimit*10;
            bytes = new byte[lowLimit.ToByteArray().LongLength];
            BigInteger q;
            do
            {
                Rng.GetBytes(bytes);
                q = new BigInteger(bytes);
            } while (q <= lowLimit || q >= upLimit);
            //делим с остатком на найденное  просто число
            q = q/p;
            while (!IsProbablePrime(q, NumberOfTests(q)))
            {
                q += 1;
            }
            //ищем ближайшее простое к нему
            return new KeyValuePair<BigInteger, BigInteger>(p, q); //Возвращаем пару найденных чисел
        }

        public static bool IsProbablePrime(BigInteger source, int certainty)
        {
//Вероятностный тест Миллера-Рабина для определения  простоты числа
            if (source == 2 || source == 3)
                return true;
            if (source < 2 || source%2 == 0)
                return false;

            BigInteger d = source - 1;
            int s = 0;

            while (d%2 == 0)
            {
                d /= 2;
                s += 1;
            }

            var bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;

            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    Rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                } while (a < 2 || a >= source - 2);

                BigInteger x = BigInteger.ModPow(a, d, source);
                if (x == 1 || x == source - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                        return false;
                    if (x == source - 1)
                        break;
                }

                if (x != source - 1)
                    return false;
            }

            return true;
        }
    }
}