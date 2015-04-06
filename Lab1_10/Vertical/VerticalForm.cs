using System;
using System.Windows.Forms;

namespace Vertical
{
    public partial class VerticalForm : Form
    {
        public VerticalForm()
        {
            InitializeComponent();
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                ICryptography cryptography = encryptTriple.Checked
                    ? new Vertical3Cryptography()
                    : (ICryptography) new VerticalCryptography();
                cryptography.SetKey(encryptKey.Text);
                encryptDest.Text = cryptography.EncryptNext(encryptSource.Text);
            }
            catch (VerticalCryptography.WrongKeyException)
            {
                MessageBox.Show(@"Неправильный ключ");
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                ICryptography cryptography = decryptTriple.Checked
                    ? new Vertical3Cryptography()
                    : (ICryptography) new VerticalCryptography();
                cryptography.SetKey(decryptKey.Text);
                decryptDest.Text = cryptography.DecryptNext(decryptSource.Text);
            }
            catch (VerticalCryptography.WrongKeyException)
            {
                MessageBox.Show(@"Неправильный ключ");
            }
        }
    }
}