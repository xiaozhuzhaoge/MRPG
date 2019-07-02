using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class AnimeMgr : Singleton<AnimeMgr> {

    static List<RuntimeAnimatorController> registerAni;
    
    public static List<RuntimeAnimatorController> RegisterAni
    {
        get
        {
            if (registerAni == null)
                registerAni = new List<RuntimeAnimatorController>();
 
            return registerAni;
        }

        set
        {
            registerAni = value;
        }
    }

    public static bool RegisterAniEvent(Animator ani, int GroupIdFromConfig)
    {
        
        if (!ContainAni(ani.runtimeAnimatorController))
        {
            Debug.Log(ani.runtimeAnimatorController);
            ///注册动画帧事件
            List<SkillChain> SkillChains = ConfigInfo.GetSkillChains(GroupIdFromConfig.ToString());
            if (SkillChains != null)
                for (int i = 0; i < SkillChains.Count; i++)
                {
                    Utility.RigisterAnimationEvent(ani, SkillChains[i].AnimationStateName, SkillChains[i].FunctionName, System.Convert.ToSingle(SkillChains[i].DoPercent), SkillChains[i].Instruction);
                }

            RegisterAni.Add(ani.runtimeAnimatorController);
            return true;
        }
        return false;
       
    }

    public static bool ContainAni(RuntimeAnimatorController ani)
    {
        return RegisterAni.Contains(ani);
    }

}
