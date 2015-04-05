using System;
using System.Text;

namespace Magic
{
    /// <summary>
    ///     Шифр на основе «магических» квадратов
    /// </summary>
    public class MagicCryptography : ICryptography
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
            if (_key < 0) throw new WrongKeyException();
            var n = (int) Math.Sqrt(plainText.Length);
            while (n*n < plainText.Length) n++;
            int[,] matrix;
            int[] index1;
            int[] index2;
            GetMatrix(out matrix, n, _key);
            GetIndeces(out index1, out index2, matrix);
            int total = matrix.GetLength(0)*matrix.GetLength(1);
            var table = new char[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < total; i++)
                table[index1[i], index2[i]] = (i < plainText.Length) ? plainText[i] : LastChar;
            var sb = new StringBuilder();
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
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
            if (_key < 0) throw new WrongKeyException();
            var n = (int) Math.Sqrt(cipherText.Length);
            while (n*n < cipherText.Length) n++;
            int[,] matrix;
            int[] index1;
            int[] index2;
            GetMatrix(out matrix, n, _key);
            GetIndeces(out index1, out index2, matrix);
            int total = matrix.GetLength(0)*matrix.GetLength(1);
            var table = new char[matrix.GetLength(0), matrix.GetLength(1)];
            int index = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    table[i, j] = (index < cipherText.Length) ? cipherText[index++] : LastChar;
            var sb = new StringBuilder();
            for (int i = 0; i < total; i++) sb.Append(table[index1[i], index2[i]]);

            return sb.ToString().TrimEnd(new[] {LastChar});
        }

        /// <summary>
        ///     Ввод ключа и сброс счётчика тактов
        /// </summary>
        /// <param name="keyText">ширина таблицы</param>
        public void SetKey(string keyText)
        {
            int key = Convert.ToInt32(keyText);
            if (key < 0) throw new WrongKeyException();
            _key = key;
        }

        /// <summary>
        ///     Сброс ключа и сброс счётчика тактов
        /// </summary>
        public void ClearKey()
        {
            _key = 0;
        }

        private void GetIndeces(out int[] index1, out int[] index2, int[,] matrix)
        {
            int total = matrix.GetLength(0)*matrix.GetLength(1);
            var array1 = new int[total];
            var array2 = new int[total];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    array1[matrix[i, j]] = i;
                    array2[matrix[i, j]] = j;
                }
            index1 = array1;
            index2 = array2;
        }

        private void FixedMagic(out int[,] matrix, int n)
        {
            if (n%2 == 0) n++;
            var array = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    array[i, j] = ((i + n - j + (n - 1)/2)%n)*n + ((i + j + (n + 1)/2)%n);
            matrix = array;
        }

        private void FlipHorz(ref int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j1 = 0, j2 = matrix.GetLength(1) - 1; j1 < j2; j1++, j2--)
                {
                    int t = matrix[i, j1];
                    matrix[i, j1] = matrix[i, j2];
                    matrix[i, j2] = t;
                }
        }

        private void FlipVert(ref int[,] matrix)
        {
            for (int i1 = 0, i2 = matrix.GetLength(0) - 1; i1 < i2; i1++, i2--)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    int t = matrix[i1, j];
                    matrix[i1, j] = matrix[i2, j];
                    matrix[i2, j] = t;
                }
        }

        private void GetMatrix(out int[,] matrix, int n, int key)
        {
            FixedMagic(out matrix, n);
            switch (key%4)
            {
                case 1:
                    FlipHorz(ref matrix);
                    break;
                case 2:
                    FlipVert(ref matrix);
                    break;
                case 3:
                    FlipHorz(ref matrix);
                    FlipVert(ref matrix);
                    break;
            }
        }

        public class WrongKeyException : Exception
        {
        }
    }
}