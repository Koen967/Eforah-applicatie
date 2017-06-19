using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EforahWebapp.Services
{
    /// <summary>
    /// Static class to hash data.
    /// </summary>
    public static class HashServices
    {
        /// <summary>
        /// Hash input with SHA512
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA512.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        /// <summary>
        /// Hash a string
        /// </summary>
        /// <param name="inputString">Data to hash</param>
        /// <returns></returns>
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            //Hash each byte
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}