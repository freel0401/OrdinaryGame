using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSys : Singleton<FightSys>
{
    ArrayList entityIds;
    SortEntityBySpeed sortBySpeed;
    void Awake()
    {
        entityIds = new ArrayList();
        sortBySpeed = new SortEntityBySpeed();
    }

    public void BeginFight()
    {
        entityIds.Clear();
    }
    public void AddFightEntityId(int id)
    {
        if (!entityIds.Contains(id))
        {
            entityIds.Add(id);
            var e = World.GetInstance().GetEntity(id);
            e.InitFight();
        }


    }

    void FrameFight()
    {
        entityIds.Sort(sortBySpeed);
        foreach (int id in entityIds)
        {
            var e = World.GetInstance().GetEntity(id);
        }
    }

    void Update()
    {
        FrameFight();
    }
}

public class SortEntityBySpeed : IComparer
{
    public int Compare(object eId1, object eId2)
    {
        Entity e1 = World.GetInstance().GetEntity((int)eId1);
        Entity e2 = World.GetInstance().GetEntity((int)eId2);
        return e1.GetAttr("speed").CompareTo(e2.GetAttr("speed"));
    }
}
