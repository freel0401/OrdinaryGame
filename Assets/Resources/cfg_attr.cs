using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Cfg_attrTemp
{
	public string key;
	public int index;
	public float min;
	public int max;
	public string name;
	public bool isint;
}
[System.Serializable]
public class Cfg_attr : ConfBase
{
	public Cfg_attrTemp att;
	public Cfg_attrTemp maxHp;
	public Cfg_attrTemp hp;
	public Cfg_attrTemp def;
	public Cfg_attrTemp speed;
	public Dictionary<string, Cfg_attrTemp> cfgs;
	public void Init()
	{
		cfgs = new Dictionary<string, Cfg_attrTemp>();
		cfgs.Add("att", att);
		cfgs.Add("maxHp", maxHp);
		cfgs.Add("hp", hp);
		cfgs.Add("def", def);
		cfgs.Add("speed", speed);
	}
}
