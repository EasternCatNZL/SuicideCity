using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITweenTestScript : MonoBehaviour {

    public Image imageA;
    public Image imageB;

	// Use this for initialization
	void Start () {
        imageA.rectTransform.DOAnchorPos(imageB.rectTransform.anchoredPosition, 4.0f);
        imageA.rectTransform.DOScale(imageB.rectTransform.localScale, 4.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
