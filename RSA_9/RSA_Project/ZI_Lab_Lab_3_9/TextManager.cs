using System;
using System.Collections.Generic;
using System.Text;

namespace RSA_Project
{
    public static class TextManager
    {
        //Класс работающий с кодировками и разбиением текста
        public static String PartitionToString(List<byte[]> p)
        {
//Переобразует разбиение в строку
            String result = String.Empty;
            foreach (var block in p)
            {
                result += GetString(block);
            }
            return result;
        }

        public static List<byte[]> PartitionOfString(String s, long length)
        {
//Разбиваем строку на блоки(байт-массивы)
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
            byte[] utf8bytes = Encoding.Unicode.GetBytes(str);
            byte[] win1251Bytes = Encoding.Convert(
                Encoding.Unicode, Encoding.GetEncoding("windows-1251"), utf8bytes);
            //byte[] bytes = Encoding.ASCII.GetBytes(str);
            return win1251Bytes;
        }

        public static string GetString(byte[] bytes)
        {
            byte[] unicodeBytes = Encoding.Convert(
                Encoding.GetEncoding("windows-1251"), Encoding.Unicode, bytes);
            return Encoding.Unicode.GetString(unicodeBytes);
        }
    }
}