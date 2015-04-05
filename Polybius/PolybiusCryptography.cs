using System;
using System.Text;

namespace Polybius
{
    /// <summary>
    ///     Квадрат Полибия
    /// </summary>
    public class PolybiusCryptography : IStreamCryptography
    {
        /// <summary>
        ///     Алфавит
        /// </summary>
        private const string Alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
                                        + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
                                        + " ;:,.!?()+-*/\\\"\'";

        private const int P = 3571; // Большое простое число (3571 имеет #500 в списке простых чисел)

        // матрица трансформации координат
        private static readonly int[][] DefaultMatrix =
        {
            new[] {0, 1, 0},
            new[] {1, 0, 0}
        };

        #region

        private readonly int[][] _forwardMatrix = DefaultMatrix; // матрица трансформации координат
        private readonly int[][] _reverseMatrix = DefaultMatrix; // матрица трансформации координат

        #endregion

        private int _additionalKey; // Значение дополнительного ключа

        private int _n; // Размер квадрата
        private string[,] _square; // заполение квадрата символами алфавита

        #region

        private int _i; // Текущий номер такта шифрования
        private int _z; // Значение дополнительного регистра

        #endregion

        /// <summary>
        ///     Сброс счётчика тактов
        /// </summary>
        public void Restart()
        {
            _i = 0;
            _z = _additionalKey;
        }

        /// <summary>
        ///     Процедура шифрования блока текста
        ///     Увеличение счётчика тактов на длину блока текста
        ///     Пересчёт значения дополнительного регистра на каждом такте
        /// </summary>
        /// <param name="plainText">Исходный текст</param>
        /// <returns>Шифрованный текст</returns>
        public string EncryptNext(string plainText)
        {
            if (_square == null) throw new EmptyKeyException();
            if (_n == 0) throw new EmptyKeyException();

            var chars = new string[plainText.Length];
            int count = 0;
            for (int i = 0; i < plainText.Length; i++)
                chars[count++] = (plainText[i] == '\\') ? "\\\\" : plainText[i].ToString();

            var sb = new StringBuilder();
            for (int i = 0; i < count;)
            {
                int row;
                int col;

                FindCoord(chars[i++], out row, out col);
                ForwardTransform(ref row, ref col);

                sb.Append(_square[row, col]);

                _i++;
                _z = (3*_z)%P;
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Процедура расшифрования блока текста
        ///     Увеличение счётчика тактов на длину блока текста
        ///     Пересчёт значения дополнительного регистра на каждом такте
        /// </summary>
        /// <param name="cipherText">Шифрованный текст</param>
        /// <returns>Исходный текст</returns>
        public string DecryptNext(string cipherText)
        {
            if (_square == null) throw new EmptyKeyException();
            if (_n == 0) throw new EmptyKeyException();

            var chars = new string[cipherText.Length];

            int count = 0;
            for (int i = 0; i < cipherText.Length;)
                if (cipherText[i] == '\\')
                    if (cipherText[i + 1] == '\\' && i + 2 < cipherText.Length)
                    {
                        chars[count++] = cipherText.Substring(i, 2);
                        i += 2;
                    }
                    else if (i + 6 < cipherText.Length)
                    {
                        chars[count++] = cipherText.Substring(i, 6);
                        i += 6;
                    }
                    else throw new WrongCharException {Character = cipherText.Substring(i)};
                else
                    chars[count++] = cipherText[i++].ToString();

            var sb = new StringBuilder();
            for (int i = 0; i < count;)
            {
                int row;
                int col;

                FindCoord(chars[i++], out row, out col);
                ReverseTransform(ref row, ref col);

                sb.Append(_square[row, col]);

                _i++;
                _z = (3*_z)%P;
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Ввод ключа и сброс счётчика тактов
        /// </summary>
        /// <param name="keyText">Ключевое слово</param>
        public void SetKey(string keyText)
        {
            if (keyText.Length < 3) throw new EmptyKeyException();
            string tableText = keyText + Alphabet;
            // Удаляем повторы символов в строке keyText + Alphabet
            for (int i = 0, j; i < tableText.Length; i++)
                while ((j = tableText.LastIndexOf(tableText[i])) > i)
                    tableText = tableText.Remove(j, 1);

            // Вычисляем размер квадрата
            var n = (int) Math.Sqrt(tableText.Length);
            while (n*n < tableText.Length) n++;

            _square = new string[n, n];

            for (int i = 0; i < tableText.Length; i++)
            {
                int row = i / n;
                int col = i % n;
                _square[row, col] = (tableText[i] == '\\') ? "\\\\" : tableText[i].ToString();
            }

            // Заполняем оставшиеся ячейки спецсимволами
            for (int i = tableText.Length; i < n*n; i++)
            {
                int row = i/n;
                int col = i%n;
                _square[row, col] = string.Format("\\{0:D5}", i);
            }

            _n = n;
            _i = 0;
        }

        /// <summary>
        ///     Сброс ключа и сброс счётчика тактов
        /// </summary>
        public void ClearKey()
        {
            _square = null;
            _additionalKey = 0;
            _n = 0;
            _i = 0;
            _z = 0;
        }

        /// <summary>
        ///     Нахождение номера строки и столбца в квадрате Полибия
        ///     (включая поиск дополнительных спецсимволов)
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void FindCoord(string ch, out int row, out int col)
        {
            for (int i = 0; i < _n; i++)
                for (int j = 0; j < _n; j++)
                    if (string.Compare(_square[i, j], ch) == 0)
                    {
                        row = i;
                        col = j;
                        return;
                    }
            throw new WrongCharException {Character = ch};
        }

        /// <summary>
        ///     Ввод дополнительного ключа и сброс счётчика тактов
        /// </summary>
        /// <param name="additionalKey">дополнительный ключ</param>
        public void SetAdditionalKey(int additionalKey)
        {
            _z = _additionalKey = additionalKey;
            _i = 0;
        }

        private void ForwardTransform(ref int row, ref int col)
        {
            int dx = (_z/_n)%_n; // модификация определяемая дополнительным ключём
            int dy = _z%_n; // модификация определяемая дополнительным ключём
            int i = (_forwardMatrix[0][0]*row + _forwardMatrix[0][1]*col + _forwardMatrix[0][2] + dx)%_n;
            int j = (_forwardMatrix[1][0]*row + _forwardMatrix[1][1]*col + _forwardMatrix[1][2] + dy)%_n;
            row = i;
            col = j;
        }

        private void ReverseTransform(ref int row, ref int col)
        {
            int dx = (_z/_n)%_n; // модификация определяемая дополнительным ключём
            int dy = _z%_n; // модификация определяемая дополнительным ключём
            int i = (_reverseMatrix[0][0]*(row + _n - dx) + _reverseMatrix[0][1]*(col + _n - dy) + _reverseMatrix[0][2])%
                    _n;
            int j = (_reverseMatrix[1][0]*(row + _n - dx) + _reverseMatrix[1][1]*(col + _n - dy) + _reverseMatrix[1][2])%
                    _n;
            row = i;
            col = j;
        }

        public class EmptyKeyException : Exception
        {
        }

        public class WrongCharException : Exception
        {
            public string Character { get; set; }
        }
    }
}