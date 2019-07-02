using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoint : MonoBehaviour {

    public Transform point;

    private void Awake()
    {
        point = Utility.FindChild<Transform>(transform, "Hand_R");
        var wp = ResourceMgr.CreateObj("Prefabs/WeaponPoint");
        wp.transform.SetParent(point,false);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
