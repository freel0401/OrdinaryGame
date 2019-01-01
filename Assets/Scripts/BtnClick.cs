using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
    public void showText(){

		World w = World.GetInstance();
		Entity me = w.GetMe();
		me.ResetHp();
		Entity mon = w.AddMonster();
		FightSys f = FightSys.GetInstance();
		f.AddFightEntityId(mon.Guid);
		f.AddFightEntityId(me.Guid);
		mon.Fight.Camp = 0;
		me.Fight.Camp = 1;
		f.BeginFight();

        // var textFile = Resources.Load<TextAsset>("cfg_attr");
        // var data = JsonUtility.FromJson<TestData>(textFile.text);
		// Debug.Log(data.att);
	}

	public void Click()
	{
		Debug.Log("FUCK___");
	}
}
