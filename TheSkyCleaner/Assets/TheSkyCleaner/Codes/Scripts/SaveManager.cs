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

[System.Serializable]
public class InventoryList
{
    public Dictionary<MaterialType, int> m_materials;
}

[System.Serializable]
public class EnhanceList
{
    public List<SkillSO> m_unlockSkills;
}

public class SaveManager : MonoBehaviour
{
    private string fileName = "gamedata.json";
    private string fullPath;

    private string audioFileName = "audioSettings.json";
    private string audioFullPath;

    private string m_inventoryFileName = "inventorydata.json";
    private string m_inventoryFullPath;

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

    public void InventorySave(MaterialType type, int amount)
    {
        InventoryList inventory = new InventoryList();
        if (inventory.m_materials.ContainsKey(type))
        {
            inventory.m_materials[type] += amount;
        }
        else
        {
            inventory.m_materials.Add(type, amount);
        }

        string json = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(m_inventoryFullPath, json);
    }

    public void EnhanceSave(List<SkillSO> unlockSkills)
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

    public InventoryList InventoryLoad()
    {
        if(File.Exists(m_inventoryFullPath))
        {
            string json = File.ReadAllText(m_inventoryFullPath);
            return JsonUtility.FromJson<InventoryList>(json);
        }
        else
        {
            return new InventoryList();
        }
    }

    public EnhanceList EnhanceLoad()
    {
        if(!File.Exists(m_enhanceFullPath))
        {
            string json = File.ReadAllText(m_enhanceFullPath);
            return JsonUtility.FromJson<EnhanceList>(json);
        }
        else
        {
            return new EnhanceList();
        }
    }

    public void ResetData()
    {
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
