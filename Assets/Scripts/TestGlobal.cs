using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class TestGlobal : MonoBehaviour
{
    void onStart()
    { }
    private void OnApplicationQuit()
    {
        //当程序退出时关闭数据库连接，不然会重复打开数据卡，造成卡顿
        // this.CloseSQLConnection();
        Sql.GetInstance().CloseConnection();
        Debug.Log("程序退出");
    }
}

public class Global
{
    static int guid = 0;
    static public StringBuilder stringBuilder = new StringBuilder();
    static public StringBuilder GetStringBuilder()
    {
        var sb = stringBuilder;
        if (sb.Length > 0)
            sb.Remove(0, sb.Length);
        return sb;
    }

    static public string FormatStrings(params string[] strTb)
    {
        StringBuilder sb = GetStringBuilder();
        foreach (string str in strTb)
        {
            sb.Append(str);
        }
        return sb.ToString();
    }

    static public int GetGuid()
    {
        return guid++;
    }

}


