using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigInfo {

    Dictionary<string, UIConfig> ui = new Dictionary<string, UIConfig>();

    public Dictionary<string, UIConfig> UI
    {
        get
        {
            return ui;
        }

        set
        {
            ui = value;
        }
    }
}
