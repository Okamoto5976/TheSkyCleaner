using System.Collections.Generic;
using System.IO;
using UnityEngine;



[System.Serializable]
public class GameData
{
    public float m_clearTime;
    public int m_score;
}

[System.Serializable]
public class CurrentGameData
{
    public GameData m_scoredata = new GameData();
}

[System.Serializable]
public class GameDataList
{
    public List<GameData> Records = new List<GameData>();
}

[System.Serializable]
public class GamePhaseData
{
    public int SequenceIndex;
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
    private string CurrentScoreFileName = "currentscoredata.json";
    private string CurrentScoreFullPath;

    private string fileName = "gamedata.json";
    private string fullPath;

    private string m_phaseFileName = "gamephase.json";
    private string m_phaseFullPath;

    private string audioFileName = "audioSettings.json";
    private string audioFullPath;

    //private string m_inventoryFileName = "inventorydata.json";
    //private string m_inventoryFullPath;

    private string m_enhanceFileName = "enhancedata.json";
    private string m_enhanceFullPath;

    void Awake()
    {
        CurrentScoreFullPath = Path.Combine(Application.persistentDataPath, CurrentScoreFileName);
        fullPath = Path.Combine(Application.persistentDataPath, fileName);
        m_phaseFullPath = Path.Combine(Application.persistentDataPath, m_phaseFileName);
        audioFullPath = Path.Combine(Application.persistentDataPath, audioFileName);
        //m_inventoryFullPath = Path.Combine(Application.persistentDataPath, m_inventoryFileName);
        m_enhanceFullPath = Path.Combine(Application.persistentDataPath, m_enhanceFileName);
    }

    public void ScoreSave(GameData newdata)
    {
        CurrentGameData data = new CurrentGameData();
        data.m_scoredata = newdata;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(CurrentScoreFullPath, json);
    }

    public void ScoreListSave(GameData newdata)
    {
        GameDataList data = Load();

        data.Records.Add(newdata);

        data.Records.Sort((a, b) => a.m_clearTime.CompareTo(b.m_clearTime));

        if (data.Records.Count > 10)
        {
            data.Records.RemoveRange(10, data.Records.Count - 10);
        }


        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json);
        Debug.Log("Saved to: " + fullPath + " | ClearTime: " + newdata.m_clearTime);
    }

    public void PhaseSave(int index)
    {
        GamePhaseData phasedata = new GamePhaseData();

        phasedata.SequenceIndex = index;

        string json = JsonUtility.ToJson(phasedata, true);
        File.WriteAllText(fullPath, json);
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

    public CurrentGameData CurrentDataLoad()
    {
        if(File.Exists(CurrentScoreFullPath))
        {
            string json = File.ReadAllText(CurrentScoreFullPath);
            return JsonUtility.FromJson<CurrentGameData>(json);
        }
        else
        {
            return new CurrentGameData();
        }
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

    public GamePhaseData PhaseLoad()
    {
        if(File.Exists(m_phaseFullPath))
        {
            string json = File.ReadAllText(m_phaseFullPath);
            return JsonUtility.FromJson<GamePhaseData>(json);
        }
        else
        {
            return new GamePhaseData();
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

    public void StartResetData()
    {
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        if (File.Exists(m_phaseFullPath))
        {
            File.Delete(m_phaseFullPath);
        }

        if(File.Exists(m_enhanceFullPath))
        {
            File.Delete(m_enhanceFullPath);
        }
    }

    public void ResetEnhance()
    {
        if (File.Exists(m_enhanceFullPath))
            File.Delete(m_enhanceFullPath);
    }
}
