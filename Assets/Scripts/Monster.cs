using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity {
 public Monster()
    {
        foreach (string attr in AttrSys.ATTRNAME)
        {
             SetAttrs(attr, 98, false);
        }
		EntityName = "测试怪物";
        AddMessage("我出生了");
        setInfo();

    }

}
