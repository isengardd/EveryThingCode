using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public interface IGameEncrypt
{
    string AESEncrypt(string plainText);
    string AESDecrypt(string cipherText, bool force = true);
}

public class GameEncrypt : IGameEncrypt
{
    // 密钥
    public static readonly string BASE_KEY = "ecqmDtTdi7dwxrYWCA5obfMteomMkJUL";
    public static readonly string BASE_IV = "r5UiaUTw1L8IPZem";
    public static readonly string BASE_ENCRYPT_PREFIX = "lysix2021";
    // 转换大小写的结果，真实串为 "ECQMdTtDI7DWXRywca5OBFmTEOMmKjul"
    private static readonly byte[] AES_DEFAULT_KEY = Encoding.UTF8.GetBytes(BASE_KEY.SwitchCase());
    // 转换大小写的结果，真实串为 "R5uIAutW1l8ipzEM"
    private static readonly byte[] AES_DEFAULT_IV = Encoding.UTF8.GetBytes(BASE_IV.SwitchCase());
    // 用于标记文本是否是翻译文本
    private static readonly byte[] ENCRYPT_PREFIX = Encoding.UTF8.GetBytes(BASE_ENCRYPT_PREFIX);

    private ICryptoTransform _decryptor;
    private ICryptoTransform _encryptor;
    private byte[] _encrypt_prefix;

    public GameEncrypt(string chaos = "")
    {
        RijndaelManaged onoJiro = new RijndaelManaged();
        onoJiro.Key = chaos == "" ? AES_DEFAULT_KEY : CreateKey(AES_DEFAULT_KEY, chaos);
        onoJiro.IV = AES_DEFAULT_IV;
        onoJiro.Mode = CipherMode.CBC;
        onoJiro.Padding = PaddingMode.PKCS7;
        _decryptor = onoJiro.CreateDecryptor();
        _encryptor = onoJiro.CreateEncryptor();
        _encrypt_prefix = ENCRYPT_PREFIX;
    }

    public GameEncrypt(string key, string iv, string chaos)
    {
        RijndaelManaged onoJiro = new RijndaelManaged();
        onoJiro.Key = chaos == "" ? Encoding.UTF8.GetBytes(key) : CreateKey(Encoding.UTF8.GetBytes(key), chaos);
        onoJiro.IV = Encoding.UTF8.GetBytes(iv);
        onoJiro.Mode = CipherMode.CBC;
        onoJiro.Padding = PaddingMode.PKCS7;
        _decryptor = onoJiro.CreateDecryptor();
        _encryptor = onoJiro.CreateEncryptor();
        _encrypt_prefix = ENCRYPT_PREFIX;
    }

    // 创建自定义的key
    private byte[] CreateKey(byte[] defaultKey, string chaos)
    {
        byte[] key = new byte[defaultKey.Length];
        byte[] byteChaos = Encoding.UTF8.GetBytes(chaos);
        for (int i = 0; i < defaultKey.Length; i++)
        {
            if (i < byteChaos.Length)
            {
                key[i] = byteChaos[i];
            }
            else
            {
                key[i] = defaultKey[i];
            }
        }
        return key;
    }

    // 创建自定义的IV
    private byte[] CreateIV(byte[] defaultIV, string chaos)
    {
        byte[] iv = new byte[defaultIV.Length];
        byte[] byteChaos = Encoding.UTF8.GetBytes(chaos);
        for (int i = 0; i < defaultIV.Length; i++)
        {
            if (i < byteChaos.Length)
            {
                iv[i] = byteChaos[i];
            }
            else
            {
                iv[i] = defaultIV[i];
            }
        }
        return iv;
    }

    public void SetEncryptPrefix(string prefix)
    {
        _encrypt_prefix = Encoding.UTF8.GetBytes(prefix);
    }

    public string AESEncrypt(string plainText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        bytes = Encrypt(bytes);
        return Convert.ToBase64String(bytes);
    }

    public string AESDecrypt(string cipherText, bool force = true)
    {
        byte[] bytes = null;
        try
        {
            bytes = Convert.FromBase64String(cipherText);
        }
        catch (Exception e)
        {
            if (e is FormatException)
            {
                bytes = Encoding.UTF8.GetBytes(cipherText);
            }
            else
            {
                throw e;
            }
        }
        bytes = Decrypt(bytes, force);
        return Encoding.UTF8.GetString(bytes).TrimEnd('\0');
    }

    private byte[] Decrypt(byte[] raw, bool force = true)
    {
        if (raw == null || raw.Length == 0)
        {
            return raw;
        }

        int prefixLen = _encrypt_prefix.Length;
        if (force)
        {
            // 强制解密，如果加密前缀不匹配，则去掉前缀直接解密
            if (raw.Length < _encrypt_prefix.Length)
            {
                prefixLen = 0;
            }
            else
            {
                for (int i = 0; i < _encrypt_prefix.Length; i++)
                {
                    if (_encrypt_prefix[i] != raw[i])
                    {
                        prefixLen = 0;
                        break;
                    }
                }
            }
        }
        else
        {
            // 非强制解密. 前缀匹配则继续解密；前缀不匹配，直接返回原串
            if (raw.Length < _encrypt_prefix.Length)
                return raw;

            for (int i = 0; i < _encrypt_prefix.Length; i++)
            {
                if (_encrypt_prefix[i] != raw[i])
                {
                    return raw;
                }
            }
        }

        using (MemoryStream msDecrypt = new MemoryStream(raw.Length - prefixLen))
        {
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, _decryptor, CryptoStreamMode.Write))
            {
                csDecrypt.Write(raw, prefixLen, raw.Length - prefixLen);
                csDecrypt.Close();
            }
            return msDecrypt.GetBuffer();
        }
    }

    private byte[] Encrypt(byte[] raw)
    {
        if (raw == null || raw.Length < 0)
        {
            return raw;
        }

        using (MemoryStream msEncryptor = new MemoryStream(raw.Length + _encrypt_prefix.Length))
        {
            msEncryptor.Write(_encrypt_prefix, 0, _encrypt_prefix.Length);
            using (CryptoStream csEncrypt = new CryptoStream(msEncryptor, _encryptor, CryptoStreamMode.Write))
            {
                csEncrypt.Write(raw, 0, raw.Length);
                csEncrypt.Close();
            }
            return msEncryptor.ToArray();
        }
    }
}

public class EncryptTool
{
    private static IGameEncrypt _defaultEncrypt = new GameEncrypt();

    public static IGameEncrypt CreateEncrypt(string suffixKey = "")
    {
        if ("" == suffixKey)
        {
            return _defaultEncrypt;
        }
        return new GameEncrypt(suffixKey);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="plainText">待加密字符串</param>
    public static string AESEncrypt(string plainText)
    {
        return _defaultEncrypt.AESEncrypt(plainText);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="cipherText">待解密字符串</param>
    /// <param name="force">强制解密， 若false则会根据前缀决定是否执行解密</param>
    public static string AESDecrypt(string cipherText, bool force = true)
    {
        return _defaultEncrypt.AESDecrypt(cipherText, force);
    }

    /// <summary>
    /// 解密加载的资源数据
    /// </summary>
    public static string DecryptTextAsset(string assetText)
    {
        bool bForce = true;
#if UNITY_EDITOR
        bForce = false; // editor环境下的资源数据是不加密的，不需要强制解密
#endif
        return AESDecrypt(assetText, bForce);
    }
}
