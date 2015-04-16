using System;

namespace Route
{
    public class Arcfour
    {
        /// <summary>
        ///     Àכפאגטע
        /// </summary>
        private const string Alphabet = "אבגדהו¸זחטיךכלםמןנסעףפץצקרשתûü‎‏ÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞ‗"
                                        + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
                                        + " ;:,.!?()+-*/\\\"\'";

        private string _keyText = string.Empty; // Êכ‏קוגמו סכמגמ

        public int[] Ksa(int n)
        {
            if (string.IsNullOrWhiteSpace(_keyText)) throw new EmptyKeyException();
            int l = _keyText.Length;
            int[] s = new int[n];
            int[] key = new int[l];
            for (int i = 0; i < l; i++)
            {
                int index = Alphabet.IndexOf(_keyText[i]);
                if (index == -1) throw new WrongCharException() { Character = _keyText[i].ToString() };
                key[i] = index;
            }
            for (int i = 0; i < n; i++) s[i] = i;
            for (int i = 0,j=0; i < n; i++)
            {
                j = (j + s[i] + key[i%l])%n;
                int temp = s[i];
                s[i] = s[j];
                s[j] = temp;
            }
            return s;
        }

        public int[] Prga(int n)
        {
            if (string.IsNullOrWhiteSpace(_keyText)) throw new EmptyKeyException();
            int l = _keyText.Length;
            int[] k = new int[n];
            int[] s = Ksa(256);
            for (int index = 0, i = 0, j = 0; index < n; index++)
            {
                i = (i + 1)%l;
                j = (j + s[i])%l;
                int temp = s[i];
                s[i] = s[j];
                s[j] = temp;
                int t = (s[i] + s[j])%l;
                k[index]=s[t];
            }
            return k;
        }

        public void SetKey(string keyText)
        {
            if (string.IsNullOrWhiteSpace(keyText)) throw new EmptyKeyException();
            _keyText = keyText;
        }

        public void ClearKey()
        {
            _keyText = string.Empty;
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