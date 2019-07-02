//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's position, rotation and scale.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
    public Vector3 from;
    public Vector3 to;

    Transform mTrans;
    Vector3 mPos;

    /// <summary>
    /// Interpolate the position, scale, and rotation.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (to != null)
        {
            if (mTrans == null)
            {
                mTrans = transform;
                mPos = mTrans.localPosition;
            }

            if (from != null)
            {
                mTrans.localPosition = from * (1f - factor) + to * factor;
            }
            else
            {
                mTrans.localPosition = mPos * (1f - factor) + to * factor;
            }
        }
    }

    /// <summary>
    /// Start the tweening operation from the current position/rotation/scale to the target transform.
    /// </summary>

    static public TweenPosition Begin(GameObject go, float duration, Vector3 to) { return Begin(go, duration, to); }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenPosition Begin(GameObject go, float duration, Vector3 from, Vector3 to)
    {
        TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
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
