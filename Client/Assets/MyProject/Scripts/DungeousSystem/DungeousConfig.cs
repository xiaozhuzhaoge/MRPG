using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeousConfig : ConfigBase {

    string name;
    string des;
    string index;

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
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

    public string Index
    {
        get
        {
            return index;
        }

        set
        {
            index = value;
        }
    }
}
