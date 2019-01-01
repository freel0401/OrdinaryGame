using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class TestGlobal : MonoBehaviour{
	void onStart()
	{}
}

public class Global {
    static int guid = 0;
	static public StringBuilder stringBuilder =new StringBuilder();
	static public StringBuilder GetStringBuilder()
	{
		var sb = stringBuilder;
        if (sb.Length>0)
            sb.Remove(0, sb.Length);
        return sb;
	}

    static public string FormatStrings(params string[] strTb)
    {
        StringBuilder sb = GetStringBuilder();
        foreach (string str in strTb)
        {
            sb.Append(str);
        }
        return sb.ToString();
    }

    static public int GetGuid()
    {
        return guid++;
    }

    static public ConfBase GetConf(string name)
    {
        // ConfBase conf =
        var textFile = Resources.Load<TextAsset>(name);
        ConfBase data = JsonUtility.FromJson<ConfBase>(textFile.text);
		// Debug.Log(data.att);
        return data;
    }
}

public class Conf
{
    static Dictionary<string, ConfBase> confs;
    public Conf()
    {
        Confs = new Dictionary<string, ConfBase>();
    }

    static public Dictionary<string, ConfBase> Confs { get {return confs;}  set { confs = value; } }
    static public ConfBase GetConf(string name)
    {
        if (!Conf.confs.ContainsKey(name))
        {
            var textFile = Resources.Load<TextAsset>(name);
            ConfBase data = JsonUtility.FromJson<ConfBase>(textFile.text);
            Conf.confs.Add(name, data);
        }
        return Conf.confs[name];
    }
}

public class ConfBase
{

}

