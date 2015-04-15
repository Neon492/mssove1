using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RSA_Project
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const byte LengthN = 28; //Длина ключа
        private static List<byte[]> _partition; //Разбиение строки на байт массивы
        private Rsa RSAcrypto; // Класс шифровальщика с открытым ключом

        public MainWindow()
        {
            InitializeComponent();
            //contentPanel.IsEnabled = false;
            RSAcrypto = new Rsa(LengthN);
            UpdateRSAInfo();
        }


        private void UpdateRSAInfo()
        {
            infoRSALabel.Content =
                String.Format("P:  {0} \nQ:  {1} \nN:  {2} \nОткртый ключ: {3} \nЗакрытый ключ: {4}\n",
                    RSAcrypto.PrimePair.Key, RSAcrypto.PrimePair.Value, RSAcrypto.Module, RSAcrypto.PublicKey,
                    RSAcrypto.PrivateKey);
        }

        private List<byte[]> EncryptMessage(List<byte[]> p)
        {
//Зашифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(RSAcrypto.Encrypt(block));
            }
            return result;
        }

        private List<byte[]> DecryptMessage(List<byte[]> p)
        {
//Разшифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(RSAcrypto.Decrypt(block));
            }
            return result;
        }

        #region События формы

        private void encryptBtn_Click(object sender, RoutedEventArgs e)
        {
            _partition = EncryptMessage(MessageManager.partitionOfString(messageBox.Text, RSAcrypto.ModuleLength - 1));
            encryptedBox.Text = MessageManager.PartitionToString(_partition);
        }


        private void decryptBtn_Click(object sender, RoutedEventArgs e)
        {
            decryptedBox.Text = MessageManager.PartitionToString(DecryptMessage(_partition));
        }

        private void messageBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            encryptedBox.Text = "";
            decryptedBox.Text = "";
        }

        private void generateNewRSAButton_Click(object sender, RoutedEventArgs e)
        {
            RSAcrypto = new Rsa(LengthN);
            UpdateRSAInfo();
            _partition = new List<byte[]>();
            messageBox.Text = String.Empty;
            encryptedBox.Text = String.Empty;
            decryptedBox.Text = String.Empty;
        }

        #endregion
    }
}