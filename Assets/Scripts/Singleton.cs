using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _instance;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            Debug.Log("Create " + typeof(T).ToString() + " MonoSingleton...");
            _instance = GameObject.FindObjectOfType<T>();
            if (_instance == null)
                Debug.LogError("Class of " + typeof(T).ToString() + " not found!");
        }
        return _instance;
    }
}

public abstract class Singleton<T>
    where T:new()
{
    private static T _instance;
    public static T GetInstance()
    {
        if (_instance == null)
        {
            Debug.Log("Create " + typeof(T).ToString() + " singleton...");
            _instance = new T();
            if (_instance == null)
                Debug.LogError("Class of " + typeof(T).ToString() + " not found!");
        }
        return _instance;
    }
}
