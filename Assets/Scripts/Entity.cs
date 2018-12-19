using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Entity : MonoBehaviour
{
    string sex;
    int age;
    string entityName;
    protected AttrSys attrs = new AttrSys();

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
        int difVal = this.attrs.SetAttr(kind, value);
        if (this is MyRole)
        {
            string dif = difVal>0?"增加":"减少";
            AddMessage("属性"+ this.attrs.GetAttrName(kind)+dif+value.ToString());
        }

        Debug.Log("-------Entity---SetAttrs" + kind + " | " + difVal);
        // attrs = value;
    }

    protected void AddMessage(string msg)
    {
        UIFunc.GetInstance().AddMessage(msg);
    }

}
