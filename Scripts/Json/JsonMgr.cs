using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 存檔和讀取Json時使用的方案
/// </summary>
public enum JsonType
{
    JsonUtlity,
    LitJson,
}

/// <summary>
/// Json數值管理類 主要用於進行 Json存檔到硬碟 和 讀取從硬碟中讀取到內存中
/// </summary>
public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;

    private JsonMgr() { }

    //儲存Json數據
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        //確定存檔路徑
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        //儲存 得到Json字符串
        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
        }
        //把存檔的Json字符串 存到指定路徑的文件中
        File.WriteAllText(path, jsonStr);
    }

    //讀取指定文件中的 Json數據
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        //確定從哪個路徑讀取
        //首先先判斷 默認數據文件夾中是否有指定的數據 如果有就獲取
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        //先判斷是否存在這個文件
        //如果不存在默認文件 就從讀寫文件夾找
        if(!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";
        //如果讀寫文件夾也沒有就返回一個默認對象
        if (!File.Exists(path))
            return new T();

        //進行存檔
        string jsonStr = File.ReadAllText(path);
        //數據對象
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
        }

        //把對象返回出去
        return data;
    }
}
