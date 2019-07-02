//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the object's position, rotation and scale.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
    [Range(0f, 1f)] public float from = 1f;
    [Range(0f, 1f)] public float to = 1f;

    bool mCached = false;
    CanvasGroup mRect;
    Material mMat;
    Image mSr;

    [System.Obsolete("Use 'value' instead")]
    public float alpha { get { return this.value; } set { this.value = value; } }

    void Cache()
    {
        mCached = true;
        mRect = GetComponent<CanvasGroup>();
        mSr = GetComponent<Image>();

        if (mRect == null && mSr == null)
        {
            Renderer ren = GetComponent<Renderer>();
            if (ren != null) mMat = ren.material;
            if (mMat == null) mRect = GetComponentInChildren<CanvasGroup>();
        }
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value
    {
        get
        {
            if (!mCached) Cache();
            if (mRect != null) return mRect.alpha;
            if (mSr != null) return mSr.color.a;
            return mMat != null ? mMat.color.a : 1f;
        }
        set
        {
            if (!mCached) Cache();

            if (mRect != null)
            {
                mRect.alpha = value;
            }
            else if (mSr != null)
            {
                Color c = mSr.color;
                c.a = value;
                mSr.color = c;
            }
            else if (mMat != null)
            {
                Color c = mMat.color;
                c.a = value;
                mMat.color = c;
            }
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenAlpha Begin(GameObject go, float duration, float alpha, float delay = 0f)
    {
        TweenAlpha comp = UITweener.Begin<TweenAlpha>(go, duration, delay);
        comp.from = comp.value;
        comp.to = alpha;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }
}
