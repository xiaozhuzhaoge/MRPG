using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaDoFile : MonoBehaviour {

    public  string instruction;
	// Use this for initialization
	void Start () {
        LuaSumlator.Instance.LoadScript(instruction);	
	}
 
}
