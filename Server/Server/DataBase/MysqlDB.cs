using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Collections.Generic;


public class MysqlDB
{

    private MySqlConnection _conn;

    public void Init(string dbname, string dataSource, string userID, string pwd)
    {
        _conn = ConnMysql(dbname, dataSource, userID, pwd);

    }

    /// <summary>
    /// 建立mysql数据库链接
    /// </summary>
    /// <returns></returns>
    public MySqlConnection ConnMysql(string dbname, string dataSource, string userID, string pwd)
    {
        String mysqlStr = string.Format("Database={0};Data Source={1};User Id={2};Password={3};pooling=false;CharSet=utf8;port=3306", dbname, dataSource, userID, pwd);

        MySqlConnection conn = null;
        try
        {
            conn = new MySqlConnection(mysqlStr);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return conn;
    }

    public void ExecNonQuery(string sql)
    {

        MySqlCommand command = new MySqlCommand(sql, _conn);

        try
        {
            _conn.Close();
            _conn.Open();
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            _conn.Close();
        }
    }


    public List<object> ExecQuery<T>(string sql)
    {
        List<object> objList = new List<object>();

        MySqlCommand command = new MySqlCommand(sql, _conn);
        MySqlDataReader reader = null;
        try
        {
            _conn.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                T obj = Activator.CreateInstance<T>();
                FieldInfo[] fields = typeof(T).GetFields();
                if(fields.Length==0)
                {

                }

                for (int j = 0; j < reader.FieldCount; j++)
                {
                    ReadField<T>(obj, fields[j], reader, j);
                }

                objList.Add(obj);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            reader.Close();
            _conn.Close();
            command.Dispose();
        }

        return objList;
    }

    public static void ReadField<T>(T obj, FieldInfo fieldInfo, MySqlDataReader reader, int ordinal)
    {
        Type type = fieldInfo.FieldType;
        System.Object value = null;
        if (type == typeof(int))
            value = reader.GetFieldValue<int>(ordinal);
        else if (type == typeof(byte))
            value = reader.GetFieldValue<byte>(ordinal);
        else if (type == typeof(uint))
            value = reader.GetFieldValue<uint>(ordinal);
        else if (type == typeof(string))
            value = reader.GetFieldValue<string>(ordinal);

        fieldInfo.SetValue(obj, value);
    }

    public void Insert(string sql)
    {
        try
        {
            _conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, _conn);
            cmd.ExecuteNonQuery();
            _conn.Close();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}