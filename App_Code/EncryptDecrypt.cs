using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.IO;

/// <summary>
/// Summary description for EncryptDecrypt
/// </summary>
public class EncryptDecrypt
{
	public EncryptDecrypt()
	{
	}

    public static string Encrypt(string _wrd)
    {
        byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(_wrd);
        byte[] rgbKey = System.Text.ASCIIEncoding.ASCII.GetBytes("Salis916");
        byte[] rgbIV = System.Text.ASCIIEncoding.ASCII.GetBytes("Katin917");
        string encryptWrd= "";

        //1024-bit encryption
        MemoryStream memStrm = new MemoryStream(1024);
        DESCryptoServiceProvider desProv = new DESCryptoServiceProvider();

        CryptoStream crypStrm = new CryptoStream(memStrm, desProv.CreateEncryptor(rgbKey, rgbIV),CryptoStreamMode.Write);

        crypStrm.Write(data, 0, data.Length);
        crypStrm.FlushFinalBlock();

        byte[] result = new byte[(int)memStrm.Position];
        memStrm.Position = 0;
        memStrm.Read(result, 0, result.Length);

        crypStrm.Close();

        encryptWrd = System.Convert.ToBase64String(result);

        return encryptWrd;
    }

    public static string Decrypt(string encryptWrd)
    {
        byte[] data = System.Convert.FromBase64String(encryptWrd);
        byte[] rgbKey = System.Text.ASCIIEncoding.ASCII.GetBytes("Salis916");
        byte[] rgbIV = System.Text.ASCIIEncoding.ASCII.GetBytes("Katin917");
        string decryptWrd = "";

        MemoryStream memStrm = new MemoryStream(data.Length);

        DESCryptoServiceProvider desProv = new DESCryptoServiceProvider();

        CryptoStream crypStrm = new CryptoStream(memStrm, desProv.CreateDecryptor(rgbKey, rgbIV),CryptoStreamMode.Read);

        memStrm.Write(data, 0, data.Length);
        memStrm.Position = 0;

        decryptWrd = new StreamReader(crypStrm).ReadToEnd();

        crypStrm.Close();

        return decryptWrd;
    }
}
