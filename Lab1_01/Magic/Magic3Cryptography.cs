namespace Magic
{
    /// <summary>
    ///     Тройной шифр основе «магических» квадратов
    ///     Шифратор из последовательно соединённых трёх шифраторов с шифром основе «магических» квадратов
    ///     Шифратор #1 работает в прямом направлении
    ///     Шифратор #2 работает в обратном направлении
    ///     Шифратор #3 работает в прямом направлении
    /// </summary>
    public class Magic3Cryptography : ICryptography
    {
        private readonly MagicCryptography _cryptography1 = new MagicCryptography();
        private readonly MagicCryptography _cryptography2 = new MagicCryptography();
        private readonly MagicCryptography _cryptography3 = new MagicCryptography();

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
            string[] keys = keyText.Split(':');
            if (keys.Length != 3) throw new MagicCryptography.WrongKeyException();
            keyText1 = keys[0];
            keyText2 = keys[1];
            keyText3 = keys[2];
        }
    }
}