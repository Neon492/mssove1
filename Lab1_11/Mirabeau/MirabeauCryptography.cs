using System;
using System.Text;

namespace Mirabeau
{
    /// <summary>
    ///     Шифр Мирабо
    /// </summary>
    public class MirabeauCryptography : IStreamCryptography
    {
        /// <summary>
        ///     Алфавит
        /// </summary>
        private const string Alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
                                        + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
                                        + " ;:,.!?()+-*/\\\"\'";

        private const int P = 3571; // Большое простое число (3571 имеет #500 в списке простых чисел)
        private const int N = 5; // Количество групп

        // матрица трансформации координат
        private static readonly int[][] DefaultMatrix =
        {
            new[] {1, 0, 0},
            new[] {0, 1, 0}
        };

        #region

        private readonly int[][] _forwardMatrix = DefaultMatrix; // матрица трансформации координат
        private readonly int[][] _reverseMatrix = DefaultMatrix; // матрица трансформации координат

        #endregion

        private int _additionalKey; // Значение дополнительного ключа

        private string[] _table; // заполение таблицы символами алфавита

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
            if (_table == null) throw new EmptyKeyException();
            var sb = new string[plainText.Length];
            for (int i = 0; i < plainText.Length;)
            {
                int row;
                int col;

                FindCoord(plainText[i], out row, out col);
                ForwardTransform(ref row, ref col);

                sb[i++] = string.Format("{0}/{1}", row + 1, col + 1);

                _i++;
                _z = (3*_z)%P;
            }
            return string.Join("+", sb);
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
            if (_table == null) throw new EmptyKeyException();
            string[] chars = cipherText.Split('+');
            var sb = new StringBuilder();
            for (int i = 0; i < chars.Length;)
            {
                string[] pair = chars[i++].Split('/');
                if (pair.Length != 2) throw new WrongCharException {Character = string.Join("/", pair)};
                int row = Convert.ToInt32(pair[0]) - 1;
                int col = Convert.ToInt32(pair[1]) - 1;

                ReverseTransform(ref row, ref col);

                if (row < 0 || row >= _table.Length) throw new WrongCharException {Character = string.Join("/", pair)};
                if (col < 0 || col >= _table[row].Length)
                    throw new WrongCharException {Character = string.Join("/", pair)};

                sb.Append(_table[row][col]);

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

            _table = new string[N];

            for (int i = 0; i < N; i++)
            {
                int start = i*tableText.Length/N;
                int next = (i + 1)*tableText.Length/N;
                _table[i] = tableText.Substring(start, next - start);
            }

            _i = 0;
        }

        /// <summary>
        ///     Сброс ключа и сброс счётчика тактов
        /// </summary>
        public void ClearKey()
        {
            _table = null;
            _additionalKey = 0;
            _i = 0;
            _z = 0;
        }

        /// <summary>
        ///     Нахождение номера строки и столбца в таблице
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void FindCoord(char ch, out int row, out int col)
        {
            for (int i = 0, j; i < N; i++)
                if ((j = _table[i].IndexOf(ch)) >= 0)
                {
                    row = i;
                    col = j;
                    return;
                }
            throw new WrongCharException {Character = ch.ToString()};
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
            int dx = _z/N; // модификация определяемая дополнительным ключём
            int dy = _z%N; // модификация определяемая дополнительным ключём
            int i = (_forwardMatrix[0][0]*row + _forwardMatrix[0][1]*col + _forwardMatrix[0][2] + dx);
            int j = (_forwardMatrix[1][0]*row + _forwardMatrix[1][1]*col + _forwardMatrix[1][2] + dy);
            row = i;
            col = j;
        }

        private void ReverseTransform(ref int row, ref int col)
        {
            int dx = _z/N; // модификация определяемая дополнительным ключём
            int dy = _z%N; // модификация определяемая дополнительным ключём
            int i = (_reverseMatrix[0][0]*(row - dx) + _reverseMatrix[0][1]*(col - dy) + _reverseMatrix[0][2]);
            int j = (_reverseMatrix[1][0]*(row - dx) + _reverseMatrix[1][1]*(col - dy) + _reverseMatrix[1][2]);
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