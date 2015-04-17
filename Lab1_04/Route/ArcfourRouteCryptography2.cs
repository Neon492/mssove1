using System.Text;

namespace Route
{
    /// <summary>
    ///     Øèôğ ìàğøğóòíîé ïåğåñòàíîâêè
    /// </summary>
    public class ArcfourRouteCryptography2 : ICryptography
    {
        private const string Alphabet = "àáâãäå¸æçèéêëìíîïğñòóôõö÷øùúûüışÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏĞÑÒÓÔÕÖ×ØÙÚÛÜİŞß"
                                        + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
                                        + " ;:,.!?()+-*/\\\"\'";

        private readonly Arcfour _arcfour1 = new Arcfour();
        private readonly Arcfour _arcfour2 = new Arcfour();
        private readonly Arcfour _arcfour3 = new Arcfour();

        public void SetKey(string keyText)
        {
            string[] keys = keyText.Split(':');
            if (keys.Length != 3) throw new RouteCryptography.WrongKeyException();
            _arcfour1.SetKey(keys[0]);
            _arcfour2.SetKey(keys[1]);
            _arcfour3.SetKey(keys[2]);
        }

        public void ClearKey()
        {
            _arcfour1.ClearKey();
            _arcfour2.ClearKey();
            _arcfour3.ClearKey();
        }

        public string EncryptNext(string plainText)
        {
            int n = Alphabet.Length;
            int len = plainText.Length;
            int[] p1 = _arcfour1.Ksa(len);
            int[] k = _arcfour2.Prga(len);
            int[] p2 = _arcfour3.Ksa(len);

            var p11 = new int[len];
            for (int i = 0; i < len; i++) p11[p1[i]] = i;

            var sb1 = new StringBuilder();
            for (int i = 0; i < len; i++) sb1.Append(plainText[p11[i]]);
            string s1 = sb1.ToString();

            var sb2 = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                int index = Alphabet.IndexOf(s1[i]);
                if (index == -1) throw new Arcfour.WrongCharException {Character = s1[i].ToString()};
                sb2.Append(Alphabet[(k[i] + n - index)%n]);
            }
            string s2 = sb2.ToString();

            var sb3 = new StringBuilder();
            for (int i = 0; i < len; i++) sb3.Append(s2[p2[i]]);
            string s3 = sb3.ToString();

            return s3;
        }

        public string DecryptNext(string cipherText)
        {
            int n = Alphabet.Length;
            int len = cipherText.Length;
            int[] p1 = _arcfour1.Ksa(len);
            int[] k = _arcfour2.Prga(len);
            int[] p2 = _arcfour3.Ksa(len);

            var p22 = new int[len];
            for (int i = 0; i < len; i++) p22[p2[i]] = i;

            var sb1 = new StringBuilder();
            for (int i = 0; i < len; i++) sb1.Append(cipherText[p22[i]]);
            string s1 = sb1.ToString();

            var sb2 = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                int index = Alphabet.IndexOf(s1[i]);
                if (index == -1) throw new Arcfour.WrongCharException {Character = s1[i].ToString()};
                sb2.Append(Alphabet[(k[i] + n - index)%n]);
            }
            string s2 = sb2.ToString();

            var sb3 = new StringBuilder();
            for (int i = 0; i < len; i++) sb3.Append(s2[p1[i]]);
            string s3 = sb3.ToString();

            return s3;
        }
    }
}