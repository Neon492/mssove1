using System;
using System.Text;

namespace Atbash
{
    /// <summary>
    ///     Шифр Атбаш
    /// </summary>
    public class AtbashCryptography
    {
        /// <summary>
        ///     Алфавит
        /// </summary>
        private const string Alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
                                        + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
                                        + " ;:,.!?()+-*/\\\"\'";

        private const int P = 3571; // Большое простое число (3571 имеет #500 в списке простых чисел)


        private int _additionalKey; // Значение дополнительного ключа
        private string _key = string.Empty; // Значение ключа

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
            if (string.IsNullOrEmpty(_key)) throw new EmptyKeyException();
            int m = _key.Length;
            var sb = new StringBuilder();
            foreach (char t in plainText)
            {
                int index = _key.IndexOf(t);
                if (index == -1) throw new WrongCharException {Character = t.ToString()};
                sb.Append(_key[(m - index - 1 + _z)%m]);
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
            if (string.IsNullOrEmpty(_key)) throw new EmptyKeyException();
            int m = _key.Length;
            var sb = new StringBuilder();
            foreach (char t in cipherText)
            {
                int index = _key.IndexOf(t);
                if (index == -1) throw new WrongCharException {Character = t.ToString()};
                sb.Append(_key[(m - index - 1 + _z)%m]);
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
            string text = keyText + Alphabet;
            // Удаляем повторы символов в строке keyText + Alphabet
            for (int i = 0, j; i < text.Length; i++)
                while ((j = text.LastIndexOf(text[i])) > i)
                    text = text.Remove(j, 1);
            _key = text;
            _i = 0;
        }

        /// <summary>
        ///     Сброс ключа и сброс счётчика тактов
        /// </summary>
        public void ClearKey()
        {
            _key = string.Empty;
            _additionalKey = 0;
            _i = 0;
            _z = 0;
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

        public class EmptyKeyException : Exception
        {
        }

        public class WrongCharException : Exception
        {
            public string Character { get; set; }
        }
    }
}