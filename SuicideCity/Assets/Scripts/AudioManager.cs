using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource LevelStart;
    public AudioSource AmbientNoise;
    public AudioSource Music;

	// Use this for initialization
	void Start () {
        if (LevelStart) LevelStart.Play();
        if (AmbientNoise) AmbientNoise.Play();
        if (Music) Music.Play();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
