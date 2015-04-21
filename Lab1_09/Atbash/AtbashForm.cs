using System;
using System.Windows.Forms;

namespace Atbash
{
    public partial class AtbashForm : Form
    {
        public AtbashForm()
        {
            InitializeComponent();
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                AtbashCryptography cryptography = new AtbashCryptography();
                cryptography.SetKey(encryptKey.Text);
                cryptography.SetAdditionalKey((int) encryptAdditionalKey.Value);
                encryptDest.Text = cryptography.EncryptNext(encryptSource.Text);
            }
            catch (AtbashCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (AtbashCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                AtbashCryptography cryptography = new AtbashCryptography();
                cryptography.SetKey(decryptKey.Text);
                cryptography.SetAdditionalKey((int)decryptAdditionalKey.Value);
                decryptDest.Text = cryptography.DecryptNext(decryptSource.Text);
            }
            catch (AtbashCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (AtbashCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }
    }
}