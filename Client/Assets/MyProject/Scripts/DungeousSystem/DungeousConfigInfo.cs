using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigInfo {

    Dictionary<string, DungeousConfig> dungeous = new Dictionary<string, DungeousConfig>();

    public Dictionary<string, DungeousConfig> Dungeous
    {
        get
        {
            return dungeous;
        }

        set
        {
            dungeous = value;
        }
    }
}
