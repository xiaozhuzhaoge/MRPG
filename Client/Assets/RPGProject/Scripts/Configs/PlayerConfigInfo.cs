using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigInfo {

    Dictionary<string, PlayerConfig> playerConfig = new Dictionary<string, PlayerConfig>();

    public Dictionary<string, PlayerConfig> PlayerDir
    {
        get
        {
            return playerConfig;
        }

        set
        {
            playerConfig = value;
        }
    }
}
