using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChain : ConfigBase
{
 

    public string GroupId
    {
        get
        {
            return groupid;
        }

        set
        {
            groupid = value;
        }
    }

    public string AnimationStateName
    {
        get
        {
            return animationStateName;
        }

        set
        {
            animationStateName = value;
        }
    }

    public string FunctionName
    {
        get
        {
            return functionName;
        }

        set
        {
            functionName = value;
        }
    }

    public double DoPercent
    {
        get
        {
            return doPercent;
        }

        set
        {
            doPercent = value;
        }
    }

    public string Instruction
    {
        get
        {
            return instruction;
        }

        set
        {
            instruction = value ;
        }
    }
 
    private string groupid;
    private string animationStateName;
    string functionName;
    double doPercent;
    string instruction;
 
}
