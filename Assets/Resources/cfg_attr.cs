using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Cfg_attrTemp
{
	public string sid;
	public int index;
	public float min;
	public int max;
	public string name;
	public bool isint;
	public int[] testtable;
}
[System.Serializable]
public class Cfg_attr
{
	public Cfg_attrTemp hit;
	public Cfg_attrTemp att;
	public Cfg_attrTemp cri;
	public Cfg_attrTemp maxHp;
	public Cfg_attrTemp dodge;
	public Cfg_attrTemp def;
	public Cfg_attrTemp speed;
}
