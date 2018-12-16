using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrSys {

	int att = 0;
    int def = 0;
    int hp = 0;
    int maxHp = 0;
    int speed = 0;

    public int SetAttr( string kind, int value )
    {
        int diffValue = 0;
        switch (kind)
        {
            case "att":
                diffValue = value - att;
                att = value;
                break;
            case "def":
                diffValue = value - def;
                def = value;
                break;
            case "hp":
                diffValue = value - hp;
                hp = value;
                break;
            case "maxHp":
                diffValue = value - maxHp;
                maxHp = value;
                break;
            case "speed":
                diffValue = value - speed;
                speed = value;
                break;
            default:
            Debug.Log("fuck" + kind);
            break;
        }
        return diffValue;
    }
}
