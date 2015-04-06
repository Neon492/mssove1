using System;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class TrithemiusForm : Form
    {
        public TrithemiusForm()
        {
            InitializeComponent();
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                TrithemiusCryptography cryptography = new TrithemiusCryptography();
                cryptography.SetKey(encryptKey.Text, (int) encryptColumnKey.Value);
                cryptography.SetAdditionalKey((int) encryptAdditionalKey.Value);
                encryptDest.Text = cryptography.EncryptNext(encryptSource.Text);
            }
            catch (TrithemiusCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (TrithemiusCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                TrithemiusCryptography cryptography = new TrithemiusCryptography();
                cryptography.SetKey(decryptKey.Text, (int)decryptColumnKey.Value);
                cryptography.SetAdditionalKey((int)decryptAdditionalKey.Value);
                decryptDest.Text = cryptography.DecryptNext(decryptSource.Text);
            }
            catch (TrithemiusCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (TrithemiusCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }
    }
}