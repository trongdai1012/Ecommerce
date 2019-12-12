using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace KLTN.Common
{
    public class Encryptor
    {
        public static string Md5Hash(string text)
        {
            var md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));

            //get hash result after compute it
            var result = md5.Hash;

            var strBuilder = new StringBuilder();
            foreach (var t in result)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(t.ToString(Constants.X2));
            }

            return strBuilder.ToString();
        }
    }
}
