using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RSA_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const byte lengthN = 31;//Длина ключа
        private static List<byte[]> partition;//Разбиение строки на байт массивы
        private RSACryptography RSAcrypto; // Класс шифровальщика с открытым ключом
        public MainWindow()
        {
            InitializeComponent();
            //contentPanel.IsEnabled = false;
            BigInteger p, q;
            BigInteger.TryParse("379479948629",out p);
            BigInteger.TryParse("8903513187032818169", out q);
            RSAcrypto = new RSACryptography(p,q);
            UpdateRSAInfo();
        }

        
        private void UpdateRSAInfo()
        {
            infoRSALabel.Content = String.Format("\nN:  {0} \nОткртый ключ: {1} \nЗакрытый ключ: {2}\n",
                                             RSAcrypto.Module, RSAcrypto.PublicKey, RSAcrypto.PrivateKey);
        }

        private List<byte[]> EncryptMessage(List<byte[]> p)
        {//Зашифровать строку
            List<byte[]> result = new List<byte[]>();
            foreach (byte[] block in p)
            {
                result.Add(RSAcrypto.Encrypt(block));
            }
            return result;
        }
        private List<byte[]> DecryptMessage(List<byte[]> p)
        {//Разшифровать строку
            List<byte[]> result = new List<byte[]>();
            foreach (byte[] block in p)
            {
                result.Add(RSAcrypto.Decrypt(block));
            }
            return result;
        }
        
        #region События формы
        private void encryptBtn_Click(object sender, RoutedEventArgs e)
        {
            partition = EncryptMessage(MessageManager.partitionOfString(messageBox.Text, RSAcrypto.ModuleLength-1));
            encryptedBox.Text = MessageManager.PartitionToString(partition);
        }

        

        private void decryptBtn_Click(object sender, RoutedEventArgs e)
        {
  
            decryptedBox.Text = MessageManager.PartitionToString(DecryptMessage(partition));
        }

        private void messageBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            encryptedBox.Text = "";
            decryptedBox.Text = "";
        }

        #endregion

    }
}
