using System;
using System.Windows.Forms;

namespace Vigenere
{
    public partial class VigenereForm : Form
    {
        public VigenereForm()
        {
            InitializeComponent();
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                IStreamCryptography cryptography = encryptTriple.Checked
                    ? new Vigenere3Cryptography()
                    : (IStreamCryptography) new VigenereCryptography();
                cryptography.SetKey(encryptKey.Text);
                encryptDest.Text = cryptography.EncryptNext(encryptSource.Text);
            }
            catch (VigenereCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (VigenereCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                IStreamCryptography cryptography = decryptTriple.Checked
                    ? new Vigenere3Cryptography()
                    : (IStreamCryptography) new VigenereCryptography();
                cryptography.SetKey(decryptKey.Text);
                decryptDest.Text = cryptography.DecryptNext(decryptSource.Text);
            }
            catch (VigenereCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (VigenereCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }
    }
}