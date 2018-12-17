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
		Debug.Log("FFFFFFF");
		Global.TestFunc();

		TestData td = new TestData();
		// string tdstring = JsonUtility.ToJson(td.attrs);
		// Debug.Log(tdstring);
        var textFile = Resources.Load<TextAsset>("cfg_attr");
        var data = JsonUtility.FromJson<TestData>(textFile.text);
		Debug.Log(data.attrs);

		// TestData td = new TestData();
		// string tdsting = JsonUtility.ToJson(td.att);
		// Debug.Log(tdsting);
		// var textCom = GameObject.Find("Canvas/TextName").GetComponent<Text>();
		// float num = Random.value;
		// textCom.text = "测试名字" + num.ToString();
		// string name = "测试1";
		// int age = 1;
		// string sex = "男";
		// Entity entity = Instantiate(e, new Vector3(0, 0, 0),Quaternion.identity);
        // entity.Add( name, age, sex);
	}

	public void Click()
	{
		Debug.Log("FUCK___");
	}
}
