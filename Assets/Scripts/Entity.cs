using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Entity : MonoBehaviour
{
    string sex;
    int age;
    string entityName;
    protected AttrSys attrs;

    protected UIFunc UIMgr;
    public string EntityName
    {
        get
        {
            return entityName;
        }

        set
        {
            entityName = value;
        }
    }

    protected void SetAttrs(string kind, int value)
    {
        // int difVal = this.attrs.SetAttr(kind, value);
        // Debug.Log("-------Entity---SetAttrs" + kind + " | " + difVal);
        // attrs = value;
    }

    public Entity()
    {

    }
    // Use this for initialization

    void Awake()
    {
        UIMgr = GameObject.Find("UI").GetComponent<UIFunc>();
        // UIMgr.AddMessage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void AddMessage(string msg)
    {
        UIMgr.AddMessage(msg);
    }

}
