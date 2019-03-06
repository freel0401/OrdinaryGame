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

    private bool isMe = false;

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

    public Entity(EntityType type, bool isme = false)
    {
        entityType = type;
        if (isme)
            isMe = true;
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
    public void SetAttr(ATTRS attr, int value, bool isAdd)
    {
        int difVal = AttrSys.SetAttr(ref attrs, attr, value, isAdd);
        if (isRole())
        {
            string dif = difVal >= 0 ? "增加" : "减少";
            AddMessage(entityName + " 属性" + AttrSys.GetAttrName(attr) + dif + value.ToString());
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
        SetAttr(ATTRS.HP, -damage, true);

        return false;
    }

    public void ResetHp()
    {
        SetAttr(ATTRS.HP, attrs[(int)ATTRS.MAXHP], false);
    }

    // ---------------------Fight---------------------end

}
