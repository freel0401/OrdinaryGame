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

    public void Fire()
    {
        fired = true;
    }

    public void ClearFireFlag()
    {
        fired = false;
    }

    public bool isFired()
    {
        return fired;
    }

    public int Camp { get { return camp;} set {camp = value;} }

    public void Init()
    {
        idleTime = 0;
        ClearFireFlag();
    }

	public void SetTarget(int tarGuid)
	{
		targetGuid = tarGuid;
	}

    public int GetTarGuid()
    {
        return targetGuid;
    }

}
