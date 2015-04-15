using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;

namespace RSA_Project
{
    public partial class StartWindow : Window
    {
        private const int Decimals = 44; //Длина ключа
        private static List<byte[]> _partition; //Разбиение строки на байт массивы
        private readonly RsaCrypto _rsa; // Класс шифровальщика с открытым ключом

        public StartWindow()
        {
            InitializeComponent();
            BigInteger p, q;
            BigInteger.TryParse("3125408473520033", out p);
            BigInteger.TryParse("24677869680065417487907", out q);
            _rsa = new RsaCrypto(p, q);
            Update();
        }


        private void generate_Click(object sender, RoutedEventArgs e)
        {
            _rsa.GeneratePairs(Decimals);
            Update();
        }

        private void Update()
        {
            infoLabel.Text = String.Format("N:  {0} \n PublicKey: {1} \n PrivateKey: {2}\n",
                _rsa.N, _rsa.PublicKey, _rsa.PrivateKey);
        }

        private List<byte[]> EncryptMessage(List<byte[]> p)
        {
//Зашифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(_rsa.EncryptMessage(block));
            }
            return result;
        }

        private List<byte[]> DecryptMessage(List<byte[]> p)
        {
//Разшифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(_rsa.DecryptMessage(block));
            }
            return result;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _partition = EncryptMessage(PartitionManager.StringToPartition(inTextMessage.Text, _rsa.NbyteLength - 1));
            encryptedBox.Text = PartitionManager.PartitionToString(_partition);
        }


        private void decryptBtn_Click(object sender, RoutedEventArgs e)
        {
            decryptedBox.Text = PartitionManager.PartitionToString(DecryptMessage(_partition));
        }

        private void inTextMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            encryptedBox.Text = "";
            decryptedBox.Text = "";
        }
    }
}