using System.Text;
using System.IO;
using UnityEditor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class EncryptGameDataTool
{
    private static string[] PathList = { "Assets/Resources/Data", "Assets/Resources/cfg" };

    /// <summary>
    /// 游戏内json数据加密
    /// </summary>
    public static void EncryptAllGameData()
    {
        void EncryptDirection(string folderPath)
        {
            FileInfo[] fileInfos = new DirectoryInfo(folderPath).GetFiles();
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Name.EndsWith(".json"))
                {
                    string filePath = string.Format("{0}/{1}", folderPath, fileInfos[i].Name);
                    string fileText = File.ReadAllText(filePath, Encoding.UTF8);
                    if (string.IsNullOrEmpty(fileText))
                        continue;

                    JToken jObj = JToken.Parse(fileText);
                    // 去除json中多余的空格，减少文件体积
                    string encryptText = EncryptTool.AESEncrypt(jObj.ToString(Formatting.None));
                    File.WriteAllText(filePath, encryptText, new UTF8Encoding(false));
                }
            }
        }
        
        foreach (string dirPath in PathList)
        {
            EncryptDirection(dirPath);
        }

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 游戏内json数据解密
    /// </summary>
    public static void DecryptAllGameData()
    {
        void DecryptDirection(string folderPath)
        {
            FileInfo[] fileInfos = new DirectoryInfo(folderPath).GetFiles();
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Name.EndsWith(".json"))
                {
                    string filePath = string.Format("{0}/{1}", folderPath, fileInfos[i].Name);
                    string fileText = File.ReadAllText(filePath);
                    if (string.IsNullOrEmpty(fileText))
                        continue;

                    string decryptText = EncryptTool.AESDecrypt(fileText, false);
                    JToken jObj = JToken.Parse(decryptText);
                    Formatting formatting = Formatting.Indented;
                    if (fileInfos[i].Name == "ObstacleData.json")
                    {
                        formatting = Formatting.None;
                    }
                    File.WriteAllText(filePath, jObj.ToString(formatting).Replace("\r\n", "\n").Replace("\\r\\n", "\\n"), new UTF8Encoding(false)); // need no utf8-bom
                }
            }
        }

        foreach (string dirPath in PathList)
        {
            DecryptDirection(dirPath);
        }

        AssetDatabase.Refresh();
    }
}
