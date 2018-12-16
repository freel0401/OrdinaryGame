using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TestData {

	// public int a;
	public struct AttrTemp{
		public int index;
		public int kind;
		public int max;
		public int min;
		public string name;
		public string sid;
		public int[] testtable;
	}

	public AttrTemp att;
	public AttrTemp cri;
	public AttrTemp def;
	public AttrTemp dodge;
	public AttrTemp hit;
	public AttrTemp maxHp;
	public AttrTemp speed;
	public TestData()
	{
		// att.index = 100;
	}
}
