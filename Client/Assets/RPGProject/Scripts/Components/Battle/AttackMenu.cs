using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AttackMenu : MonoSingleton<AttackMenu> {

    public bool[] flags;
    public Action<int,bool>[] flagCallback;
    public int num;

    protected override void Initialize()
    {

        flags = new bool[num];
        flagCallback = new Action<int,bool>[num];
    }
    

    public void OnButtonClick(int index, bool flag) {
        flags[index] = flag;
        if(flagCallback != null && flag == true)
            if(flagCallback[index] != null)
                flagCallback[index](index,flag);
    }

    public void LateUpdate()
    {
        Reset();
    }

    public void Reset()
    {
        for(int i = 0;i < flags.Length; i++)
        {
            flags[i] = false;
        }
    }

    public bool GetFlag(int index)
    {
        return flags[index];
    }

    public void SetFlag(int index)
    {
        flags[index] = false;
    }
}
