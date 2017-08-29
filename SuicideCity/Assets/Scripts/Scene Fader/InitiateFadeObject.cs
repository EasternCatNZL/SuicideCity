using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateFadeObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Fade(string scene, Color col, float damp)
    {
        GameObject Fader = new GameObject();
        //init.name = "SceneFader";
        Fader.AddComponent<SceneFadeInOut>();
        SceneFadeInOut SceneFader = Fader.GetComponent<SceneFadeInOut>();
        SceneFader.fadeDamp = damp;
        SceneFader.fadeScene = scene;
        SceneFader.fadeColor = col;
        SceneFader.start = true;
    }
}
