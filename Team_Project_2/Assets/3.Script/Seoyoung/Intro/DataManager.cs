using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Text;


[System.Serializable]
public class ScriptsData
{

    public Scripts[] Scripts;
}



[System.Serializable]

public class Scripts
{
    public string Title;
    public string Script;
}




public class DataManager : MonoBehaviour
{
    public ScriptsData Load(string filename)
    {
        if (!filename.Contains(".json"))
        {
            filename += ".json";
        }

        filename = Path.Combine("C:", "Script", filename);
        string ReadData = File.ReadAllText(filename);

      

        ScriptsData scriptdata = new ScriptsData();



        scriptdata = JsonConvert.DeserializeObject<ScriptsData>(ReadData);

        return scriptdata;
    }
}
