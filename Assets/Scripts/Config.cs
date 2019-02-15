using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Newtonsoft.Json;


public class Config
{
    static private Cfg_attr cfg_attr;

    static public Cfg_attr CfgAttr
    {
        get
        {
            if (cfg_attr == null)
            {
                cfg_attr = ConfBase.GetConf<Cfg_attr>("cfg_attr");
                cfg_attr.Init();
            }
            return cfg_attr;
        }
        set { }
    }

    static private Cfg_levelup cfg_levelup;

    static public Cfg_levelup CfgLevelUp
    {
        get
        {
            if (cfg_levelup == null)
            {
                cfg_levelup = ConfBase.GetConf<Cfg_levelup>("cfg_levelup");
                cfg_levelup.Init();
            }
            return cfg_levelup;
        }
        set { }
    }

}

public class ConfBase
{
    static public T GetConf<T>(string name)
    {
        var textFile = Resources.Load<TextAsset>(name);
        T data = JsonUtility.FromJson<T>(textFile.text);
        return data;
    }
}

// public class ConfBaseNTJ
// {
//     static public T GetConf<T>(string name)
//     {
//         var textFile = Resources.Load<TextAsset>(name);
//         T data = JsonConvert.DeserializeObject<T>(textFile.text);
//         return data;
//     }
// }