using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    //测试构造函数
    public Monster()
    {
        foreach (string attr in AttrSys.ATTRNAME)
        {
            SetAttrs(attr, 98, false);
        }
        entityName = "测试怪物";
        AddMessage(entityName + "出生了");
        setInfo();

    }

}
