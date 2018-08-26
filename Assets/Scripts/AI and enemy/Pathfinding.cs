using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    /// Enemy Pathfinding, usable on meele and ranged enemies
    /// The point of this script is to use 2 points and create a pathfinding AI based on the transforms
    /// This is meant to be usable with other scripts for enemies 
    /// Made by: Leonardo Delafiori


    #region Declaring Variables 
    //Main variables
    public bool isPathfinding = true;
    [SerializeField] private float speed = 0.2f;
    private bool changePoint = false;
    private bool onCoroutine = false;
    //transforms
    public Transform PointA;
    public Transform PointB;
    [SerializeField] private float waitTime = 2;
    //Transforms for animations
    public bool idle;
    public bool moving;
    #endregion

    #region void update, moving the enemy on the paths
    void Update () {
        if (isPathfinding)
        {
            if (!changePoint)
            {
                if (transform.position.x - PointA.position.x < 0.2 && !changePoint)
                {
                    if (!onCoroutine)
                    {
                        StartCoroutine(wait());
                    }
                } else
                {
                    Move(PointA);
                }

            } else
            {
                if (PointB.position.x - transform.position.x < 0.2 && changePoint)
                {
                    if (!onCoroutine)
                    {
                        StartCoroutine(wait());
                    }
                }
                else
                {
                    Move(PointB);       
                }

            }
            
        }
	}
    #endregion
    #region Move() 
    private void Move(Transform point)
    {
        moving = true;
        idle = false;
        transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
    }
    #endregion
    IEnumerator wait()
    {
        onCoroutine = true;
        moving = false;
        idle = true;
        yield return new WaitForSeconds(waitTime);
        changePoint = !changePoint;
        onCoroutine = false;
    }

}
