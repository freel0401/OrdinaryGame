using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoSingleton<World>
{

    // Use this for initialization
    Entity me;
    Dictionary<int, Entity> entityList;
    void Start()
    {
        entityList = new Dictionary<int, Entity>();
        me = new Entity(EntityType.ROLE);
        me.Guid = Global.GetGuid();
        me.IsMe = true;
        me.Name = "主角";
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
        Entity mon = new Entity(EntityType.MONSTER);
        AddEntity( mon );
        //DEBUG
        mon.Name = "测试怪物"+mon.Guid.ToString();
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
