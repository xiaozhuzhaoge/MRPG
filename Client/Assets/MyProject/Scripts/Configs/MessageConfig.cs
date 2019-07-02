using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageConfig : ConfigBase{

    string text;

    public string Text
    {
        get
        {
            return text;
        }

        set
        {
            text = value;
        }
    }
}
