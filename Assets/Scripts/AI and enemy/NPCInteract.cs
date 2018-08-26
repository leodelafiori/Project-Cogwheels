using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteract : MonoBehaviour {

    #region Declaring variables
    //main variables
    private Transform whosTalking;
    private GameObject player;
    private Transform playerTransform;
    private GameObject npc;
    private Transform npcTransform;
    private bool isTalking = false;
 //Interacting when close variables
    [SerializeField] private GameObject message;
    private bool isClose;
 //Text for dialog
    [SerializeField] private GameObject textCanvas;
    private float distanceSet = 1; 
    [SerializeField] private Transform textCanvasPos;
    [SerializeField] private float textCanvasYoffset = 0.7f;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcDialogText;
    [SerializeField] private string[] sentences;
    [SerializeField] private string[] names;
    private int index;
    [SerializeField] private float typingSpeed = 0.02f;
    private int dialogContinue = 0;

    #endregion
    
    #region Awake, setting the different components into variables
    private void Awake()
    {
        npc = null;
        npcTransform = transform;
        textCanvas.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        whosTalking = null;

    }
    #endregion
    #region coroutine for the typing animation on the dialog
    IEnumerator Type()
    {
        isTalking = true;
        npcNameText.text = names[index];
        npcDialogText.text = "";
        foreach (char letter in sentences[index].ToCharArray())
        {
            npcDialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTalking = false;
    }
    #endregion
    #region Update
    private void Update()
    {
        #region Showing message
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance < distanceSet)
        {
            npc = gameObject;
            isClose = true;
            //message.SetActive(true);
        } else
        {
            npc = null;
            isClose = false;
            message.SetActive(false);
        }
        #endregion
        #region Interaction
        //Interaction with NPC
        if (index == sentences.Length - 1 && isClose && Input.GetKeyUp(KeyCode.E) && !isTalking || Input.GetKeyUp(KeyCode.Escape)) //Ending the dialog
        {
            textCanvas.SetActive(false);
            dialogContinue = 0;
            index = 0;
            CameraManager.instance.inDialog = false;
            Player_Human_Movement.isMovable = true;

        } else if (isClose && Input.GetKeyUp(KeyCode.E) && dialogContinue > 0 && !isTalking)
        {
            NextSentence();
            StartCoroutine(Type());
        } else if (isClose && Input.GetKeyUp(KeyCode.E) && !isTalking)
        {
            // Dialog
            Player_Human_Movement.isMovable = false;
            textCanvas.SetActive(true);
            StartCoroutine(Type());
            CameraManager.instance.inDialog = true;
            dialogContinue = 1;
           
        }
        #endregion
        #region Who is talking
        if (CameraManager.instance.inDialog && npc == gameObject)
        {
            if (names[index] == "Jim")
            {
                textCanvasPos.position = new Vector3(playerTransform.position.x, playerTransform.position.y + textCanvasYoffset, textCanvasPos.position.z);
                whosTalking = playerTransform;
            }
            else
            {
                textCanvasPos.position = new Vector3(npcTransform.position.x, npcTransform.position.y + textCanvasYoffset, textCanvasPos.position.z);
                whosTalking = npcTransform;
            }
        } else
        {
          
        }
        #endregion
        #region focusing the camera
        if (CameraManager.instance.inDialog && npc == gameObject)
        {
            CameraManager.instance.WhosTalking(whosTalking);
        }
        #endregion

    }
    #endregion
    #region NextSentence method
    private void NextSentence()
    {
        if (index < sentences.Length - 1)
        { //arrays start at zero
            index++;
            npcDialogText.text = "";
        }
    }
    #endregion


}
