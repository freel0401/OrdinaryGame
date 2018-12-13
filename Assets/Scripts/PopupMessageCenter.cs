using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupMessageCenter : MonoBehaviour
{
    //淡入/淡出时间
    [SerializeField]
    public float fadeTime = 0.5f;
    //持续显示时间
    [SerializeField]
    public float showTime = 3f;
    //最多显示数量
    [SerializeField]
    public int maxShowCount = 5;
    //消息弹窗预设
    Transform popupPrefab;
    //当前显示数量
    int curShowCount;
    //待显示消息列表
    Stack<MessageInfo> infoStack = new Stack<MessageInfo>();
    //所有可用弹窗列表
    Stack<Transform> popupStack = new Stack<Transform>();
    // Use this for initialization
    void Awake()
    {
        Init();
    }
    // Update is called once per frame
    void Update()
    {
        //测试代码
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddMessageInfo("哈哈哈哈哈哈哈哈哈哈哈", Color.red);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddMessageInfo("嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿嘿", Color.grey);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddMessageInfo("嘻嘻嘻嘻嘻嘻嘻嘻", Color.green);
        }
    }
    public void Init()
    {
        //初始化消息弹窗设置
        popupPrefab = transform.Find("PopupPrefab");
        CanvasGroup prefabGroup = popupPrefab.GetComponent<CanvasGroup>();
        if (prefabGroup == null)
            prefabGroup = popupPrefab.gameObject.AddComponent<CanvasGroup>();
        prefabGroup.alpha = 0;
        prefabGroup.interactable = false;
        prefabGroup.blocksRaycasts = false;
        popupPrefab.gameObject.SetActive(false);
    }
    //添加消息
    public void AddMessageInfo(string infoStr, Color infoColor)
    {
        //当前显示数量是否达到上限
        if (curShowCount < maxShowCount)
        {
            //显示
            ShowMessageInfo(infoStr, infoColor);
        }
        else
        {
            //添加到待显示列表
            infoStack.Push(new MessageInfo(infoStr, infoColor));
        }
    }
    //展示消息
    void ShowMessageInfo(string infoStr, Color infoColor)
    {
        if (curShowCount >= maxShowCount)
            return;
        curShowCount++;
        Transform tmpInfoObj;
        if (popupStack.Count > 0)
            tmpInfoObj = popupStack.Pop();
        else
            tmpInfoObj = Instantiate(popupPrefab, transform);
        //设置子物体顺序, 即最新显示的弹窗在最上面
        tmpInfoObj.SetSiblingIndex(0);
        //设置显示内容
        Text tmpText = tmpInfoObj.GetComponentInChildren<Text>();
        tmpText.text = infoStr;
        tmpText.color = infoColor;
        StartCoroutine(ShowMessagePopup(tmpInfoObj.gameObject));
    }
    IEnumerator ShowMessagePopup(GameObject infoObj)
    {
        CanvasGroup tmpGroup = infoObj.GetComponent<CanvasGroup>();
        tmpGroup.alpha = 0;
        //开始显示
        infoObj.gameObject.SetActive(true);
        tmpGroup.DOFade(1, fadeTime);
        //持续一段时间
        yield return new WaitForSeconds(fadeTime + showTime);
        //开始隐藏
        tmpGroup.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        infoObj.gameObject.SetActive(false);
        popupStack.Push(infoObj.transform);
        curShowCount--;
        //如果待显示列表不为空, 继续显示
        if (infoStack.Count > 0)
        {
            MessageInfo tmpInfo = infoStack.Pop();
            if (tmpInfo != null)
            {
                ShowMessageInfo(tmpInfo.infoStr, tmpInfo.color);
            }
        }
    }
}

//弹窗消息类
class MessageInfo
{
    public string infoStr { get; private set; }
    public Color color { get; private set; }
    public MessageInfo(string messageStr, Color messageColor)
    {
        infoStr = messageStr;
        color = messageColor;
    }
}
