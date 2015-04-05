using System;
using System.Text;

namespace Trithemius
{
    /// <summary>
    ///     Шифр Трисемуса
    /// </summary>
    public class TrithemiusCryptography
    {
        /// <summary>
        ///     Алфавит
        /// </summary>
        private const string Alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
                                        + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
                                        + " ;:,.!?()+-*/\\\"\'";

        private const int P = 3571; // Большое простое число (3571 имеет #500 в списке простых чисел)

        private int _additionalKey; // Значение дополнительного ключа

        private int _n; // число колонок
        private string _tableText = string.Empty; // заполение таблицы символами алфавита

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
            if (string.IsNullOrWhiteSpace(_tableText)) throw new EmptyKeyException();
            if (_n == 0) throw new EmptyKeyException();

            var sb = new StringBuilder();
            for (int i = 0; i < plainText.Length;)
            {
                int row;
                int col;

                FindCoord(plainText[i++], out row, out col);
                ForwardTransform(ref row, ref col);

                sb.Append(_tableText[row*_n + col]);

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
            if (string.IsNullOrWhiteSpace(_tableText)) throw new EmptyKeyException();
            if (_n == 0) throw new EmptyKeyException();

            var sb = new StringBuilder();
            for (int i = 0; i < cipherText.Length;)
            {
                int row;
                int col;

                FindCoord(cipherText[i++], out row, out col);
                ReverseTransform(ref row, ref col);

                sb.Append(_tableText[row*_n + col]);

                _i++;
                _z = (3*_z)%P;
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Ввод ключа и сброс счётчика тактов
        /// </summary>
        /// <param name="keyText">Ключевое слово</param>
        /// <param name="n">Количество колонок</param>
        public void SetKey(string keyText, int n)
        {
            if (keyText.Length < 3) throw new EmptyKeyException();
            string tableText = keyText + Alphabet;
            // Удаляем повторы символов в строке keyText + Alphabet
            for (int i = 0, j; i < tableText.Length; i++)
                while ((j = tableText.LastIndexOf(tableText[i])) > i)
                    tableText = tableText.Remove(j, 1);


            _tableText = tableText;
            _n = n;
            _i = 0;
        }

        /// <summary>
        ///     Сброс ключа и сброс счётчика тактов
        /// </summary>
        public void ClearKey()
        {
            _tableText = string.Empty;
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
        private void FindCoord(char ch, out int row, out int col)
        {
            int index = _tableText.IndexOf(ch);
            if (index < 0) throw new WrongCharException {Character = ch.ToString()};
            row = index/_n;
            col = index%_n;
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
            int n2 = _tableText.Length /_n;
            if (n2*_n + col < _tableText.Length) n2++;
            int dy = _z%n2; // модификация определяемая дополнительным ключём
            row = (row + 1 + dy)%n2;
            col = col;
        }

        private void ReverseTransform(ref int row, ref int col)
        {
            int n2 = _tableText.Length / _n;
            if (n2 * _n + col < _tableText.Length) n2++;
            int dy = _z % n2; // модификация определяемая дополнительным ключём
            row = (row + n2 - 1 + n2 - dy)%n2;
            col = col;
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