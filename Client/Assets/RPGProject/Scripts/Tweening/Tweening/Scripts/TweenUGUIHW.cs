//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the UGUI width and hegith
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween UGUIHeightAndWidth")]
public class TweenUGUIHW : UITweener
{
    public Vector2 from;
    public Vector2 to;

    RectTransform mTrans;
    Vector2 hv;
 
    /// <summary>
    /// Interpolate the position, scale, and rotation.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (to != null)
        {
            if (mTrans == null)
            {
                mTrans = transform as RectTransform;
                hv = mTrans.sizeDelta;
                
            }

            if (from != null)
            {
                mTrans.sizeDelta = from * (1f - factor) + to * factor;
             
            }
            else
            {
                mTrans.sizeDelta = hv * (1f - factor) + to * factor;
                
            }
        }
    }

    /// <summary>
    /// Start the tweening operation from the current position/rotation/scale to the target transform.
    /// </summary>

    static public TweenUGUIHW Begin(GameObject go, float duration, Vector2 to) { return Begin(go, duration, to); }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenUGUIHW Begin(GameObject go, float duration, Vector2 from, Vector2 to)
    {
        TweenUGUIHW comp = UITweener.Begin<TweenUGUIHW>(go, duration);
        comp.from = from;
        comp.to = to;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
}
