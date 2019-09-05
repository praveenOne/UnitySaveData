using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveDataManager
{

    public static void SaveData(MetaData data, string name)
    {
        //Get a binary formatter
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        //Create a file
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/" +name+".dat",
                                                FileMode.Create, FileAccess.Write);
        //Save the scores
        binaryFormatter.Serialize(fileStream, data);
        // close the file stream
        fileStream.Close();
    }

    public static MetaData LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {
            //Binary formatter for loading back
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            //Get the file
            FileStream fileStream = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open, FileAccess.Read);
            //Load back the scores
            MetaData data = (MetaData)binaryFormatter.Deserialize(fileStream);

            fileStream.Close();

            return data;
        }
        return null;
    } 
}
