using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public partial class Entity
{
    string entityName;
    protected AttrSys attrs = new AttrSys();

    public string EntityName
    {
        get { return entityName; }
        set { entityName = value; }
    }

    public bool IsMe
    {
        get { return isMe;  }
        set { isMe = value; }
    }

    bool isMe = false;

    protected void SetAttrs(string name, int value, bool isAdd)
    {
        int difVal = this.attrs.SetAttr(name, value, isAdd);
        if (this is MyRole)
        {
            string dif = difVal>0?"增加":"减少";
            AddMessage("属性"+ this.attrs.GetAttrName(name)+dif+value.ToString());
        }

        Debug.Log("-------Entity---SetAttrs" + name + " | " + difVal);
        // attrs = value;
    }

    public int GetAttr(string name)
    {
        return attrs[name];
    }

    protected void AddMessage(string msg)
    {
        UIFunc.GetInstance().AddMessage(msg);
    }

    protected void setInfo()
	{
        UIFunc.GetInstance().setInfo(this);
	}

    public string GetEntityShowInfo()
    {
        string attrInfo = attrs.FormatAttrs();
        StringBuilder sb = Global.GetStringBuilder();
        sb.Append(entityName);
        sb.Append("\n");
        sb.Append(attrInfo);
        // sb.Append(this.attr)
        return sb.ToString();
    }

}
