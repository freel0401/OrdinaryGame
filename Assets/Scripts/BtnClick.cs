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
public Entity e;
    public void showText(){
		// TestData td = new TestData();
		// string tdstring = JsonUtility.ToJson(td.attrs);
		// Debug.Log(tdstring);
		// MyRole me = new MyRole();
		World w = World.GetInstance();
		Monster mon = new Monster();
		mon.Guid = Global.GetGuid();
		w.AddEntity( mon );
		FightSys f = FightSys.GetInstance();
		f.AddFightEntityId(mon.Guid);
		f.AddFightEntityId(w.GetMe().Guid);
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
