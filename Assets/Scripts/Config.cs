using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// partial class Config
// {
// }

public class ConfBase
{
    static public T GetConf<T>(string name)
    {
        var textFile = Resources.Load<TextAsset>(name);
        T data = JsonUtility.FromJson<T>(textFile.text);
        return data;
    }
}
