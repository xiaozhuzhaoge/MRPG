using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SrollUV : MonoBehaviour {
    public float speed = 0.01f;
    public RawImage btn;
    private void Awake()
    {
        btn = GetComponent<RawImage>();
        rect = new Rect(0, 0, 1, 1);
    }
    Rect rect;
	// Update is called once per frame
	void Update () {
        rect.x += speed;
        btn.uvRect = rect;
    }
}
