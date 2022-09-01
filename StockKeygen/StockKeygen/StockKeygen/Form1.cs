using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace StockKeygen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = GenerateKey(textBox1.Text);
        }

        static string GenerateKey(string deviceID)
        {
            string temp = "";

            for (int i = 0; i < deviceID.Length; i++)
            {
                if (deviceID[i] == '-')
                {
                    continue;
                }

                temp += deviceID[i];
            }

            deviceID = temp;

            System.Text.Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            byte[] unicodeText = new byte[deviceID.Length * 2];
            enc.GetBytes(deviceID.ToCharArray(), 0, deviceID.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            string serial = "";

            for (int i = 0; i < 16; i++)
            {                
                serial += sb.ToString()[i * 2 + 1];

                if (i % 4 == 3 && i != 15)
                {
                    serial += "-";
                }
            }

            return serial;
        }
    }
}
