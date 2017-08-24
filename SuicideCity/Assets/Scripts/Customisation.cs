using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customisation : MonoBehaviour {

    public Button[] UIButtons;
    public Text Hint;

    public GameObject Player;

    private bool CheckInput = false;
    private bool InMenu = false;

    public enum SkinOptions
    {
        Red,
        Blue,
        Green
    };

    private void Update()
    {
        if(CheckInput)
        {
            if (InMenu)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    InMenu = false;
                    DisableUI();
                    CameraController.UnlockCamera();
                    PlayerController.UnlockPlayer();
                    Hint.text = "Press 'F' to change appearance";
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    InMenu = true;
                    EnableUI();
                    CameraController.LockCamera();
                    PlayerController.LockPlayer();
                    Hint.text = "Press 'F' to exit";
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        print("Trigger");
        Hint.enabled = true;
        CheckInput = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        Hint.enabled = false;
        CheckInput = false;
    }

    public void EnableUI()
    {
        foreach(Button it in UIButtons)
        {
            it.gameObject.SetActive(true);
        }
    }

    public void DisableUI()
    {
        foreach (Button it in UIButtons)
        {
            it.gameObject.SetActive(false);
        }
    }
    public void ChangeSkin(int _Option)
    {
        switch(_Option)
        {
            case 0:
                Player.GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case 1:
                Player.GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            case 2:
                Player.GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            default:break;
        }
    }
}
