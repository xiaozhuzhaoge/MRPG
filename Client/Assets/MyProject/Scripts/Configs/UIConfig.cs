using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConfig : ConfigBase{

    string uiName;
    string prams;

    public string UIName
    {
        get
        {
            return uiName;
        }

        set
        {
            uiName = value;
        }
    }

    public string Prams
    {
        get
        {
            return prams;
        }

        set
        {
            prams = value;
        }
    }
}
