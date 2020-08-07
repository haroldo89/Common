using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace AlbatrosSoft.Common.Security
{
    /// <summary>
    /// Agrupa un conjunto de utilidades especificas de cifrado y descifrado de datos.
    /// </summary>
    public static class CryptoUtils
    {
        private static readonly string PASSWORD_KEY = "w1z3nz7ec8n0log1e5";
        private static readonly byte[] SALT_VALUE = { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x5, 0x4, 0x3, 0x2, 0x1, 0x0 };

        private static SymmetricAlgorithm _SymmetricEncryptionAlgorithm;

        private static SymmetricAlgorithm SymmetricEncryptionAlgorithm
        {
            get
            {
                if (_SymmetricEncryptionAlgorithm == null)
                {
                    _SymmetricEncryptionAlgorithm = GetSymmetricAlgorithm();
                }
                return _SymmetricEncryptionAlgorithm;
            }
        }

        /// <summary>
        /// Realiza el cifrado de una cadena en texto plano a través de un algoritmo de encripción simétrica.
        /// </summary>
        /// <param name="plainText">Texto a cifrar.</param>
        /// <returns>Cadena de texto cifrada.</returns>
        public static string EncryptText(string plainText)
        {
            string cipherText = string.Empty;
            byte[] srcDataBytes;
            byte[] cipherData;
            //Convertir el texto plano a su representacion en bytes
            srcDataBytes = GetBytesFromString(plainText);
            //Encriptar el texto plano
            using (MemoryStream sourceStream = new MemoryStream(srcDataBytes))
            {
                MemoryStream destStream = new MemoryStream();
                CryptoStream cryproStream = new CryptoStream(sourceStream, SymmetricEncryptionAlgorithm.CreateEncryptor(), CryptoStreamMode.Read);
                MoveBytes(cryproStream, destStream);
                cipherData = destStream.ToArray();
            }
            cipherText = cipherData == null ? string.Empty : Convert.ToBase64String(cipherData);
            return cipherText;
        }

        /// <summary>
        /// Permite obtener el valor en texto plano de una cadena cifrada a partir de un algoritmo de encripción simétrica.
        /// </summary>
        /// <param name="cipherText">Texto cifrado.</param>
        /// <returns>Texto plano que representa el valor cifrado.</returns>
        public static string DecryptText(string cipherText)
        {
            byte[] cipherData = Convert.FromBase64String(cipherText);
            string decryptedText = string.Empty;
            byte[] decryptedData;
            if (cipherData != null)
            {
                using (MemoryStream sourceStream = new MemoryStream(cipherData))
                {
                    MemoryStream destStream = new MemoryStream();
                    CryptoStream cryproStream = new CryptoStream(sourceStream, SymmetricEncryptionAlgorithm.CreateDecryptor(), CryptoStreamMode.Read);
                    MoveBytes(cryproStream, destStream);
                    decryptedData = destStream.ToArray();
                    //Convertir los datos desencriptados en texto plano
                    decryptedText = GetStringFromBytes(decryptedData);
                }
            }
            return decryptedText;
        }

        /// <summary>
        /// Permite calcular el valor hash correspondiente a una cadena de texto determinada.
        /// </summary>
        /// <param name="inputString">Cadena de entrada</param>
        /// <returns>Valor hash de la cadena de texto.</returns>
        public static string ComputeHash(string inputString)
        {
            var sha = new SHA256Managed();
            var bytes = Encoding.Unicode.GetBytes(inputString);
            var hashValue = sha.ComputeHash(bytes);
            return ToHexString(hashValue);
        }

        /// <summary>
        /// Obtiene la representación en formato hexadecimal de un conjunto de bytes.
        /// </summary>
        /// <param name="bytes">Conjunto de bytes a transformar,</param>
        /// <returns>Representación hexadecimal del conjunto de bytes.</returns>
        public static string ToHexString(byte[] bytes)
        {
            var hex = new StringBuilder();
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        private static void MoveBytes(Stream sourceStream, Stream destStream)
        {
            byte[] buffer = new byte[4096];
            try
            {
                var count = sourceStream.Read(buffer, 0, buffer.Length);
                while (count != 0)
                {
                    destStream.Write(buffer, 0, count);
                    count = sourceStream.Read(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {
                destStream.Flush();
            }
        }

        private static SymmetricAlgorithm GetSymmetricAlgorithm()
        {
            //Generar la llave basada en una contraseña y un saltValue
            var key = new Rfc2898DeriveBytes(PASSWORD_KEY, SALT_VALUE);
            //Definir el Algoritmo de Encripcion (Rijndael)
            var algorithm = new RijndaelManaged()
            {
                Key = key.GetBytes(16),
                IV = key.GetBytes(16)
            };
            return algorithm;
        }

        private static byte[] GetBytesFromString(string inputString)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            return encoding.GetBytes(inputString);
        }

        private static string GetStringFromBytes(byte[] data)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            return encoding.GetString(data);
        }
    }
}
