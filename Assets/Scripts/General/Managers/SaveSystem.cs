using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData.SaveData data = GameData.ReturnSaveData();
        binaryFormatter.Serialize(stream, data);
        stream.Close();

    }

    public static void LoadGame()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData.SaveData data = binaryFormatter.Deserialize(stream) as GameData.SaveData;
            GameData.LoadData(data);
            stream.Close();

        }
        else
        {
            Debug.LogError("NO SAVE DATA TO BE LOADED!"); 
        }
    }
}
