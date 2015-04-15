using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;

namespace RSA_Project
{
    public partial class MainWindow : Window
    {
        private const int Log10N = 26; //Длина ключа
        private static List<byte[]> _partition; //Разбиение строки на байт массивы
        private readonly RsaCryptography _cryptography; // Класс шифровальщика с открытым ключом

        public MainWindow()
        {
            InitializeComponent();
            //contentPanel.IsEnabled = false;
            BigInteger p, q;
            BigInteger.TryParse("379479948629", out p);
            BigInteger.TryParse("8903513187032818169", out q);
            _cryptography = new RsaCryptography(p, q);
            UpdateInfo();
        }


        private void UpdateInfo()
        {
            infoRSALabel.Content = String.Format("\nN:  {0} \nОткртый ключ: {1} \nЗакрытый ключ: {2}\n",
                _cryptography.Module, _cryptography.E, _cryptography.D);
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _partition =
                _cryptography.EncryptMessage(PartitionManager.StringToPartition(inTextMessage.Text,
                    _cryptography.ModuleLength - 1));
            encryptedBox.Text = PartitionManager.PartitionToString(_partition);
        }


        private void decryptBtn_Click(object sender, RoutedEventArgs e)
        {
            decryptedBox.Text = PartitionManager.PartitionToString(_cryptography.DecryptMessage(_partition));
        }

        private void inTextMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            encryptedBox.Text = "";
            decryptedBox.Text = "";
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            _cryptography.GenerateKeys(Log10N);
            UpdateInfo();
        }
    }
}