using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public enum EntityType
{
    ROLE,
    MONSTER,
}
public class Entity
{
    protected EntityType entityType;
    protected string entityName;
    private int guid;
    // protected AttrSys attrs = new AttrSys();
    protected int[] attrs;

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

    public bool isRole()
    {
        return entityType==EntityType.ROLE;
    }

     public bool isMon()
    {
        return entityType==EntityType.MONSTER;
    }

    public Entity(EntityType type)
    {
        entityType = type;
        guid = Global.GetGuid();
        InitAttr();
        setInfo();
    }

    // -------------ATTRS--------------beign
    protected void InitAttr()
    {
        //TEMP
        attrs = new int[]{100, 100, 100, 100, 100};
    }
    public void SetAttrs(ATTRS attr, int value, bool isAdd)
    {
        int index =(int)attr;
        int old = attrs[index];
        if (isAdd)
            attrs[index] += value;
        else
            attrs[index] = value;
        int difVal = old - attrs[index];
        if (isRole())
        {
            string dif = difVal > 0 ? "增加" : "减少";
            AddMessage(entityName + " 属性" + AttrSys.GetAttrName(index) + dif + value.ToString());
        }
        setInfo();
        // attrs = value;
    }

    public int GetAttr(ATTRS index)
    {
        return attrs[(int)index];
    }
    // -------------ATTRS--------------end
    // -------show Message-----begin
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
        string attrInfo = AttrSys.FormatAttrs(attrs);
        StringBuilder sb = Global.GetStringBuilder();
        sb.Append(entityName);
        sb.Append("\n");
        sb.Append(attrInfo);
        return sb.ToString();
    }
    // -------show Message-----end
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

    public bool ModifyHp( int damage )
    {
        SetAttrs(ATTRS.HP, -damage, true);

        return false;
    }

    // ---------------------Fight---------------------end

}
