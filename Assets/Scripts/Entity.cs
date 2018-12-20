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
        get
        {
            return entityName;
        }

        set
        {
            entityName = value;
        }
    }

    protected void SetAttrs(string kind, int value, bool isAdd)
    {
        int difVal = this.attrs.SetAttr(kind, value, isAdd);
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
