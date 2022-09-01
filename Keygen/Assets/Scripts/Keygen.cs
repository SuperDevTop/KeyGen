using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Keygen : MonoBehaviour
{
    public GameObject main;
    public InputField deviceID;
    //public Text serialNumber;
    public InputField serialNumber;

    void Start()
    {
        main.transform.localScale = new Vector3(Screen.width / 2532f, Screen.height / 1170f, 1);
    }

    void Update()
    {
        main.transform.localScale = new Vector3(Screen.width / 2532f, Screen.height / 1170f, 1);
    }

    public void GenerateBtnClick()
    {
        serialNumber.text = GenerateKey(deviceID.text);
    }

    public void ExitBtnClick()
    {
        Application.Quit();
    }

    public string GenerateKey(string deviceID)
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
