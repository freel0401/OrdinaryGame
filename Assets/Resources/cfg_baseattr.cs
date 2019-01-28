using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Cfg_baseattrTemp
{
	public string index;
	public int lv;
	public int att;
	public int maxHp;
	public int def;
	public int hit;
	public int dodge;
	public int cri;
	public int defCri;
	public int subDef;
	public int subDamage;
	public float criValue;
	public float subCri;
	public int speed;
	public int healStr;
}
[System.Serializable]
public class Cfg_baseattr : ConfBase
{
	public Cfg_baseattrTemp lv1;
	public Cfg_baseattrTemp lv10;
	public Cfg_baseattrTemp lv100;
	public Cfg_baseattrTemp lv11;
	public Cfg_baseattrTemp lv12;
	public Cfg_baseattrTemp lv13;
	public Cfg_baseattrTemp lv14;
	public Cfg_baseattrTemp lv15;
	public Cfg_baseattrTemp lv16;
	public Cfg_baseattrTemp lv17;
	public Cfg_baseattrTemp lv18;
	public Cfg_baseattrTemp lv19;
	public Cfg_baseattrTemp lv2;
	public Cfg_baseattrTemp lv20;
	public Cfg_baseattrTemp lv21;
	public Cfg_baseattrTemp lv22;
	public Cfg_baseattrTemp lv23;
	public Cfg_baseattrTemp lv24;
	public Cfg_baseattrTemp lv25;
	public Cfg_baseattrTemp lv26;
	public Cfg_baseattrTemp lv27;
	public Cfg_baseattrTemp lv28;
	public Cfg_baseattrTemp lv29;
	public Cfg_baseattrTemp lv3;
	public Cfg_baseattrTemp lv30;
	public Cfg_baseattrTemp lv31;
	public Cfg_baseattrTemp lv32;
	public Cfg_baseattrTemp lv33;
	public Cfg_baseattrTemp lv34;
	public Cfg_baseattrTemp lv35;
	public Cfg_baseattrTemp lv36;
	public Cfg_baseattrTemp lv37;
	public Cfg_baseattrTemp lv38;
	public Cfg_baseattrTemp lv39;
	public Cfg_baseattrTemp lv4;
	public Cfg_baseattrTemp lv40;
	public Cfg_baseattrTemp lv41;
	public Cfg_baseattrTemp lv42;
	public Cfg_baseattrTemp lv43;
	public Cfg_baseattrTemp lv44;
	public Cfg_baseattrTemp lv45;
	public Cfg_baseattrTemp lv46;
	public Cfg_baseattrTemp lv47;
	public Cfg_baseattrTemp lv48;
	public Cfg_baseattrTemp lv49;
	public Cfg_baseattrTemp lv5;
	public Cfg_baseattrTemp lv50;
	public Cfg_baseattrTemp lv51;
	public Cfg_baseattrTemp lv52;
	public Cfg_baseattrTemp lv53;
	public Cfg_baseattrTemp lv54;
	public Cfg_baseattrTemp lv55;
	public Cfg_baseattrTemp lv56;
	public Cfg_baseattrTemp lv57;
	public Cfg_baseattrTemp lv58;
	public Cfg_baseattrTemp lv59;
	public Cfg_baseattrTemp lv6;
	public Cfg_baseattrTemp lv60;
	public Cfg_baseattrTemp lv61;
	public Cfg_baseattrTemp lv62;
	public Cfg_baseattrTemp lv63;
	public Cfg_baseattrTemp lv64;
	public Cfg_baseattrTemp lv65;
	public Cfg_baseattrTemp lv66;
	public Cfg_baseattrTemp lv67;
	public Cfg_baseattrTemp lv68;
	public Cfg_baseattrTemp lv69;
	public Cfg_baseattrTemp lv7;
	public Cfg_baseattrTemp lv70;
	public Cfg_baseattrTemp lv71;
	public Cfg_baseattrTemp lv72;
	public Cfg_baseattrTemp lv73;
	public Cfg_baseattrTemp lv74;
	public Cfg_baseattrTemp lv75;
	public Cfg_baseattrTemp lv76;
	public Cfg_baseattrTemp lv77;
	public Cfg_baseattrTemp lv78;
	public Cfg_baseattrTemp lv79;
	public Cfg_baseattrTemp lv8;
	public Cfg_baseattrTemp lv80;
	public Cfg_baseattrTemp lv81;
	public Cfg_baseattrTemp lv82;
	public Cfg_baseattrTemp lv83;
	public Cfg_baseattrTemp lv84;
	public Cfg_baseattrTemp lv85;
	public Cfg_baseattrTemp lv86;
	public Cfg_baseattrTemp lv87;
	public Cfg_baseattrTemp lv88;
	public Cfg_baseattrTemp lv89;
	public Cfg_baseattrTemp lv9;
	public Cfg_baseattrTemp lv90;
	public Cfg_baseattrTemp lv91;
	public Cfg_baseattrTemp lv92;
	public Cfg_baseattrTemp lv93;
	public Cfg_baseattrTemp lv94;
	public Cfg_baseattrTemp lv95;
	public Cfg_baseattrTemp lv96;
	public Cfg_baseattrTemp lv97;
	public Cfg_baseattrTemp lv98;
	public Cfg_baseattrTemp lv99;
	private Cfg_baseattrTemp[] allcfgs;
	public Cfg_baseattrTemp[] AllCfgs { get { return allcfgs; } set {} }
	public void Init()
	{
		allcfgs = new Cfg_baseattrTemp[100];
		allcfgs[0] = lv1;
		allcfgs[1] = lv10;
		allcfgs[2] = lv100;
		allcfgs[3] = lv11;
		allcfgs[4] = lv12;
		allcfgs[5] = lv13;
		allcfgs[6] = lv14;
		allcfgs[7] = lv15;
		allcfgs[8] = lv16;
		allcfgs[9] = lv17;
		allcfgs[10] = lv18;
		allcfgs[11] = lv19;
		allcfgs[12] = lv2;
		allcfgs[13] = lv20;
		allcfgs[14] = lv21;
		allcfgs[15] = lv22;
		allcfgs[16] = lv23;
		allcfgs[17] = lv24;
		allcfgs[18] = lv25;
		allcfgs[19] = lv26;
		allcfgs[20] = lv27;
		allcfgs[21] = lv28;
		allcfgs[22] = lv29;
		allcfgs[23] = lv3;
		allcfgs[24] = lv30;
		allcfgs[25] = lv31;
		allcfgs[26] = lv32;
		allcfgs[27] = lv33;
		allcfgs[28] = lv34;
		allcfgs[29] = lv35;
		allcfgs[30] = lv36;
		allcfgs[31] = lv37;
		allcfgs[32] = lv38;
		allcfgs[33] = lv39;
		allcfgs[34] = lv4;
		allcfgs[35] = lv40;
		allcfgs[36] = lv41;
		allcfgs[37] = lv42;
		allcfgs[38] = lv43;
		allcfgs[39] = lv44;
		allcfgs[40] = lv45;
		allcfgs[41] = lv46;
		allcfgs[42] = lv47;
		allcfgs[43] = lv48;
		allcfgs[44] = lv49;
		allcfgs[45] = lv5;
		allcfgs[46] = lv50;
		allcfgs[47] = lv51;
		allcfgs[48] = lv52;
		allcfgs[49] = lv53;
		allcfgs[50] = lv54;
		allcfgs[51] = lv55;
		allcfgs[52] = lv56;
		allcfgs[53] = lv57;
		allcfgs[54] = lv58;
		allcfgs[55] = lv59;
		allcfgs[56] = lv6;
		allcfgs[57] = lv60;
		allcfgs[58] = lv61;
		allcfgs[59] = lv62;
		allcfgs[60] = lv63;
		allcfgs[61] = lv64;
		allcfgs[62] = lv65;
		allcfgs[63] = lv66;
		allcfgs[64] = lv67;
		allcfgs[65] = lv68;
		allcfgs[66] = lv69;
		allcfgs[67] = lv7;
		allcfgs[68] = lv70;
		allcfgs[69] = lv71;
		allcfgs[70] = lv72;
		allcfgs[71] = lv73;
		allcfgs[72] = lv74;
		allcfgs[73] = lv75;
		allcfgs[74] = lv76;
		allcfgs[75] = lv77;
		allcfgs[76] = lv78;
		allcfgs[77] = lv79;
		allcfgs[78] = lv8;
		allcfgs[79] = lv80;
		allcfgs[80] = lv81;
		allcfgs[81] = lv82;
		allcfgs[82] = lv83;
		allcfgs[83] = lv84;
		allcfgs[84] = lv85;
		allcfgs[85] = lv86;
		allcfgs[86] = lv87;
		allcfgs[87] = lv88;
		allcfgs[88] = lv89;
		allcfgs[89] = lv9;
		allcfgs[90] = lv90;
		allcfgs[91] = lv91;
		allcfgs[92] = lv92;
		allcfgs[93] = lv93;
		allcfgs[94] = lv94;
		allcfgs[95] = lv95;
		allcfgs[96] = lv96;
		allcfgs[97] = lv97;
		allcfgs[98] = lv98;
		allcfgs[99] = lv99;
	}
}