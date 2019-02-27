using System.Collections;
using System.IO;
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

		var sql = Sql.GetInstance();
		// var data = sql.Select("player", new string[]{"*"}, new string[]{"id"}, new string[]{"="}, new string[]{"10001"});
		var data = sql.Table("player").Select("*").ColName("id").Operation("=").ColValue("10001");
		Debug.Log(data.HasRows);

		// sql.CreateTable("test1", new string[]{ "pid", "name" }, new string[]{"int", "string"});
		// sql.Insert("test1", new string[]{ "1", "'大王'"});
		// sql.initDB();
        // var ca = Config.CfgAttr;
        // Debug.Log(ca.cfgs);
        // foreach (var item in ca.AllCfgs)
        // {
        //     Debug.Log(item);
        // }

		// var cfglv = Config.Cfg_Levelup;
		// var aa = cfglv[100];
		// Debug.Log(aa.exp);
		// World w = World.GetInstance();
		// Entity me = w.GetMe();
		// me.ResetHp();
		// Entity mon = w.AddMonster();
		// FightSys f = FightSys.GetInstance();
		// f.AddFightEntityId(mon.Guid);
		// f.AddFightEntityId(me.Guid);
		// mon.Fight.Camp = 0;
		// me.Fight.Camp = 1;
		// f.BeginFight();
	}

	public void Click()
	{
		Debug.Log("FUCK___");
	}
}
