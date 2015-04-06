using System;
using System.Text;

namespace Route
{
    /// <summary>
    ///     Шифр маршрутной перестановки
    /// </summary>
    public class RouteCryptography : ICryptography
    {
        private const char LastChar = '>';
        private int _key;

        /// <summary>
        ///     Процедура шифрования блока текста
        /// </summary>
        /// <param name="plainText">Исходный текст</param>
        /// <returns>Шифрованный текст</returns>
        public string EncryptNext(string plainText)
        {
            if (_key == 0) throw new WrongKeyException();
            int cols = _key;
            int rows = (plainText.Length + cols - 1)/cols;
            var table = new char[rows, cols];
            int index = 0;
            for (int i = 0; i < rows; i++)
                if (i%2 == 0)
                    for (int j = 0; j < cols; j++)
                        table[i, j] = (index < plainText.Length) ? plainText[index++] : LastChar;
                else
                    for (int j = cols; j-- > 0;)
                        table[i, j] = (index < plainText.Length) ? plainText[index++] : LastChar;
            var sb = new StringBuilder();
            for (int j = 0; j < cols; j++)
                if (j%2 == 0)
                    for (int i = 0; i < rows; i++)
                        sb.Append(table[i, j]);
                else
                    for (int i = rows; i-- > 0;)
                        sb.Append(table[i, j]);
            return sb.ToString().TrimEnd(new[] {LastChar});
        }

        /// <summary>
        ///     Процедура расшифрования блока текста
        /// </summary>
        /// <param name="cipherText">Шифрованный текст</param>
        /// <returns>Исходный текст</returns>
        public string DecryptNext(string cipherText)
        {
            if (_key == 0) throw new WrongKeyException();
            int cols = _key;
            int rows = (cipherText.Length + cols - 1)/cols;
            var table = new char[rows, cols];
            int index = 0;
            for (int j = 0; j < cols; j++)
                if (j%2 == 0)
                    for (int i = 0; i < rows; i++)
                        table[i, j] = (index < cipherText.Length) ? cipherText[index++] : LastChar;
                else
                    for (int i = rows; i-- > 0;)
                        table[i, j] = (index < cipherText.Length) ? cipherText[index++] : LastChar;
            var sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
                if (i%2 == 0)
                    for (int j = 0; j < cols; j++)
                        sb.Append(table[i, j]);
                else
                    for (int j = cols; j-- > 0;)
                        sb.Append(table[i, j]);
            return sb.ToString().TrimEnd(new[] {LastChar});
        }

        /// <summary>
        ///     Ввод ключа и сброс счётчика тактов
        /// </summary>
        /// <param name="keyText">ширина таблицы</param>
        public void SetKey(string keyText)
        {
            int key = Convert.ToInt32(keyText);
            if (key < 2) throw new WrongKeyException();
            _key = key;
        }

        /// <summary>
        ///     Сброс ключа и сброс счётчика тактов
        /// </summary>
        public void ClearKey()
        {
            _key = 0;
        }

        public class WrongKeyException : Exception
        {
        }
    }
}