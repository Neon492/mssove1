namespace Route
{
    /// <summary>
    ///     Интерфейс поточного текстового шифратора
    /// </summary>
    public interface ICryptography
    {
        void SetKey(string keyText);
        void ClearKey();
        string EncryptNext(string plainText);
        string DecryptNext(string cipherText);
    }
}