using System;
using System.Text;

namespace Vigenere
{
    /// <summary>
    ///     Шифр Виженера
    /// </summary>
    public class VigenereCryptography : IStreamCryptography
    {
        /// <summary>
        ///     Алфавит
        /// </summary>
        private const string Alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
                                        + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
                                        + " ;:,.!?()+-*/\\\"\'";

        private int _i; // Текуший номер такта шифрования
        private string _keyText = string.Empty; // Ключевое слово

        /// <summary>
        ///     Сброс счётчика тактов
        /// </summary>
        public void Restart()
        {
            _i = 0;
        }

        /// <summary>
        ///     Процедура шифрования блока текста
        ///     Увеличение счётчика тактов на длину блока текста
        /// </summary>
        /// <param name="plainText">Исходный текст</param>
        /// <returns>Шифрованный текст</returns>
        public string EncryptNext(string plainText)
        {
            if (string.IsNullOrWhiteSpace(_keyText)) throw new EmptyKeyException();
            var sb = new StringBuilder();
            for (int i = 0; i < plainText.Length;)
            {
                int index1 = Alphabet.IndexOf(plainText[i++]);
                int index2 = Alphabet.IndexOf(_keyText[_i++%_keyText.Length]);
                if (index1 == -1) 
                    throw new WrongCharException {Character = plainText[i - 1]};
                if (index2 == -1)
                    throw new WrongCharException {Character = _keyText[(_i + _keyText.Length - 1)%_keyText.Length]};
                sb.Append(Alphabet[(index1 + index2)%Alphabet.Length]);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Процедура расшифрования блока текста
        ///     Увеличение счётчика тактов на длину блока текста
        /// </summary>
        /// <param name="cipherText">Шифрованный текст</param>
        /// <returns>Исходный текст</returns>
        public string DecryptNext(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(_keyText)) throw new EmptyKeyException();
            var sb = new StringBuilder();
            for (int i = 0; i < cipherText.Length;)
            {
                int index1 = Alphabet.IndexOf(cipherText[i++]);
                int index2 = Alphabet.IndexOf(_keyText[_i++%_keyText.Length]);
                if (index1 == -1)
                    throw new WrongCharException { Character = cipherText[i - 1] };
                if (index2 == -1)
                    throw new WrongCharException { Character = _keyText[(_i + _keyText.Length - 1) % _keyText.Length] };
                sb.Append(Alphabet[(index1 + Alphabet.Length - index2) % Alphabet.Length]);
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
            _keyText = keyText;
            _i = 0;
        }

        /// <summary>
        ///     Сброс ключа и сброс счётчика тактов
        /// </summary>
        public void ClearKey()
        {
            _keyText = string.Empty;
            _i = 0;
        }

        public class EmptyKeyException : Exception
        {
        }

        public class WrongCharException : Exception
        {
            public char Character { get; set; }
        }
    }
}