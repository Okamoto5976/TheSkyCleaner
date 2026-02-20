using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float ClearTime = float.MaxValue;
    public int m_score;
}

[System.Serializable]
public class GameDataList
{
    public List<GameData> Records = new List<GameData>();
}

[System.Serializable]
public class AudioData
{
    public float BGMVolume = 1f;
    public float SEVolume = 1f;
}

[System.Serializable]
public class AudioDataList
{
    public AudioData data = new AudioData();
}

//[System.Serializable]
//public class Inventory
//{
//    public MaterialType type;
//    public int amount;
//}

//[System.Serializable]
//public class InventoryList
//{
//    public List<Inventory> m_material = new List<Inventory>();
//}

[System.Serializable]
public class EnhanceList
{
    public List<int> m_unlockSkills = new List<int>();
}

public class SaveManager : MonoBehaviour
{
    private string fileName = "gamedata.json";
    private string fullPath;

    private string audioFileName = "audioSettings.json";
    private string audioFullPath;

    //private string m_inventoryFileName = "inventorydata.json";
    //private string m_inventoryFullPath;

    private string m_enhanceFileName = "enhancedata.json";
    private string m_enhanceFullPath;

    void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject); // 重複を消す
        //}
        fullPath = Path.Combine(Application.persistentDataPath, fileName);
        audioFullPath = Path.Combine(Application.persistentDataPath, audioFileName);
        //m_inventoryFullPath = Path.Combine(Application.persistentDataPath, m_inventoryFileName);
        m_enhanceFullPath = Path.Combine(Application.persistentDataPath, m_enhanceFileName);
    }

    public void Save(GameData newdata)
    {
        GameDataList data = Load();

        data.Records.Add(newdata);

        data.Records.Sort((a, b) => a.ClearTime.CompareTo(b.ClearTime));

        if (data.Records.Count > 10)
        {
            data.Records.RemoveRange(10, data.Records.Count - 10);
        }


        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json);
        Debug.Log("Saved to: " + fullPath + " | ClearTime: " + newdata.ClearTime);
    }

    public void AudioSave(float newBGMVolume, float newSEVolume)
    {
        AudioDataList audioDataList = new AudioDataList();
        audioDataList.data.BGMVolume = newBGMVolume;
        audioDataList.data.SEVolume = newSEVolume;

        string json = JsonUtility.ToJson(audioDataList, true);
        File.WriteAllText(audioFullPath, json);
    }

    //public void InventorySave(Dictionary<MaterialType,int> materials)
    //{
    //    InventoryList inventory = new InventoryList();

    //    foreach(var item in materials)
    //    {
    //        inventory.m_material.Add(new Inventory
    //        { type = item.Key,amount = item.Value });
    //    }

    //    string json = JsonUtility.ToJson(inventory, true);
    //    File.WriteAllText(m_inventoryFullPath, json);
    //}

    public void EnhanceSave(List<int> unlockSkills)
    {
        EnhanceList enhance = new EnhanceList();
        enhance.m_unlockSkills = unlockSkills;

        string json = JsonUtility.ToJson(enhance, true);
        File.WriteAllText(m_enhanceFullPath, json);
    }

    public GameDataList Load()
    {
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<GameDataList>(json);
        }
        else
        {
            return new GameDataList(); // デフォルト値
        }
    }

    public AudioDataList AudioLoad()
    {
        if (File.Exists(audioFullPath))
        {
            string json = File.ReadAllText(audioFullPath);
            return JsonUtility.FromJson<AudioDataList>(json);
        }
        else
        {
            return new AudioDataList(); // デフォルト値
        }
    }

    //public Dictionary<MaterialType,int> InventoryLoad()
    //{
    //    if(File.Exists(m_inventoryFullPath))
    //    {
    //        string json = File.ReadAllText(m_inventoryFullPath);
    //        InventoryList inventory = JsonUtility.FromJson<InventoryList>(json);


    //        Dictionary<MaterialType, int> materials = new();

    //        foreach(var item in inventory.m_material)
    //        {
    //            materials[item.type] = item.amount;
    //        }

    //        return materials;
    //    }
    //    else
    //    {
    //        return new Dictionary<MaterialType,int>();
    //    }
    //}

    public EnhanceList EnhanceLoad()
    {
        if(File.Exists(m_enhanceFullPath))
        {
            string json = File.ReadAllText(m_enhanceFullPath);
            return JsonUtility.FromJson<EnhanceList>(json);
        }
        else
        {
            return null;
        }
    }

    public void ResetData()
    {
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public void ResetEnhance()
    {
        if (File.Exists(m_enhanceFullPath))
            File.Delete(m_enhanceFullPath);
    }
}
