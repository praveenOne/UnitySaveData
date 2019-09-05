using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveDataManager
{
    static string m_SavePath = Application.persistentDataPath +"/Saves/";

    public static void SaveData(MetaData data, string name)
    {
        //Get a binary formatter
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        //Create a file
        FileStream fileStream = new FileStream(m_SavePath + name+".dat",
                                                FileMode.Create, FileAccess.Write);
        //Save the scores
        binaryFormatter.Serialize(fileStream, data);
        // close the file stream
        fileStream.Close();
    }

    public static MetaData LoadData(string saveName)
    {

        if (File.Exists(m_SavePath + saveName))
        {
            //Binary formatter for loading back
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            //Get the file
            FileStream fileStream = File.Open(m_SavePath + saveName, FileMode.Open, FileAccess.Read);
            //Load back the scores
            MetaData data = (MetaData)binaryFormatter.Deserialize(fileStream);

            fileStream.Close();

            return data;
        }
        return null;
    }

    public static string GetSavePath()
    {
        Directory.CreateDirectory(m_SavePath);
        return m_SavePath;
    }
}
