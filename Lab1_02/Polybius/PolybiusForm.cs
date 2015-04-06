using System;
using System.Windows.Forms;

namespace Polybius
{
    public partial class PolybiusForm : Form
    {
        public PolybiusForm()
        {
            InitializeComponent();
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                PolybiusCryptography cryptography = new PolybiusCryptography();
                cryptography.SetKey(encryptKey.Text);
                cryptography.SetAdditionalKey((int) encryptAdditionalKey.Value);
                encryptDest.Text = cryptography.EncryptNext(encryptSource.Text);
            }
            catch (PolybiusCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (PolybiusCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                PolybiusCryptography cryptography = new PolybiusCryptography();
                cryptography.SetKey(decryptKey.Text);
                cryptography.SetAdditionalKey((int)decryptAdditionalKey.Value);
                decryptDest.Text = cryptography.DecryptNext(decryptSource.Text);
            }
            catch (PolybiusCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (PolybiusCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }
    }
}