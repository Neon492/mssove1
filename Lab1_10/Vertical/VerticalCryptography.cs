using System;
using System.Text;

namespace Vertical
{
    /// <summary>
    ///     Шифр вертикальной перестановки
    /// </summary>
    public class VerticalCryptography : ICryptography
    {
        private int[] _key;
        private const char LastChar = '>';
        /// <summary>
        ///     Процедура шифрования блока текста
        /// </summary>
        /// <param name="plainText">Исходный текст</param>
        /// <returns>Шифрованный текст</returns>
        public string EncryptNext(string plainText)
        {
            if (_key == null) throw new WrongKeyException();
            int cols = _key.Length;
            int rows = (plainText.Length + cols - 1)/cols;
            var table = new char[rows, cols];
            int index = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    table[i, j] = (index < plainText.Length) ? plainText[index++] : LastChar;
            var sb = new StringBuilder();
            for (int j = 0; j < cols; j++)
                for (int i = 0; i < rows; i++)
                    sb.Append(table[i, _key[j]]);
            return sb.ToString().TrimEnd(new[] { LastChar });
        }

        /// <summary>
        ///     Процедура расшифрования блока текста
        /// </summary>
        /// <param name="cipherText">Шифрованный текст</param>
        /// <returns>Исходный текст</returns>
        public string DecryptNext(string cipherText)
        {
            if (_key == null) throw new WrongKeyException();
            int cols = _key.Length;
            int rows = (cipherText.Length + cols - 1)/cols;
            var table = new char[rows, cols];
            int index = 0;
            for (int j = 0; j < cols; j++)
                for (int i = 0; i < rows; i++)
                    table[i, _key[j]] = (index < cipherText.Length) ? cipherText[index++] : LastChar;
            var sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    sb.Append(table[i, j]);
            return sb.ToString().TrimEnd(new[] { LastChar });
        }

        /// <summary>
        ///     Ввод ключа и сброс счётчика тактов
        /// </summary>
        /// <param name="keyText">Ключевое слово</param>
        public void SetKey(string keyText)
        {
            string[] keys = keyText.Split();
            int count = keys.Length;
            if (count < 2) throw new WrongKeyException();
            var key = new int[count];
            var flags = new int[count];
            for (int i = 0; i < count; i++) flags[i] = 0;
            for (int i = 0; i < count; i++)
                if ((key[i] = Convert.ToInt32(keys[i]) - 1) < 0 || key[i] >= count)
                    throw new WrongKeyException();
                else flags[key[i]] = 1;
            int count1 = 0;
            for (int i = 0; i < count; i++) count1 += flags[i];
            if (count != count1) throw new WrongKeyException();
            _key = key;
        }

        /// <summary>
        ///     Сброс ключа и сброс счётчика тактов
        /// </summary>
        public void ClearKey()
        {
            _key = null;
        }

        public class WrongKeyException : Exception
        {
        }
    }
}