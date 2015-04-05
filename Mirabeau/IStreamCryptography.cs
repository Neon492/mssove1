namespace Mirabeau
{
    /// <summary>
    ///     Интерфейс поточного текстового шифратора
    /// </summary>
    public interface IStreamCryptography
    {
        void SetKey(string keyText);
        void ClearKey();
        void Restart();
        string EncryptNext(string plainText);
        string DecryptNext(string cipherText);
    }
}