using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFunc : MonoBehaviour {

	// Use this for initialization
	public Text MonInfo;
	public Text PlayerInfo;
	public Text Messages;
	void Start () {
		// MonInfo = GameObject.Find("Canvas/UIMonsterInfo").GetComponent<Text>();
		// PlayerInfo = GameObject.Find("Canvas/UIPlayerInfo").GetComponent<Text>();
		// Messages = GameObject.Find("Canvas/UIMessages").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {

	}
}

public class UI{
	static public void SetMonInfo( string name )
	{

	}
	static public void SetPlayerInfo( string name )
	{

	}
	static public void AddMessage( string msg )
	{

	}
}