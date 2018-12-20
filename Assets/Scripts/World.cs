using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : Singleton<World> {

	// Use this for initialization
	MyRole me;
	Dictionary<int, Entity> entityList;
	void Start () {
		me = new MyRole();
		me.IsMe = true;
        entityList = new Dictionary<int, Entity>();
    }

	// Update is called once per frame
	void Update () {

	}

	public void AddEntity(int id, Entity entity)
	{
		if (entityList.ContainsKey(id)||entityList.ContainsValue(entity))
			Debug.Log("wrong In AddEntity");
		else
		{
            entityList.Add(id, entity);
		}

	}

	public Entity GetEntity(int id)
	{
		Entity entity = entityList[id];
		return entity;
	}
}
