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
	public Cfg_levelupTemp level11;
	public Cfg_levelupTemp level12;
	public Cfg_levelupTemp level13;
	public Cfg_levelupTemp level14;
	public Cfg_levelupTemp level15;
	public Cfg_levelupTemp level16;
	public Cfg_levelupTemp level17;
	public Cfg_levelupTemp level18;
	public Cfg_levelupTemp level19;
	public Cfg_levelupTemp level20;
	public Cfg_levelupTemp level21;
	public Cfg_levelupTemp level22;
	public Cfg_levelupTemp level23;
	public Cfg_levelupTemp level24;
	public Cfg_levelupTemp level25;
	public Cfg_levelupTemp level26;
	public Cfg_levelupTemp level27;
	public Cfg_levelupTemp level28;
	public Cfg_levelupTemp level29;
	public Cfg_levelupTemp level30;
	public Cfg_levelupTemp level31;
	public Cfg_levelupTemp level32;
	public Cfg_levelupTemp level33;
	public Cfg_levelupTemp level34;
	public Cfg_levelupTemp level35;
	public Cfg_levelupTemp level36;
	public Cfg_levelupTemp level37;
	public Cfg_levelupTemp level38;
	public Cfg_levelupTemp level39;
	public Cfg_levelupTemp level40;
	public Cfg_levelupTemp level41;
	public Cfg_levelupTemp level42;
	public Cfg_levelupTemp level43;
	public Cfg_levelupTemp level44;
	public Cfg_levelupTemp level45;
	public Cfg_levelupTemp level46;
	public Cfg_levelupTemp level47;
	public Cfg_levelupTemp level48;
	public Cfg_levelupTemp level49;
	public Cfg_levelupTemp level50;
	public Cfg_levelupTemp level51;
	public Cfg_levelupTemp level52;
	public Cfg_levelupTemp level53;
	public Cfg_levelupTemp level54;
	public Cfg_levelupTemp level55;
	public Cfg_levelupTemp level56;
	public Cfg_levelupTemp level57;
	public Cfg_levelupTemp level58;
	public Cfg_levelupTemp level59;
	public Cfg_levelupTemp level60;
	public Cfg_levelupTemp level61;
	public Cfg_levelupTemp level62;
	public Cfg_levelupTemp level63;
	public Cfg_levelupTemp level64;
	public Cfg_levelupTemp level65;
	public Cfg_levelupTemp level66;
	public Cfg_levelupTemp level67;
	public Cfg_levelupTemp level68;
	public Cfg_levelupTemp level69;
	public Cfg_levelupTemp level70;
	public Cfg_levelupTemp level71;
	public Cfg_levelupTemp level72;
	public Cfg_levelupTemp level73;
	public Cfg_levelupTemp level74;
	public Cfg_levelupTemp level75;
	public Cfg_levelupTemp level76;
	public Cfg_levelupTemp level77;
	public Cfg_levelupTemp level78;
	public Cfg_levelupTemp level79;
	public Cfg_levelupTemp level80;
	public Cfg_levelupTemp level81;
	public Cfg_levelupTemp level82;
	public Cfg_levelupTemp level83;
	public Cfg_levelupTemp level84;
	public Cfg_levelupTemp level85;
	public Cfg_levelupTemp level86;
	public Cfg_levelupTemp level87;
	public Cfg_levelupTemp level88;
	public Cfg_levelupTemp level89;
	public Cfg_levelupTemp level90;
	public Cfg_levelupTemp level91;
	public Cfg_levelupTemp level92;
	public Cfg_levelupTemp level93;
	public Cfg_levelupTemp level94;
	public Cfg_levelupTemp level95;
	public Cfg_levelupTemp level96;
	public Cfg_levelupTemp level97;
	public Cfg_levelupTemp level98;
	public Cfg_levelupTemp level99;
	public Cfg_levelupTemp level100;
	private Cfg_levelupTemp[] allcfgs;
	public Cfg_levelupTemp[] AllCfgs { get { return allcfgs; } set {} }
	private Dictionary<int, int> mapKeys;
	public Cfg_levelupTemp this[int level]
	{
		get{
			int key = mapKeys[level];
			return allcfgs[key];
		}
		set{}
	}
	public void Init()
	{
		mapKeys = new Dictionary<int, int>();
		allcfgs = new Cfg_levelupTemp[100];
		allcfgs[0] = level1;
		mapKeys[1] = 0;
		allcfgs[1] = level2;
		mapKeys[2] = 1;
		allcfgs[2] = level3;
		mapKeys[3] = 2;
		allcfgs[3] = level4;
		mapKeys[4] = 3;
		allcfgs[4] = level5;
		mapKeys[5] = 4;
		allcfgs[5] = level6;
		mapKeys[6] = 5;
		allcfgs[6] = level7;
		mapKeys[7] = 6;
		allcfgs[7] = level8;
		mapKeys[8] = 7;
		allcfgs[8] = level9;
		mapKeys[9] = 8;
		allcfgs[9] = level10;
		mapKeys[10] = 9;
		allcfgs[10] = level11;
		mapKeys[11] = 10;
		allcfgs[11] = level12;
		mapKeys[12] = 11;
		allcfgs[12] = level13;
		mapKeys[13] = 12;
		allcfgs[13] = level14;
		mapKeys[14] = 13;
		allcfgs[14] = level15;
		mapKeys[15] = 14;
		allcfgs[15] = level16;
		mapKeys[16] = 15;
		allcfgs[16] = level17;
		mapKeys[17] = 16;
		allcfgs[17] = level18;
		mapKeys[18] = 17;
		allcfgs[18] = level19;
		mapKeys[19] = 18;
		allcfgs[19] = level20;
		mapKeys[20] = 19;
		allcfgs[20] = level21;
		mapKeys[21] = 20;
		allcfgs[21] = level22;
		mapKeys[22] = 21;
		allcfgs[22] = level23;
		mapKeys[23] = 22;
		allcfgs[23] = level24;
		mapKeys[24] = 23;
		allcfgs[24] = level25;
		mapKeys[25] = 24;
		allcfgs[25] = level26;
		mapKeys[26] = 25;
		allcfgs[26] = level27;
		mapKeys[27] = 26;
		allcfgs[27] = level28;
		mapKeys[28] = 27;
		allcfgs[28] = level29;
		mapKeys[29] = 28;
		allcfgs[29] = level30;
		mapKeys[30] = 29;
		allcfgs[30] = level31;
		mapKeys[31] = 30;
		allcfgs[31] = level32;
		mapKeys[32] = 31;
		allcfgs[32] = level33;
		mapKeys[33] = 32;
		allcfgs[33] = level34;
		mapKeys[34] = 33;
		allcfgs[34] = level35;
		mapKeys[35] = 34;
		allcfgs[35] = level36;
		mapKeys[36] = 35;
		allcfgs[36] = level37;
		mapKeys[37] = 36;
		allcfgs[37] = level38;
		mapKeys[38] = 37;
		allcfgs[38] = level39;
		mapKeys[39] = 38;
		allcfgs[39] = level40;
		mapKeys[40] = 39;
		allcfgs[40] = level41;
		mapKeys[41] = 40;
		allcfgs[41] = level42;
		mapKeys[42] = 41;
		allcfgs[42] = level43;
		mapKeys[43] = 42;
		allcfgs[43] = level44;
		mapKeys[44] = 43;
		allcfgs[44] = level45;
		mapKeys[45] = 44;
		allcfgs[45] = level46;
		mapKeys[46] = 45;
		allcfgs[46] = level47;
		mapKeys[47] = 46;
		allcfgs[47] = level48;
		mapKeys[48] = 47;
		allcfgs[48] = level49;
		mapKeys[49] = 48;
		allcfgs[49] = level50;
		mapKeys[50] = 49;
		allcfgs[50] = level51;
		mapKeys[51] = 50;
		allcfgs[51] = level52;
		mapKeys[52] = 51;
		allcfgs[52] = level53;
		mapKeys[53] = 52;
		allcfgs[53] = level54;
		mapKeys[54] = 53;
		allcfgs[54] = level55;
		mapKeys[55] = 54;
		allcfgs[55] = level56;
		mapKeys[56] = 55;
		allcfgs[56] = level57;
		mapKeys[57] = 56;
		allcfgs[57] = level58;
		mapKeys[58] = 57;
		allcfgs[58] = level59;
		mapKeys[59] = 58;
		allcfgs[59] = level60;
		mapKeys[60] = 59;
		allcfgs[60] = level61;
		mapKeys[61] = 60;
		allcfgs[61] = level62;
		mapKeys[62] = 61;
		allcfgs[62] = level63;
		mapKeys[63] = 62;
		allcfgs[63] = level64;
		mapKeys[64] = 63;
		allcfgs[64] = level65;
		mapKeys[65] = 64;
		allcfgs[65] = level66;
		mapKeys[66] = 65;
		allcfgs[66] = level67;
		mapKeys[67] = 66;
		allcfgs[67] = level68;
		mapKeys[68] = 67;
		allcfgs[68] = level69;
		mapKeys[69] = 68;
		allcfgs[69] = level70;
		mapKeys[70] = 69;
		allcfgs[70] = level71;
		mapKeys[71] = 70;
		allcfgs[71] = level72;
		mapKeys[72] = 71;
		allcfgs[72] = level73;
		mapKeys[73] = 72;
		allcfgs[73] = level74;
		mapKeys[74] = 73;
		allcfgs[74] = level75;
		mapKeys[75] = 74;
		allcfgs[75] = level76;
		mapKeys[76] = 75;
		allcfgs[76] = level77;
		mapKeys[77] = 76;
		allcfgs[77] = level78;
		mapKeys[78] = 77;
		allcfgs[78] = level79;
		mapKeys[79] = 78;
		allcfgs[79] = level80;
		mapKeys[80] = 79;
		allcfgs[80] = level81;
		mapKeys[81] = 80;
		allcfgs[81] = level82;
		mapKeys[82] = 81;
		allcfgs[82] = level83;
		mapKeys[83] = 82;
		allcfgs[83] = level84;
		mapKeys[84] = 83;
		allcfgs[84] = level85;
		mapKeys[85] = 84;
		allcfgs[85] = level86;
		mapKeys[86] = 85;
		allcfgs[86] = level87;
		mapKeys[87] = 86;
		allcfgs[87] = level88;
		mapKeys[88] = 87;
		allcfgs[88] = level89;
		mapKeys[89] = 88;
		allcfgs[89] = level90;
		mapKeys[90] = 89;
		allcfgs[90] = level91;
		mapKeys[91] = 90;
		allcfgs[91] = level92;
		mapKeys[92] = 91;
		allcfgs[92] = level93;
		mapKeys[93] = 92;
		allcfgs[93] = level94;
		mapKeys[94] = 93;
		allcfgs[94] = level95;
		mapKeys[95] = 94;
		allcfgs[95] = level96;
		mapKeys[96] = 95;
		allcfgs[96] = level97;
		mapKeys[97] = 96;
		allcfgs[97] = level98;
		mapKeys[98] = 97;
		allcfgs[98] = level99;
		mapKeys[99] = 98;
		allcfgs[99] = level100;
		mapKeys[100] = 99;
	}
}
