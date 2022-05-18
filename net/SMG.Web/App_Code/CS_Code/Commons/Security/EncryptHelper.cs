using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Iberdrola.Commons.Exceptions;
using Iberdrola.SMG.BLL;
using System.Security.Cryptography.X509Certificates;
using Iberdrola.Commons.Messages;

namespace Iberdrola.Commons.Security
{
    /// <summary>
    /// Clase para proporcionar encriptación y desencriptación de cadenas de texto.
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// Variable para la encriptación.
        /// </summary>
        private static string tempIV = "1b46123aaed34e869af8";

        /// <summary>
        /// Clave con la que realiza la encriptación.
        /// </summary>
        private static string key = "abc";

        /// <summary>
        /// Encripta el texto pasado por parámetro. Hace un URL encode antes de devolver el texto
        /// para que los caracteres especiales viajen sin problemas por la web.
        /// </summary>
        /// <param name="sourceText">Texto a cifrar.</param>
        /// <returns>Texto Cifrado.</returns>
        public static string Encrypt(string sourceText)
        {
            try
            {
                SymmetricAlgorithm objCrptoService = new DESCryptoServiceProvider();

                // Create a memory stream.         
                MemoryStream objMemStream = new MemoryStream();

                // Set the legal keys and initialization verctors.       
                objCrptoService.Key = GetLegalsecretKey(key, objCrptoService);
                objCrptoService.IV = GetLegalIV(objCrptoService);

                // Create a CryptoStream using the memory stream and the cryptographic service provider  version        
                // of the Data Encryption stanadard algorithm key.          
                CryptoStream objCryptStream = new CryptoStream(objMemStream, objCrptoService.CreateEncryptor(), CryptoStreamMode.Write);

                // Create a StreamWriter to write a string to the stream.        
                StreamWriter objStreamWriter = new StreamWriter(objCryptStream);

                // Write the sourceText to the memroy stream.        
                objStreamWriter.WriteLine(sourceText);

                // Close the StreamWriter and CryptoStream objects.        
                objStreamWriter.Close();
                objCryptStream.Close();

                // Get an array of bytes that represents the memory stream.        
                byte[] outputBuffer = objMemStream.ToArray();

                // Close the memory stream.        
                objMemStream.Close();

                // Return the encrypted byte array.        
                string signedText = System.Convert.ToBase64String(outputBuffer);
                signedText = System.Web.HttpUtility.UrlEncode(signedText);
                return signedText;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1028");
            }
        }

        /// <summary>
        /// Desencripta el texto pasado por parámetro. NN Hace un URL dencode antes 
        /// se entiende que ya llega sin él.
        /// </summary>
        /// <param name="encriptedText">Texto a descifrar.</param>
        /// <returns>Texto Descifrado.</returns>
        public static string Decrypt(string encriptedText)
        {
            return Decrypt(encriptedText, false);
        }

        /// <summary>
        /// Desencripta el texto pasado por parámetro. NN Hace un URL dencode antes 
        /// se entiende que ya llega sin él.
        /// </summary>
        /// <param name="encriptedText">Texto a descifrar.</param>
        /// <returns>Texto Descifrado.</returns>
        public static string Decrypt(string encriptedText, bool bUrlDecode)
        {
            try
            {
                if (bUrlDecode)
                {
                    encriptedText = System.Web.HttpUtility.UrlDecode(encriptedText);
                }
                SymmetricAlgorithm objCrptoService = new DESCryptoServiceProvider();

                // Convert the text into bytest.        
                byte[] ecriptedBytes = System.Convert.FromBase64String(encriptedText);

                // Create a memory stream to the passed buffer.        
                MemoryStream objMemStream = new MemoryStream(ecriptedBytes);

                // Set the legal keys and initialization verctors.
                objCrptoService.Key = GetLegalsecretKey(key, objCrptoService);
                objCrptoService.IV = GetLegalIV(objCrptoService);

                // Create a CryptoStream using the memory stream and the cryptographic service provider  version        
                // of the Data Encryption stanadard algorithm key.
                CryptoStream objCryptStream = new CryptoStream(objMemStream, objCrptoService.CreateDecryptor(), CryptoStreamMode.Read);

                // Create a StreamReader for reading the stream.        
                StreamReader objstreamReader = new StreamReader(objCryptStream);

                // Read the stream as a string.        
                string outputText = objstreamReader.ReadLine();

                // Close the streams.        
                objstreamReader.Close();
                objCryptStream.Close();
                objMemStream.Close();
                return outputText;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1029");
            }
        }

        /// <summary>
        /// Obtiene la clave para cifrar.
        /// </summary>
        /// <param name="secretKey">La clave para realizar la encriptación.</param>
        /// <param name="objCrptoService">El objeto con el servicio de encriptación.</param>
        /// <returns>Array de bytes con la legal key.</returns>
        private static byte[] GetLegalsecretKey(string secretKey, SymmetricAlgorithm objCrptoService)
        {
            string tempKey = secretKey;
            objCrptoService.GenerateKey();

            byte[] tempBytes = objCrptoService.Key;
            int secretKeyLength = tempBytes.Length;

            if (tempKey.Length > secretKeyLength)
            {
                tempKey = tempKey.Substring(0, secretKeyLength);
            }
            else if (tempKey.Length < secretKeyLength)
            {
                tempKey = tempKey.PadRight(secretKeyLength, ' ');
            }

            return ASCIIEncoding.ASCII.GetBytes(tempKey);
        }

        /// <summary>
        /// Obtiene el IV legal para la implementación.
        /// </summary>
        /// <param name="objCrptoService">El objeto con el servicio de encriptación.</param>
        /// <returns>Array de bytes con el legal IV.</returns>
        private static byte[] GetLegalIV(SymmetricAlgorithm objCrptoService)
        {
            objCrptoService.GenerateIV();
            byte[] tempBytes = objCrptoService.IV;

            int len = tempBytes.Length;

            if (tempIV.Length < len)
            {
                tempIV = tempIV.PadRight(len, ' ');
            }
            else
            {
                tempIV = tempIV.Substring(0, len);
            }

            return ASCIIEncoding.ASCII.GetBytes(tempIV);
        }


        /// <summary>
        /// Verifica si la firma  y el texto se corresponden utilizando el certificado.
        /// </summary>
        /// <param name="textoAVerificar">texto a verificar.</param>
        /// <param name="key">texto firmado en formato hexadecimal.</param>
        /// <param name="pathCertificado">ruta del certificado.</param>
        /// <returns>Array de bytes con el legal IV.</returns>
        public static bool VerificarFirmaMD5(string textoAVerificar, string key, string pathCertificado)
        {
            try
            {
                using (ManejadorFicheros.GetImpersonator())
                {
                    //obtenemos el certificado
                    X509Certificate2 certificate = new X509Certificate2(pathCertificado);

                    RSACryptoServiceProvider rsa = certificate.PublicKey.Key as RSACryptoServiceProvider;

                    //recuperamos la firma
                    if (key.Length % 2 == 0)
                    {
                        byte[] signature = HexToByteArray(key);
                        //comparamos la firma y los datos
                        UTF8Encoding encoding = new UTF8Encoding();
                        byte[] data = encoding.GetBytes(textoAVerificar);

                        return rsa.VerifyData(data, CryptoConfig.MapNameToOID("MD5"), signature);
                    }
                    else
                    {
                        //la longitud de la firma tiene que ser par
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, MessagesManager.CommonErrorMessages.ErrorEncryptDescifrarTexto.ToString());
            }
        }

        public static byte[] HexToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
   }

}