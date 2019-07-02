using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobConfig : ConfigBase {

    string icon;
    string des;

    public string Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
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
