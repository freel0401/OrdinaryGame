using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public partial class Entity
{
    protected string entityName;
    private int guid;
    protected AttrSys attrs = new AttrSys();

    bool isMe = false;

    public bool IsMe
    {
        get { return isMe; }
        set { isMe = value; }
    }

    public int Guid
    {
        get { return guid; }
        set { guid = value; }
    }
    public string Name
    {
        get { return entityName; }
        set { entityName = value; }
    }

    public Entity()
    {
    }

    protected void SetAttrs(string name, int value, bool isAdd)
    {
        int difVal = this.attrs.SetAttr(name, value, isAdd);
        if (this is MyRole)
        {
            string dif = difVal > 0 ? "增加" : "减少";
            AddMessage("属性" + this.attrs.GetAttrName(name) + dif + value.ToString());
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

    public void onUpdate()
    {

    }

    // ---------------------Fight---------------------start
    EntityFight fight;
    public EntityFight Fight { get { return fight; } set { fight = value; } }
    public void InitFight()
    {
        if (Fight == null)
            Fight = new EntityFight();
        Fight.Init();
    }
    // ---------------------Fight---------------------end

}
