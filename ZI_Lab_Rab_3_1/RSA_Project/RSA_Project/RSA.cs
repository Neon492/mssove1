using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;
using System.Diagnostics;

namespace RSA_Project
{
    public class RSACryptography
    {
        private int[] fermatNumbers = new int[] { 17, 257, 65537 };//Числа ферма
        private BigInteger  N, P, Q;//P,Q -простые числа. N - их произведение
        private BigInteger public_key, private_key;//откртый и закрытый ключи

        #region  Свойства
        public BigInteger Module
        {//Возвращает значения модуля(N)
            get
            {
                return N;
            }
        }


        public BigInteger PrivateKey
        {//значение закрытого ключа
            get
            {
                return private_key;
            }
        }

        public BigInteger PublicKey
        {//значение откртыго ключа
            get
            {
                return public_key;
            }
        }
        #endregion
        public RSACryptography(BigInteger _P,BigInteger _Q)
        {//Инициализация класса
            this.P = _P;
            this.Q = _Q;
            this.N = P * Q;
            BigInteger eulierFunction = (P - 1) * (Q - 1);
            this.public_key = fermatNumbers[new Random().Next(0,fermatNumbers.Length)];
            this.private_key = calcPrivateKey(eulierFunction);
        }

        public byte[] Encrypt(byte[] message)
        {//Шифруем сообщение открытым ключом
            BigInteger val = new BigInteger(message);
            val = BigInteger.ModPow(val, public_key, N);
            return val.ToByteArray();
        }

        public long ModuleLength
        {//Длина модуля в байтах
            get
            {
                long l = N.ToByteArray().Length;
                return l; 
            }
        }


        public byte[] Decrypt(byte[] message)
        {//Расшифруем cообщение открытм ключом
            BigInteger val = new BigInteger(message);
            val = BigInteger.ModPow(val, private_key, N);
            return val.ToByteArray();
        }

        private BigInteger calcPrivateKey(BigInteger E)
        {//Вычисляем закрытый ключ, находя обратный по модулю элемент кольца
            BigInteger x,y;
            BigInteger g = GCD(public_key,E ,out x,out  y);
            return (x % E + E) % E;
        }


        
        private BigInteger GCD(BigInteger a, BigInteger b,out BigInteger x,out BigInteger y)
        {//Расширеный Алгоритм Евклида
            if (a.IsZero)//Находит НОД чисел A и B, и коэфициенты x,y уравнения Ax +By = НОД(A,B)
            {
                x = 0;
                y = 1;
                return b;
            }
            BigInteger x1, y1;
            BigInteger d = GCD(b % a, a, out x1, out y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }
    }
}
