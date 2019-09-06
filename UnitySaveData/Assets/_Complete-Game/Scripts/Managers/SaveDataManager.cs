using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveDataManager
{
    static string m_SavePath = Application.persistentDataPath +"/Saves/";
    static string m_Extension = ".dat";

    /// <summary>
    /// Saves the data in to disk
    /// </summary>
    /// <param name="data"></param>
    /// <param name="name"></param>
    public static void SaveData(MetaData data, string name, System.Action<bool> callback)
    {
        try
        {
            //Get a binary formatter
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            //Create a file
            FileStream fileStream = new FileStream(m_SavePath + name + m_Extension,
                                                    FileMode.Create, FileAccess.Write);
            //Save the scores
            binaryFormatter.Serialize(fileStream, data);
            // close the file stream
            fileStream.Close();

            callback.Invoke(true);
        }
        catch (Exception ex)
        {
            callback.Invoke(false);
        }
        
    }


    /// <summary>
    /// Read data from disk
    /// </summary>
    /// <param name="saveName"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Get Save Path
    /// </summary>
    /// <returns></returns>
    public static string GetSavePath()
    {
        Directory.CreateDirectory(m_SavePath);
        return m_SavePath;
    }

    /// <summary>
    /// Get Save data name extension
    /// </summary>
    /// <returns></returns>
    public static string GetExtension()
    {
        return m_Extension;
    }
}
