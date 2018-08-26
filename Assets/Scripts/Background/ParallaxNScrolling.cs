using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxNScrolling : MonoBehaviour {

    #region Declaring variables
    private Transform cameraTransform;
    private Transform[] childTransforms;
    private int leftIndex;
    private int rightIndex;
    private float lastCameraX;
    [SerializeField] private float backgroundSize;
    [SerializeField] private float parallaxSpeed;
    #endregion


    #region Start, setting all the main variables and child transforms
    void Start () {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        childTransforms = new Transform[transform.childCount]; // creating the array Lengh to be the children
        for (int i = 0; i < transform.childCount; i++)
        {
            childTransforms[i] = transform.GetChild(i); // inserting the children using a loop
        }

        leftIndex = 0;
        rightIndex = childTransforms.Length - 1;

	}
    #endregion

    #region ScrollLeft and ScrollRight Functions, used to make the background "infinite"
    private void ScrollLeft()
    {
        int lastRight = rightIndex;
        childTransforms[rightIndex].position = Vector3.right * (childTransforms[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if(rightIndex < 0)
        {
            rightIndex = childTransforms.Length - 1;
        }
    }

    private void ScrollRight()
    {
        int lastLeft = leftIndex;
        childTransforms[leftIndex].position = Vector3.right * (childTransforms[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex ++;
        if (leftIndex == childTransforms.Length)
        {
            leftIndex = 0;
        }
    }
    #endregion

    #region update, to keep track of where the camera is and roll/parallax
    void Update () {

        //rolling the background
		if (cameraTransform.position.x < (childTransforms[leftIndex].transform.position.x) + backgroundSize)
        {
            ScrollLeft();
        }
        if (cameraTransform.position.x > (childTransforms[rightIndex].transform.position.x) - backgroundSize)
        {
            ScrollRight();
        }

        //parallax
        float deltaX = cameraTransform.position.x - lastCameraX;
        transform.position += Vector3.right * (deltaX * parallaxSpeed);
        lastCameraX = cameraTransform.position.x;
    }
    #endregion
}
