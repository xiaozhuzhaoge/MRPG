using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdgeCaches : MonoBehaviour {

    public string prefix_脚本前缀;
    Dictionary<string, C_Lua_Bridge> Birdges = new Dictionary<string, C_Lua_Bridge>();
    public C_Lua_Bridge currentBirdge;

    public void AddBirdage(string name,C_Lua_Bridge bridge) {

        if(!Birdges.ContainsKey(name))
            Birdges.Add(name, bridge);
    }

    public C_Lua_Bridge GetBirdage(string name)
    {
        if (Birdges.ContainsKey(name))
            return Birdges[name];
        return null;
    }

    public void OnEnter()
    {
        currentBirdge.OnEnter();
    }

    public void OnUpdate()
    {
        currentBirdge.OnUpdate();
    }

    public void OnExit()
    {
        currentBirdge.OnExit();
    }

    public void OnDestroy()
    {
        foreach(var bird in Birdges)
        {
            bird.Value.Destory();
        }
    }
}
