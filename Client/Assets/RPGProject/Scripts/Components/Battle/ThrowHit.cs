using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 投掷类 
/// </summary>
public class ThrowHit : MonoBehaviour {

    public CharacterCtrlBase from; 
    /// <summary>
    /// 可受击一次或多次
    /// </summary>
    public bool OneHit = false;
    public SkillData skillData;

    public void Init(CharacterCtrlBase sender){
        from = sender;
    }

    private void OnTriggerEnter(Collider other)
    {
        ///层级相同屏蔽
        if(other.gameObject.layer == from.gameObject.layer)
            return;
        if(other.GetComponent<IHitAnalysis>() != null)
        {
            other.GetComponent<IHitAnalysis>().BeHit(skillData, from, transform);
            if (OneHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
