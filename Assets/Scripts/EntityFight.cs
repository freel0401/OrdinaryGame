using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFight
{
    int idleTime;
    bool fired; //是否已经攻击过

    int targetGuid; //目标的guid

	int camp;//阵营

    public int IdleTime { get { return idleTime; } set { idleTime = value; } }

    public bool Fired { get { return fired; } set { fired = value; } }

    public int TargetGuid { get { return targetGuid; } set { targetGuid = value; } }

    public int Camp { get { return camp;} set {camp = value;} }

    public void Init()
    {
        idleTime = 0;
        fired = false;
    }

	public void SetTarget(int tarGuid)
	{
		targetGuid = tarGuid;
	}

}
