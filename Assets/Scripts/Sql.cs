using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Text;

public class Sql : Singleton<Sql>
{

    // 数据库连接定义
    private SqliteConnection dbConnection;
    // SQL命令定义
    private SqliteCommand dbCommand;

    // 数据读取定义
    private SqliteDataReader dataReader;

    private List<string> sqlStrings;

    private string sqlName = "orginarygame.db";
    public Sql()
    {
        sqlStrings = new List<string>();
        // TODO 不同平台的区分
        // string connectionString = "data source=" + Application.streamingAssetsPath + "/" + this.sqlName;
        string connectionString;

        // #if UNITY_STANDALONE_WIN
        // connectionString = "data source=" + Application.dataPath + "/" + this.sqlName;
        connectionString = "data source=" + Application.persistentDataPath + "/" + sqlName;
        // #elif UNITY_EDITOR_OSX
        // #elif UNITY_ANDROID
        // #elif  UNITY_IOS
        // #endif
        //构造数据库连接
        dbConnection = new SqliteConnection(connectionString);
        //打开数据库
        dbConnection.Open();
    }

    // 执行SQL命令
    public SqliteDataReader ExecuteQuery(string queryString)
    {
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryString;
        dataReader = dbCommand.ExecuteReader();
        return dataReader;
    }

    // 关闭数据库连接
    public void CloseConnection()
    {
        //销毁Command
        if (dbCommand != null)
        {
            dbCommand.Cancel();
        }
        dbCommand = null;

        //销毁Reader
        if (dataReader != null)
        {
            dataReader.Close();
        }
        dataReader = null;

        //销毁Connection
        if (dbConnection != null)
        {
            dbConnection.Close();
        }
        dbConnection = null;
    }

    /// 读取整张数据表
    public SqliteDataReader SelectAll(string tableName)
    {
        string queryString = "SELECT * FROM " + tableName;
        return ExecuteQuery(queryString);
    }

    /// 向指定数据表中插入数据
    public SqliteDataReader Insert(string tableName, string[] values)
    {
        //获取数据表中字段数目
        int fieldCount = SelectAll(tableName).FieldCount;
        //当插入的数据长度不等于字段数目时引发异常 TODO 使用defaultDB检测 避免频繁查表
        if (values.Length != fieldCount)
        {
            throw new SqliteException("values.Length!=fieldCount");
        }
        sqlStrings.Clear();
        sqlStrings.Add("INSERT INTO ");
        sqlStrings.Add(tableName);
        sqlStrings.Add(" VALUES (");
        sqlStrings.Add(values[0]);
        for (int i = 1; i < values.Length; i++)
        {
            sqlStrings.Add(", ");
            sqlStrings.Add(values[i]);
        }
        sqlStrings.Add(" )");
        string queryString = buildSql();
        return ExecuteQuery(queryString);
    }

    /// 更新指定数据表内的数据
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    /// <param name="key">关键字</param>
    /// <param name="value">关键字对应的值</param>
    public SqliteDataReader Update(string tableName, string[] colNames, string[] colValues, string key, string operation, string value)
    {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length");
        }

        // string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
        sqlStrings.Clear();
        sqlStrings.Add("UPDATE ");
        sqlStrings.Add(tableName);
        sqlStrings.Add(" SET ");
        sqlStrings.Add(colNames[0]);
        sqlStrings.Add("=");
        sqlStrings.Add(colValues[0]);
        for (int i = 1; i < colValues.Length; i++)
        {
            sqlStrings.Add(", ");
            sqlStrings.Add(colNames[i]);
            sqlStrings.Add("=");
            sqlStrings.Add(colValues[i]);
        //     queryString += ", " + colNames[i] + "=" + colValues[i];
        }
        sqlStrings.Add(" WHERE ");
        sqlStrings.Add(key);
        sqlStrings.Add(operation);
        sqlStrings.Add(value);
        // TODO 缓存 sql语句

        return ExecuteQuery(buildSql());
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += "OR " + colNames[i] + operations[0] + colValues[i];
        }
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += " AND " + colNames[i] + operations[i] + colValues[i];
        }
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 创建数据表
    /// </summary> +
    /// <returns>The table.</returns>
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>
    public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
    {
        string queryString = "CREATE TABLE " + tableName + "( " + colNames[0] + " " + colTypes[0];
        for (int i = 1; i < colNames.Length; i++)
        {
            queryString += ", " + colNames[i] + " " + colTypes[i];
        }
        queryString += "  ) ";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// Reads the table.
    /// </summary>
    /// <returns>The table.</returns>
    /// <param name="tableName">Table name.</param>
    /// <param name="items">Items.</param>
    /// <param name="colNames">Col names.</param>
    /// <param name="operations">Operations.</param>
    /// <param name="colValues">Col values.</param>
    public SqliteDataReader Select(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
    {
        // string queryString = "SELECT " + items[0];
        sqlStrings.Clear();
        sqlStrings.Add("SELECT ");
        sqlStrings.Add(items[0]);
        for (int i = 1; i < items.Length; i++)
        {
            // queryString += ", " + items[i];
            sqlStrings.Add(", ");
            sqlStrings.Add(items[i]);
        }
        sqlStrings.Add(" FROM ");
        sqlStrings.Add(tableName);
        sqlStrings.Add(" WHERE ");
        sqlStrings.Add(colNames[0]);
        sqlStrings.Add(" ");
        sqlStrings.Add(operations[0]);
        sqlStrings.Add(" ");
        sqlStrings.Add(colValues[0]);
        // queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
        for (int i = 0; i < colNames.Length; i++)
        {
            // queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
            sqlStrings.Add(" AND ");
            sqlStrings.Add(colNames[i]);
            sqlStrings.Add(" ");
            sqlStrings.Add(operations[i]);
            sqlStrings.Add(" ");
            sqlStrings.Add(colValues[0]);
            sqlStrings.Add(" ");
        }
        return ExecuteQuery(buildSql());
    }

    //-----------------工具 构造sql查询字符串
    private StringBuilder stringBuilder;
    private string buildSql()
    {
        if (stringBuilder == null)
        {
            stringBuilder  = new StringBuilder();
        }
        else
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(0, stringBuilder.Length);
        }
        for (int i = 0; i < sqlStrings.Count; i++)
        {
            stringBuilder.Append(sqlStrings[i]);
        }
        return stringBuilder.ToString();
    }

    // ---------------------
    // 作者：PayneQin
    // 来源：CSDN
    // 原文：https://blog.csdn.net/qinyuanpei/article/details/46812655
    // 版权声明：本文为博主原创文章，转载请附上博文链接！
}
