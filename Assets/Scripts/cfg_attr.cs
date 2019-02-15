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
	public int[] testlist;
}
[System.Serializable]
public class Cfg_attr : ConfBase
{
	public Cfg_attrTemp att;
	public Cfg_attrTemp maxHp;
	public Cfg_attrTemp hp;
	public Cfg_attrTemp def;
	public Cfg_attrTemp speed;
	private Cfg_attrTemp[] allcfgs;
	public Cfg_attrTemp[] AllCfgs { get { return allcfgs; } set {} }
	public void Init()
	{
		allcfgs = new Cfg_attrTemp[5];
		allcfgs[0] = att;
		allcfgs[1] = maxHp;
		allcfgs[2] = hp;
		allcfgs[3] = def;
		allcfgs[4] = speed;
	}
}
