using System;
using System.Windows.Forms;

namespace Route
{
    public partial class RouteForm : Form
    {
        public RouteForm()
        {
            InitializeComponent();
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                ICryptography cryptography = encryptTriple.Checked
                    ? new ArcfourRouteCryptography2()
                    : (ICryptography) new ArcfourRouteCryptography();
                cryptography.SetKey(encryptKey.Text);
                encryptDest.Text = cryptography.EncryptNext(encryptSource.Text);
            }
            catch (RouteCryptography.WrongKeyException)
            {
                MessageBox.Show(@"Неправильный ключ");
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                ICryptography cryptography = decryptTriple.Checked
                    ? new ArcfourRouteCryptography2()
                    : (ICryptography) new ArcfourRouteCryptography();
                cryptography.SetKey(decryptKey.Text);
                decryptDest.Text = cryptography.DecryptNext(decryptSource.Text);
            }
            catch (RouteCryptography.WrongKeyException)
            {
                MessageBox.Show(@"Неправильный ключ");
            }
        }
    }
}