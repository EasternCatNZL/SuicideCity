using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeInOut : MonoBehaviour {
    
    public bool Start = false;
    public float FadeSpeed = 0.5f;
    public string FadeScene;
    public float Alpha = 0.0f;
    public Color FadeColor;
    public bool isFadeIn = false;

    // Use this for initialization
    void start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnGUI()
    {
        if (!Start)
        {
            return;
        }

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Alpha);

        Texture2D TempTexture;
        TempTexture = new Texture2D(1, 1);
        TempTexture.SetPixel(0, 0, FadeColor);
        TempTexture.Apply();

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), TempTexture);

        if (isFadeIn)
        {
            Alpha = Mathf.Lerp(Alpha, -0.1f, FadeSpeed * Time.deltaTime);
        }            
        else
        {
            Alpha = Mathf.Lerp(Alpha, 1.1f, FadeSpeed * Time.deltaTime);
        }            
        if (Alpha >= 1 && !isFadeIn)
        {
            SceneManager.LoadScene(FadeScene);
            DontDestroyOnLoad(gameObject);
        }
        else if (Alpha <= 0 && isFadeIn)
        {
            Destroy(gameObject);
        }

    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        isFadeIn = true;
    }
}
