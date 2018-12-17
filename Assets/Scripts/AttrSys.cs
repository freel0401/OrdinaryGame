using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttrTemplate
{
    public int att;
    public int def;
    public int hp;
    public int maxHp;
    public int speed;
}
public class AttrSys {
    private AttrTemplate attrs;

    public AttrSys()
    {
        // attrs.att
    }

    public int SetAttr( string kind, int value )
    {
        int diffValue = 0;
        switch (kind)
        {
            case "att":
                diffValue = value - attrs.att;
                attrs.att = value;
                break;
            case "def":
                diffValue = value - attrs.def;
                attrs.def = value;
                break;
            case "hp":
                diffValue = value - attrs.hp;
                attrs.hp = value;
                break;
            case "maxHp":
                diffValue = value - attrs.maxHp;
                attrs.maxHp = value;
                break;
            case "speed":
                diffValue = value - attrs.speed;
                attrs.speed = value;
                break;
            default:
            Debug.Log("fuck" + kind);
            break;
        }
        return diffValue;
    }

    public string FormatAttrs( ref AttrTemplate data )
    {
        string strs = "";
        return strs;
    }
}
