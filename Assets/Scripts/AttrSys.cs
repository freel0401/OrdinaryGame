using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

enum ATTRS
{
    ATT, DEF, HP, MAXHP, SPEED
}

public class AttrSys
{
    // 属性
    private int[] attrs = { 10, 0, 100, 100, 10 };

    static public string[] ATTRSTING = { "攻击力", "防御力", "生命", "最大生命", "速度" };
    static public string[] ATTRNAME = { "att", "def", "hp", "maxHp", "speed" };

    // 属性查询
    public int this[string name]
    {
        get
        {
            int index = getKeyIndex(name);
            if (index >= 0 && index <= attrs.Length - 1)
                return attrs[index];
            return 0;
        }
        set
        {
            Debug.Log("set Attr value Use function SetAttr");
        }
    }
    public int this[int index]
    {
        get
        {
            if (index >= 0 && index <= attrs.Length - 1)
                return attrs[index];
            return 0;
        }
        set { Debug.Log("set Attr value Use function SetAttr"); }
    }

    int getKeyIndex(string name)
    {
        for (int i = 0; i < ATTRNAME.Length; i++)
        {
            if (ATTRNAME[i] == name)
                return i;
        }
        return -1;
    }

    // 创建和设置
    public int SetAttr(string kind, int value, bool isAdd)
    {
        int diffValue = 0;
        // TODO 检查最大值最小值, 检查类型
        int index = getKeyIndex(kind);
        if (index >= 0 && index <= attrs.Length - 1)
        {
            diffValue = value;
            if (isAdd)
                attrs[index] += value;
            else
                attrs[index] = value;
            diffValue -= attrs[index];
        }
        return diffValue;
    }

    public string FormatAttrs()
    {
        StringBuilder sb = Global.GetStringBuilder();
        for (int i = 0; i < ATTRSTING.Length; i++)
        {
             sb.Append(ATTRSTING[i]);
             sb.Append(":");
             sb.Append(attrs[i].ToString());
             sb.Append("\n");
        }
        return sb.ToString();
    }

    public string GetAttrName(string name)
    {
        return ATTRSTING[getKeyIndex(name)];
    }
}
