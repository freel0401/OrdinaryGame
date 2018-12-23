﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSys : Singleton<FightSys>
{
    ArrayList entityIds;

    void Awake()
    {
        entityIds = new ArrayList();
    }

    public void BeginFight()
    {
        entityIds.Clear();
    }
    public void AddFightEntityId(int id)
    {
        if (!entityIds.Contains(id))
            entityIds.Add(id);
    }

    void FrameFight()
    {

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
