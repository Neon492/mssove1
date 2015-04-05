using System;
using System.Windows.Forms;

namespace Mirabeau
{
    public partial class MirabeauForm : Form
    {
        public MirabeauForm()
        {
            InitializeComponent();
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                MirabeauCryptography cryptography = new MirabeauCryptography();
                cryptography.SetKey(encryptKey.Text);
                cryptography.SetAdditionalKey((int) encryptAdditionalKey.Value);
                encryptDest.Text = cryptography.EncryptNext(encryptSource.Text);
            }
            catch (MirabeauCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (MirabeauCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                MirabeauCryptography cryptography = new MirabeauCryptography();
                cryptography.SetKey(decryptKey.Text);
                cryptography.SetAdditionalKey((int)decryptAdditionalKey.Value);
                decryptDest.Text = cryptography.DecryptNext(decryptSource.Text);
            }
            catch (MirabeauCryptography.WrongCharException exception)
            {
                MessageBox.Show(@"Используются символы не содержащиеся в алфавите (" + exception.Character + ")");
            }
            catch (MirabeauCryptography.EmptyKeyException)
            {
                MessageBox.Show(@"Ключ не содержит символов");
            }
        }
    }
}