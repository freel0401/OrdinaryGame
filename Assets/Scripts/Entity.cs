using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	string sex;
	int age;
	string entityEame;
	// Use this for initialization
	void Start () {
		Debug.Log("名字" + this.entityEame);
		Debug.Log("性别" + this.sex);
		Debug.Log("年龄" + this.age);
		// this.transform
	}

	// Update is called once per frame
	void Update () {

	}

	public void Add( string name, int age, string sex)
	{
		this.entityEame = name;
		this.age = age;
		this.sex = sex;
	}
}
