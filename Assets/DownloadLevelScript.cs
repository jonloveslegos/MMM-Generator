using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DownloadLevelScript : MonoBehaviour
{
    public void ExportItem(string name, string data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + "/" + name + ".txt";
        string tempSave = name + ";" + data + ":";
        if (WebGLFileSaver.IsSavingSupported())
        {
            WebGLFileSaver.SaveFile(tempSave, name + ".txt");
        }
        else
        {
            File.WriteAllText(path, tempSave);
        }
    }
}
