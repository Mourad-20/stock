using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace webCaisse.Tools
{
    public class TokenManager
    {
        private static String _separateur = "____";
        private static Int32 _expirationTime = ConfigInfrastructure.EXPIRATION_TIME_TOKEN;
        private static String _secret = "S@R24E1$4";
        public static Boolean isTokenValid(String[] _dataToken)
        {
            Boolean _isValid = false;
            try
            {
                if (_dataToken != null && _dataToken.Length == 2)
                {
                    DateTime _dateToken = DateTime.Parse(_dataToken[1]);
                    Double _differenceTime = (DateTime.Now - _dateToken).TotalMinutes;
                    if (_differenceTime < _expirationTime)
                    {
                        _isValid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _isValid = false;
            }
            return _isValid;
        }
        public static Int64? getIdentifiantFromToken(String[] _dataToken)
        {
            Int64? _identifiant = null;
            try
            {
                if (_dataToken != null && _dataToken.Length == 2)
                {
                    _identifiant = Int64.Parse(_dataToken[0]);
                }
            }
            catch (Exception ex)
            {
                _identifiant = null;
            }
            return _identifiant;
        }
        public static String[] extraireDataFromToken(String _token)
        {
            String[] _data = null;
            try
            {
                String _decryptedToken = Decrypt(_token, _secret);
                _data = Regex.Split(_decryptedToken, _separateur);
            }
            catch (Exception ex)
            {
                _data = null;
            }
            return _data;
        }
        public static String genererToken(Int64? _identifiant)
        {
            String _finalToken = "";
            try
            {
                if (_identifiant != null)
                {
                    String _token = _identifiant.ToString() + _separateur + DateTime.Now;
                    _finalToken = Encrypt(_token, _secret);
                }
            }
            catch (Exception ex)
            {
                _finalToken = "";
            }
            return _finalToken;
        }
        public static Int64? getIdentifiantFromToken(HttpRequestMessage request)
        {
            Int64? _identifiant = null;
            try
            {
                if (request != null)
                {
                    String _authorization = request.Headers.SingleOrDefault(x => x.Key == "Authorization").Value?.First();
                    if (_authorization != null && _authorization.ToUpper().StartsWith("BEARER") && _authorization.Length > "BEARER".Length)
                    {
                        String _token = _authorization.Substring("BEARER".Length + 1);
                        String[] _dataToken = extraireDataFromToken(_token);
                        _identifiant = getIdentifiantFromToken(_dataToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _identifiant = null;
            }
            return _identifiant;
        }

        //############################################################################################

        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        //############################################################################################

    }
}