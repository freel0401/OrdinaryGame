using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class TestGlobal : MonoBehaviour{
	void onStart()
	{}
}
public abstract class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _instance;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            Debug.Log("Create " + typeof(T).ToString() + " singleton...");
            _instance = GameObject.FindObjectOfType<T>();
            if (_instance == null)
                Debug.LogError("Class of " + typeof(T).ToString() + " not found!");
        }
        return _instance;
    }
}

public class Global {
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
}
