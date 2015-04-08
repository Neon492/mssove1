namespace Vigenere
{
    /// <summary>
    ///     Тройной шифр Виженера
    ///     Шифратор из последовательно соединённых трёх шифраторов с шифром Виженера
    ///     Шифратор #1 работает в прямом направлении
    ///     Шифратор #2 работает в обратном направлении
    ///     Шифратор #3 работает в прямом направлении
    /// </summary>
    public class Vigenere3Cryptography : IStreamCryptography
    {
        private readonly VigenereCryptography _cryptography1 = new VigenereCryptography();
        private readonly VigenereCryptography _cryptography2 = new VigenereCryptography();
        private readonly VigenereCryptography _cryptography3 = new VigenereCryptography();

        public void SetKey(string keyText)
        {
            string keyText1;
            string keyText2;
            string keyText3;
            SplitKey(keyText, out keyText1, out keyText2, out keyText3);
            _cryptography1.SetKey(keyText1);
            _cryptography2.SetKey(keyText2);
            _cryptography3.SetKey(keyText3);
        }

        public void ClearKey()
        {
            _cryptography1.ClearKey();
            _cryptography2.ClearKey();
            _cryptography3.ClearKey();
        }

        public void Restart()
        {
            _cryptography1.Restart();
            _cryptography2.Restart();
            _cryptography3.Restart();
        }

        public string EncryptNext(string plainText)
        {
            return _cryptography3.EncryptNext(_cryptography2.DecryptNext(_cryptography1.EncryptNext(plainText)));
        }

        public string DecryptNext(string cipherText)
        {
            return _cryptography1.DecryptNext(_cryptography2.EncryptNext(_cryptography3.DecryptNext(cipherText)));
        }

        /// <summary>
        ///     Формирование из мастер-ключа ключей для последовательно соединёных шифраторов
        /// </summary>
        /// <param name="keyText">Мастер-ключ</param>
        /// <param name="keyText1">Ключ шифратора #1</param>
        /// <param name="keyText2">Ключ шифратора #2</param>
        /// <param name="keyText3">Ключ шифратора #3</param>
        private void SplitKey(string keyText, out string keyText1, out string keyText2, out string keyText3)
        {
            if (keyText.Length == 0) throw new VigenereCryptography.EmptyKeyException();
            if (keyText.Length < 3)
            {
                keyText1 = keyText;
                keyText2 = keyText;
                keyText3 = keyText;

            }
            else
            {
                keyText1 = keyText.Substring(0, keyText.Length - 2);
                keyText2 = keyText.Substring(0, keyText.Length - 1);
                keyText3 = keyText.Substring(0, keyText.Length - 0);
            }
        }
    }
}