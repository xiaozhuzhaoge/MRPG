using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigInfo  {

    Dictionary<string, MessageConfig> messageDir = new Dictionary<string, MessageConfig>();

    public Dictionary<string, MessageConfig> MessageDir
    {
        get
        {
            return messageDir;
        }

        set
        {
            messageDir = value;
        }
    }
}
