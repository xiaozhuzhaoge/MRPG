using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineConfig : ConfigBase {

    string combine;
    string getProp;
    string useNum;
    string getNum;
    string des;

    public string Combine
    {
        get
        {
            return combine;
        }

        set
        {
            combine = value;
        }
    }

    public string GetProp
    {
        get
        {
            return getProp;
        }

        set
        {
            getProp = value;
        }
    }

    public string UseNum
    {
        get
        {
            return useNum;
        }

        set
        {
            useNum = value;
        }
    }

    public string GetNum
    {
        get
        {
            return getNum;
        }

        set
        {
            getNum = value;
        }
    }

    public string Des
    {
        get
        {
            return des;
        }

        set
        {
            des = value;
        }
    }
}
