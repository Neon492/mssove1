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
        private const int Len = 49; //Длина ключа
        private static List<byte[]> _partition; //Разбиение строки на байт массивы
        private readonly RsaCrypt _crypt; // Класс шифровальщика с открытым ключом

        public MainWindow()
        {
            InitializeComponent();
            _crypt = new RsaCrypt(Len);
            UpdateInfo();
        }


        private void UpdateInfo()
        {
            infoRSALabel.Content = String.Format("\nN:  {0} \nОткртый ключ: {1} \nЗакрытый ключ: {2}\n",
                _crypt.Module, _crypt.PubKey, _crypt.PriKey);
        }

        #region События формы

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            _crypt.GenerateKeys(Len);
            UpdateInfo();
        }

        private void encryptBtn_Click(object sender, RoutedEventArgs e)
        {
            _partition =
                _crypt.EncryptMessage(TextManager.PartitionOfString(messageBox.Text,
                    _crypt.ModuleLength - 1));
            encryptedBox.Text = TextManager.PartitionToString(_partition);
        }


        private void decryptBtn_Click(object sender, RoutedEventArgs e)
        {
            decryptedBox.Text = TextManager.PartitionToString(_crypt.DecryptMessage(_partition));
        }

        private void messageBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            encryptedBox.Text = "";
            decryptedBox.Text = "";
        }

        #endregion
    }
}