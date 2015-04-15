using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms;

namespace RSACryptosystemProject
{
    public partial class Form1 : Form
    {
        private const int Decimals = 44; //Длина ключа
        private static List<byte[]> _partition; //Разбиение строки на байт массивы
        private readonly Cryptosystem _cry; // Класс шифровальщика с открытым ключом
        private BigInteger p, q;

        public Form1()
        {
            InitializeComponent();
            BigInteger.TryParse("522224862977635043", out p);
            BigInteger.TryParse("19847157582713581370902789", out q);
            _cry = new Cryptosystem(p, q);
            Refresh();
        }

        private List<byte[]> EncryptMessage(List<byte[]> p)
        {
//Зашифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(_cry.Encrypt(block));
            }
            return result;
        }

        private List<byte[]> DecryptMessage(List<byte[]> p)
        {
//Разшифровать строку
            var result = new List<byte[]>();
            foreach (var block in p)
            {
                result.Add(_cry.Decrypt(block));
            }
            return result;
        }

        private void Refresh()
        {
//Обновляем инофрмацию и объекты
            infoBox.Text = String.Format("N:  {0} \r\nОткртый ключ: {1} \r\nЗакрытый ключ: {2}\n",
                _cry.Module, _cry.PublicKey, _cry.PrivateKey);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _partition = EncryptMessage(StringConverter.StringToPartition(textBox1.Text, _cry.ModuleLength - 1));
            textBox2.Text = StringConverter.PartitionToString(_partition);
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = StringConverter.PartitionToString(DecryptMessage(_partition));
        }

        private void generate_Click(object sender, EventArgs e)
        {
            _cry.GeneratePairs(Decimals);
            Refresh();
        }
    }
}