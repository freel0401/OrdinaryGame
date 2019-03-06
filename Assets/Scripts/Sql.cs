using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Text;

public class Sql
{

    private static Sql _instance;
    public static Sql GetInstance()
    {
        if (_instance == null)
        {
            Debug.Log("Create " + typeof(Sql).ToString() + " singleton...");
            _instance = new Sql();
        }
        return _instance;
    }
    // 数据库连接定义
    private SqliteConnection dbConnection;
    // SQL命令定义
    private SqliteCommand dbCommand;

    // 数据读取定义
    private SqliteDataReader dataReader;

    private string cacheTableName;
    private List<string> cacheItems;
    private List<string> cacheColNames;
    private List<string> cacheOperations;
    private List<string> cacheColValues;

    // 生成数据库表的List缓存, 复用 使用前清理
    private List<string> sqlStrings;
    // 初始化字符串缓存, 省内存呀
    private void initCaches()
    {
        sqlStrings = new List<string>();
        cacheItems = new List<string>();
        cacheColNames = new List<string>();
        cacheOperations = new List<string>();
        cacheColValues = new List<string>();
    }

    private void clearCaches()
    {
        sqlStrings.Clear();
        cacheItems.Clear();
        cacheColNames.Clear();
        cacheOperations.Clear();
        cacheColValues.Clear();
    }

    // 数据库名 TODO 读取config
    private string sqlName = "orginarygame.db";


    private string getSqlPath()
    {
        string sqlFilePath;

        // #if UNITY_STANDALONE_WIN
#if UNITY_EDITOR
        sqlFilePath = Application.dataPath + "/" + sqlName;
#else
            sqlFilePath = Application.persistentDataPath + "/" + sqlName;
        // #elif UNITY_EDITOR_OSX
        // #elif UNITY_ANDROID
        // #elif  UNITY_IOS
#endif
        return sqlFilePath;
    }
    public Sql()
    {
        initCaches();

        string sqlFilePath = getSqlPath();

        bool needInit = File.Exists(sqlFilePath);

        // if (!File.Exists(sqlFilePath))
        //     needInit = true;
        //构造数据库连接
        dbConnection = new SqliteConnection("data source=" + sqlFilePath);
        //打开数据库
        dbConnection.Open();
        if (needInit != true)
            initDB();
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


    /// 删除指定数据表内的数据
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

    /// 删除指定数据表内的数据
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

    public SqliteDataReader ReadTable(string tableName, List<string> items, List<string> colNames, List<string> operations, List<string> colValues)
    {
        // string queryString = "SELECT " + items[0];
        sqlStrings.Clear();
        sqlStrings.Add("SELECT ");
        sqlStrings.Add(items[0]);
        for (int i = 1; i < items.Count; i++)
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
        for (int i = 0; i < colNames.Count; i++)
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

    public Sql Table(string tableName)
    {
        this.cacheTableName = tableName;
        return this;
    }

    public Sql Select(string item)//, string[] items, string[] colNames, string[] operations, string[] colValues)
    {
        this.cacheItems.Add(item);
        return this;
    }

    public Sql Select(string[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            this.cacheItems.Add(items[i]);
        }
        return this;
    }

    public Sql ColName(string colName)
    {
        this.cacheColNames.Add(colName);
        return this;
    }

    public Sql ColName(string[] colNames)
    {
        for (int i = 0; i < colNames.Length; i++)
        {
            this.cacheColNames.Add(colNames[i]);
        }
        return this;
    }

    public Sql Operation(string opt)
    {
        this.cacheOperations.Add(opt);
        return this;
    }

    public Sql Operation(string[] opts)
    {
        for (int i = 0; i < opts.Length; i++)
        {
            this.cacheOperations.Add(opts[i]);
        }
        return this;
    }

    public SqliteDataReader ColValue(string colValue)
    {
        this.cacheColValues.Add(colValue);
        SqliteDataReader results = ReadTable(this.cacheTableName, this.cacheItems, this.cacheColNames, this.cacheOperations, this.cacheColValues);
        clearCaches();
        return results;
    }

    public SqliteDataReader ColValue(string[] colValues)
    {
        for (int i = 0; i < colValues.Length; i++)
        {
            this.cacheColValues.Add(colValues[i]);
        }
        SqliteDataReader results = ReadTable(this.cacheTableName, this.cacheItems, this.cacheColNames, this.cacheOperations, this.cacheColValues);
        clearCaches();
        return results;
    }

    //-----------------工具 构造sql查询字符串
    private StringBuilder stringBuilder;
    private string buildSql()
    {
        if (stringBuilder == null)
        {
            stringBuilder = new StringBuilder();
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

    public void initDB()
    {
        string dbScriptsPath = "table.sql";
        string textFile = File.ReadAllText("Assets\\Resources\\" + dbScriptsPath);
        Debug.Log(textFile);
        ExecuteQuery(textFile);
    }

    // ---------------------
    // 作者：PayneQin
    // 来源：CSDN
    // 原文：https://blog.csdn.net/qinyuanpei/article/details/46812655
    // 版权声明：本文为博主原创文章，转载请附上博文链接！
}
