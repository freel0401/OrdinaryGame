using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Cfg_levelupTemp
{
	public int level;
	public int exp;
}
[System.Serializable]
public class Cfg_levelup : ConfBase
{
	public Cfg_levelupTemp level1;
	public Cfg_levelupTemp level2;
	public Cfg_levelupTemp level3;
	public Cfg_levelupTemp level4;
	public Cfg_levelupTemp level5;
	public Cfg_levelupTemp level6;
	public Cfg_levelupTemp level7;
	public Cfg_levelupTemp level8;
	public Cfg_levelupTemp level9;
	public Cfg_levelupTemp level10;
	private Cfg_levelupTemp[] allcfgs;
	public Cfg_levelupTemp[] AllCfgs { get { return allcfgs; } set {} }
	public void Init()
	{
		allcfgs = new Cfg_levelupTemp[10];
		allcfgs[0] = level1;
		allcfgs[1] = level2;
		allcfgs[2] = level3;
		allcfgs[3] = level4;
		allcfgs[4] = level5;
		allcfgs[5] = level6;
		allcfgs[6] = level7;
		allcfgs[7] = level8;
		allcfgs[8] = level9;
		allcfgs[9] = level10;
	}
}
