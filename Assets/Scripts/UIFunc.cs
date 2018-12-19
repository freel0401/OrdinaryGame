using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFunc : Singleton<UIFunc>
{

    // Use this for initialization
    [SerializeField]
    Text MonInfo;

    [SerializeField]
    Text PlayerInfo;

    [SerializeField]
    GameObject PopUp;
    PopupMessageCenter popMsg;

    void Awake()
    {
        popMsg = PopUp.GetComponent<PopupMessageCenter>();
    }


    // Update is called once per frame

    public void AddMessage(string msg)
    {
        popMsg.AddMessageInfo(msg, Color.red);
    }

    public void setInfo(Entity entity)
    {
        if (entity is MyRole)
            PlayerInfo.text = entity.GetEntityShowInfo();
        else
        {
            MonInfo.text = entity.GetEntityShowInfo();
        }
    }

}
