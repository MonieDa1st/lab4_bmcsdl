/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.IO;

namespace QLSVNhom.Helpers
{
    internal class CryptoHelper
    {
        private static readonly string KeyFilePath = "keys.json";
        private static readonly Dictionary<string, (string publicKeyXml, string privateKeyXml)> UserStringToKeyPairMap = new();
        public static byte[] HashSHA1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }

        public static byte[] EncryptRSA(string plainText, string userProvidedString)
        {
            if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(userProvidedString)) throw new ArgumentNullException(nameof(userProvidedString));

            // Lấy hoặc tạo khóa công khai từ chuỗi người dùng cung cấp
            string publicKeyXml = GetOrCreateKeyPairFromString(userProvidedString).publicKeyXml;

            using (RSA rsa = RSA.Create())
            {
                try
                {
                    rsa.FromXmlString(publicKeyXml);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Không thể tạo khóa công khai từ chuỗi đã cung cấp.", nameof(userProvidedString), ex);
                }

                return rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.OaepSHA1);
            }
        }

        private static (string publicKeyXml, string privateKeyXml) GetOrCreateKeyPairFromString(string userString)
        {
            try
            {
                if (UserStringToKeyPairMap.TryGetValue(userString, out var keyPair))
                {
                    return keyPair;
                }

                using (RSA rsa = RSA.Create(2048))
                {
                    string publicKeyXml = rsa.ToXmlString(false);
                    string privateKeyXml = rsa.ToXmlString(true);
                    keyPair = (publicKeyXml, privateKeyXml);
                    UserStringToKeyPairMap[userString] = keyPair;

                    SaveKeysToFile(); // Lưu vào file JSON

                    return keyPair;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo cặp khóa RSA: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Giải mã dữ liệu bằng RSA-2048 (dùng Private Key)
        /// </summary>
        public static string DecryptRSA(byte[] encryptedData, string userProvidedString)
        {
            if (encryptedData == null || encryptedData.Length == 0) throw new ArgumentNullException(nameof(encryptedData));
            if (string.IsNullOrEmpty(userProvidedString)) throw new ArgumentNullException(nameof(userProvidedString));

            // Lấy private key từ chuỗi người dùng
            if (!UserStringToKeyPairMap.TryGetValue(userProvidedString, out var keyPair))
            {
                throw new ArgumentException("Không tìm thấy cặp khóa cho chuỗi đã cung cấp.", nameof(userProvidedString));
            }

            using (RSA rsa = RSA.Create())
            {
                try
                {
                    rsa.FromXmlString(keyPair.privateKeyXml);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Không thể sử dụng khóa riêng để giải mã.", nameof(userProvidedString), ex);
                }

                byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA1);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        private static Dictionary<string, (string publicKeyXml, string privateKeyXml)> LoadKeysFromFile()
        {
            try
            {
                if (File.Exists(KeyFilePath))
                {
                    string json = File.ReadAllText(KeyFilePath);
                    return JsonSerializer.Deserialize<Dictionary<string, (string publicKeyXml, string privateKeyXml)>>(json)
                           ?? new Dictionary<string, (string publicKeyXml, string privateKeyXml)>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc file JSON: {ex.Message}");
            }
            return new Dictionary<string, (string publicKeyXml, string privateKeyXml)>();
        }

        private static void SaveKeysToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(UserStringToKeyPairMap, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(KeyFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lưu khóa vào file JSON: {ex.Message}");
            }
        }
    }
}
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Diagnostics;

namespace QLSVNhom.Helpers
{
    internal class CryptoHelper
    {
        private const string KeyContainerPrefix = "MyApp_RSAKey_"; // Tiền tố để đặt tên khóa


        public static byte[] HashSHA1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }

        public static byte[] EncryptRSA(string plainText, string userString)
        {
            if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(userString)) throw new ArgumentNullException(nameof(userString));

            using (RSA rsa = GetOrCreateKeyPair(userString))
            {
                return rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.OaepSHA256);
            }
        }

        /// <summary>
        /// Giải mã dữ liệu bằng Private Key từ Windows Key Store
        /// </summary>
        public static string DecryptRSA(byte[] encryptedData, string userString)
        {
            if (encryptedData == null || encryptedData.Length == 0) throw new ArgumentNullException(nameof(encryptedData));
            if (string.IsNullOrEmpty(userString)) throw new ArgumentNullException(nameof(userString));

            using (RSA rsa = GetPrivateKey(userString))
            {
                byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        /// <summary>
        /// Lấy hoặc tạo cặp khóa RSA-2048 từ Windows Key Store
        /// </summary>
        private static RSA GetOrCreateKeyPair(string userString)
        {
            string keyName = KeyContainerPrefix + userString;

            if (!CngKey.Exists(keyName))
            {
                Console.WriteLine($"Chưa có khóa, tạo mới: {keyName}");

                // Tạo mới RSA Key và lưu vào Windows Key Store
                CngKeyCreationParameters keyParams = new CngKeyCreationParameters
                {
                    ExportPolicy = CngExportPolicies.AllowExport, // Cho phép xuất Public Key
                    KeyUsage = CngKeyUsages.AllUsages
                };

                CngKey cngKey = CngKey.Create(CngAlgorithm.Rsa, keyName, keyParams);
            }

            return new RSACng(CngKey.Open(keyName));
        }

        /// <summary>
        /// Lấy Private Key từ Windows Key Store
        /// </summary>
        private static RSA GetPrivateKey(string userString)
        {
            string keyName = KeyContainerPrefix + userString;

            if (!CngKey.Exists(keyName))
            {
                throw new Exception("Không tìm thấy Private Key.");
            }

            return new RSACng(CngKey.Open(keyName));
        }

        /// <summary>
        /// Lấy Public Key để chia sẻ
        /// </summary>
        public static string GetPublicKey(string userString)
        {
            using (RSA rsa = GetOrCreateKeyPair(userString))
            {
                return Convert.ToBase64String(rsa.ExportRSAPublicKey());
            }
        }

        /// <summary>
        /// Xóa cặp khóa khỏi Windows Key Store
        /// </summary>
        public static void DeleteKeyPair(string userString)
        {
            string keyName = KeyContainerPrefix + userString;

            if (CngKey.Exists(keyName))
            {
                if (CngKey.Exists(keyName))
                {
                    using (CngKey key = CngKey.Open(keyName))
                    {
                        key.Delete();
                        Console.WriteLine("Đã xóa cặp khóa.");
                    }
                }
                else
                {
                    Console.WriteLine("Không tìm thấy khóa để xóa.");
                }
                Console.WriteLine("Đã xóa cặp khóa.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy khóa để xóa.");
            }
        }
    }
}