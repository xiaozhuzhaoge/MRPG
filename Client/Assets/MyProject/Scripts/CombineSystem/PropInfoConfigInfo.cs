using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigInfo {

    Dictionary<string, PropInfoConfig> props = new Dictionary<string, PropInfoConfig>();

    public Dictionary<string, PropInfoConfig> Props
    {
        get
        {
            return props;
        }

        set
        {
            props = value;
        }
    }

    public static PropInfoConfig GetProp(string propId)
    {
        Debug.Log(propId);
        return ConfigInfo.Instance.Props[propId];
    }
}
