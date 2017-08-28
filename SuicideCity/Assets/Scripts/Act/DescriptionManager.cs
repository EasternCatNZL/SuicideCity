using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DescriptionManager : MonoBehaviour {

    [Header("Filenames")]
    [Tooltip("Filenames for descriptions for victim")]
    public string[] filenamesVictim = new string[0];
    [Tooltip("Filenames for descriptions for scene")]
    public string[] filenamesScene = new string[0];
    [Tooltip("Filenames for descriptions for evidence")]
    public string[] filenamesEvidence = new string[0];
    [Tooltip("Filename for no interest captured")]
    public string filenameNothing;
    [Tooltip("Base filename")]
    public string filenameBase = "";

	// Use this for initialization
	void Start () {
        filenameBase = Application.dataPath + "/Text/";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //returns a description from an external file
    public string GetDescription(InterestBehaviour interestFromPhoto)
    {
        //setup
        string description = "";
        //check what the focus of the interest photo is
        if (interestFromPhoto.interestType == InterestBehaviour.InterestType.Victim)
        {
            //check the state, and then retrieve the description
            if (interestFromPhoto.progress == InterestBehaviour.ActProgress.BeforeAct)
            {
                description = ReadFromExternalFile(filenamesVictim[0]);
            }
            else if (interestFromPhoto.progress == InterestBehaviour.ActProgress.InAct)
            {
                description = ReadFromExternalFile(filenamesVictim[1]);
            }
            else if (interestFromPhoto.progress == InterestBehaviour.ActProgress.AfterAct)
            {
                description = ReadFromExternalFile(filenamesVictim[2]);
            }
        }
        else if (interestFromPhoto.interestType == InterestBehaviour.InterestType.Scene)
        {
            //check the state, and then retrieve the description
            if (interestFromPhoto.progress == InterestBehaviour.ActProgress.BeforeAct)
            {
                description = ReadFromExternalFile(filenamesScene[0]);
            }
            else if (interestFromPhoto.progress == InterestBehaviour.ActProgress.InAct)
            {
                description = ReadFromExternalFile(filenamesScene[1]);
            }
            else if (interestFromPhoto.progress == InterestBehaviour.ActProgress.AfterAct)
            {
                description = ReadFromExternalFile(filenamesScene[2]);
            }
        }
        return description;
    }

    //get no interest description
    public string GetNoInterestDescription()
    {
        string description = ReadFromExternalFile(filenameNothing);
        return description;
    }

    //read from file
    private string ReadFromExternalFile(string filename)
    {
        //setup
        string description = "" ;
        //get the file into stream reader
        StreamReader reader = new StreamReader(filenameBase + filename, Encoding.Default);
        //read the whole file
        description = reader.ReadToEnd();

        return description;
    }
}
