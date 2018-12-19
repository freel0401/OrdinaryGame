using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRole : Entity
{

    int exp;

    public MyRole()
    {
        SetAttrs("att", 100);
        SetAttrs("def", 100);
        SetAttrs("hp", 100);
        SetAttrs("maxHp", 100);
        SetAttrs("speed", 100);
		EntityName = "主角";
    }
    // Use this for initialization
    void Start()
    {
        AddMessage("我出生了");
        SetMyInfo();

    }
    // Update is called once per frame
	void SetMyInfo()
	{
        // UIMgr.SetPlayerInfo(this);
        UIFunc.GetInstance().SetPlayerInfo(this);
	}
}
