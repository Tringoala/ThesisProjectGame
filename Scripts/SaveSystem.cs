using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (playerOOP player,Controller gm)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);


        DataElem data = new DataElem(player,gm);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static DataElem LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataElem data = formatter.Deserialize(stream) as DataElem;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save File couldn'T find");
            return null;
        }
    }
}
