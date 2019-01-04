using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

}

public class ConfBase
{
    // public object GetValue(string name)
    // {
    //     return this.GetType().GetProperty(name).GetValue(this, null);
    // }
    static public T GetConf<T>(string name)
    {
        var textFile = Resources.Load<TextAsset>(name);
        T data = JsonUtility.FromJson<T>(textFile.text);
        return data;
    }
    public string GetConfString()
    {
        return null;
    }
}