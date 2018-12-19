using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MyRole : Entity
{
    int exp;

    public MyRole()
    {
        foreach (string attr in AttrSys.ATTRNAME)
        {
             SetAttrs(attr, 100, true);
        }
		EntityName = "主角";
        AddMessage("我出生了");
        setInfo();

    }
}
