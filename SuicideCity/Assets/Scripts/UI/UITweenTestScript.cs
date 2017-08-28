using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using System.Text;

public class UITweenTestScript : MonoBehaviour {

    public Image imageA;
    public Image imageB;
    private string filenameBase = "";
    public string filename;

    // Use this for initialization
    void Start () {
        filenameBase = Application.dataPath + "/Text/";
        StreamReader reader = new StreamReader(filenameBase + filename, Encoding.Default);
        string test = reader.ReadToEnd();
        print(test);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
