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

    public static void Fade(string scene, Color colour, float FadeSpeed)
    {
        GameObject Fader = new GameObject();
        //init.name = "SceneFader";
        Fader.AddComponent<SceneFadeInOut>();
        SceneFadeInOut SceneFader = Fader.GetComponent<SceneFadeInOut>();
        SceneFader.FadeSpeed = FadeSpeed;
        SceneFader.FadeScene = scene;
        SceneFader.FadeColor = colour;
        SceneFader.Start = true;
    }
}
