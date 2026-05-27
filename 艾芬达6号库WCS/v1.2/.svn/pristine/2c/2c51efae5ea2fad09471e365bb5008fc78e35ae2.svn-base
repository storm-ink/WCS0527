using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WCSValidity
{
    public static class ValidityHelper
    {
        //public static void load()
        //{
        //    string cfg_fn = Application.StartupPath + "\\WCSValidity.xml";
        //    XmlDocument doc = new XmlDocument();
        //    try { doc.Load(cfg_fn); }
        //    catch (Exception ex)
        //    {

        //    }

        //    var node = doc.SelectNodes("root/model")[0];
        //    //scheduler.currentModel = (SchedulerModels)Enum.Parse(typeof(SchedulerModels), node.Attributes["value"].Value);
        //}

        /// <summary>
        /// 唯一加密方式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string WeiYiJiaMiGuid(string str)
        {
            string keys = "GEN-SONG";
            return MD5Encrypt(str, keys) + "=" + keys;      //这里我把要加密的字符串和生成的key给拼接起来，这样我在调用 WeiJiaMiGuid方法是只需要传文本框text值就可以了；
        }

        /// <summary>
        /// 唯一解密方式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string WeiYiJieMiGuid(string str)
        {
            string[] pwa = str.Split(new char[] { '=' });   //分割一下    然后调解密
            return MD5Decrypt(pwa[0], pwa[1]);
        }

        /// <summary>
        /// 创建Key
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create("gen-song123");
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// MD5解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        public static DateTime StartDate;
        public static Int32 DayCount;
        public static Int32 OutDay;
        public static Boolean IsOverTime(String str)
        {
            try
            {
                var _str = WeiYiJieMiGuid(str);
                if (DateTime.TryParse(_str.Split(new char[] { '_' })[0], out StartDate))
                {
                    DayCount = Convert.ToInt32(_str.Split(new char[] { '_' })[1]);
                    OutDay = Convert.ToInt32(_str.Split(new char[] { '_' })[2]);
                    if (DayCount < OutDay)
                        return true;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                return true;
            }

            if (Convert.ToInt64(StartDate.ToString("yyyyMMdd")) > Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")))
                return true;

            var _days = Math.Ceiling(DateTime.Now.Subtract(StartDate).TotalDays);
            if (_days < 0 || _days > DayCount)
                return true;
            else
                return false;
        }
    }
}
