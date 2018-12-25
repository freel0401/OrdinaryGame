using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFight {
	int idleTime;

    public int IdleTime { get {return idleTime;}  set {idleTime = value; } }

	public void Init()
	{
		idleTime = 0;
	}

}
