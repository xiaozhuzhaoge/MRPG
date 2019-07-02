using System;
using System.Collections.Generic;
using System.IO;

public class MobaLog
{
    private static StreamWriter _file;
    public void Init()
    {
        string filePath = "Log/Log.txt";
        if (File.Exists(filePath))
            File.Delete(filePath);

        FileStream fs = new FileStream(filePath, FileMode.Create);
        _file = new System.IO.StreamWriter(fs);
    }

    public void End()
    {
        _file.Flush();
        _file.Close();
    }

    public static void Log(object obj)
    {
        Console.WriteLine(obj);

        System.DateTime t = System.DateTime.Now;
        _file.WriteLine(t + ": " + t.Millisecond + ": " + obj);
    }
}