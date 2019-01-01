using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

// public abstract class Singleton<T>
