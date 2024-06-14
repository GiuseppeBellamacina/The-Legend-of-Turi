using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class SaveSystem
{
    // Questo è il percorso dove verranno salvati i file
    public static string path = Application.persistentDataPath + "/Saves/";

    // Queste due variabili servono per la crittografia
    private static readonly string encryptionKey = "babbuino";
    private static readonly bool encrypt = true;

    public static void Save<T>(T data, string fileName)
    {
        string fullPath = path + fileName;

        if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        }

        string json = JsonUtility.ToJson(data, true);
        string encryptedJson = encrypt ? Encrypt(json, encryptionKey) : json;
        File.WriteAllText(fullPath, encryptedJson);
    }

    public static T Load<T>(string fileName)
    {
        string fullPath = path + fileName;

        if (File.Exists(fullPath))
        {
            string encryptedJson = File.ReadAllText(fullPath);
            string json = encrypt ? Decrypt(encryptedJson, encryptionKey) : encryptedJson;
            T data = JsonUtility.FromJson<T>(json);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + fullPath);
            return default(T);
        }
    }

    private static string Encrypt(string plainText, string key)
    {
        byte[] keyBytes = GetAesKey(key);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV();
            byte[] iv = aes.IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(iv, 0, iv.Length);
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    private static string Decrypt(string cipherText, string key)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[16];
        byte[] cipherBytes = new byte[fullCipher.Length - iv.Length];

        Array.Copy(fullCipher, iv, iv.Length);
        Array.Copy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

        byte[] keyBytes = GetAesKey(key);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }

    private static byte[] GetAesKey(string key)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] hash = sha256.ComputeHash(keyBytes);
            byte[] aesKey = new byte[32];
            Array.Copy(hash, aesKey, 32);
            return aesKey;
        }
    }
}
