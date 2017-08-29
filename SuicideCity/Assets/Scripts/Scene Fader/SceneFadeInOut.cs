using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeInOut : MonoBehaviour {
    
    public bool start = false;
    public float fadeDamp = 0.5f;
    public string fadeScene;
    public float alpha = 0.0f;
    public Color fadeColor;
    public bool isFadeIn = false;

    // Use this for initialization
    void Start () {
        
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
        if (!start)
        {
            return;
        }

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        Texture2D TempTexture;
        TempTexture = new Texture2D(1, 1);
        TempTexture.SetPixel(0, 0, fadeColor);
        TempTexture.Apply();

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), TempTexture);

        if (isFadeIn)
        {
            alpha = Mathf.Lerp(alpha, -0.1f, fadeDamp * Time.deltaTime);
        }            
        else
        {
            alpha = Mathf.Lerp(alpha, 1.1f, fadeDamp * Time.deltaTime);
        }            
        if (alpha >= 1 && !isFadeIn)
        {
            SceneManager.LoadScene(fadeScene);
            DontDestroyOnLoad(gameObject);
        }
        else if (alpha <= 0 && isFadeIn)
        {
            Destroy(gameObject);
        }

    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        isFadeIn = true;
    }
}
