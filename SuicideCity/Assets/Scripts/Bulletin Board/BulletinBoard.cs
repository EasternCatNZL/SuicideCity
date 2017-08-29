using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletinBoard : MonoBehaviour {

    public string NextScene;
    public Color FadeColor = Color.black;
    public float FadeSpeed = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FadeScene()
    {
        InitiateFadeObject.Fade(NextScene, FadeColor, FadeSpeed);
    }

}
