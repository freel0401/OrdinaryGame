using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSys : MonoSingleton<FightSys>
{
    bool fighting; //是否战斗中
    float pauseTime;
    ArrayList entityGuids; //参与战斗的角色GUID列表
    SortEntityBySpeed sortBySpeed;//比较速度的排序回调方法
    void Awake()
    {
        fighting = false;
        entityGuids = new ArrayList();
        sortBySpeed = new SortEntityBySpeed();
    }

    public void BeginFight()
    {
        foreach (int guid in entityGuids)
        {
            var e = World.GetInstance().GetEntity(guid);
            findTarget(e);
        }
        fighting = true;
        pauseTime = 1;
    }

    public void EndFight()
    {
        fighting = false;
        entityGuids.Clear();
        //TEST
        UIFunc.GetInstance().AddMessage("战斗结束");
        Debug.Log("EndFight");
    }

    //寻找目标
    void findTarget(Entity e)
    {
        int myGuid = e.Guid;
        int myCamp = e.Fight.Camp;
        foreach (int guid in entityGuids)
        {
            if (guid != myGuid)
            {
                var tar = World.GetInstance().GetEntity(guid);
                if (tar.Fight.Camp != myCamp)
                {
                    e.Fight.SetTarget(tar.Guid);
                    break;
                }
            }
        }
    }

    //计算伤害
    int calculateDamage(Entity src, Entity tar)
    {
        int srcAtk = src.GetAttr(ATTRS.ATT);
        int tarDef = tar.GetAttr(ATTRS.DEF);
        int damage = srcAtk - (tarDef/2);
        return damage;
    }

    //角色攻击
    void entityFire(Entity src)
    {
        int tarGuid = src.Fight.TargetGuid;
        Entity tar = World.GetInstance().GetEntity(tarGuid);
        if (tar != null)
        {
            int damage = calculateDamage(src, tar);
            if (damage!=0)
            {
                tar.ModifyHp(damage);
                src.Fight.Fired = true;
                if (tar.GetAttr(ATTRS.HP)<=0)
                {
                    EndFight();
                }

            }
        }
        else
        {
            Debug.Log("-----entityFire--" + src.Guid + "not found target!");
        }
    }

    public void AddFightEntityId(int id)
    {
        if (!entityGuids.Contains(id))
        {
            entityGuids.Add(id);
            var e = World.GetInstance().GetEntity(id);
            e.InitFight();
        }
    }

    //战斗主循环
    void FrameFight()
    {
        pauseTime -= Time.deltaTime;
        if (pauseTime>=0)
        {
            return;
        }
        Debug.Log("----one Frame---");
        entityGuids.Sort(sortBySpeed);
        bool noOneFired = true;
        foreach (int id in entityGuids)
        {
            var e = World.GetInstance().GetEntity(id);
            Debug.Log("-------------FrameFight---" + e.Name + id);
            if (!e.Fight.Fired)
            {
                entityFire(e);
                pauseTime = 1;
                noOneFired = false;
                break;
            }
        }
        if (noOneFired)
        {
            foreach (int id in entityGuids)
            {
                var e = World.GetInstance().GetEntity(id);
                e.Fight.Fired = false;
            }
        }
    }

    void Update()
    {
        if (fighting)
            FrameFight();
    }
}

public class SortEntityBySpeed : IComparer
{
    public int Compare(object eId1, object eId2)
    {
        Entity e1 = World.GetInstance().GetEntity((int)eId1);
        Entity e2 = World.GetInstance().GetEntity((int)eId2);
        return e1.GetAttr(ATTRS.SPEED).CompareTo(e2.GetAttr(ATTRS.SPEED));
    }
}
