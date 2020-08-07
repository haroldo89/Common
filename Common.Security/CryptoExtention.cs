using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security
{
    static public class CryptoExtention
    {
        static readonly byte[] Salt = Encoding.ASCII.GetBytes("chk846342bM7c5");

        /// <summary>
        /// Encrypt the given string using AES. The string can be decrypted using 
        /// Decrypt(). The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        public static string Encrypt(this string plainText, string sharedSecret)
        {
            //Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(plainText));
            //Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(sharedSecret));

            return CryptoHelper(plainText, sharedSecret, true);
        }

        /// <summary>
        /// Decrypt the given string. Assumes the string was encrypted using 
        /// Encrypt(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        public static string Decrypt(this string cipherText, string sharedSecret)
        {
            //Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(sharedSecret));

            return !string.IsNullOrWhiteSpace(cipherText) ?
                CryptoHelper(cipherText, sharedSecret, false) :
                string.Empty;
        }

        static string CryptoHelper(string text, string sharedSecret, bool encrypt)
        {
            var cryptoMode = encrypt ? CryptoStreamMode.Write : CryptoStreamMode.Read;
            CryptoStream cs = null;

            try
            {
                using (var ms = CreateMemoryStream(text, encrypt))
                using (var key = new Rfc2898DeriveBytes(sharedSecret, Salt))
                using (var aes = new RijndaelManaged())
                {
                    aes.Key = key.GetBytes(aes.KeySize / 8);

                    if (encrypt)
                    {
                        ms.Write(BitConverter.GetBytes(aes.IV.Length), 0, sizeof(int));
                        ms.Write(aes.IV, 0, aes.IV.Length);
                    }
                    else { aes.IV = ReadPrependedIV(ms); }

                    using (var crypto = CreateCryptor(aes, encrypt))
                    {
                        cs = new CryptoStream(ms, crypto, cryptoMode);
                        return encrypt
                            ? Writing(ref cs, ms, text)
                            : Reading(ref cs);
                    }
                }
            }
            finally
            {
                if (cs != null) cs.Dispose();
            }
        }

        static MemoryStream CreateMemoryStream(string text, bool encrypt)
        {
            return encrypt
                ? new MemoryStream()
                : new MemoryStream(Convert.FromBase64String(text));
        }

        static ICryptoTransform CreateCryptor(RijndaelManaged aes, bool encrypt)
        {
            return encrypt
                ? aes.CreateEncryptor(aes.Key, aes.IV)
                : aes.CreateDecryptor(aes.Key, aes.IV);
        }

        static string Reading(ref CryptoStream cs)
        {
            using (var reader = new StreamReader(cs))
            {
                cs = null;
                return reader.ReadToEnd();
            }
        }

        static string Writing(ref CryptoStream cs, MemoryStream ms, string text)
        {
            using (var writer = new StreamWriter(cs))
            {
                cs = null;
                writer.Write(text);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        static byte[] ReadPrependedIV(Stream stream)
        {
            var aesSize = ReadIVLength(stream);
            var buffer = new byte[aesSize];

            if (stream.Read(buffer, 0, aesSize) == aesSize)
                return buffer;

            throw new SystemException("Did not read byte array properly");
        }

        static int ReadIVLength(Stream stream)
        {
            var ret = new byte[sizeof(int)];

            if (stream.Read(ret, 0, ret.Length) == ret.Length)
                return BitConverter.ToInt32(ret, 0);

            throw new SystemException("Stream did not contain properly formatted byte array");
        }
    }
}
