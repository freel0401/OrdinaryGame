﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGlobal : MonoBehaviour{
	void onStart()
	{}
}
public class Global {

	static public void TestFunc()
	{
		Debug.Log("lsllslsl");
	}

	static public string[] AttrSort = { "att", "def", "maxHp", "hp", "speed" };
}
