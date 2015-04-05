using System;
using System.Windows.Forms;

namespace Magic
{
    public partial class MagicForm : Form
    {
        public MagicForm()
        {
            InitializeComponent();
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                ICryptography cryptography = encryptTriple.Checked
                    ? new Magic3Cryptography()
                    : (ICryptography) new MagicCryptography();
                cryptography.SetKey(encryptKey.Text);
                encryptDest.Text = cryptography.EncryptNext(encryptSource.Text);
            }
            catch (MagicCryptography.WrongKeyException)
            {
                MessageBox.Show(@"Неправильный ключ");
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                ICryptography cryptography = decryptTriple.Checked
                    ? new Magic3Cryptography()
                    : (ICryptography) new MagicCryptography();
                cryptography.SetKey(decryptKey.Text);
                decryptDest.Text = cryptography.DecryptNext(decryptSource.Text);
            }
            catch (MagicCryptography.WrongKeyException)
            {
                MessageBox.Show(@"Неправильный ключ");
            }
        }
    }
}