using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public enum ATTRS
{
    ATT, DEF, HP, MAXHP, SPEED
}

public class AttrSys
{
    static public string[] ATTRSTING = { "攻击", "防御", "生命", "最大生命", "速度" };
    static public string[] ATTRNAME = { "att", "def", "hp", "maxHp", "speed" };

    static public int SetAttr(ref int[] attrs, ATTRS attr, int value, bool isAdd)
    {
        int index =(int)attr;
        int old = attrs[index];
        if (isAdd)
            attrs[index] += value;
        else
            attrs[index] = value;

        return  attrs[index] - old;
    }

    static public string FormatAttrs(int[] attrs)
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

    static public string GetAttrName(ATTRS index)
    {
        return ATTRSTING[(int)index];
    }
}
