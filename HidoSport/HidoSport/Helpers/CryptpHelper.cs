using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;


namespace HidoSport.Helpers
{
    public static class CryptpHelper
    {
        /// <summary>
        /// Tính toán SHA1 của một chuỗi bất kỳ
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CalculateSHA1(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
            return hash.ToLower();
        }

        /// <summary>
        ///     Gets a MD5 hash of the current instance.
        /// </summary>
        /// <param name="instance">
        ///     The instance being extended.
        /// </param>
        /// <returns>
        ///     A base 64 encoded string representation of the hash.
        /// </returns>
        public static string GetMD5Hash(this object instance)
        {
            return instance.GetHash<MD5CryptoServiceProvider>();
        }

        /// <summary>
        /// Gets a hash of the current instance.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the Cryptographic Service Provider to use.
        /// </typeparam>
        /// <param name="instance">
        /// The instance being extended.
        /// </param>
        /// <returns>
        /// A base 64 encoded string representation of the hash.
        /// </returns>
        public static string GetHash<T>(this object instance) where T : HashAlgorithm, new()
        {
            T cryptoServiceProvider = new T();
            return ComputeHash(instance, cryptoServiceProvider);
        }

        private static string ComputeHash<T>(object instance, T cryptoServiceProvider) where T : HashAlgorithm, new()
        {
            DataContractSerializer serializer = new DataContractSerializer(instance.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, instance);
                cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                return Convert.ToBase64String(cryptoServiceProvider.Hash);
            }
        }
        /// <summary>
        /// Converts to hexadecimal string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertToHexString(long value)
        {
            var valueBytes = Encoding.ASCII.GetBytes(value.ToString(CultureInfo.InvariantCulture));
            return BitConverter.ToString(valueBytes).Replace("-", "");
        }

        public static long ConvertHexStringToInt(string hexString)
        {
            int numberChars = hexString.Length;
            var bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return Convert.ToInt64(Encoding.ASCII.GetString(bytes));
        }

        /// <summary>
        /// Sinh mã xác thực
        /// </summary>
        /// <param name="codelen"></param>
        /// <returns></returns>
        public static string GenerateVerificationCode(int codelen)
        {
            string code = "";
            var rnd = new Random();
            for (int i = 0; i < codelen; i++)
            {
                code += rnd.Next(10);
            }
            return code;
        }
    }
}