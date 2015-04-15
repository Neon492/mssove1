using System;
using System.Collections.Generic;
using System.Text;

namespace RSA_Project
{
    //Класс работающий с кодировками и разбиением текста
    public static class PartitionManager
    {
        public static String PartitionToString(List<byte[]> p)
        {
//Переобразует разбиение в строку
            String result = "";
            foreach (var b in p)
            {
                result += GetString(b);
            }
            return result;
        }

        public static List<byte[]> StringToPartition(String s, long length)
        {
/*Разбиваем строку на блоки
          * s - входная строка, length - длина блока в байтах
            */
            var result = new List<byte[]>();
            byte[] bytes = GetBytes(s);
            var block = new List<byte>();
            int count = 0;
            foreach (byte x in bytes)
            {
                count++;
                if (count < length)
                {
                    block.Add(x);
                }
                else
                {
                    result.Add(block.ToArray());
                    block.Clear();
                    block.Add(x);
                    count = 1;
                }
            }
            if (block.Count > 0) result.Add(block.ToArray());
            return result;
        }


        public static byte[] GetBytes(string str)
        {
/*
          * Препобразует строку кодировки windows-1251 в байт-массив 
          * 
            */
            return Encoding.Convert(Encoding.Unicode,
                Encoding.GetEncoding("windows-1251"),
                Encoding.Unicode.GetBytes(str));
        }

        public static string GetString(byte[] bytes)
        {
/*
          * Преобразует байт-массив в строку в кодировке windows-1251
          * */
            byte[] unicodeBytes = Encoding.Convert(
                Encoding.GetEncoding("windows-1251"), Encoding.Unicode, bytes);
            return Encoding.Unicode.GetString(unicodeBytes);
        }
    }
}