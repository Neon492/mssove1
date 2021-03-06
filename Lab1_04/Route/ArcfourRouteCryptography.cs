using System.Text;

namespace Route
{
    /// <summary>
    ///     ���� ���������� ������������
    /// </summary>
    public class ArcfourRouteCryptography : ICryptography
    {
        private const string Alphabet = "�������������������������������������Ũ��������������������������"
                                        + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
                                        + " ;:,.!?()+-*/\\\"\'";

        private readonly Arcfour _arcfour1 = new Arcfour();
        private readonly Arcfour _arcfour2 = new Arcfour();

        public void SetKey(string keyText)
        {
            string[] keys = keyText.Split(':');
            if (keys.Length != 2) throw new RouteCryptography.WrongKeyException();
            _arcfour1.SetKey(keys[0]);
            _arcfour2.SetKey(keys[1]);
        }

        public void ClearKey()
        {
            _arcfour1.ClearKey();
            _arcfour2.ClearKey();
        }

        public string EncryptNext(string plainText)
        {
            int len = plainText.Length;
            int[] p1 = _arcfour1.Ksa(len);
            int[] p2 = _arcfour2.Ksa(len);

            var p11 = new int[len];
            for (int i = 0; i < len; i++) p11[p1[i]] = i;

            var sb1 = new StringBuilder();
            for (int i = 0; i < len; i++) sb1.Append(plainText[p11[i]]);
            string s1 = sb1.ToString();

            var sb2 = new StringBuilder();
            for (int i = 0; i < len; i++) sb2.Append(s1[p2[i]]);
            string s2 = sb2.ToString();

            return s2;
        }

        public string DecryptNext(string cipherText)
        {
            int len = cipherText.Length;
            int[] p1 = _arcfour1.Ksa(len);
            int[] p2 = _arcfour2.Ksa(len);

            var p22 = new int[len];
            for (int i = 0; i < len; i++) p22[p2[i]] = i;

            var sb1 = new StringBuilder();
            for (int i = 0; i < len; i++) sb1.Append(cipherText[p22[i]]);
            string s1 = sb1.ToString();

            var sb2 = new StringBuilder();
            for (int i = 0; i < len; i++) sb2.Append(s1[p1[i]]);
            string s2 = sb2.ToString();

            return s2;
        }
    }
}