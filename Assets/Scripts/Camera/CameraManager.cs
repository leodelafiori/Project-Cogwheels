using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    /// Singleton
    /// ////////////////// CameraManager
    /// Responsible for who to focus the camera during gameplay

    #region Declaring variables
    public static CameraManager instance = null;
    public bool inDialog = false;
 //Declaring player and camera transforms
    private GameObject player;
    private Transform playerTransform;
    [SerializeField] private Transform camera;
 //Camera tweak variables
    [SerializeField] float smoothSpeed = 4; //smoothes the movement
    [SerializeField] private float cameraHeight = 0.75f; // Adjusts the height of the following camera
    [SerializeField] private float cameraOffset = -5f; // Adjusts the x of the camera, to not be inside the player
 // Declaring CameraClamping
    [SerializeField] private float xMin = -4.3f;
    [SerializeField] private float xMax = 1.6f;
    [SerializeField] private float yMin = -2f;
    [SerializeField] private float yMax = 0.2f;


    #endregion
    #region Awake method
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
    }
    #endregion
    #region FixedUpdate (CameraClamping/Following)
    private void FixedUpdate() //Late update used because the character uses update, the camera will wait the update frame to occur before moving the camera
    {
        if (!inDialog) // Normally following player when out of dialog
        {
            //Using clamp function to get values between the minimum and maximum number in the player position
            float x = Mathf.Clamp(playerTransform.position.x, xMin, xMax);
            float y = Mathf.Clamp(playerTransform.position.y, yMin, yMax);
            //Making the target position become the clamp + adding the offsets
            Vector3 TargetPos = new Vector3(x, y + cameraHeight, playerTransform.position.z + cameraOffset);
            Vector3 SmoothPos = Vector3.Lerp(camera.position, TargetPos, smoothSpeed * Time.deltaTime);
            camera.position = SmoothPos;
        }
    }
    #endregion

    #region focusing on subject method
    public void WhosTalking(Transform subject)
    {
        //Making the target position
        Vector3 TargetPos = new Vector3(subject.position.x, subject.position.y + cameraHeight, subject.transform.position.z + cameraOffset);
        Vector3 SmoothPos = Vector3.Lerp(camera.position, TargetPos, smoothSpeed * Time.deltaTime);
        camera.position = SmoothPos;
    }
    #endregion
}
