using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DataLayer.Security
{
    public static class PasswordHelper
    {
        public static string EncodePasswordMd5(string pass) //Encrypt using SHA256   
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pass));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }


    }
}
