using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PhotobookBehaviour : MonoBehaviour {

    //the photos currently taken, in list format
    public List<GameObject> photoList = new List<GameObject>();

    [Header("Photobook GUI component")]
    [Tooltip("Ref to gui component of photobook menu")]
    public GameObject photobookMenuObject;
    [Tooltip("The main image being shown, the big one")]
    public Image displayImage;
    [Tooltip("The currently selected image")]
    public Image selectedImage;
    [Tooltip("The left side image")]
    public Image leftImage;
    [Tooltip("The right side image")]
    public Image rightImage;
    [Tooltip("The text")]
    public Text text;
    [Tooltip("The left button")]
    public Button leftButton;
    [Tooltip("The right button")]
    public Button rightButton;

    //image object list of all photos
    private List<Image> photoImageList = new List<Image>();

    [Header("No photo image")]
    [Tooltip("The image that shows when no photos are in the photobook")]
    public Sprite noPhotosImage;

    [Header("Transition vars")]
    [Tooltip("Time needed for transitions")]
    public float transitionTime = 1.5f;

    //control vars
    private int currentPhoto = 0;
    private bool isPhotobookOpen = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isPhotobookOpen)
            {
                ClosePhotobook();
            }
            else
            {
                OpenPhotobook();
            }
        }
        if (isPhotobookOpen)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                ChangePhotoPrev();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                ChangePhotoNext();
            }
        }
	}

    public void AddPhotoToBook(GameObject newPhoto)
    {
        photoList.Add(newPhoto);
    }

    //when photobook opens, populate photoimage list with a new set of image objects
    private void PopulatePhotoList()
    {
        //clear old list each time <- make sure its cleared
        photoImageList.Clear();
        //for the first photo, instantiate using currently selected photos transform
        Image firstPhoto = Instantiate(selectedImage, selectedImage.rectTransform.anchoredPosition, selectedImage.transform.rotation);
        //set the image of the first photo
        firstPhoto.sprite = photoList[0].GetComponent<PhotoBehaviour>().photo;
        firstPhoto.transform.SetParent(photobookMenuObject.transform, false);
        //add the first photo to the list
        photoImageList.Add(firstPhoto);
        //for all other photos, instantiate using right photos transform
        //start at one, ignore first, if no others, ignore rest
        for (int i = 1; i < photoList.Count; i++)
        {
            Image newPhoto = Instantiate(rightImage, rightImage.rectTransform.anchoredPosition, rightImage.transform.rotation);
            newPhoto.transform.SetParent(photobookMenuObject.transform, false);
            //set the image of the photo
            newPhoto.sprite = photoList[i].GetComponent<PhotoBehaviour>().photo;
            //add to the list
            photoImageList.Add(newPhoto);
        }
    }

    //lock the player when photobook menu opens
    private void OpenPhotobook()
    {
        //set photobook to open
        isPhotobookOpen = true;
        //lock the players movement in place
        PlayerController.LockPlayer();
        //Activate the photobook object
        photobookMenuObject.SetActive(true);
        //always look at the first photo first
        currentPhoto = 0;
        //clear out any text that may exist from previous uses
        text.text = "";
        //check if photobook has any photos
        //if there are no photos
        if (photoList.Count == 0)
        {
            //set the shown image to the no photos image
            displayImage.sprite = noPhotosImage;

        }
        //else there are photos
        else
        {
            //populate the list with new photos
            PopulatePhotoList();
            //for first photo <- must be there to reach this point
            displayImage.sprite = photoList[currentPhoto].GetComponent<PhotoBehaviour>().photo;
            //display text
            ChangeTextDescription();
        }
    }

    //unlock the player and close the photobook menu
    private void ClosePhotobook()
    {
        //set photobook open to false
        isPhotobookOpen = false;
        //unlock the player
        PlayerController.UnlockPlayer();
        //destroy all the items in the created photolist
        for (int i = 0; i < photoImageList.Count; i++)
        {
            Destroy(photoImageList[i]);
        }
        //clear the photo image list
        photoImageList.Clear();
        //deactivate the menu
        photobookMenuObject.SetActive(false);
    }

    //Change to next image
    public void ChangePhotoNext()
    {
        //check if theres a next from current photo
        if (currentPhoto < photoImageList.Count - 1)
        {
            //move the current photo to left image pos
            photoImageList[currentPhoto].rectTransform.DOAnchorPos(leftImage.rectTransform.anchoredPosition, transitionTime, false);
            //scale the image to the same as left image
            photoImageList[currentPhoto].rectTransform.DOScale(leftImage.rectTransform.localScale, transitionTime);
            //move the image on the right to current photo location
            photoImageList[currentPhoto + 1].rectTransform.DOAnchorPos(selectedImage.rectTransform.anchoredPosition, transitionTime, false);
            photoImageList[currentPhoto + 1].rectTransform.DOScale(selectedImage.rectTransform.localScale, transitionTime);
            //increment current
            currentPhoto++;
            //change display image to current
            displayImage.sprite = photoImageList[currentPhoto].sprite;
            //print("Current index = " + currentPhoto);
            //rearrange order
            RearrangeGUIComponents();
            //display text
            ChangeTextDescription();
        }
    }

    //change to previous image
    public void ChangePhotoPrev()
    {
        //check if there is a prev photo
        if (currentPhoto > 0)
        {
            //move current photo to right image pos
            photoImageList[currentPhoto].rectTransform.DOAnchorPos(rightImage.rectTransform.anchoredPosition, transitionTime, false);
            //scale the image to the same as left image
            photoImageList[currentPhoto].rectTransform.DOScale(rightImage.rectTransform.localScale, transitionTime);
            //move the image on the left to current photo location
            photoImageList[currentPhoto - 1].rectTransform.DOAnchorPos(selectedImage.rectTransform.anchoredPosition, transitionTime, false);
            photoImageList[currentPhoto - 1].rectTransform.DOScale(selectedImage.rectTransform.localScale, transitionTime);
            //decrement current
            currentPhoto--;
            //change display image to current
            displayImage.sprite = photoImageList[currentPhoto].sprite;
            //rearrange order
            RearrangeGUIComponents();
            //print("Current index = " + currentPhoto);
            //display text
            ChangeTextDescription();
        }
    }

    //change the text description
    public void ChangeTextDescription()
    {
        //change the text to what the current photo is
        text.text = photoList[currentPhoto].GetComponent<PhotoBehaviour>().description;
    }

    //move gui components around such that certain parts are always displayed at the front
    private void RearrangeGUIComponents()
    {
        //if left and right images exists, make sure the next on either side is on top of pile
        //if there is something on the left
        if (currentPhoto > 0)
        {
            photoImageList[currentPhoto - 1].transform.SetAsLastSibling();
        }
        //if there is something on the right
        if (currentPhoto < photoImageList.Count - 1)
        {
            photoImageList[currentPhoto + 1].transform.SetAsLastSibling();
        }
        //move the buttons such that they are always on top
        rightButton.transform.SetAsLastSibling();
        leftButton.transform.SetAsLastSibling();
    }
}
