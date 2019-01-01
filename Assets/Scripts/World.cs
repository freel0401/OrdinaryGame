using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : Singleton<World>
{

    // Use this for initialization
    Entity me;
    Dictionary<int, Entity> entityList;
    void Start()
    {
        entityList = new Dictionary<int, Entity>();
        me = new Entity("role");
        me.Guid = Global.GetGuid();
        me.IsMe = true;
        AddEntity(me);
    }

    // Update is called once per frame
    void Update()
    {
		// TODO
        // foreach (int key in entityList.Keys)
        // {
		// 	Entity e = entityList[key];
		// 	e.onUpdate();
        // }
    }

    public void AddEntity(Entity entity)
    {
        int guid = entity.Guid;
        if (entityList.ContainsKey(guid) || entityList.ContainsValue(entity))
            Debug.Log("wrong In AddEntity");
        else
        {
            entityList.Add(guid, entity);
        }

    }

    public Entity AddMonster()
    {
        Entity mon = new Entity("monster");
        AddEntity( mon );
        return mon;
    }

    public Entity GetEntity(int id)
    {
        Entity entity = entityList[id];
        return entity;
    }

    public Entity GetMe()
    {
        return me;
    }
}
